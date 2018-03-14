using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
//ROZSZERZONY ALGORYTM EUKLIDESA
namespace POD_lab6
{

    class Program
    {
        static int NWD(int a, int b)
        {
            while (a != b)
            {
                if (a > b)
                    a -= b;
                else
                    b -= a;
            }
            return a;
        }
        static int rozszerzony_algorytm_Euklidesa(int a, int b)
        {
            int u = 1; int x = 0;
            int w = a; int z = b;
            int q;
            while(w!=0)
            {
                if (w < z)
                {
                    q = u; u = x;
                    x = q; q = w;
                    w = z; z = q;
                }
                q = w / z;
                u = u - q * x;
                w = w - q * z;
            }
            if (z == 1)
            {
                if (x < 0)
                {
                    x = x + b;                    
                }
                return x;
            }
            else
                return (-1000);
        }
        static int[] RSA_kluczpubliczny(int p, int q)
        {
            int n = p * q;
            int fi = (p - 1) * (q - 1);
            int e;
            int[] klucz = new int[2];
            Random losuj = new Random(Environment.TickCount);
            do
            {
                e = losuj.Next(3, (fi-1));
            } while (NWD(e, fi) != 1);
            klucz[0] = e;
            klucz[1] = n;
            return klucz;
        }
        static int[] RSA_klucztajny(int p, int q, int e)
        {
            int n = p * q;
            int fi = (p - 1) * (q - 1);
            int d;
            int[] klucz = new int[2];
            d = rozszerzony_algorytm_Euklidesa(e, fi);
            klucz[0] = d;
            klucz[1] = n;
            return klucz;
        }
        static string Dodaj(int wartosc, int klucz)
        {
            string temp = "";
            for (int i = 0; i < (Convert.ToString(klucz).Length - Convert.ToString(wartosc).Length); i++)
                    temp = temp + "0";
            temp = temp + wartosc;
            return temp;
        }
        static string RSA_szyfrowanie(int[] klucz, string wiadomosc)
        {
            string ciag="";
            foreach (char c in wiadomosc)
            {
                if ((int)c < 10)
                    ciag = ciag + "00" + (int)c;
                else if ((int)c < 100)
                    ciag = ciag + "0" + (int)c;
                else
                    ciag = ciag + (int)c;
            }
            string zaszyfrowana_wiadomosc="";
            String podciag;
            int intVal;
            for (int i = 0; i < ciag.Length; i += 3)
            {
                
                if ((ciag.Length-i- 3) <0)
                {
                    podciag = ciag.Substring(i, ciag.Length - i);
                    intVal = (int)BigInteger.ModPow(Int32.Parse(podciag), klucz[0], klucz[1]);
                    zaszyfrowana_wiadomosc = zaszyfrowana_wiadomosc + Dodaj(intVal,klucz[1]);
                    break;
                }
                podciag = ciag.Substring(i, 3);
                intVal = (int)BigInteger.ModPow(Int32.Parse(podciag), klucz[0], klucz[1]);
                zaszyfrowana_wiadomosc = zaszyfrowana_wiadomosc + Dodaj(intVal, klucz[1]);
            }
            return zaszyfrowana_wiadomosc;
        }
        static string RSA_deszyfrowanie(int[] klucz, string zasz_wiadomosc)
        {
            string odszyfrowana_wiadomosc = "";
            string podciag;
            int intVal;
            for (int i = 0; i < zasz_wiadomosc.Length; i += Convert.ToString(klucz[1]).Length)
            {
                podciag = zasz_wiadomosc.Substring(i, Convert.ToString(klucz[1]).Length);
                intVal = (int)BigInteger.ModPow(Int32.Parse(podciag), klucz[0], klucz[1]);
                odszyfrowana_wiadomosc = odszyfrowana_wiadomosc + (char)intVal;
            }
            return odszyfrowana_wiadomosc;
        }
        static void Main(string[] args)
        {
            int p = 5651;
            int q = 5623;
            int[] klucz_publiczny = RSA_kluczpubliczny(p, q);
            int[] klucz_tajny = RSA_klucztajny(p, q, klucz_publiczny[0]);
            Console.WriteLine("Klucz publiczny " + klucz_publiczny[0] + " " + klucz_publiczny[1]);
            Console.WriteLine("\n\n\n");
            Console.WriteLine("Klucz tajny " + klucz_tajny[0] + " " + klucz_tajny[1]);
            Console.WriteLine("\n\n\n");
            string wyg = RSA_szyfrowanie(klucz_publiczny, "LOREM IPSUM, ala ma kota.");
            Console.WriteLine("Wygenereowany ciag " + wyg);
            Console.WriteLine("\n\n\n");
            Console.WriteLine("Rozszyfrowana wiadomosc");
            Console.WriteLine(RSA_deszyfrowanie(klucz_tajny, wyg));
            Console.ReadKey();
        }
    }
}
