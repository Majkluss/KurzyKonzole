using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace KurzyKonzole
{
    class StahovaniDat
    {
        public bool zapsano = false;
        public string StahniData(string mena)
        {
            string kurzy;
            string result ="";
            using (var wc = new System.Net.WebClient())
                kurzy = wc.DownloadString("https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.xml");


            using (XmlReader reader = XmlReader.Create(new StringReader(kurzy)))
            {
                while (reader.Read())
                {
                    if (reader.Name == "radek")
                    {

                        if (reader.GetAttribute("kod") == mena)
                        {
                            result = String.Format("Kurz {0}: {1} CZK; Datum: {2}", mena, reader.GetAttribute("kurz"), DateTime.Now.ToString("dd/MM/yyyy"));
                            zapsano = true;
                        }

                        /*else if (reader.GetAttribute("kod") != mena)
                        {
                            result = "Nepodařilo se stáhnout kurz!";
                        }*/
                    }
                }
            }
            return result;
        }
        public void Smycka()
        {
            Slozka slozka = new Slozka();
            string dalsi = "";
            while (dalsi != "ne")
            {
                Console.WriteLine("Jakou měnu chcete stáhnout?");
                string odp = Console.ReadLine();
                slozka.ZapisKurz(StahniData(odp.ToUpper()).ToString());
                Console.WriteLine("Chcete vložit další kurz? ano/ne");
                dalsi = Console.ReadLine().ToLower();
            }
            slozka.PrectiSoubor();
        }
    }
    class Slozka
    {
        public void VytvorSlozku()
        {
            string cesta = "";
            try
            {
                // Nastaví cestu do složky AppData, kde se má vytvořit složka "C sharp test"
                // GetFolderPath() vrátí něco takového: C:\Users\vase_jmeno\AppData\Roaming\C sharp test
                cesta = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "C sharp test");

                // Pokud složka v cestě neexistuje, vytvoří ji
                if (!Directory.Exists(cesta))
                {
                    Directory.CreateDirectory(cesta);
                }
            }
            catch
            {
                Console.WriteLine("Nepodařilo se vytvořit složku {0}, zkontrolujte prosím svá oprávnění.", cesta);
            }

            if (File.Exists(Path.Combine(cesta, "databaze.dat")))
            {
                try
                {
                    // Umístění kódu pro načtení nastavení ze souboru
                }
                catch (Exception e)
                {
                    Console.WriteLine("Při načítání nastavení došlo k následující chybě: {0}", e.Message);
                }
            }
            else
            {
                try
                {
                    // Umístění kódu pro vytvoření nastavení
                }
                catch (Exception e)
                {
                    Console.WriteLine("Při vytvoření nastavení došlo k následující chybě: {0}", e.Message);
                }
            }
        }
        public void VytvorSoubor()
        {
            using (StreamWriter sw = new StreamWriter(@"soubor.txt", true))
            {

            }
        }

        public void ZapisKurz(string zapis)
        {
            StahovaniDat stah = new StahovaniDat();
            using (StreamWriter sw = new StreamWriter(@"soubor.txt", true))
            {
                sw.WriteLine(zapis);
                sw.Flush();
            }
            if (stah.zapsano)
            {
                Console.WriteLine("Kurz byl úspěšně zapsán.");
            }
        }

        public void PrectiSoubor()
        {
            Console.WriteLine("Vypisuji obsah souboru:");
            using (StreamReader sr = new StreamReader(@"soubor.txt"))
            {
                string s;
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Slozka slozka = new Slozka();
            StahovaniDat sd = new StahovaniDat();

            slozka.VytvorSlozku();
            slozka.VytvorSoubor();

            sd.Smycka();
            Console.ReadKey();
        }
    }
}
