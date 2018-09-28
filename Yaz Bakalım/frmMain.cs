using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yaz_Bakalim
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            BGWIslemYap.DoWork += BGWIslemYap_DoWork;
            BGWIslemYap.WorkerSupportsCancellation = true;
            CheckForIllegalCrossThreadCalls = false;
        }

        public int OnceX;
        public int OnceY;
        public int Puan;
        public int EnYuksekPuan;
        bool Sorunlu = false;
        private void BGWIslemYap_DoWork(object sender, DoWorkEventArgs e)
        {
            Action SorunsuzBitti = () =>
             {
                 this.Text = "";
                 if (Puan > EnYuksekPuan && !Sorunlu) 
                    this.Text += " Harika! Sorunsuz yazdın, Puan = " + Puan+" Tekrar yapmak istersen F5 basabilirsin!";
                 if(Puan<EnYuksekPuan && !Sorunlu)
                     this.Text += " Harika! Sorunsuz yazdın, Puan = " + EnYuksekPuan + " Tekrar yapmak istersen F5 basabilirsin!";
             };
            for (int i = 0; i < Koordinatlar.Count; i++)
            {
                if (OnceX == Koordinatlar[i].X && OnceY == Koordinatlar[i].Y)
                {
                    MessageBox.Show("Kaybettin puan : " + Puan);
                    Koordinatlar.Clear();
                    g.Clear(Color.Black);
                    button1.Location = new Point(12, 12);
                    if (EnYuksekPuan < Puan)
                        EnYuksekPuan = Puan;
                    this.Text = "Tekrar dene bakalım !";
                    this.Text += "          En yüksek puan = " + EnYuksekPuan.ToString();
                    Sorunlu = true;
                    BGWIslemYap.CancelAsync();
                    break;
                }
                else
                {
                    Puan += 10;
                    button1.Location = new Point(Koordinatlar[i].X, Koordinatlar[i].Y);
                    OnceX = Koordinatlar[i].X;
                    OnceY = Koordinatlar[i].Y;
                    Thread.Sleep(80);
                    Sorunlu = false;
                    continue;
                }
            }
            if (!Sorunlu) { SorunsuzBitti(); }
            Puan = 0;
            OnceX = 0;
            OnceY = 0;
            Koordinatlar.Clear();
        }

        BackgroundWorker BGWIslemYap = new BackgroundWorker();
        Graphics g;
        List<Point> Koordinatlar = new List<Point>();
        private Point Koordinat1;
        private void Ciz(Point koordinat1)
        {
            g = this.CreateGraphics();
            Pen Sekil = new Pen(Color.Green);
            
             g.DrawRectangle(Sekil, koordinat1.X, koordinat1.Y,8,8);
            Thread.Sleep(10);
        }

        private void frmMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Koordinat1 = new Point(e.X, e.Y);
                Koordinatlar.Add(Koordinat1);
                Ciz(Koordinat1);
                if (!BGWIslemYap.IsBusy)
                    BGWIslemYap.RunWorkerAsync();
            }
        }

        private void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F5)
            {
                this.Text = "Yaz Bakalım!";
                g.Clear(Color.Black);
                button1.Location = new Point(12, 12);
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            Baslangic.Current.Main = this;
            Baslangic.Current.BGWBaslangic.RunWorkerAsync();
        }
    }
}
