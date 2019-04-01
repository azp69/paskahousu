using System;
using System.Diagnostics;
using System.Collections.Generic;


public class Korttipakka
{
    private int tos;
    private Kortti[] kortit = new Kortti[52];

    public Korttipakka()
	{
        tos = -1;
	}

    public void Tyhjenna()
    {
        tos = -1;
    }
    public Kortti OtaKortti()
    {
        return kortit[tos--];
        // tos--;
    }

    public void LisaaKortti(Kortti k)
    {
        kortit[++tos] = k;
    }

    public Kortti PaallimmainenKortti()
    {
        if (tos >= 0)
            return kortit[tos];
        else
            return null;
    }

    public int KorttejaPakassa()
    {
        return tos + 1;
    }

    public void Sekoita()
    {
        Random rnd = new Random();
        int i1, i2;

        for (int i = 0; i < 500; i++)
        {
            i1 = rnd.Next(0, 52);
            i2 = rnd.Next(0, 52);
            Kortti k = kortit[i1];
            kortit[i1] = kortit[i2];
            kortit[i2] = k;
        }
    }

    public void Alusta()
    {
        string maa = "ru";
        int laskuri = 0;

        for (int j = 0; j < 4; j++)
        {
            switch (j)
            { 
                case 0:
                maa = "he";
                break;
            case 1:
                maa = "pa";
                break;
            case 2:
                maa = "ru";
                break;
            
            default:
                maa = "ri";
                break;
            }
            
            for (int i = 1; i < 14; i++)
            {
                kortit[laskuri] = new Kortti(maa,i);
                laskuri++;
            }
        }
        tos = 51; // tää 51, eli täys pakka
        Debug.WriteLine("");
    }
}

public class Kortti
{
    private string maa;
    private int arvo;
    private string nimi;
    
    
    public Kortti(string m, int a)
    {
        nimi = m.ToString() + "-" + a.ToString();
        maa = m;
        arvo = a;
    }
    public string Maa()
    {
        return maa;
    }

    public int Arvo()
    {
        return arvo;
    }

    public string Nimi()
    {
        return nimi;
    }
}

public class KasiPakka
{
    private List<Kortti> kortit = new List<Kortti>();

    public List<Kortti> HaePakka()
    {
        return kortit;
    }

    public void LisaaKortti(Kortti k)
    {
        kortit.Add(k);
    }

    public void PoistaKortti(Kortti k)
    {
        kortit.Remove(k);
    }

    public int KorttejaPakassa()
    {
        return kortit.Count;
    }

    public Kortti EtsiKortti(string nimi)
    {
        int index = kortit.FindIndex(s => s.Nimi() == nimi);    // etsitään korttia listalta
        Kortti k = kortit[index];
        return k;
        
    }

    
}