using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Yaz_Bakalim
{
    class Baslangic
    {
        private static Baslangic current;

        public static Baslangic Current
        {
            get { if (current == null) { current = new Baslangic(); } return current; }
            set { current = value; }
        }
        public Baslangic()
        {
            BGWBaslangic.DoWork += BGWBaslangic_DoWork;
        }
        public frmMain Main = new frmMain();
        List<Point> BaslangicKoordinatlar = new List<Point>();
        private Point GelenSatirToPoint(string Satir)
        {
            string[] SatirDizi = Satir.Split(',');
            int xPoint = int.Parse(SatirDizi[0]);
            int yPoint = int.Parse(SatirDizi[1]);
            Point DondurulenPoint = new Point(xPoint, yPoint);
            return DondurulenPoint;
        }
        private bool BaslangicKoordinat_Oku()
        {
            int Boyut = 128;
            using (var TxtKoordinat = File.OpenRead(Application.StartupPath + @"\Baslangic.txt"))
            using(var TxtOku = new StreamReader(TxtKoordinat, Encoding.UTF8, true, Boyut))
            {
                String Satir;
                while ((Satir = TxtOku.ReadLine()) != null)
                {
                    if (Satir.Length > 2)
                    {
                       Point CevrilenSatir = GelenSatirToPoint(Satir);
                        BaslangicKoordinatlar.Add(CevrilenSatir);
                    }
                }
            }
            return true;
        }
        private char[] baslangic { get; set; }
        private string BaslangicMetin { get; set; }
        private void BGWBaslangic_DoWork(object sender, DoWorkEventArgs e)
        {
            BaslangicMetin = "B A K A L I M !";
            baslangic = BaslangicMetin.ToCharArray(); 
            if (BaslangicKoordinat_Oku())
            {
                Graphics g = Main.CreateGraphics();
                Pen Sekil = new Pen(Color.Green);
                for (int i = 0; i < BaslangicKoordinatlar.Count; i++)
                {
                    g.DrawRectangle(Sekil, BaslangicKoordinatlar[i].X, BaslangicKoordinatlar[i].Y, 8, 8);
                    Main.BackColor = Color.Black;
                    Thread.Sleep(50);
                }
                foreach(char metin in baslangic)
                {
                    Main.lblYazBakalim.Text += metin.ToString();
                }
                g.Clear(Color.Black);
                Thread.Sleep(1000);
                Main.lblYazBakalim.Visible = false;
                for (int i = 0; i < BaslangicKoordinatlar.Count; i++)
                {
                    g.DrawRectangle(Sekil, BaslangicKoordinatlar[i].X, BaslangicKoordinatlar[i].Y, 8, 8);
                    Main.BackColor = Color.Black;
                }
                Main.lblYazBakalim.Visible = true;
                Thread.Sleep(1000);
                Main.lblYazBakalim.Visible = false;
                g.Clear(Color.Black);
            }
        }

        public BackgroundWorker BGWBaslangic = new BackgroundWorker();
    }
}
