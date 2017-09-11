using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Fallout4_Texture_Compressor
{
    public partial class MainForm : Form
    {
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
        }

        private void startbutton_Click(object sender, EventArgs e)
        {
            if (pathtextbox.Text != "")
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
                    //main
                    double filessize = 0;
                    double newfilessize = 0;
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
                            if ((ignoresn_check.Checked) && (fileinf.Name.Contains("_s") || fileinf.Name.Contains("_n")))
                            {
                                sni++;
                            }
                            string newfolders = fileinf.DirectoryName.Replace(pathtextbox.Text, "");
                            Directory.CreateDirectory(Application.StartupPath + "\\backup" + newfolders);
                            File.Copy(file, Application.StartupPath + "\\backup" + fileinf.FullName.Replace(pathtextbox.Text, ""), true);
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
                        listBox1.Items.Add("Backup:");
                        listBox1.Items.Add("Archived " + (i - sni) + " of " + i + " files");
                        if (ignoresn_check.Checked) listBox1.Items.Add(sni + " files were ignored");
                        listBox1.Items.Add("");
                    }
                    //compressing/resizing
                    listBox1.Items.Add("Compression:");
                    i = 0;
                    foreach (string file in allfiles)
                    {
                        FileInfo fileinf = new FileInfo(file);
                        listBox1.Items.Add(i + " : " + file);
                        if (ignoresn_check.Checked && (fileinf.Name.Contains("_s.dds") || fileinf.Name.Contains("_n.dds") || fileinf.Name.Contains("_S.dds") || fileinf.Name.Contains("_N.dds")))
                        {
                            listBox1.Items.Add("Speculars and normals are ignored, skipping");
                            i++;
                            form.Text = "Compressing files : " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                        }
                        else
                        {
                            ddsfileinfo ddsinfo = checkdds(file);
                            if (ddsinfo.format.Contains("Unsupported format"))
                            {
                                i++;
                                form.Text = "Compressing files : " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                            }
                            else
                            {
                                i++;
                                double filesize = Math.Round((Double)new FileInfo(file).Length / 1024, 1);
                                filessize += filesize;
                                listBox1.Items.Add("file size = " + filesize + " kb");
                                int size = 0;
                                if (ddsinfo.height > ddsinfo.width) { size = ddsinfo.height; } else { size = ddsinfo.width; }
                                if (compressbc_check.Checked == true && resize_check.Checked == true) // compress + resize
                                {
                                    int ifgreater = 8192;
                                    if (texturesize_combo.Text.Contains("512")) { ifgreater = 512; }
                                    else if (texturesize_combo.Text.Contains("1024")) { ifgreater = 1024; }
                                    else if (texturesize_combo.Text.Contains("2048")) { ifgreater = 2048; }
                                    else if (texturesize_combo.Text.Contains("4096")) { ifgreater = 4096; }
                                    else if (texturesize_combo.Text.Contains("128")) { ifgreater = 128; }
                                    else if (texturesize_combo.Text.Contains("256")) { ifgreater = 256; }
                                    else if (texturesize_combo.Text.Contains("8192")) { ifgreater = 8192; }
                                    else if (texturesize_combo.Text.Contains("4")) { ifgreater = 4; }
                                    else if (texturesize_combo.Text.Contains("8")) { ifgreater = 8; }
                                    else if (texturesize_combo.Text.Contains("16")) { ifgreater = 16; }
                                    else if (texturesize_combo.Text.Contains("32")) { ifgreater = 32; }
                                    else if (texturesize_combo.Text.Contains("64")) { ifgreater = 64; }
                                    else
                                    {
                                        MessageBox.Show("Avoid editing resize combobox's text. Use only numbers for custom parameter (without if<> and any words). Resize parameter has been set to custom number.");
                                        try { ifgreater = int.Parse(texturesize_combo.Text); }
                                        catch { MessageBox.Show("Failed parse custom parameter. Parameter has been reset to 8192"); ifgreater = 8192; }
                                    }
                                    bool needtogenmm = false;
                                    if (size > ifgreater)//if size is bigger than compared value
                                    {
                                        form.Text = "Compressing and resizing files : " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                                        if (!ddsinfo.format.Contains("BC1"))//skip if already bc1 (only resize)
                                        {
                                            if (ddsinfo.format.Contains("BC"))//check if bc format and compress to bc1
                                            {
                                                if (safecompress_check.Checked && ddsinfo.alpha)//safe compression and alpha
                                                {
                                                    if (ddsinfo.format.Contains("BC2") || ddsinfo.format.Contains("BC3"))//skip on bc2 or bc3
                                                    {
                                                        listBox1.Items.Add("Safe compress checked, texture has alpha channel, skipping");
                                                    }
                                                    else
                                                    {
                                                        if (ddsinfo.format.Contains("SRGB"))
                                                        {
                                                            texconv("\"" + file + "\" -y -f BC3_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                                            listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                                            listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                                            listBox1.Items.Add("new format = BC3_UNORM_SRGB");
                                                        }
                                                        else
                                                        {
                                                            texconv("\"" + file + "\" -y -f BC3_UNORM -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                                            listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                                            listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                                            listBox1.Items.Add("new format = BC3_UNORM");
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (ddsinfo.format.Contains("SRGB"))
                                                    {
                                                        texconv("\"" + file + "\" -y -f BC1_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                                        listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                                        listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                                        listBox1.Items.Add("new format = BC1_UNORM_SRGB");
                                                    }
                                                    else
                                                    {
                                                        texconv("\"" + file + "\" -y -f BC1_UNORM -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                                        listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                                        listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                                        listBox1.Items.Add("new format = BC1_UNORM");
                                                    }
                                                }
                                            }
                                            else if (compressother_check.Checked == true)//if other format and compress checked, compress to selected bc
                                            {
                                                if (ddsinfo.format.Contains("SRGB"))
                                                {
                                                    texconv("\"" + file + "\" -y -f " + compressother_combo.Text + "_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                                    listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                                    listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                                    listBox1.Items.Add("new format = " + compressother_combo.Text + "_UNORM_SRGB");
                                                }
                                                else
                                                {
                                                    texconv("\"" + file + "\" -y -f " + compressother_combo.Text + "_UNORM -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                                    listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                                    listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                                    listBox1.Items.Add("new format = " + compressother_combo.Text + "_UNORM");
                                                }
                                            }
                                            else
                                            {
                                                listBox1.Items.Add("Other format compress is unchecked, skipping");
                                            }
                                        }
                                        else//no need to compress
                                        {
                                            texconv("\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                            listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                            listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                        }
                                        needtogenmm = true;
                                    }
                                    else//no need to resize
                                    {
                                        form.Text = "Compressing files : " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                                        //compress func
                                        compress(ddsinfo, file, fileinf);
                                    }
                                    if (needtogenmm)
                                    {
                                        //gen mipmaps
                                        texconv("\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 0");
                                    }
                                }
                                else if (compressbc_check.Checked == true) //compress
                                {
                                    form.Text = "Compressing files : " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                                    //compress func
                                    compress(ddsinfo, file, fileinf);
                                }
                                else if (resize_check.Checked == true) //resize
                                {
                                    form.Text = "Resizing files : " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                                    int ifgreater = 8192;
                                    if (texturesize_combo.Text.Contains("512")) { ifgreater = 512; }
                                    else if (texturesize_combo.Text.Contains("1024")) { ifgreater = 1024; }
                                    else if (texturesize_combo.Text.Contains("2048")) { ifgreater = 2048; }
                                    else if (texturesize_combo.Text.Contains("4096")) { ifgreater = 4096; }
                                    else if (texturesize_combo.Text.Contains("128")) { ifgreater = 128; }
                                    else if (texturesize_combo.Text.Contains("256")) { ifgreater = 256; }
                                    else if (texturesize_combo.Text.Contains("8192")) { ifgreater = 8192; }
                                    else if (texturesize_combo.Text.Contains("4")) { ifgreater = 4; }
                                    else if (texturesize_combo.Text.Contains("8")) { ifgreater = 8; }
                                    else if (texturesize_combo.Text.Contains("16")) { ifgreater = 16; }
                                    else if (texturesize_combo.Text.Contains("32")) { ifgreater = 32; }
                                    else if (texturesize_combo.Text.Contains("64")) { ifgreater = 64; }
                                    else
                                    {
                                        MessageBox.Show("Avoid editing resize combobox. Use only numbers for custom paramter (without if<> and any words). Resize parameter has been set to custom number.");
                                        try { ifgreater = int.Parse(texturesize_combo.Text); }
                                        catch { MessageBox.Show("Failed parse custom parameter. Parameter has been reset to 8192"); ifgreater = 8192; }
                                    }
                                    if (size > ifgreater)
                                    {
                                        texconv("\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                        //gen mipmaps
                                        texconv("\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 0");
                                        listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                        listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                    }
                                }
                                filesize = Math.Round((Double)new FileInfo(file).Length / 1024, 1);
                                newfilessize += filesize;
                                if (!listBox1.Items[listBox1.Items.Count - 1].ToString().Contains("Already compressed") && !listBox1.Items[listBox1.Items.Count - 1].ToString().Contains("Other format compress is unchecked, skipping") && !listBox1.Items[listBox1.Items.Count - 1].ToString().Contains("Safe compress checked, texture has alpha channel, skipping"))
                                {
                                    listBox1.Items.Add("new file size = " + filesize + " kb");
                                }
                                //keep listbox scrolled to bottom
                                listBox1.TopIndex = listBox1.Items.Count - 1;
                            }
                        }
                    }
                    if (Directory.Exists(Application.StartupPath + "\\temp")) Directory.Delete(Application.StartupPath + "\\temp", true);
                    form.Text = "Compressed from " + Math.Round(filessize / 1024, 3) + "mb to " + Math.Round(newfilessize / 1024, 3) + "mb Saved = " + Math.Round((filessize - newfilessize) / 1024, 3) + "mb";
                    listBox1.Items.Add("");
                    listBox1.Items.Add("Original files size = " + Math.Round(filessize / 1024, 3) + " mb");
                    listBox1.Items.Add("Compressed files size = " + Math.Round(newfilessize / 1024, 3) + " mb");
                    listBox1.Items.Add("Saved = " + Math.Round((filessize - newfilessize) / 1024, 3) + " mb");
                }
                else
                {
                    MessageBox.Show("No .dds files found.");
                }
            }
            else
            {
                MessageBox.Show("You need to set destination folder");
            }
        }

        private void texconv(string arguments)
        {
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = Application.StartupPath + "\\bin\\texconv.exe";
            startInfo.Arguments = arguments;//args
            process.StartInfo = startInfo;
            process.Start();
            process.WaitForExit();
        }

        private void compress(ddsfileinfo ddsinfo, string file, FileInfo fileinf)
        {
            if (!ddsinfo.format.Contains("BC1"))//skip if already bc1
            {
                if (ddsinfo.format.Contains("BC"))//check if bc format and compress to bc1
                {
                    if (safecompress_check.Checked && ddsinfo.alpha)//safe compression and alpha
                    {
                        if (ddsinfo.format.Contains("BC2") || ddsinfo.format.Contains("BC3"))//skip on bc2 or bc3
                        {
                            listBox1.Items.Add("Safe compress checked, texture has alpha channel, skipping");
                        }
                        else
                        {
                            string[] copyfile = Directory.GetFiles(Application.StartupPath + "\\temp", fileinf.Name, SearchOption.AllDirectories);
                            if (copyfile.Length == 1)
                            {
                                File.Copy(copyfile[0], fileinf.FullName, true);
                                if (ddsinfo.format.Contains("SRGB"))
                                {
                                    listBox1.Items.Add("new format = BC3_UNORM_SRGB");
                                }
                                else
                                {
                                    listBox1.Items.Add("new format = BC3_UNORM");
                                }
                            }
                            else
                            {
                                if (ddsinfo.format.Contains("SRGB"))
                                {
                                    texconv("\"" + file + "\" -y -f BC3_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                                    listBox1.Items.Add("new format = BC3_UNORM_SRGB");
                                }
                                else
                                {
                                    texconv("\"" + file + "\" -y -f BC3_UNORM -o \"" + fileinf.DirectoryName + "\"");
                                    listBox1.Items.Add("new format = BC3_UNORM");
                                }
                            }
                        }
                    }
                    else//no alpha or safe compression
                    {
                        if (ddsinfo.format.Contains("SRGB"))
                        {
                            texconv("\"" + file + "\" -y -f BC1_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                            listBox1.Items.Add("new format = BC1_UNORM_SRGB");
                        }
                        else
                        {
                            texconv("\"" + file + "\" -y -f BC1_UNORM -o \"" + fileinf.DirectoryName + "\"");
                            listBox1.Items.Add("new format = BC1_UNORM");
                        }
                    }
                }
                else if (compressother_check.Checked == true)//if other format and compress checked, compress to selected bc
                {
                    if (ddsinfo.format.Contains("SRGB"))
                    {
                        texconv("\"" + file + "\" -y -f " + compressother_combo.Text + "_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                        listBox1.Items.Add("new format = " + compressother_combo.Text + "_UNORM_SRGB");
                    }
                    else
                    {
                        texconv("\"" + file + "\" -y -f " + compressother_combo.Text + "_UNORM -o \"" + fileinf.DirectoryName + "\"");
                        listBox1.Items.Add("new format = " + compressother_combo.Text + "_UNORM");
                    }
                }
                else
                {
                    listBox1.Items.Add("Other format compress is unchecked, skipping");
                }
            }
            else
            {
                listBox1.Items.Add("Already compressed");
            }
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
            process.WaitForExit();
            listBox1.Items.Add("height = " + ddsinfo.height);
            listBox1.Items.Add(" width = " + ddsinfo.width);
            listBox1.Items.Add("format = " + ddsinfo.format);
            ddsinfo.alpha = false;//initial assumption
            //safe mode
            if (safecompress_check.Checked == true && (ddsinfo.format.Contains("BC2") || ddsinfo.format.Contains("BC3") || ddsinfo.format.Contains("BC4") || ddsinfo.format.Contains("BC5") || ddsinfo.format.Contains("BC7")))
            {
                Directory.CreateDirectory(Application.StartupPath + "\\temp");
                FileInfo fileinf = new FileInfo(file);
                if (ddsinfo.format.Contains("BC3"))
                {
                    File.Copy(file, Application.StartupPath + "\\temp\\" + fileinf.Name);
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
                }
                process.WaitForExit();
                if (blocks6 > 0 && blocks8 > 0) ddsinfo.alpha = true;
                //Directory.Delete(Application.StartupPath + "\\temp", true);
            }
            listBox1.Items.Add("alpha = " + ddsinfo.alpha.ToString());
            return ddsinfo;
        }

        private void browsebutton_Click(object sender, EventArgs e)
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
    }

    public class ddsfileinfo
    {
        public int width { get; set; }
        public int height { get; set; }
        public string format { get; set; }
        public bool alpha { get; set; }
    }
}