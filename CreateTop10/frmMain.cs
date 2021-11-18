using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace CreateTop10
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("Pokemon_trainers.ini");
            string[] sUserblocks = sr.ReadToEnd().Split('[');
            List<PokeTrainer> lsTrainers = new List<PokeTrainer>();
            foreach(string s in sUserblocks)
            {
                if(!string.IsNullOrEmpty(s))
                {
                    string sUsername = s.Split(']')[0];
                    int iPokedex = 0;
                    int iShinys = 0;
                    Match m = Regex.Match(s.Split(']')[1].Replace("\n",""), "pokedex=\"(\\d{1,3})\\.000000\"");
                    if (m.Success)
                    {
                        iPokedex = Convert.ToInt32(m.ToString().Replace("pokedex=", "").Replace("\"", "").Replace(".000000", ""));
                    }

                    Match m2 = Regex.Match(s.Split(']')[1].Replace("\n", ""), "shinys=\"(\\d{1,3})\\.000000\"");
                    if (m2.Success)
                    {
                        iShinys = Convert.ToInt32(m2.ToString().Replace("shinys=", "").Replace("\"", "").Replace(".000000", ""));
                    }

                    if (!string.IsNullOrEmpty(sUsername))
                    {
                        PokeTrainer p = new PokeTrainer(sUsername, iPokedex, iShinys);
                        lsTrainers.Add(p);
                    }
                }
            }
            lsTrainers = lsTrainers.OrderBy(o => o.iDex).ToList();
            lsTrainers.Reverse();

            StreamWriter sw = new StreamWriter("rankings.ini",false);
            sw.WriteLine("[normal]");

            for (int i = 0; i < lsTrainers.Count; i++)
            {
                //MessageBox.Show((i + 1) + "." + lsTrainers[i].sName + " - " + lsTrainers[i].iDex.ToString() + " - " + lsTrainers[i].iShinys);
                sw.WriteLine((i + 1).ToString() + "=\"" + lsTrainers[i].sName + "\"");
            }

            sw.WriteLine("[normalnames]");

            for (int i = 0; i < lsTrainers.Count; i++)
            {
                sw.WriteLine(lsTrainers[i].sName + "=\"" + (i + 1).ToString() + "\"");
            }

            lsTrainers = lsTrainers.OrderBy(o => o.iShinys).ToList();
            lsTrainers.Reverse();

            sw.WriteLine("[shiny]");

            for (int i = 0; i < lsTrainers.Count; i++)
            {
                sw.WriteLine((i + 1).ToString() + "=\"" + lsTrainers[i].sName + "\"");
            }

            sw.WriteLine("[shinynames]");

            for (int i = 0; i < lsTrainers.Count; i++)
            {
                sw.WriteLine(lsTrainers[i].sName + "=\"" + (i + 1).ToString() + "\"");
            }

            sw.Flush();
            sw.Close();
            Application.Exit();
        }

        
    }

    public class PokeTrainer
    {
        public string sName;
        public int iDex;
        public int iShinys;

        public PokeTrainer(string sName, int iDex, int iShinys)
        {
            this.sName = sName;
            this.iDex = iDex;
            this.iShinys = iShinys;
        }
    }
}
