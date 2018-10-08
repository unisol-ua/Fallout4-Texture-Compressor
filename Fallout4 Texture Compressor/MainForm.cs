using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Fallout4_Texture_Compressor
{
    public partial class MainForm : Form
    {
        int compresslvl, compressalphalvl;
        double originalfilessize = 0, compressedfilessize = 0;
        string compresswithalpha, compressnoalpha, texturesize;
        bool ba2 = false;
        int currentthreads = 0;
        Queue<string> logqueue;

        public MainForm()
        {
            InitializeComponent();
        }

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

        private void startbutton_Click(object sender, EventArgs e)
        {
            if (pathtextbox.Text != "")
            {
                //set vars
                if (compressnoalpha_combo.Text.Contains("BC1")) compresslvl = 1;
                else if (compressnoalpha_combo.Text.Contains("BC3")) compresslvl = 3;
                else if (compressnoalpha_combo.Text.Contains("BC5")) compresslvl = 5;
                else if (compressnoalpha_combo.Text.Contains("BC7")) compresslvl = 7;

                if (compresswithalpha_combo.Text.Contains("BC1")) compressalphalvl = 1;
                else if (compresswithalpha_combo.Text.Contains("BC3")) compressalphalvl = 3;
                else if (compresswithalpha_combo.Text.Contains("BC5")) compressalphalvl = 5;
                else if (compresswithalpha_combo.Text.Contains("BC7")) compressalphalvl = 7;

                compresswithalpha = compresswithalpha_combo.Text;
                compressnoalpha = compressnoalpha_combo.Text;
                texturesize = texturesize_combo.Text;
                logqueue = new Queue<string>();

                originalfilessize = 0;
                compressedfilessize = 0;

                optionsbutton.Text = "Options";

                if (ba2)
                {
                    ba2compress();
                }
                else
                {
                    string path = pathtextbox.Text;
                    string[] allfiles = Directory.GetFiles(path, "*.dds", SearchOption.AllDirectories);
                    if (allfiles.Length > 0)
                    {
                        //visibility
                        listBox1.Visible = true;
                        listBox1.BringToFront();
                        optionsbutton.Visible = true;
                        exportlogbutton.Visible = true;
                        //
                        Stopwatch watch = new Stopwatch();
                        watch.Start();

                        MainForm form = this;
                        listBox1.Items.Clear();
                        double i = 0;
                        if (backup_check.Checked == true) // backup
                        {
                            double sni = 0;
                            foreach (string file in allfiles)
                            {
                                i++;
                                form.Text = "Copying files: " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                                FileInfo fileinf = new FileInfo(file);
                                if ((ignoresn_check.Checked) && (fileinf.Name.Contains("_s") || fileinf.Name.Contains("_n") || fileinf.Name.Contains("_g.dds") || fileinf.Name.Contains("_S") || fileinf.Name.Contains("_N") || fileinf.Name.Contains("_G.dds")))
                                {
                                    sni++;
                                }
                                else
                                {
                                    string newfolders = fileinf.DirectoryName.Replace(pathtextbox.Text, "");
                                    Directory.CreateDirectory(Application.StartupPath + "\\backup" + newfolders);
                                    File.Copy(file, Application.StartupPath + "\\backup" + fileinf.FullName.Replace(pathtextbox.Text, ""), true);
                                }
                            }
                            form.Text = "Archiving files";
                            string time = DateTime.Now.Second + "s_" + DateTime.Now.Minute + "m_" + DateTime.Now.Hour + "h_" + DateTime.Now.Day + "d_" + DateTime.Now.Month + "m_" + DateTime.Now.Year + "y";
                            Process process = new Process();
                            ProcessStartInfo startInfo = new ProcessStartInfo();
                            startInfo.CreateNoWindow = true;
                            startInfo.UseShellExecute = false;
                            startInfo.RedirectStandardOutput = true;
                            startInfo.RedirectStandardError = true;
                            startInfo.FileName = Application.StartupPath + "\\bin\\7za.exe";
                            if (backupname.Text == "") { startInfo.Arguments = "a backup_" + time + ".zip \"" + Application.StartupPath + "\\backup\\*\" -mx6 -o" + Application.StartupPath + "\\"; }
                            else { startInfo.Arguments = "a \"" + backupname.Text + "_" + time + ".zip\" \"" + Application.StartupPath + "\\backup\\*\" -mx6 -o" + Application.StartupPath + "\\"; }
                            process.StartInfo = startInfo;
                            process.Start();
                            process.WaitForExit();
                            Directory.Delete(Application.StartupPath + "\\backup", true);
                            listBox1.Items.Add("Backup: " + Application.StartupPath + "\\" + backupname.Text + "_" + time + ".zip");
                            listBox1.Items.Add("Archived " + (i - sni) + " of " + i + " files");
                            if (ignoresn_check.Checked) listBox1.Items.Add(sni + " files were ignored");
                            listBox1.Items.Add("");
                        }

                        //////////
                        listBox1.Items.Add("Compression:");
                        i = 0;
                        if (threading_check.Checked)
                        {
                            int maxthreads = int.Parse(threads_combo.Text);
                            if (allfiles.Length < maxthreads) maxthreads = allfiles.Length;
                            int files = 0;
                            while(files < allfiles.Length || currentthreads > 0)
                            {
                                if(currentthreads < maxthreads && files < allfiles.Length)
                                {
                                    startcompressthread(allfiles[files], ignoresn_check.Checked, ignoreface_check.Checked, ignorediffuse_check.Checked, files+1);
                                    currentthreads++;
                                    files++;
                                    i++;
                                }
                                if (logqueue.Count > 0) if(logqueue.Peek() != null) listBox1.Items.Add(logqueue.Dequeue());
                                form.Text = "Compressing files: " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%" + " Threads: " + currentthreads;
                                listBox1.TopIndex = listBox1.Items.Count - 1;
                                Thread.Sleep(10);
                            }
                            if (logqueue.Count > 0)
                            {
                                string[] fixqueue = logqueue.ToArray();
                                logqueue.Clear();
                                foreach (string entry in fixqueue) listBox1.Items.Add(entry);
                            }
                            listBox1.TopIndex = listBox1.Items.Count - 1;
                        }
                        else
                        {
                            foreach (string file in allfiles)
                            {
                                i++;
                                compressmaster(file, ignoresn_check.Checked, ignoreface_check.Checked, ignorediffuse_check.Checked, (int)i, false);
                                form.Text = "Compressing files: " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                                listBox1.TopIndex = listBox1.Items.Count - 1;
                            }
                        }
                        if (Directory.Exists(Application.StartupPath + "\\temp")) Directory.Delete(Application.StartupPath + "\\temp", true);
                        form.Text = "Compressed from " + Math.Round(originalfilessize / 1024, 3) + "mb to " + Math.Round(compressedfilessize / 1024, 3) + "mb Saved = " + Math.Round((originalfilessize - compressedfilessize) / 1024, 3) + "mb";
                        listBox1.Items.Add("");
                        listBox1.Items.Add("Original files size = " + Math.Round(originalfilessize / 1024, 3) + " mb");
                        listBox1.Items.Add("Compressed files size = " + Math.Round(compressedfilessize / 1024, 3) + " mb");
                        listBox1.Items.Add("Saved = " + Math.Round((originalfilessize - compressedfilessize) / 1024, 3) + " mb");
                        watch.Stop();
                        TimeSpan ts = watch.Elapsed;
                        string elapsedTime = String.Format("{0:00}h {1:00}m {2:00}s {3:00}ms", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
                        listBox1.Items.Add("Elapsed time = " + elapsedTime);
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }
                    else
                    {
                        MessageBox.Show("No .dds files found.");
                    }
                }
                GC.Collect();
            }
            else
            {
                MessageBox.Show("You need to set destination folder or BA2 archive");
            }
        }

        private Thread startcompressthread(string file, bool ignoresn, bool ignoreface, bool ignorediffuse, int num)
        {
            var t = new Thread(() => compressmaster(file, ignoresn, ignoreface, ignorediffuse, num, true));
            t.Start();
            Debug.WriteLine("Thread " + num + " started");
            return t;
        }

        private void compressmaster(string file, bool ignoresn, bool ignoreface, bool ignorediffuse, int num, bool isthreaded)
        {
            List<string> log = new List<string>();
            FileInfo fileinf = new FileInfo(file);
            log.Add(num + ": " + file);
            if (ignoresn && (fileinf.Name.Contains("_s.dds") || fileinf.Name.Contains("_n.dds") || fileinf.Name.Contains("_g.dds") || fileinf.Name.Contains("_S.dds") || fileinf.Name.Contains("_N.dds") || fileinf.Name.Contains("_G.dds")))
            {
                log.Add("Specular, normal and glowmaps are ignored, skipping");
            }
            else if (ignoreface && (fileinf.Name.Contains("femalehead") || fileinf.Name.Contains("malehead")))
            {
                log.Add("Face textures are ignored, skipping");
            }
            else if (ignorediffuse && !(fileinf.Name.Contains("_s.dds") || fileinf.Name.Contains("_n.dds") || fileinf.Name.Contains("_g.dds") || fileinf.Name.Contains("_S.dds") || fileinf.Name.Contains("_N.dds") || fileinf.Name.Contains("_G.dds")))
            {
                log.Add("Diffuse textures are ignored, skipping");
            }
            else
            {
                ddsfileinfo ddsinfo = checkdds(file);
                log.Add("height = " + ddsinfo.height);
                log.Add("width  = " + ddsinfo.width);
                log.Add("format = " + ddsinfo.format);
                log.Add("alpha = " + ddsinfo.alpha.ToString());
                if (ddsinfo.format.Contains("Unsupported format"))
                {
                    log.Add("Unsupported format, skipping");
                }
                else
                {
                    double filesize = Math.Round((Double)new FileInfo(file).Length / 1024, 1);
                    originalfilessize += filesize;
                    log.Add("file size = " + filesize + " kb");

                    //compress
                    if (compress_check.Checked)
                    {
                        string compressed = compress(ddsinfo, file, fileinf);
                        if (!compressed.Contains("Already compressed")) log.Add("new format = " + compressed);
                    }
                    //resize
                    if (resize_check.Checked)
                    {
                        bool resized = resize(ddsinfo, file, fileinf);
                        if (resized)
                        {
                            log.Add("new width  = " + (ddsinfo.width / 2));
                            log.Add("new height = " + (ddsinfo.height / 2));
                        }
                    }
                    //
                    double newfilesize = Math.Round((Double)new FileInfo(file).Length / 1024, 1);
                    compressedfilessize += newfilesize;
                    if (newfilesize < filesize)
                    {
                        log.Add("new file size = " + newfilesize + " kb");
                    }
                }
            }
            if (!isthreaded) foreach (string entry in log) listBox1.Items.Add(entry);
            else
            {
                foreach (string entry in log) logqueue.Enqueue(entry);
                if (isthreaded) currentthreads--;
            }
        }

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

        private string compress(ddsfileinfo ddsinfo, string file, FileInfo fileinf)
        {
            string newformat = "Already compressed";
            int ddslvl = 10;
            if (ddsinfo.format.Contains("BC1")) ddslvl = 1;
            else if (ddsinfo.format.Contains("BC2")) ddslvl = 3;
            else if (ddsinfo.format.Contains("BC3")) ddslvl = 3;
            else if (ddsinfo.format.Contains("BC4")) ddslvl = 5;
            else if (ddsinfo.format.Contains("BC5")) ddslvl = 5;
            else if (ddsinfo.format.Contains("BC7")) ddslvl = 7;
            if (ddslvl == 1)
            {
                return newformat;
            }
            if(ddsinfo.alpha == true)
            {
                if (ddslvl > compressalphalvl)
                {
                    if (ddsinfo.format.Contains("SRGB"))
                    {
                        if (compresswithalpha == "BC5")
                        {
                            texconv("\"" + file + "\" -y -f BC3_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                            newformat = "BC3_UNORM_SRGB";
                        }
                        else
                        {
                            texconv("\"" + file + "\" -y -f " + compresswithalpha + "_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                            newformat = compresswithalpha + "_UNORM_SRGB";
                        }
                    }
                    else
                    {
                        texconv("\"" + file + "\" -y -f " + compresswithalpha + "_UNORM -o \"" + fileinf.DirectoryName + "\"");
                        newformat = compresswithalpha + "_UNORM";
                    }
                }
            }
            else
            {
                if (ddslvl > compresslvl)
                {
                    if (ddsinfo.format.Contains("SRGB"))
                    {
                        if (compressnoalpha == "BC5")
                        {
                            texconv("\"" + file + "\" -y -f BC3_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                            newformat = "BC3_UNORM_SRGB";
                        }
                        else
                        {
                            texconv("\"" + file + "\" -y -f " + compressnoalpha + "_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                            newformat = compressnoalpha + "_UNORM_SRGB";
                        }
                    }
                    else
                    {
                        texconv("\"" + file + "\" -y -f " + compressnoalpha + "_UNORM -o \"" + fileinf.DirectoryName + "\"");
                        newformat = compressnoalpha + "_UNORM";
                    }
                }
            }
            return newformat;
        }

        private bool resize(ddsfileinfo ddsinfo, string file, FileInfo fileinf)
        {
            int size = 0;
            if (ddsinfo.height > ddsinfo.width) { size = ddsinfo.height; } else { size = ddsinfo.width; }
            int ifgreater = 8192;
            if (texturesize.Contains("256")) { ifgreater = 256; }
            else if (texturesize.Contains("512")) { ifgreater = 512; }
            else if (texturesize.Contains("1024")) { ifgreater = 1024; }
            else if (texturesize.Contains("2048")) { ifgreater = 2048; }
            else if (texturesize.Contains("4096")) { ifgreater = 4096; }
            else if (texturesize.Contains("All")) { ifgreater = 4; }
            if (size > ifgreater)
            {
                texconv("\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                //gen mipmaps
                texconv("\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\"" + " -m 0");
                return true;
            }
            else return false;
        }

        private ddsfileinfo checkdds(string file)
        {
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
            ddsfileinfo ddsinfo = new ddsfileinfo();
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                if (line.Contains("height"))
                {
                    //workaround for rare int.parse errors
                    int temp;
                    if (int.TryParse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2), out temp))
                    {
                        ddsinfo.height = temp;
                    }
                    else
                    {
                        try
                        {
                            ddsinfo.height = int.Parse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2), CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Couldn't parse dds height. Save current message and next ones for providing more info about error. Error log: " + ex.ToString());
                            MessageBox.Show("Height line unedited: " + line);
                            MessageBox.Show("Height line edited: " + line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2));
                        }
                    }
                }
                if (line.Contains("width"))
                {
                    //workaround for rare int.parse errors
                    int temp;
                    if (int.TryParse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2), out temp))
                    {
                        ddsinfo.width = temp;
                    }
                    else
                    {
                        try
                        {
                            ddsinfo.width = int.Parse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2), CultureInfo.InvariantCulture);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Couldn't parse dds width. Save current message and next ones for providing more info about error. Error log: " + ex.ToString());
                            MessageBox.Show("Width line unedited: " + line);
                            MessageBox.Show("Width line edited: " + line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2));
                        }
                    }
                }
                if (line.Contains("format"))
                {
                    ddsinfo.format = line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2);
                }
                if (line.Contains("FAILED"))
                {
                    ddsinfo.format = "Unsupported format";
                    ddsinfo.width = 0;
                    ddsinfo.height = 0;
                }
            }
            //process.WaitForExit();
            //
            ddsinfo.alpha = false;
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
            startInfo.Arguments = "analyze \"" + Application.StartupPath + "\\temp\\" + fileinf.Name + "\"";
            process.StartInfo = startInfo;
            process.Start();
            bool finished6 = false;
            bool finished8 = false;
            int blocks6 = 0;
            int blocks8 = 0;
            while (!process.StandardOutput.EndOfStream && !(finished6 && finished8))
            {
                string line = process.StandardOutput.ReadLine();
                if (line.Contains("8 alpha blocks") && !finished8)
                {
                    finished8 = true;
                    //workaround for possible int.parse errors
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
                            MessageBox.Show("Couldn't parse dds alpha8 channel info. Save current message and next ones for providing more info about error. Error log: " + ex.ToString());
                            MessageBox.Show("Unedited line: " + line);
                            MessageBox.Show("Edited line: " + line.Replace("8 alpha blocks - ", ""));
                        }
                    }
                }
                if (line.Contains("6 alpha blocks") && !finished6)
                {
                    finished6 = true;
                    //workaround for possible int.parse errors
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
                            MessageBox.Show("Couldn't parse dds alpha6 channel info. Save current message and next ones for providing more info about error. Error log: " + ex.ToString());
                            MessageBox.Show("Unedited line: " + line);
                            MessageBox.Show("Edited line: " + line.Replace("8 alpha blocks - ", ""));
                        }
                    }
                }
                //process.WaitForExit();
                if (blocks6 > 0 && blocks8 > 0) ddsinfo.alpha = true;
                if (File.Exists(Application.StartupPath + "\\temp\\" + fileinf.Name + "\"")) File.Delete(Application.StartupPath + "\\temp\\" + fileinf.Name + "\"");
            }
            return ddsinfo;
        }

        private void ba2compress()
        {
            FileInfo archivefile = new FileInfo(pathtextbox.Text);
            double archivesize = Math.Round((Double)archivefile.Length / 1024 / 1024, 3);
            //open
            OpenArchive(pathtextbox.Text);

            //extract
            string ba2folder = Application.StartupPath + "\\ba2temp";
            if (!Directory.Exists(ba2folder)) Directory.CreateDirectory(ba2folder);
            this.Text = "Extracting files from archive...";
            foreach (FileEntry entry in EditableArchive.Entries)
            {
                entry.ExtractTo(ba2folder);
            }

            //compress
            string[] allfiles = Directory.GetFiles(ba2folder, "*.dds", SearchOption.AllDirectories);
            if (allfiles.Length > 0)
            {
                //visibility
                listBox1.Visible = true;
                listBox1.BringToFront();
                optionsbutton.Visible = true;
                exportlogbutton.Visible = true;
                //
                Stopwatch watch = new Stopwatch();
                watch.Start();

                MainForm form = this;
                listBox1.Items.Clear();
                if (backup_check.Checked == true) // backup
                {
                    form.Text = "Copying BA2 Acthive";
                    File.Copy(pathtextbox.Text, Application.StartupPath + "\\" + archivefile.Name, true);
                    listBox1.Items.Add("Backup: " + Application.StartupPath + "\\" + archivefile.Name);
                    listBox1.Items.Add("");
                }

                listBox1.Items.Add("Compression:");
                int i = 0;
                if (threading_check.Checked)
                {
                    int maxthreads = int.Parse(threads_combo.Text);
                    if (allfiles.Length < maxthreads) maxthreads = allfiles.Length;
                    int files = 0;
                    while (files < allfiles.Length || currentthreads > 0)
                    {
                        if (currentthreads < maxthreads && files < allfiles.Length)
                        {
                            startcompressthread(allfiles[files], ignoresn_check.Checked, ignoreface_check.Checked, ignorediffuse_check.Checked, files + 1);
                            currentthreads++;
                            files++;
                            i++;
                        }
                        if (logqueue.Count > 0) if (logqueue.Peek() != null) listBox1.Items.Add(logqueue.Dequeue());
                        form.Text = "Compressing files: " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%" + " Threads: " + currentthreads;
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                        Thread.Sleep(10);
                    }
                    if (logqueue.Count > 0)
                    {
                        string[] fixqueue = logqueue.ToArray();
                        logqueue.Clear();
                        foreach (string entry in fixqueue) listBox1.Items.Add(entry);
                    }
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
                else
                {
                    foreach (string file in allfiles)
                    {
                        i++;
                        compressmaster(file, ignoresn_check.Checked, ignoreface_check.Checked, ignorediffuse_check.Checked, (int)i, false);
                        form.Text = "Compressing files: " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                        listBox1.TopIndex = listBox1.Items.Count - 1;
                    }
                }
                if (Directory.Exists(Application.StartupPath + "\\temp")) Directory.Delete(Application.StartupPath + "\\temp", true);

                //
                //new archive
                this.EditableArchive.NewArchive();
                this.SetDirty(false);
                //addfolders
                if (originalfilessize > compressedfilessize)
                {
                    foreach (string folder in Directory.GetDirectories(ba2folder))
                    {
                        AddFolderToArchive(folder);
                    }
                    //save
                    SaveArchive();

                    double newarchivesize = Math.Round((Double)new FileInfo(pathtextbox.Text).Length / 1024 / 1024, 3);
                    listBox1.Items.Add("");
                    listBox1.Items.Add("Original Archive size = " + archivesize + " mb");
                    listBox1.Items.Add("Compressed Archive size = " + newarchivesize + " mb");
                    listBox1.Items.Add("Saved = " + (archivesize - newarchivesize) + " mb");
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
                //delete temp files
                if(Directory.Exists(ba2folder)) Directory.Delete(ba2folder, true);
            }
            else
            {
                if (Directory.Exists(ba2folder)) Directory.Delete(ba2folder, true);
                MessageBox.Show("No .dds files found in archive.");
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

        private void aboutbutton_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.Show();
        }

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
        //disable text edit of comboboxes
        private void compressother_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }
        private void compressother_combo_KeyPress(object sender, KeyPressEventArgs e)
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

        private void compressnoalpha_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void compressnoalpha_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void threads_combo_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void threads_combo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

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
        public string format { get; set; }
        public bool alpha { get; set; }
    }
}