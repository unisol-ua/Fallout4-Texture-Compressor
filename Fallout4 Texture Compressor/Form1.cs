using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace Fallout4_Texture_Compressor
{
    public partial class Form1 : Form
    {
        public Form1()
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
                        textBox1.Text = xnode.InnerText;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double filessize = 0;
            double newfilessize = 0;
            Form1 form = this;
            listBox1.Items.Clear();
            string path = textBox1.Text;
            double i = 0;
            string[] allfiles = Directory.GetFiles(path, "*.dds", SearchOption.AllDirectories);
            if (checkBox3.Checked == true) // backup
            {
                foreach (string file in allfiles)
                {
                    i++;
                    form.Text = "Copying files: " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                    FileInfo fileinf = new FileInfo(file);
                    string newfolders = fileinf.DirectoryName.Replace(textBox1.Text, "");
                    Directory.CreateDirectory(Application.StartupPath + "\\backup" + newfolders);
                    File.Copy(file, Application.StartupPath + "\\backup" + fileinf.FullName.Replace(textBox1.Text, ""), true);
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
                if(textBox2.Text == "") { startInfo.Arguments = "a backup_" + time + ".zip \"" + Application.StartupPath + "\\backup\\*\" -mx6 -o" + Application.StartupPath + "\\"; }
                else { startInfo.Arguments = "a " + textBox2.Text + "_" + time + ".zip \"" + Application.StartupPath + "\\backup\\*\" -mx6 -o" + Application.StartupPath + "\\"; }
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
                Directory.Delete(Application.StartupPath + "\\backup", true);
            }
            i = 0;
            foreach (string file in allfiles)
            {
                FileInfo fileinf = new FileInfo(file);
                listBox1.Items.Add(file);
                fileinfo ddsinfo = checkdds(file);
                double filesize = Math.Round((Double)new FileInfo(file).Length / 1024, 1);
                filessize += filesize;
                listBox1.Items.Add("file size = " + filesize + " kb");
                int size = 0;
                if (ddsinfo.height > ddsinfo.width) { size = ddsinfo.height; }
                else { size = ddsinfo.width; }
                //checkBox1 - compress
                //checkBox2 - resize
                if (checkBox1.Checked == true && checkBox2.Checked == true) // compress + resize
                {
                    i++;
                    form.Text = "Compressing and resizing files : " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                    int ifgreater = 8192;
                    if (comboBox1.Text.Contains("512")) { ifgreater = 512; }
                    else if (comboBox1.Text.Contains("1024")) { ifgreater = 1024; }
                    else if (comboBox1.Text.Contains("2048")) { ifgreater = 2048; }
                    else if (comboBox1.Text.Contains("4096")) { ifgreater = 4096; }
                    else if (comboBox1.Text.Contains("128")) { ifgreater = 128; }
                    else if (comboBox1.Text.Contains("256")) { ifgreater = 256; }
                    else if (comboBox1.Text.Contains("8192")) { ifgreater = 8192; }
                    else if (comboBox1.Text.Contains("4")) { ifgreater = 4; }
                    else if (comboBox1.Text.Contains("8")) { ifgreater = 8; }
                    else if (comboBox1.Text.Contains("16")) { ifgreater = 16; }
                    else if (comboBox1.Text.Contains("32")) { ifgreater = 32; }
                    else if (comboBox1.Text.Contains("64")) { ifgreater = 64; }
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = true;
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = Application.StartupPath + "\\bin\\texconv.exe";
                    bool needtogenmm = false;
                    if (size > ifgreater)
                    {
                        if (!ddsinfo.format.Contains("BC1"))
                        {
                            if (ddsinfo.format.Contains("SRGB"))
                            {
                                startInfo.Arguments = "\"" + file + "\" -y -f BC1_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1";
                                listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                listBox1.Items.Add("new format = BC1_UNORM_SRGB");
                            }
                            else
                            {
                                startInfo.Arguments = "\"" + file + "\" -y -f BC1_UNORM -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1";
                                listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                listBox1.Items.Add("new format = BC1_UNORM");
                            }
                        }
                        else
                        {
                            startInfo.Arguments = "\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1";
                            listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                            listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                        }
                        needtogenmm = true;
                    }
                    else
                    {
                        if (!ddsinfo.format.Contains("BC1"))
                        {
                            if (ddsinfo.format.Contains("SRGB"))
                            {
                                startInfo.Arguments = "\"" + file + "\" -y -f BC1_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"";
                                listBox1.Items.Add("new format = BC1_UNORM_SRGB");
                            }
                            else
                            {
                                startInfo.Arguments = "\"" + file + "\" -y -f BC1_UNORM -o \"" + fileinf.DirectoryName + "\"";
                                listBox1.Items.Add("new format = BC1_UNORM");
                            }
                        }
                        else
                        {
                            listBox1.Items.Add("Already compressed");
                        }
                    }
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                    if (needtogenmm)
                    {
                        //gen mipmaps
                        startInfo.Arguments = "\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 0";
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                    }
                }
                else if (checkBox1.Checked == true) //compress
                {
                    i++;
                    form.Text = "Compressing files : " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                    Process process = new Process();
                    ProcessStartInfo startInfo = new ProcessStartInfo();
                    startInfo.CreateNoWindow = true;
                    startInfo.UseShellExecute = false;
                    startInfo.FileName = Application.StartupPath + "\\bin\\texconv.exe";
                    if (!ddsinfo.format.Contains("BC1"))
                    {
                        if (ddsinfo.format.Contains("SRGB"))
                        {
                            startInfo.Arguments = "\"" + file + "\" -y -f BC1_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"";
                            listBox1.Items.Add("new format = BC1_UNORM_SRGB");
                        }
                        else
                        {
                            startInfo.Arguments = "\"" + file + "\" -y -f BC1_UNORM -o \"" + fileinf.DirectoryName + "\"";
                            listBox1.Items.Add("new format = BC1_UNORM");
                        }
                    }
                    else
                    {
                        listBox1.Items.Add("Already compressed");
                    }
                    process.StartInfo = startInfo;
                    process.Start();
                    process.WaitForExit();
                }
                else if (checkBox2.Checked == true) //resize
                {
                    i++;
                    form.Text = "Resizing files : " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                    int ifgreater = 8192;
                    if (comboBox1.Text.Contains("512")) { ifgreater = 512; }
                    else if (comboBox1.Text.Contains("1024")) { ifgreater = 1024; }
                    else if (comboBox1.Text.Contains("2048")) { ifgreater = 2048; }
                    else if (comboBox1.Text.Contains("4096")) { ifgreater = 4096; }
                    else if (comboBox1.Text.Contains("128")) { ifgreater = 128; }
                    else if (comboBox1.Text.Contains("256")) { ifgreater = 256; }
                    else if (comboBox1.Text.Contains("8192")) { ifgreater = 8192; }
                    else if (comboBox1.Text.Contains("4")) { ifgreater = 4; }
                    else if (comboBox1.Text.Contains("8")) { ifgreater = 8; }
                    else if (comboBox1.Text.Contains("16")) { ifgreater = 16; }
                    else if (comboBox1.Text.Contains("32")) { ifgreater = 32; }
                    else if (comboBox1.Text.Contains("64")) { ifgreater = 64; }
                    if (size > ifgreater)
                    {
                        Process process = new Process();
                        ProcessStartInfo startInfo = new ProcessStartInfo();
                        startInfo.CreateNoWindow = true;
                        startInfo.UseShellExecute = false;
                        startInfo.FileName = Application.StartupPath + "\\bin\\texconv.exe";
                        startInfo.Arguments = "\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1";
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                        //gen mipmaps
                        startInfo.Arguments = "\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 0";
                        process.StartInfo = startInfo;
                        process.Start();
                        process.WaitForExit();
                        listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                        listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                    }
                }
                filesize = Math.Round((Double)new FileInfo(file).Length / 1024, 1);
                newfilessize += filesize;
                if (!listBox1.Items[listBox1.Items.Count - 1].ToString().Contains("Already compressed"))
                {
                    listBox1.Items.Add("new file size = " + filesize + " kb");
                }
            }
            form.Text = "Compressed from " + Math.Round(filessize / 1024, 3) + "mb to " + Math.Round(newfilessize / 1024, 3) + "mb Saved = " + Math.Round((filessize - newfilessize)/1024, 3) + "mb";
        }

        private fileinfo checkdds(string file)
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
            fileinfo ddsinfo = new fileinfo();
            while (!process.StandardOutput.EndOfStream)
            {
                string line = process.StandardOutput.ReadLine();
                if (line.Contains("height"))
                {
                    ddsinfo.height = int.Parse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2));
                }
                if (line.Contains("width"))
                {
                    ddsinfo.width = int.Parse(line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2));
                }
                if (line.Contains("format"))
                {
                    ddsinfo.format = line.Substring(line.IndexOf("=") + 2, line.Length - line.IndexOf("=") - 2);
                }
            }
            process.WaitForExit();
            listBox1.Items.Add("height = " + ddsinfo.height);
            listBox1.Items.Add(" width = " + ddsinfo.width);
            listBox1.Items.Add("format = " + ddsinfo.format);
            return ddsinfo;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.Description = "Choose textures folder";
            folderBrowserDialog1.SelectedPath = textBox1.Text; ;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                XDocument xdoc = new XDocument();
                XElement settings = new XElement("settings");
                XElement dir = new XElement("lastdir", textBox1.Text);
                settings.Add(dir);
                xdoc.Add(settings);
                xdoc.Save("settings.xml");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            StreamWriter stream = new StreamWriter("log.txt", true, Encoding.UTF8);
            foreach (string line in listBox1.Items)
            {
                stream.WriteLine(line);
            }
            stream.Close();
            MessageBox.Show("log.txt created in program directory");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            About form = new About();
            form.Show();
        }
    }

    public class fileinfo
    {
        public int width { get; set; }
        public int height { get; set; }
        public string format { get; set; }
    }
}
