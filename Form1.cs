using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Renamer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = "C:\\Users\\Mark\\Desktop\\TriblerDownloads\\One_Piece_1-516\\";
            var extensions = new string[] { ".avi", ".mp4", ".mkv" };
            var files = GetFiles(path, extensions.ToList());
            string[] p;
            FileDetails file;
            var details = new List<FileDetails>();
            List<FileDetails> results;

            foreach (string f in files)
            {
                p = f.ToLower().Split('\\');
                file = new FileDetails(p[p.Length - 1]);
                p = file.name.Split('.');
                file.extension = p[p.Length - 1];
                file.episode = GetEpisodeNumber(file.name);
                file.renamed = "One Piece - Ep " + file.episode.ToString("000") + "." + file.extension;
                details.Add(file);

                try
                {
                    File.Move(path + file.name, path + file.renamed);
                }catch(Exception ex) { }
            }
            results = details.OrderBy(o => o.episode).ToList();
            dataFiles.AutoGenerateColumns = false;
            dataFiles.DataSource = results;
        }

        private int GetEpisodeNumber(string str)
        {
            var i = -1;
            var o = 0;
            var s = "";
            var result = 0;
            for (var x = 0; x < str.Length; x++)
            {
                s = str.Substring(x, 1);
                if (int.TryParse(s, out o) == true)
                {
                    if (i == -1)
                    {
                        //found beginning of number
                        i = x;
                    }
                }
                else
                {
                    if (i >= 0)
                    {
                        //found end of number
                        o = int.Parse(str.Substring(i, x - i));
                        if (o < 1 || o > 516)
                        {
                            //not a believable episode number
                            i = -1;
                        }
                        else
                        {
                            //found episode number, exit
                            result = o;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        private List<string> GetFiles(string folder, List<string> extensions)
        {
            List<string> files = new List<string>();
            try
            {
                foreach (string f in Directory.GetFiles(folder).Where(d => extensions.Count(e => d.IndexOf(e) > 0) > 0).ToList())
                {
                    files.Add(f);
                }
                foreach (string d in Directory.GetDirectories(folder))
                {
                    files.AddRange(GetFiles(d, extensions));
                }
            }
            catch (Exception excpt)
            {
                MessageBox.Show(excpt.Message);
            }

            return files;
        }
    }

    public class FileDetails
    {
        public string name { get; set; }
        public string renamed { get; set; } 
        public string extension { get; set; }
        public int episode { get; set; }

        public FileDetails(string name = "", string renamed = "", string extension = "")
        {
            this.name = name;
            this.renamed = renamed;
            this.extension = extension;
            this.episode = 0;
        }
    }
}
