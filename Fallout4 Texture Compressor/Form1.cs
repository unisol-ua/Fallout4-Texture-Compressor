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
            if (allfiles.Length > 0)
            {
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
                    if (textBox2.Text == "") { startInfo.Arguments = "a backup_" + time + ".zip \"" + Application.StartupPath + "\\backup\\*\" -mx6 -o" + Application.StartupPath + "\\"; }
                    else { startInfo.Arguments = "a \"" + textBox2.Text + "_" + time + ".zip\" \"" + Application.StartupPath + "\\backup\\*\" -mx6 -o" + Application.StartupPath + "\\"; }
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
                        else
                        {
                            MessageBox.Show("Avoid editing resize combobox. Use only numbers for custom paramter (without if<> and any words). Resize parameter has been set to custom number.");
                            try { ifgreater = int.Parse(comboBox1.Text); }
                            catch { MessageBox.Show("Failed parse custom parameter. Parameter has been reset to 8192"); ifgreater = 8192; }
                        }
                        bool needtogenmm = false;
                        if (size > ifgreater)
                        {
                            if (!ddsinfo.format.Contains("BC1"))//skip if already bc1 (only resize)
                            {
                                if (ddsinfo.format.Contains("BC"))//check if bc format and compress to bc1
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
                                else if (checkBox4.Checked == true)//if other format and compress checked, compress to bc7
                                {
                                    if (ddsinfo.format.Contains("SRGB"))
                                    {
                                        texconv("\"" + file + "\" -y -f BC7_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                        listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                        listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                        listBox1.Items.Add("new format = BC7_UNORM_SRGB");
                                    }
                                    else
                                    {
                                        texconv("\"" + file + "\" -y -f BC7_UNORM -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                        listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                        listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                                        listBox1.Items.Add("new format = BC7_UNORM");
                                    }
                                }
                                else
                                {
                                    listBox1.Items.Add("Other format compress is unchecked, skipping");
                                }
                            }
                            else
                            {
                                texconv("\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 1");
                                listBox1.Items.Add("new  width = " + (ddsinfo.width / 2));
                                listBox1.Items.Add("new height = " + (ddsinfo.height / 2));
                            }
                            needtogenmm = true;
                        }
                        else
                        {
                            if (!ddsinfo.format.Contains("BC1"))//skip if already bc1
                            {
                                if (ddsinfo.format.Contains("BC"))//check if bc format and compress to bc1
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
                                else if (checkBox4.Checked == true)//if other format and compress checked, compress to bc7
                                {
                                    if (ddsinfo.format.Contains("SRGB"))
                                    {
                                        texconv("\"" + file + "\" -y -f BC7_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                                        listBox1.Items.Add("new format = BC7_UNORM_SRGB");
                                    }
                                    else
                                    {
                                        texconv("\"" + file + "\" -y -f BC7_UNORM -o \"" + fileinf.DirectoryName + "\"");
                                        listBox1.Items.Add("new format = BC7_UNORM");
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
                        if (needtogenmm)
                        {
                            //gen mipmaps
                            texconv("\"" + file + "\" -y -o \"" + fileinf.DirectoryName + "\" -w " + ddsinfo.width / 2 + " -h " + ddsinfo.height / 2 + " -m 0");
                        }
                    }
                    else if (checkBox1.Checked == true) //compress
                    {
                        i++;
                        form.Text = "Compressing files : " + i + " of " + allfiles.Length + " : " + Math.Round(i / ((Double)allfiles.Length / 100), 2) + "%";
                        if (!ddsinfo.format.Contains("BC1"))//skip if already bc1
                        {
                            if (ddsinfo.format.Contains("BC"))//check if bc format and compress to bc1
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
                            else if (checkBox4.Checked == true)//if other format and compress checked, compress to bc7
                            {
                                if (ddsinfo.format.Contains("SRGB"))
                                {
                                    texconv("\"" + file + "\" -y -f BC7_UNORM_SRGB -o \"" + fileinf.DirectoryName + "\"");
                                    listBox1.Items.Add("new format = BC7_UNORM_SRGB");
                                }
                                else
                                {
                                    texconv("\"" + file + "\" -y -f BC7_UNORM -o \"" + fileinf.DirectoryName + "\"");
                                    listBox1.Items.Add("new format = BC7_UNORM");
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
                        else
                        {
                            MessageBox.Show("Avoid editing resize combobox. Use only numbers for custom paramter (without if<> and any words). Resize parameter has been set to custom number.");
                            try { ifgreater = int.Parse(comboBox1.Text); }
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
                    if (!listBox1.Items[listBox1.Items.Count - 1].ToString().Contains("Already compressed") && !listBox1.Items[listBox1.Items.Count - 1].ToString().Contains("Other format compress is unchecked, skipping"))
                    {
                        listBox1.Items.Add("new file size = " + filesize + " kb");
                    }
                    //keep listbox scrolled to bottom
                    listBox1.TopIndex = listBox1.Items.Count - 1;
                }
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
