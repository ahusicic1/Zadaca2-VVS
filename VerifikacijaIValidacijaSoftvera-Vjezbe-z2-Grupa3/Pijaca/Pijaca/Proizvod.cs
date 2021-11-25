using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pijaca
{
    public class Proizvod
    {
        #region Atributi

        Namirnica vrstaNamirnice;
        string šifraProizvoda, ime;
        int količinaNaStanju, očekivanaKoličina;
        DateTime datumPristizanja, datumOčekivaneKoličine;
        double cijenaProizvoda;
        bool certifikat387;
        static int brojač = 1000;

        #endregion

        #region Properties

        public Namirnica VrstaNamirnice { get => vrstaNamirnice; set => vrstaNamirnice = value; }
        
        public string ŠifraProizvoda { get => šifraProizvoda; }
        
        public string Ime 
        { 
            get => ime;
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new FormatException("Ime proizvoda ne može biti prazno!");
                else if (value.Length > 20)
                    throw new FormatException("Ime proizvoda ne smije biti duže od 20 karaktera!");

                ime = value;
            }
        }

        public int KoličinaNaStanju 
        { 
            get => količinaNaStanju;
            set
            {
                if (value < 0)
                    throw new FormatException("Dostupna količina ne može biti manja od 0!");
                else if (value > 50)
                    throw new FormatException("Štand ne dopušta više od 50 proizvoda!");

                količinaNaStanju = value;
            }
        }

        public int OčekivanaKoličina 
        { 
            get => očekivanaKoličina;
            set
            {
                if (value < 0)
                    throw new FormatException("Neispravno unesena očekivana količina!");
                else if (value > 10)
                    throw new FormatException("Štand ne dopušta više od 10 novih proizvoda!");

                očekivanaKoličina = value;
            }
        }
        
        public DateTime DatumPristizanja 
        { 
            get => datumPristizanja;
            set
            {
                if (value > DateTime.Now)
                    throw new FormatException("Datum pristizanja robe ne može biti u budućnosti!");

                datumPristizanja = value;
            }
        }
        
        public DateTime DatumOčekivaneKoličine 
        { 
            get => datumOčekivaneKoličine;
            set
            {
                if (value < DateTime.Now)
                    throw new FormatException("Datum pristizanja robe ne može biti u prošlosti!");

                datumOčekivaneKoličine = value;
            }
        }
        
        public double CijenaProizvoda 
        { 
            get => cijenaProizvoda;
            set
            {
                if (value < 0.1)
                    throw new FormatException("Proizvod ne može biti besplatan!");
                else if (value > 25.0)
                    throw new FormatException("Pijaca nije namijenjena za skupe proizvode!");

                cijenaProizvoda = value;
            }
        }
        
        public bool Certifikat387 
        { 
            get => certifikat387;
            set
            {
                if (šifraProizvoda.StartsWith("387"))
                    if (value == false)
                        throw new FormatException("Domaći proizvod se mora označiti kao takav!");
                    else
                        certifikat387 = true;
                else if (value == true)
                    throw new FormatException("Strani proizvod se ne smije označiti kao domaći!!");
                else
                    certifikat387 = false;
            }
        }

        #endregion

        #region Konstruktor

        public Proizvod(Namirnica vrsta, string ime, int kolicina, DateTime datum, double cijena, bool domaci)
        {
            šifraProizvoda = GenerišiŠifru(domaci);
            Certifikat387 = domaci;
            VrstaNamirnice = vrsta;
            Ime = ime;
            KoličinaNaStanju = kolicina;
            OčekivanaKoličina = 0;
            DatumPristizanja = datum;
            CijenaProizvoda = cijena;
        }

        #endregion

        #region Metode

        /// <summary>
        /// Metoda koja vrši generisanje šifre za proizvod.
        /// Potrebno je generisati šifru formata:
        /// šifra države - trenutna vrijednost brojača - kontrolna cifra
        /// Ukoliko je proizvod domaći, šifra države je 387, u suprotnom je šifra 111.
        /// Nakon upotrebe trenutne vrijednosti brojača, potrebno je uvećati brojač za 1.
        /// Kontrolna cifra je jednaka ostatku pri dijeljenju zbiru svih cifara sa 10.
        /// Generisana šifra se vraća kao rezultat funkcije.
        /// </summary>
        /// <param name="domaći"></param>
        /// <returns></returns>
        public string GenerišiŠifru(bool domaći)
        {
            throw new NotImplementedException();
        }

        public void NaručiKoličinu(int količina, DateTime očekivaniDatumPristizanja)
        {
            OčekivanaKoličina += količina;
            DatumOčekivaneKoličine = očekivaniDatumPristizanja;
        }

        #endregion
    }
}
