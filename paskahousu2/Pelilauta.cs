using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace paskahousu2
{
    public partial class Paskahousu : Form
    {


        Random randomi = new Random();

        // public Pelaaja p1 = new Pelaaja("Pelaaja 1", false);
        // public Pelaaja p2 = new Pelaaja("Pelaaja 2", false);

        public Pelaaja[] pelaajat;  // pelaajat

        public int pelaajia = 4;    // montako pelaajaa
        public int aktiivinen_pelaaja = 0; // kenen vuoro

        // Pelikohtaiset muuttujat
        public Korttipakka pakka = new Korttipakka();                               // Korttipakka, josta pelataan
        public Korttipakka poyta = new Korttipakka();                               // Pöydällä olevat pelatut kortit
        public List<MyPictureBox> poytaKorttiPictureBox = new List<MyPictureBox>(); // Pictureboxit pelatuille korteille
        public MyPictureBox pakkaPictureBox = new MyPictureBox();                   // Pakan picturebox
        const int _kuviaTamanPaalle = 6;                                             // kuvakortteja voi lyödä tämän päälle

        public Paskahousu()
        {
            InitializeComponent();
            AloitaPeli();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void PaivitaKorttipakka()
        {
            MyPictureBox p = new MyPictureBox
            {
                Top = 10,
                Left = 550, // 140
                Image = (Bitmap)Image.FromFile("..\\..\\kuvat\\tausta.png"),
                Width = 120,
                Height = 220,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
            };
            pakkaPictureBox = p;
        }

        public void TyhjennaPoyta()
        {
            foreach (MyPictureBox p in poytaKorttiPictureBox)
            {
                p.Dispose();
            }
            poytaKorttiPictureBox.Clear();
            poyta.Tyhjenna();
        }

        public void PelaaKortti(Kortti lyotavaKortti)
        {
            bool voiLyoda = false;
            bool kaatuu = false;
            bool vuoroJatkuu = false;

            int korttiJotaPelataan, ylinKorttiPoydassa;
            Kortti k = lyotavaKortti;   // Kortti jota ollaan lyömässä pöytään
            korttiJotaPelataan = k.Arvo();

            if (poyta.KorttejaPakassa() > 0)
            {
                Kortti p = poyta.PaallimmainenKortti();
                ylinKorttiPoydassa = p.Arvo();
            }
            else
            {
                ylinKorttiPoydassa = 0;
            }

            bool[] tilanne = VoikoPelata(korttiJotaPelataan, ylinKorttiPoydassa);
            voiLyoda = tilanne[0];
            kaatuu = tilanne[1];

            if (voiLyoda)
            {
                if (k.Arvo() == 0 || k.Arvo() == 1)
                {
                    int seuraavaPelaaja = aktiivinen_pelaaja + 1;
                    if (seuraavaPelaaja >= pelaajia)
                        seuraavaPelaaja = 0;

                    pelaajat[aktiivinen_pelaaja].kasi.PoistaKortti(k);
                    pelaajat[seuraavaPelaaja].kasi.LisaaKortti(k);

                }
                else
                {
                    pelaajat[aktiivinen_pelaaja].kasi.PoistaKortti(k);
                    poyta.LisaaKortti(k);
                }
                PiirraKasi();
                if (kaatuu)
                {
                    TyhjennaPoyta();
                    vuoroJatkuu = true;
                }
                NostaKortti();
                PiirraPoyta();

                if (!vuoroJatkuu)
                {
                    pelaajat[aktiivinen_pelaaja].minunVuoro = false;
                    jatkaVuoroaBtn.Enabled = true;
                }

                TsekkaaTilanne();
                kenenVuoroLabel.Text = pelaajat[aktiivinen_pelaaja].nimi;
            }
        }

        public bool[] VoikoPelata(int korttiJotaPelataan, int ylinKorttiPoydassa)
        {
            bool voiLyoda, kaatuu;
            voiLyoda = false;
            kaatuu = false;

            bool[] tilanne = new bool[2];

            if ((korttiJotaPelataan < 10 && ylinKorttiPoydassa < 10) && (korttiJotaPelataan >= ylinKorttiPoydassa)
                && (korttiJotaPelataan != 1) && (korttiJotaPelataan != 10))
            {                                                        // molemmat on alle kymmenen, sekä pelattava on isompi                                                                           
                voiLyoda = true;                                     // tai yhtä suuri kuin pöydässä oleva, eikä se ole ässä tai kymppi
            }

            else if (korttiJotaPelataan == 10 && ylinKorttiPoydassa > 1 && ylinKorttiPoydassa < 10)
            {
                voiLyoda = true;
                kaatuu = true;
            }

            else if ((korttiJotaPelataan > 10 && ylinKorttiPoydassa >= _kuviaTamanPaalle) && korttiJotaPelataan >= ylinKorttiPoydassa)
            {
                voiLyoda = true;
            }

            else if (korttiJotaPelataan == 1 && ylinKorttiPoydassa > 10)
            {
                voiLyoda = true;
                kaatuu = true;
            }

            else if ((korttiJotaPelataan == 1 || korttiJotaPelataan == 10) && ylinKorttiPoydassa == 0) // ässä tai kymppi tyhjään pöytään
            {
                voiLyoda = true;
                kaatuu = true;
            }

            else if ((korttiJotaPelataan > 10 && ylinKorttiPoydassa == 0))
            {
                voiLyoda = true;
            }

            tilanne[0] = voiLyoda;
            tilanne[1] = kaatuu;

            return tilanne;
        }

        public void TsekkaaTilanne()
        {
            int poytaKortti = 0;

            if (pelaajat[aktiivinen_pelaaja].kasi.KorttejaPakassa() > 0) // tsekkaa tämä vielä
            {
                if (poyta.KorttejaPakassa() > 0)
                {
                    Kortti p = poyta.PaallimmainenKortti();
                    poytaKortti = p.Arvo();
                }
                else
                {
                    poytaKortti = 0;
                }

                List<Kortti> kp = pelaajat[aktiivinen_pelaaja].kasi.HaePakka();

                bool voiPelata = false;

                foreach (Kortti k in kp)
                {
                    bool[] tmp = VoikoPelata(k.Arvo(), poytaKortti);
                    voiPelata = tmp[0];
                    if (voiPelata)
                        break;
                }

                if (!voiPelata && pelaajat[aktiivinen_pelaaja].minunVuoro == true)
                {
                    if (!pelaajat[aktiivinen_pelaaja].ai)
                        MessageBox.Show("Nyt ollaan jumissa, joudut nostamaan pöydässä olevat kortit");
                    else
                        Debug.WriteLine("Kone joutuu nostamaan kortit");

                    while (poyta.KorttejaPakassa() > 0)
                    {
                        pelaajat[aktiivinen_pelaaja].kasi.LisaaKortti(poyta.OtaKortti());
                    }
                    PiirraKasi();
                    TyhjennaPoyta();
                    jatkaVuoroaBtn.Enabled = true;
                }
            }
            else if (pelaajat[aktiivinen_pelaaja].kasi.KorttejaPakassa() == 0)
            {
                if (!pelaajat[aktiivinen_pelaaja].ai)
                    MessageBox.Show("Voitit pelin!");
                else
                    MessageBox.Show("Pelaaja {0} voitti pelin", pelaajat[aktiivinen_pelaaja].nimi);

                AloitaPeli();
            }

        }


        public void PiirraPoyta()
        {
            if (poyta.KorttejaPakassa() > 0)
            {

                Kortti k = poyta.PaallimmainenKortti();
                MyPictureBox p = new MyPictureBox
                {
                    Top = (300 + (randomi.Next(-60, 60))),
                    Left = (580 + (randomi.Next(-50, 50))), // 140
                    Image = (Bitmap)Image.FromFile("..\\..\\kuvat\\" + k.Nimi() + ".png"),
                    Width = 120,
                    Height = 220,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    numero = k.Arvo(),
                    maa = k.Maa(),
                    nimi = k.Maa() + "-" + k.Arvo()
                };
                poytaKorttiPictureBox.Add(p);
                this.Controls.Add(p);
                p.BringToFront();
            }
        }

        public void KorttiaPainettu(object sender, EventArgs e)   // Kädessä olevaa korttia klikattu, eventhandleri 
        {
            if (pelaajat[aktiivinen_pelaaja].minunVuoro)                                // Onko minun vuoro lyödä
            {
                MyPictureBox p = sender as MyPictureBox;
                Kortti k = pelaajat[aktiivinen_pelaaja].kasi.EtsiKortti(p.nimi);        // Etsi pelattava kortti kädestä
                PelaaKortti(k);                       // Lähdetään eteenpäin
            }
        }

        public void PiirraKasi()
        {
            foreach (Pelaaja pelaaja in pelaajat)
            {
                foreach (MyPictureBox p in pelaaja.kasiKuvatPictureBox)
                {
                    p.Dispose();
                }
            }

            if (pelaajat[aktiivinen_pelaaja].kasiKuvatPictureBox.Count > 0)
            {
                foreach (MyPictureBox p in pelaajat[aktiivinen_pelaaja].kasiKuvatPictureBox)
                {
                    p.Dispose();
                }
            }
            pelaajat[aktiivinen_pelaaja].kasiKuvatPictureBox.Clear();
            List<Kortti> k = pelaajat[aktiivinen_pelaaja].kasi.HaePakka();
            int rundi = 0;
            int rivit = 0;
            foreach (Kortti ko in k)
            {
                MyPictureBox p = new MyPictureBox
                {
                    Top = 10 + (rivit * 90),
                    Left = 10 + (70 * rundi) + (rundi) * 10, // 140
                    Image = (Bitmap)Image.FromFile("..\\..\\kuvat\\" + ko.Nimi() + ".png"),
                    Width = 120,
                    Height = 220,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    BackColor = Color.Transparent,
                    numero = ko.Arvo(),
                    maa = ko.Maa(),
                    nimi = ko.Maa() + "-" + ko.Arvo()
                };
                pelaajat[aktiivinen_pelaaja].kasiKuvatPictureBox.Add(p);
                p.Click += new EventHandler(KorttiaPainettu);

                this.Controls.Add(p);
                p.BringToFront();
                rundi++;

                if (rundi > 5) // Mikäli kortteja kädessä enemmän kuin viisi, niin lisätään seuraavalle riville
                {
                    rundi = 0;
                    rivit++;
                }
            }
        }

        public void NostaKortti()
        {
            if (pakka.KorttejaPakassa() > 0 && pelaajat[aktiivinen_pelaaja].kasi.KorttejaPakassa() < 5)
            {
                pelaajat[aktiivinen_pelaaja].kasi.LisaaKortti(pakka.OtaKortti());
                PiirraKasi();
            }
            else if (pakka.KorttejaPakassa() <= 0)
            {
                pakkaPictureBox.Image = null;
                pakkaPictureBox.Dispose();
            }
        }

        public void PakkaaPainettu(object sender, EventArgs e)
        {
            NostaKortti();
        }

        public void JaaKortit()
        {
            int montaKorttia = 5;
            for (int j = 0; j < pelaajia; j++)
            {
                for (int i = 0; i < montaKorttia; i++)
                {
                    pelaajat[j].kasi.LisaaKortti(pakka.OtaKortti());
                }
            }
        }

        public void AloitaPeli()
        {
            pelaajat = new Pelaaja[pelaajia];
            pelaajat[0] = new Pelaaja("Pelaaja 1", false);

            for (int i = 1; i < pelaajia; i++)
            {
                pelaajat[i] = new Pelaaja("Pelaaja " + i, true);
            }

            // pelaajat[0] = new Pelaaja("Pelaaja 1", false);
            // pelaajat[1] = new Pelaaja("Konepelaaja", true);

            aktiivinen_pelaaja = 0;
            pelaajat[aktiivinen_pelaaja].minunVuoro = true;
            kenenVuoroLabel.Text = pelaajat[aktiivinen_pelaaja].nimi;
            TyhjennaPoyta();
            pakka.Alusta();
            pakka.Sekoita();
            JaaKortit();
            PiirraKasi();
            PaivitaKorttipakka();

            this.Controls.Add(pakkaPictureBox);
            pakkaPictureBox.Click += new EventHandler(PakkaaPainettu);
        }

        public void TeeAISiirto()
        {
            Debug.WriteLine("Tsekataan tilanne, voidaanko pelata");
            TsekkaaTilanne();
            bool saatiinko_lyotya = false;
            bool kaadettiin_kortit = false;
            int paallimmainenKortti = 0;

            while (pelaajat[aktiivinen_pelaaja].minunVuoro)
            {
                List<Kortti> _kasi = pelaajat[aktiivinen_pelaaja].kasi.HaePakka();
                if (poyta.KorttejaPakassa() > 0)
                {
                    foreach (Kortti k in _kasi)
                    {
                        bool[] v = VoikoPelata(k.Arvo(), poyta.PaallimmainenKortti().Arvo());
                        if (v[0] || v[1])
                        {
                            /*
                             tilanne[0] = voiLyoda;
                             tilanne[1] = kaatuu;
                             */
                            Debug.WriteLine("Pelataan kortti " + k.Nimi());
                            PelaaKortti(k);
                            saatiinko_lyotya = true;
                            if (v[1])
                                kaadettiin_kortit = true;

                            break;
                        }
                        // PelaaKortti(k);
                    }
                }
                else
                {
                    int minimi = 13;
                    Kortti pienin = _kasi.ElementAt(0);
                    foreach (Kortti k in _kasi)
                    {
                        if (k.Arvo() < minimi)
                        {
                            minimi = k.Arvo();
                            pienin = k;
                        }
                    }
                    Debug.WriteLine("Pelataan kortti " + pienin.Nimi());
                    PelaaKortti(pienin);
                    saatiinko_lyotya = true;
                }
                TsekkaaTilanne();
                if (saatiinko_lyotya == false)
                {
                    Debug.WriteLine("Ei saatu lyötyä korttia.. ei löydy sopivaa");
                    break;
                }
            }
            VaihdaVuoroa();
        }

        public void VaihdaVuoroa()
        {
            aktiivinen_pelaaja++;
            if (aktiivinen_pelaaja > pelaajia - 1)
            {
                aktiivinen_pelaaja = 0;
            }

            pelaajat[aktiivinen_pelaaja].minunVuoro = true;

            if (pelaajat[aktiivinen_pelaaja].ai)
            {
                TeeAISiirto();
            }
            else
            {
                jatkaVuoroaBtn.Enabled = false;
                PiirraKasi();
                kenenVuoroLabel.Text = pelaajat[aktiivinen_pelaaja].nimi;
                TsekkaaTilanne();
            }

        }

        private void JatkaVuoroaBtn_Click(object sender, EventArgs e)
        {
            VaihdaVuoroa();
        }
    }

    public class MyPictureBox : PictureBox
    {
        public string nimi;
        public string maa;
        public int numero;
    }

    public class Pelaaja
    {
        public string nimi;
        public bool ai;
        public bool minunVuoro;
        private bool pelissaMukana;
        private int pisteet;

        // Pelaajakohtaiset muuttujat
        public int korttejaKadessa;                                                 // Monta korttia on kädessä
        public KasiPakka kasi = new KasiPakka();                                    // Kädessä olevat kortit
        public List<MyPictureBox> kasiKuvatPictureBox = new List<MyPictureBox>();   // Kädessä olevien korttien pictureboxit

        public Pelaaja(string _nimi, bool _ai)
        {
            nimi = _nimi;
            ai = _ai;
            minunVuoro = false;
            pelissaMukana = true;
            pisteet = 0;
        }

    }
}
