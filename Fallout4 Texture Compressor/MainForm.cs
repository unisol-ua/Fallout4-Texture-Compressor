using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Runtime;

namespace Fallout4_Texture_Compressor
{
    public partial class MainForm : Form
    {
        int compress_lvl, compress_lvl_alpha;
        double original_files_size = 0, compressed_files_size = 0;
        string compress_lvl_string, compress_lvl_alpha_string, texturerate, texturesize, maxtexturesize, mintexturesize, mipsgen;
        bool ba2 = false;
        int currentthreads = 0;
        Queue<string> logqueue;

        public MainForm()
        {
            InitializeComponent();
        }

        //Form load, loads settings from xml
        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists("settings.xml"))
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load("settings.xml");
                XmlElement xRoot = xDoc.DocumentElement;
                foreach (XmlNode xnode in xRoot)
                {
                    if (xnode.Name == "lastdir")
                    {
                        pathtextbox.Text = xnode.InnerText;
                    }
                }
            }
            if (Directory.Exists(Application.StartupPath + "\\temp")) Directory.Delete(Application.StartupPath + "\\temp", true);
            if (Directory.Exists(Application.StartupPath + "\\backup")) Directory.Delete(Application.StartupPath + "\\backup", true);
            if (Directory.Exists(Application.StartupPath + "\\ba2temp")) Directory.Delete(Application.StartupPath + "\\ba2temp", true);
        }

        //Start button click
        private void startbutton_Click(object sender, EventArgs e)
        {
            if (pathtextbox.Text != "")
            {
                //Set global vars for compression
                if (compress_lvl_string_combo.Text.Contains("BC1")) compress_lvl = 1;
                else if (compress_lvl_string_combo.Text.Contains("BC3")) compress_lvl = 3;
                else if (compress_lvl_string_combo.Text.Contains("BC5")) compress_lvl = 5;
                else if (compress_lvl_string_combo.Text.Contains("BC7")) compress_lvl = 7;

                if (compress_lvl_alpha_string_combo.Text.Contains("BC1")) compress_lvl_alpha = 1;
                else if (compress_lvl_alpha_string_combo.Text.Contains("BC3")) compress_lvl_alpha = 3;
                else if (compress_lvl_alpha_string_combo.Text.Contains("BC5")) compress_lvl_alpha = 5;
                else if (compress_lvl_alpha_string_combo.Text.Contains("BC7")) compress_lvl_alpha = 7;

                compress_lvl_alpha_string = compress_lvl_alpha_string_combo.Text;
                compress_lvl_string = compress_lvl_string_combo.Text;
                texturerate = texturerate_combo.Text;
                texturesize = texturesize_combo.Text;
                maxtexturesize = maxtexturesize_combo.Text;
                mintexturesize = mintexturesize_combo.Text;
                mipsgen = mipsgen_combo.Text;
                logqueue = new Queue<string>();

                original_files_size = 0;
                compressed_files_size = 0;

                optionsbutton.Text = "Options";

                //Starting point of compression
                if (ba2)
                {
                    ba2compress();
                }
                else
                {
                    loosecompress();
                }

                //Clean some garbage
                GC.Collect();
            }
            else
            {
                MessageBox.Show("You need to set destination folder or BA2 archive");
            }
        }

        //Compress loose files
        private void loosecompress()
        {
            string path = pathtextbox.Text;
            string[] allfiles = Directory.GetFiles(path, "*.dds", SearchOption.AllDirectories);
            if (allfiles.Length > 0)
            {
                //Hide settings, show log
                listBox1.Visible = true;
                listBox1.BringToFront();
                optionsbutton.Visible = true;
                exportlogbutton.Visible = true;

                //Timer
                Stopwatch watch = new Stopwatch();
                watch.Start();

                MainForm form = this;
                listBox1.Items.Clear();

                //Backup our files if checkbox is checked. No multithreading for this :(
                Backup(form, allfiles);


                listBox1.Items.Add("Compression options");
                listBox1.Items.Add("Force compression: " + force_compression_check.Checked.ToString());
                listBox1.Items.Add("Ignore face textures: " + ignore_face_check.Checked.ToString());
                listBox1.Items.Add("Ignore specular, normal and glowmaps: " + ignore_sng_maps_check.Checked.ToString());
                listBox1.Items.Add("Ignore diffuse: " + ignore_diffuse_check.Checked.ToString());
                listBox1.Items.Add("Resize textures down: " + resize_check.Checked.ToString());
                listBox1.Items.Add("Multithreading: " + threading_check.Checked.ToString());
		listBox1.Items.Add("Reduce texture size, times: " + texturerate);
		listBox1.Items.Add("Resize textures sized over: " + texturesize);
		listBox1.Items.Add("Max resized texture size: " + maxtexturesize);
		listBox1.Items.Add("Min resized texture size: " + mintexturesize);
		listBox1.Items.Add("Generate mipmaps: " + mipsgen);
                listBox1.Items.Add("");

                //indexed files
                double i = 0;
                if (threading_check.Checked)//Multithreaded compression
                {
                    int maxthreads = int.Parse(threads_combo.Text);
                    if (allfiles.Length < maxthreads) maxthreads = allfiles.Length;
                    int files = 0;
		    int loggerthread = 10;
                    while (files < allfiles.Length || currentthreads > 0)
                    {
			loggerthread--;
			if (loggerthread==0) loggerthread=10;
                        if (currentthreads < maxthreads && files < allfiles.Length)//Trying not to go over maximum allowed threads
                        {
                            startcompressthread(allfiles[files], ignore_sng_maps_check.Checked, ignore_face_check.Checked, ignore_diffuse_check.Checked, force_compression_check.Checked, files + 1);
                            currentthreads++;
                            files++;
                            i++;
                        }

                        //Log some stuff and show progress
                        if (logqueue.Count > 0) if (logqueue.Peek() != null) listBox1.Items.Add(logqueue.Dequeue());
                        form.Text = "Processing files: " + i + " of " + allfiles.Length + " |  Running Threads: " + currentthreads;
                        listBox1.TopIndex = listBox1.Items.Count - 1;

                        //Get some rest
                        Thread.Sleep(10);

                        //Add log entries to listbox while still processing, so users won't panic
                        if ((logqueue.Count > 0)&& (loggerthread==10) )
                        {
                            string[] fixqueue = logqueue.ToArray();
                            logqueue.Clear();
                            foreach (string entry in fixqueue) { if (entry != null ) listBox1.Items.Add(entry); }
                        }


                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }

                    //Add remaining log entries to listbox
                    if (logqueue.Count > 0)
                    {
                        string[] fixqueue = logqueue.ToArray();
                        logqueue.Clear();
                        foreach (string entry in fixqueue) listBox1.Items.Add(entry);
                    }
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
                else//Singlethreaded compression
                {
                    foreach (string file in allfiles)
                    {
                        i++;
                        compressmaster(file, ignore_sng_maps_check.Checked, ignore_face_check.Checked, ignore_diffuse_check.Checked, force_compression_check.Checked, (int)i, false);
                        form.Text = "Processing files: " + i + " of " + allfiles.Length;
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }
                }
                //Delete temp files
                if (Directory.Exists(Application.StartupPath + "\\temp")) Directory.Delete(Application.StartupPath + "\\temp", true);

                //Log our success
                form.Text = "Compressed from " + Math.Round(original_files_size / 1024, 3) + "mb to " + Math.Round(compressed_files_size / 1024, 3) + "mb Saved = " + Math.Round((original_files_size - compressed_files_size) / 1024, 3) + "mb";
                listBox1.Items.Add("");
                listBox1.Items.Add("Original files size = " + Math.Round(original_files_size / 1024, 3) + " mb");
                listBox1.Items.Add("Compressed files size = " + Math.Round(compressed_files_size / 1024, 3) + " mb");
                listBox1.Items.Add("Saved = " + Math.Round((original_files_size - compressed_files_size) / 1024, 3) + " mb");

                //Some info for speedrunners
                watch.Stop();
                TimeSpan ts = watch.Elapsed;
                string elapsedTime = String.Format("{0:00}h {1:00}m {2:00}s {3:00}ms", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                listBox1.Items.Add("Elapsed time = " + elapsedTime);
                listBox1.TopIndex = listBox1.Items.Count - 1;
            }
            else
            {
                MessageBox.Show("No .dds files were found in the folder.");
            }
        }

        //Backup loose files
        private void Backup(MainForm form, string[] allfiles)
        {
            //indexed files
            double i = 0;
            if (backup_check.Checked == true)
            {
                //ignored files
                double ignored = 0;

                //Copy files to temporal folder for backup
                foreach (string file in allfiles)
                {
                    i++;
                    form.Text = "Copying files: " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                    FileInfo fileinf = new FileInfo(file);

                    //Ignore files if set to
                    if (
                        (ignore_sng_maps_check.Checked && (fileinf.Name.CIContains("_s") || fileinf.Name.CIContains("_n") || fileinf.Name.CIContains("_g.dds"))) ||
                        (ignore_diffuse_check.Checked && fileinf.Name.CIContains("_d"))
                        )
                    {
                        ignored++;
                    }
                    else
                    {
                        string newfolders = fileinf.DirectoryName.Replace(pathtextbox.Text, "");
                        Directory.CreateDirectory(Application.StartupPath + "\\backup" + newfolders);
                        File.Copy(file, Application.StartupPath + "\\backup" + fileinf.FullName.Replace(pathtextbox.Text, ""), true);
                    }
                }

                //Create zip archive
                form.Text = "Archiving files";
                string time = DateTime.Now.Second + "s_" + DateTime.Now.Minute + "m_" + DateTime.Now.Hour + "h_" + DateTime.Now.Day + "d_" + DateTime.Now.Month + "m_" + DateTime.Now.Year + "y";
                Process process = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.UseShellExecute = false;
                startInfo.RedirectStandardOutput = true;
                startInfo.RedirectStandardError = true;
                startInfo.FileName = Application.StartupPath + "\\bin\\7za.exe";

                //Set our parameters
                if (backupname.Text == "") { startInfo.Arguments = "a backup_" + time + ".zip \"" + Application.StartupPath + "\\backup\\*\" -mx6 -o" + Application.StartupPath + "\\"; }
                else { startInfo.Arguments = "a \"" + backupname.Text + "_" + time + ".zip\" \"" + Application.StartupPath + "\\backup\\*\" -mx6 -o" + Application.StartupPath + "\\"; }

                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();

                //Delete temporal folder
                Directory.Delete(Application.StartupPath + "\\backup", true);

                //Log some stuff
                listBox1.Items.Add("Backup: " + Application.StartupPath + "\\" + backupname.Text + "_" + time + ".zip");
                listBox1.Items.Add("Archived " + (i - ignored) + " of " + i + " files");
                if (ignore_sng_maps_check.Checked) listBox1.Items.Add(ignored + " files were ignored");
                listBox1.Items.Add("");
            }
        }

        //Ultimate Multithreaded Compression Thread Starter
        private Thread startcompressthread(string file, bool ignore_sng_maps, bool ignore_face, bool ignore_diffuse, bool force_compression, int num)
        {
            var t = new Thread(() => compressmaster(file, ignore_sng_maps, ignore_face, ignore_diffuse, force_compression, num, true));
            t.Start();
            Debug.WriteLine("Thread " + num + " started");
            return t;
        }

        //Compression master that actually give a shit for user given options
        private void compressmaster(string file, bool ignore_sng_maps, bool ignore_face, bool ignore_diffuse, bool force_compression, int num, bool isthreaded)
        {
            List<string> log = new List<string>();
            FileInfo fileinf = new FileInfo(file);
            log.Add(num + ": " + file);

            //Ignored specular, normal and glow maps
            if (ignore_sng_maps && (fileinf.Name.CIContains("_s.dds") || fileinf.Name.CIContains("_n.dds") || fileinf.Name.CIContains("_g.dds")))
            {
                log.Add("Specular, normal and glowmaps are ignored, skipping");
            }
            //Ignore face textures
            else if (ignore_face && (fileinf.Name.CIContains("femalehead") || fileinf.Name.CIContains("malehead")))
            {
                log.Add("Face textures are ignored, skipping");
            }
            //Ignore diffuse textures
            else if (ignore_diffuse && !(fileinf.Name.CIContains("_s.dds") || fileinf.Name.CIContains("_n.dds") || fileinf.Name.CIContains("_g.dds")))
            {
                log.Add("Diffuse textures are ignored, skipping");
            }
            else
            {
                //Analyze dds file
                ddsfileinfo ddsinfo = checkdds(file,0);
log.Add("format = " + ddsinfo.format + "; alpha = " + ddsinfo.alpha.ToString() + "; height = " + ddsinfo.height + " width  = " + ddsinfo.width);

                if (ddsinfo.format.Contains("Unsupported format"))
                {
                    log.Add("Unsupported format, skipping");
                }
                else
                {
                    //Some serious calculations
                    double filesize = Math.Round((Double)new FileInfo(file).Length / 1024, 1);
                    original_files_size += filesize;
                    log.Add("file size = " + filesize + " kb");

                    //Compression
/*                    if ((compress_check.Checked)&&(!resize_check.Checked))
                    {
                        string compressed = CompressDDS(ddsinfo, file, fileinf, force_compression);
                        if (!compressed.Contains("Already compressed")) log.Add("new format = " + compressed);
                    }
*/
                    //Resizing
                    if (resize_check.Checked)
                    {
                        string resized = resize(ddsinfo, file, fileinf);
/*                        if (resized>1)
                        {
                            log.Add("new width  = " + (ddsinfo.width / resized));
                            log.Add("new height = " + (ddsinfo.height / resized));
                        }
*/
log.Add(resized);
                    }

                    //Log size of the file
                    double newfilesize = Math.Round((Double)new FileInfo(file).Length / 1024, 1);
                    compressed_files_size += newfilesize;
                    if (newfilesize < filesize)
                    {
                        log.Add("new file size = " + newfilesize + " kb");
                    }
                }
            }

            //Add entries from log to listbox if run in singlethreaded mode
            if (!isthreaded) foreach (string entry in log) listBox1.Items.Add(entry);
            //Use Queue for our log entries and report thread finish if run in multithreaded mode
            else
            {
                foreach (string entry in log) logqueue.Enqueue(entry);
                if (isthreaded) currentthreads--;
            }
        }

        //Compression
        //Compose Arguments for texconv from our parameters and run it
        private string CompressDDS(ddsfileinfo ddsinfo, string file, FileInfo fileinf, bool force_compression)
        {
            string newformat = "Already compressed";

            //Convert our string ddsfile type info to int
            int ddslvl = 10;
            //Skip if run in force comrpession mode
            if (!force_compression)
                if (ddsinfo.format.Contains("BC1")) ddslvl = 1;
                else if (ddsinfo.format.Contains("BC2")) ddslvl = 3;
                else if (ddsinfo.format.Contains("BC3")) ddslvl = 3;
                else if (ddsinfo.format.Contains("BC4")) ddslvl = 4;
                else if (ddsinfo.format.Contains("BC5")) ddslvl = 5;
                else if (ddsinfo.format.Contains("BC7")) ddslvl = 7;

            //SRGB textures should be compressed to the SRGB format and non SRGB to non SRGB. EASY
            string srgb_suffix = "";
            if (ddsinfo.format.Contains("SRGB"))
                srgb_suffix = "_SRGB";



            //Maximum compression is already achieved, congratulations.
            if (ddslvl == 1)
                return newformat;
	    //Maximum for alpha
	    if ((ddsinfo.alpha == true) && (ddslvl == 3))
		return newformat;
            //Parameters for texture with alpha channel
            if (ddsinfo.alpha == true)
            {
                if (ddslvl > compress_lvl_alpha)
                    if (ddsinfo.format.Contains("SRGB") && compress_lvl_alpha_string == "BC5")//SRGB BC5 is SASSY so we should only compress it to BC3
                    {
//                        texconv("\"" + file + "\" -y -f BC3_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                        texconv("\"" + file + "\" -y -f BC7_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                        newformat = "BC7_UNORM_SRGB";
                    }
                    else
                    {
                        texconv("\"" + file + "\" -y -f " + compress_lvl_alpha_string + "_UNORM" + srgb_suffix + " -o \"" + fileinf.DirectoryName + "\"");
                        newformat = compress_lvl_alpha_string + "_UNORM" + srgb_suffix;
                    }
            }
            //Parameters for texture without alpha channel
            else
            {
                if (ddslvl > compress_lvl)
                    if (ddsinfo.format.Contains("SRGB") && compress_lvl_string == "BC5")//SRGB BC5 is SASSY so we should only compress it to BC3
                    {
//                        texconv("\"" + file + "\" -y -f BC3_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                        texconv("\"" + file + "\" -y -f BC7_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                        newformat = "BC7_UNORM_SRGB";
                    }
                    else
                    {
                        texconv("\"" + file + "\" -y -f " + compress_lvl_string + "_UNORM" + srgb_suffix + " -o \"" + fileinf.DirectoryName + "\"");
                        newformat = compress_lvl_string + "_UNORM" + srgb_suffix;
                    }
            }
            return newformat;
        }

        //Resizing
        //Compose Arguments for texconv from our parameters and run it
        private string resize(ddsfileinfo ddsinfo, string file, FileInfo fileinf)
        {
            int size = 0;
	    int minsize = 0;
	    int divisor = 1;
	    int mips=1;
	    string BC_prefix = "";
	    string signnorm="";
            string srgb_suffix = "";
	 string convargs = "";
            if (ddsinfo.format.Contains("SRGB"))
                srgb_suffix = "_SRGB";
            if (ddsinfo.format.Contains("BC4")) BC_prefix = "BC4"; else
            if (ddsinfo.format.Contains("BC3")) BC_prefix = "BC3"; else //"BC7"
            if (ddsinfo.format.Contains("BC1")) BC_prefix = "BC1"; //"BC7"

            if (ddsinfo.format.Contains("_SNORM")) {
                signnorm = "_SNORM"; if (ddsinfo.format.Contains("BC5")) BC_prefix = "BC5";
		} else if (ddsinfo.format.Contains("BC5")) BC_prefix = "BC3";

		 if (ddsinfo.format.Contains("_TYPELESS"))
		signnorm = "_TYPELESS"; else
		 if (ddsinfo.format.Contains("_UNORM"))
		signnorm = "_UNORM";

            //Get the biggest size in case if dimensions are unequal
            if (ddsinfo.height > ddsinfo.width) { size = ddsinfo.height; minsize=ddsinfo.width; } else { size = ddsinfo.width; minsize = ddsinfo.height;}

            //Convert text parameter to int
            int ifgreater = 8192;
	    int maxts = 8192;
	    int mints = 8192;

            if (texturerate.Contains("2")) { divisor = 2; }
            else if (texturerate.Contains("4")) { divisor = 4; }
            else if (texturerate.Contains("8")) { divisor = 8; }

            if (texturesize.Contains("128")) { ifgreater = 128; }
            else if (texturesize.Contains("256")) { ifgreater = 256; }
            else if (texturesize.Contains("512")) { ifgreater = 512; }
            else if (texturesize.Contains("1024")) { ifgreater = 1024; }
            else if (texturesize.Contains("2048")) { ifgreater = 2048; }
            else if (texturesize.Contains("4096")) { ifgreater = 4096; }
            else if (texturesize.Contains("All")) { ifgreater = 4; }

            if (maxtexturesize.Contains("128")) { maxts = 128; }
            else if (maxtexturesize.Contains("256")) { maxts = 256; }
            else if (maxtexturesize.Contains("512")) { maxts = 512; }
            else if (maxtexturesize.Contains("1024")) { maxts = 1024; }
            else if (maxtexturesize.Contains("2048")) { maxts = 2048; }
            else if (maxtexturesize.Contains("4096")) { maxts = 4096; }


            if (mintexturesize.Contains("32")) { mints = 32; }
            else if (mintexturesize.Contains("64")) { mints = 64; }
            else if (mintexturesize.Contains("128")) { mints = 128; }
            else if (mintexturesize.Contains("256")) { mints = 256; }
            else if (mintexturesize.Contains("512")) { mints = 512; }
            else if (mintexturesize.Contains("All")) { mints = 16; }

            if (mipsgen.Contains("1")) { mips = 1; }
            else if (mipsgen.Contains("2")) { if (ddsinfo.mips>1)  mips = 2; }
            else if (mipsgen.Contains("half")) { if (ddsinfo.mips>1)  mips = Convert.ToInt32(Math.Floor( (Math.Log(minsize/divisor ) / Math.Log( 2 ))/2)); }
            else if (mipsgen.Contains("All")) {  if (ddsinfo.mips>1)  mips = Convert.ToInt32(Math.Log( minsize/divisor ) / Math.Log( 2 )); }

            if (size > ifgreater) 
		{
		if (size > maxts*4) divisor = size / maxts ; 
		if (minsize/divisor <= mints) divisor = minsize/mints;
		if ((minsize <= mints)||((minsize/divisor <= mints)&&(divisor==2))||(divisor<=1)) return "1";

                //Resize
//                texconv("\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / divisor + " -h " + ddsinfo.height / divisor + " -m 1");
                //Generate mipmaps
//                texconv("\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\"" + " -m 0");
                //Resize

//if (ddsinfo.mips>2)  mips = Convert.ToInt32(Math.Log( minsize/divisor ) / Math.Log( 2 ));
convargs="\"" + file + "\" -y -f " + BC_prefix + signnorm + srgb_suffix  + " -w " + ddsinfo.width / divisor + " -h " + ddsinfo.height / divisor + " -m " + mips.ToString() + " -o \"" + fileinf.DirectoryName + "\"";
                texconv(convargs);
                //Generate mipmaps
//                texconv("\"" + file + "\" -y -f BC7_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"" + " -m 0");


                return convargs;//Report our success
            }
            else
                return "1";//Report that we are already gucci
        }

        //TexConv Utility Starter
        private void texconv(string arguments)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = Application.StartupPath + "\\bin\\texconv.exe";
            startInfo.Arguments = arguments;
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        //Run texdiag tool for gathering info about dds file
        private ddsfileinfo checkdds(string file, int checkalpha)
        {
            //Setup
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            //startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.FileName = Application.StartupPath + "\\bin\\texdiag.exe";
            startInfo.Arguments = "info \"" + file + "\"";
            process.StartInfo = startInfo;
            process.Start();

            //Start gathering info
            ddsfileinfo ddsinfo = new ddsfileinfo();
            while (!process.StandardOutput.EndOfStream)
            {
                //Read line, duuuh
                string line = process.StandardOutput.ReadLine();

                //Parse height of texture
                if (line.Contains("height"))
                {
                    //Some users reported int.parse errors so we are doing this long ctrl-c + ctrl-v shit now that might not actually work
                    int temp;
                    if (int.TryParse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2), out temp))
                    {
                        ddsinfo.height = temp;
                    }
                    else
                        try
                        {
                            ddsinfo.height = int.Parse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2), CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Couldn't parse dds height. Screenshot this message and send it to the author and attach the file that caused this error. File: " + file + " Error log: " + ex.ToString());
                        }
                }

                //Parse width of texture
                if (line.Contains("width"))
                {
                    //Some users reported int.parse errors so we are doing this long ctrl-c + ctrl-v shit now that might not actually work
                    int temp;
                    if (int.TryParse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2), out temp))
                        ddsinfo.width = temp;
                    else
                        try
                        {
                            ddsinfo.width = int.Parse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2), CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Couldn't parse dds height. Screenshot this message and send it to the author and attach the file that caused this error. File: " + file + " Error log: " + ex.ToString());
                        }
                }

                //Parse width of texture
                if (line.Contains("mipLevels"))
                {
                    //Some users reported int.parse errors so we are doing this long ctrl-c + ctrl-v shit now that might not actually work
                    int temp;
                    if (int.TryParse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2), out temp))
                        ddsinfo.mips = temp;
                    else
                        try
                        {
                            ddsinfo.mips = int.Parse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2), CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Couldn't parse dds mip levels. Screenshot this message and send it to the author and attach the file that caused this error. File: " + file + " Error log: " + ex.ToString());
                        }
                }

                //Get format
                if (line.Contains("format"))
                {
                    ddsinfo.format = line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2);
                }

                //Done goofed, no luck here
                if (line.Contains("FAILED"))
                {
                    ddsinfo.format = "Unsupported format";
                    ddsinfo.width = 0;
                    ddsinfo.height = 0;
                }
            }
            //process.WaitForExit();

if (checkalpha < 1 ) 
	ddsinfo.alpha = true; //avoid alpha checks speedup
	else {

            //Now get ready for some crazy stuff
            //With the help of the Elder Gods and curiosity I've found the way to determine if texture actually has alpha channel, because texdiag doesn't show it straight on :(
            //1: Convert it to BC3 so we only have 2 channels
            //2: Analyze that shit
            //3a: Textures with alpha channel have data in both blocks, which means that block6 and block8 are > 0
            //3b: Textures without alpha channenl use only one of the blocks, which means that either block6 or block8 is = 0
            //MAGIC

            ddsinfo.alpha = false;

            //Convert to BC3
            if (!Directory.Exists(Application.StartupPath + "\\temp")) Directory.CreateDirectory(Application.StartupPath + "\\temp");
            FileInfo fileinf = new FileInfo(file);
            if (ddsinfo.format.Contains("BC3"))
            {
                File.Copy(file, Application.StartupPath + "\\temp\\" + fileinf.Name, true);
            }
            else
            {
                if (ddsinfo.format.Contains("SRGB"))
                {
                    texconv("\"" + file + "\" -y -f BC3_UNORM_SRGB -o \"" + Application.StartupPath + "\\temp" + "\"");
                }
                else
                {
                    texconv("\"" + file + "\" -y -f BC3_UNORM -o \"" + Application.StartupPath + "\\temp" + "\"");
                }
            }

            //Analyze
            startInfo.Arguments = "analyze \"" + Application.StartupPath + "\\temp\\" + fileinf.Name + "\"";
            process.StartInfo = startInfo;
            process.Start();
            bool finished6 = false;
            bool finished8 = false;
            int blocks6 = 0;
            int blocks8 = 0;

            //Analyze doesn't close itself sometimes so we have to check ourselves if we read everything we need
            while (!process.StandardOutput.EndOfStream && !(finished6 && finished8))
            {
                string line = process.StandardOutput.ReadLine();

                //8block
                if (line.Contains("8 alpha blocks") && !finished8)
                {
                    finished8 = true;

                    //Some users reported int.parse errors so we are doing this long ctrl-c + ctrl-v shit now that might not actually work
                    int temp;
                    if (int.TryParse(line.Replace("8 alpha blocks - ", ""), out temp))
                    {
                        blocks8 = temp;
                    }
                    else
                    {
                        try
                        {
                            blocks8 = int.Parse(line.Replace("8 alpha blocks - ", ""), CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Couldn't parse dds height. Screenshot this message and send it to the author and attach the file that caused this error. File: " + file + " Error log: " + ex.ToString());
                        }
                    }
                }

                //6block
                if (line.Contains("6 alpha blocks") && !finished6)
                {
                    finished6 = true;

                    //Some users reported int.parse errors so we are doing this long ctrl-c + ctrl-v shit now that might not actually work
                    int temp;
                    if (int.TryParse(line.Replace("6 alpha blocks - ", ""), out temp))
                    {
                        blocks6 = temp;
                    }
                    else
                    {
                        try
                        {
                            blocks6 = int.Parse(line.Replace("6 alpha blocks - ", ""), CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Couldn't parse dds height. Screenshot this message and send it to the author and attach the file that caused this error. File: " + file + " Error log: " + ex.ToString());
                        }
                    }
                }
                //process.WaitForExit();

                //Alpha = rekt
                if (blocks6 > 0 && blocks8 > 0) ddsinfo.alpha = true;
                //Delete converted file
//                if (File.Exists("\""+Application.StartupPath + "\\temp\\" + fileinf.Name + "\"")) File.Delete("\"" + Application.StartupPath + "\\temp\\" + fileinf.Name + "\""); else MessageBox.Show("No \"" + Application.StartupPath + "\\temp\\" + fileinf.Name + "\"");
//                if (File.Exists(Application.StartupPath + "\\temp\\" + fileinf.Name )) File.Delete(Application.StartupPath + "\\temp\\" + fileinf.Name ); else MessageBox.Show("No " + Application.StartupPath + "\\temp\\" + fileinf.Name );
            }
//                File.Delete("\"" + Application.StartupPath + "\\temp\\" + fileinf.Name + "\"");
if (File.Exists(Application.StartupPath + "\\temp\\" + fileinf.Name )) File.Delete( Application.StartupPath + "\\temp\\" + fileinf.Name ); //else MessageBox.Show("\"" + Application.StartupPath + "\\temp\\" + fileinf.Name + "\"");
	}
            return ddsinfo;
        }

        private void ba2compress()
        {
            FileInfo archivefile = new FileInfo(pathtextbox.Text);
            double archivesize = Math.Round((Double)archivefile.Length / 1024 / 1024, 3);
            
            //Open Archive
            OpenArchive(pathtextbox.Text);

            //Extract files to temporal folder
            string ba2folder = Application.StartupPath + "\\ba2temp";
            if (!Directory.Exists(ba2folder)) Directory.CreateDirectory(ba2folder);
            this.Text = "Extracting files from archive...";
            foreach (FileEntry entry in EditableArchive.Entries)
            {
                entry.ExtractTo(ba2folder);
            }

            //Compress files
            string[] allfiles = Directory.GetFiles(ba2folder, "*.dds", SearchOption.AllDirectories);
            if (allfiles.Length > 0)
            {
                //Hide settings, show log
                listBox1.Visible = true;
                listBox1.BringToFront();
                optionsbutton.Visible = true;
                exportlogbutton.Visible = true;

                //Timer
                Stopwatch watch = new Stopwatch();
                watch.Start();

                MainForm form = this;
                listBox1.Items.Clear();

                //Backup our precious Archive
                if (backup_check.Checked == true)
                {
                    form.Text = "Copying BA2 Acthive";
                    File.Copy(pathtextbox.Text, Application.StartupPath + "\\" + archivefile.Name, true);
                    listBox1.Items.Add("Backup: " + Application.StartupPath + "\\" + archivefile.Name);
                    listBox1.Items.Add("");
                }

                listBox1.Items.Add("Compression options");
                listBox1.Items.Add("Force compression: " + force_compression_check.Checked.ToString());
                listBox1.Items.Add("Ignore face textures: " + ignore_face_check.Checked.ToString());
                listBox1.Items.Add("Ignore specular, normal and glowmaps: " + ignore_sng_maps_check.Checked.ToString());
                listBox1.Items.Add("Ignore diffuse: " + ignore_diffuse_check.Checked.ToString());
                listBox1.Items.Add("Resize textures down: " + resize_check.Checked.ToString());
                listBox1.Items.Add("Multithreading: " + threading_check.Checked.ToString());
                listBox1.Items.Add("");

                //indexed files
                double i = 0;
                if (threading_check.Checked)//Multithreaded compression
                {
                    int maxthreads = int.Parse(threads_combo.Text);
                    if (allfiles.Length < maxthreads) maxthreads = allfiles.Length;
                    int files = 0;
                    while (files < allfiles.Length || currentthreads > 0)
                    {
                        if (currentthreads < maxthreads && files < allfiles.Length)//Trying not to go over maximum allowed threads
                        {
                            startcompressthread(allfiles[files], ignore_sng_maps_check.Checked, ignore_face_check.Checked, ignore_diffuse_check.Checked, force_compression_check.Checked, files + 1);
                            currentthreads++;
                            files++;
                            i++;
                        }

                        //Log some stuff and show progress
                        if (logqueue.Count > 0) if (logqueue.Peek() != null) listBox1.Items.Add(logqueue.Dequeue());
                        form.Text = "Processing files: " + i + " of " + allfiles.Length + " :  Running Threads: " + currentthreads;
                        listBox1.TopIndex = listBox1.Items.Count - 1;

                        //Get some rest
                        Thread.Sleep(10);

                        //Add log entries to listbox while still processing, so users won't panic
                        if (logqueue.Count > 0)
                        {
                            string[] fixqueue = logqueue.ToArray();
                            logqueue.Clear();
                            foreach (string entry in fixqueue) listBox1.Items.Add(entry);
                        }
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }

                    //Add remaining log entries to listbox
                    if (logqueue.Count > 0)
                    {
                        string[] fixqueue = logqueue.ToArray();
                        logqueue.Clear();
                        foreach (string entry in fixqueue) listBox1.Items.Add(entry);
                    }
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
                else//Singlethreaded compression
                {
                    foreach (string file in allfiles)
                    {
                        i++;
                        compressmaster(file, ignore_sng_maps_check.Checked, ignore_face_check.Checked, ignore_diffuse_check.Checked, force_compression_check.Checked, (int)i, false);
                        form.Text = "Processing files: " + i + " of " + allfiles.Length;
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }
                }
                //Delete temp files
                if (Directory.Exists(Application.StartupPath + "\\temp")) Directory.Delete(Application.StartupPath + "\\temp", true);

                //Files are compressed
                //Time to assemble new archive
                this.EditableArchive.NewArchive();
                this.SetDirty(false);

                //Check if new file are actually lighter than original (except if force compression enabled)
                if (original_files_size > compressed_files_size || force_compression_check.Checked)
                {
                    //Add folders
                    foreach (string folder in Directory.GetDirectories(ba2folder))
                    {
                        AddFolderToArchive(folder);
                    }

                    //Save
                    SaveArchive();

                    //Log our success
                    double newarchivesize = Math.Round((Double)new FileInfo(pathtextbox.Text).Length / 1024 / 1024, 3);
                    listBox1.Items.Add("");
                    listBox1.Items.Add("Original Archive size = " + archivesize + " mb");
                    listBox1.Items.Add("Compressed Archive size = " + newarchivesize + " mb");
                    listBox1.Items.Add("Saved = " + (archivesize - newarchivesize) + " mb");

                    //Some info for speedrunners
                    watch.Stop();
                    TimeSpan ts = watch.Elapsed;
                    string elapsedTime = String.Format("{0:00}h {1:00}m {2:00}s {3:00}ms", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                    listBox1.Items.Add("Elapsed time = " + elapsedTime);
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                    form.Text = "Compressed from " + archivesize + "mb to " + newarchivesize + "mb Saved = " + (archivesize - newarchivesize) + "mb";
                }
                else
                {
                    listBox1.Items.Add("");
                    listBox1.Items.Add("Archive is already compressed");
                }
                //Delete temp files
                if(Directory.Exists(ba2folder)) Directory.Delete(ba2folder, true);
            }
            else
            {
                if (Directory.Exists(ba2folder)) Directory.Delete(ba2folder, true);
                MessageBox.Show("No .dds files were found in the archive.");
            }
        }

        #region ba2funcs

        private bool bSaveStringTable = true;
        private string sArchiveName = "";
        private bool bDirty;
        private Archive EditableArchive = new Archive();

        private void OpenArchive(string aArchiveName)
        {
            Debug.WriteLine(string.Format("Opened archive \"{0}\"", (object)aArchiveName));
            this.EditableArchive.LoadArchive(aArchiveName, false);
            this.SetFileName(aArchiveName);
            this.SetDirty(false);
            Debug.WriteLine(string.Format("Archive contains {0} files", (object)this.EditableArchive.Entries.Count));
        }

        private void SaveArchive()
        {
            try
            {
                this.EditableArchive.SaveArchive(this.sArchiveName, this.bSaveStringTable);
                this.SetDirty(false);
                Debug.WriteLine(string.Format("Saved archive \"{0}\"", (object)this.sArchiveName));
            }
            catch (IOException ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                    message = ex.InnerException.Message;
                Debug.WriteLine(string.Format("Failed to save archive\"{0}\": {1}", (object)this.sArchiveName, (object)message));
            }
        }
        private void AddFolderToArchive(string asFolderName)
        {
            bool flag = !Archive.HasDataInPath(asFolderName);
            DirectoryInfo directoryInfo = new DirectoryInfo(asFolderName);
            string str = asFolderName;
            if (directoryInfo.Parent != null)
                str = directoryInfo.Parent.FullName;
            foreach (FileSystemInfo enumerateFile in directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories))
                this.AddFileToArchive(enumerateFile.FullName, flag ? str : "");
        }

        private void AddFileToArchive(string aFilename, string aRelativeTo)
        {
            string aArchiveName = !string.IsNullOrEmpty(aRelativeTo) ? Archive.GetPathRelativeToOther(aFilename, aRelativeTo) : Archive.GetPathRelativeToData(aFilename);
            FileInfo fileInfo = new FileInfo(aFilename);
            if (!fileInfo.Exists)
                Debug.WriteLine(string.Format("Skipped non-existing file \"{0}\"", (object)aFilename));
            else if (fileInfo.Length == 0L)
            {
                Debug.WriteLine(string.Format("Skipped zero-length file \"{0}\"", (object)aFilename));
            }
            else
            {
                try
                {
                    this.EditableArchive.AddOrReplaceFile((FileEntry)new LooseFileEntry(aArchiveName, aFilename));
                    this.SetDirty(true);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("Skipped file \"{0}\" due to invalid format", ex.ToString()));
                }
            }
        }

        private void SetFileName(string asNewFilename)
        {
            sArchiveName = asNewFilename;
        }

        private void SetDirty(bool abDirty)
        {
            bDirty = abDirty;
        }
        #endregion

        //Browse folder or BA2 Archive button
        private void browsebutton_Click(object sender, EventArgs e)
        {
            if (!ba2)
            {
                folderBrowserDialog1.Description = "Choose textures folder";
                folderBrowserDialog1.SelectedPath = pathtextbox.Text; ;
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    pathtextbox.Text = folderBrowserDialog1.SelectedPath;
                    XDocument xdoc = new XDocument();
                    XElement settings = new XElement("settings");
                    XElement dir = new XElement("lastdir", pathtextbox.Text);
                    settings.Add(dir);
                    xdoc.Add(settings);
                    xdoc.Save("settings.xml");
                }
            }
            else
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Title = "Open Archive";
                openFileDialog.Filter = "BA2 Archive|*.ba2";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    pathtextbox.Text = openFileDialog.FileName;
                }
            }
        }

        //Switch between Loose files and BA2 archive mode
        private void BA2btn_Click(object sender, EventArgs e)
        {
            pathtextbox.Text = "";
            if (ba2)
            {
                ba2 = false;
                BA2btn.Text = "Loose";
            }
            else
            {
                ba2 = true;
                BA2btn.Text = "BA2";
            }
        }

        //Export log
        private void exportlogbutton_Click(object sender, EventArgs e)
        {
            StreamWriter stream = new StreamWriter("log.txt", true, Encoding.UTF8);
            foreach (string line in listBox1.Items)
            {
                stream.WriteLine(line);
            }
            stream.Close();
            MessageBox.Show("log.txt created in program directory");
        }

        //About menu
        private void aboutbutton_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.Show();
        }

        //Switch between Options menu and Log
        private void optionsbutton_Click(object sender, EventArgs e)
        {
            if (optionsbutton.Text.Contains("Options"))
            {
                optionsbutton.Text = "Log";
                listBox1.Visible = false;
            }
            else if (optionsbutton.Text.Contains("Log"))
            {
                optionsbutton.Text = "Options";
                listBox1.Visible = true;
            }
        }
        
        //Disable text editing of comboboxes
        private void compress_lvl_alpha_string_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void compress_lvl_alpha_string_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void texturerate_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void texturerate_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void texturesize_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void texturesize_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void maxtexturesize_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void maxtexturesize_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void mintexturesize_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void mintexturesize_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void mipsgen_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void mipsgen_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void compress_lvl_string_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void compress_lvl_string_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void threads_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        //Help Description tooltips for compression options
        private void force_compression_check_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(force_compression_check, "Textures will be compressed to given formats without checking if they have lower compression. With this BC1 can be compressed to BC7. Use with caution.");
        }
        private void ignore_face_check_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(ignore_face_check, "Face textures likes to stay in BC3 (from my experience) and don't wont to be touched. Otherwise they'll break in game and your character will have a black face.");
        }
        private void ignore_sng_maps_check_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(ignore_sng_maps_check, "This option will make sure these maps (_n, _g, _s) won't be compressed.");
        }
        private void ignore_diffuse_check_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(ignore_diffuse_check, "Diffuse (_d) textures are ignored.");
        }
        private void resize_check_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(resize_check, "Resizes textures down dividing by 2, if dimensions are unequal the biggest value used.");
        }
        private void backup_check_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(backup_check, "Backups are always fun. But you know whats more fun? Not creating them.");
        }
        private void threading_check_MouseHover(object sender, EventArgs e)
        {
            toolTip1.SetToolTip(threading_check, "Pump your compression speed by using more instances of texture converting tool. Don't use too much threads if your pc can't handle it.");
        }

        private void threads_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        //Links for peaople who want to Learn some stuff
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://en.wikipedia.org/wiki/S3_Texture_Compression");
        }
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://msdn.microsoft.com/en-us/library/windows/desktop/hh308955.aspx");
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetText(listBox1.SelectedItem.ToString());
        }
    }

    public class ddsfileinfo
    {
        public int width { get; set; }
        public int height { get; set; }
	public int mips { get; set; }
        public string format { get; set; }
        public bool alpha { get; set; }
    }

    //Case insensitive text.Contains right from stackoverflow. Just kidding, it's from the other site.
    public static class Extensions
    {
        public static bool CIContains(this string text, string value,
            StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }
    }
}