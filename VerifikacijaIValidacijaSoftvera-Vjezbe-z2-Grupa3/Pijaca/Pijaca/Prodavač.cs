using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pijaca
{
    public class Prodavač
    {
        #region Atributi

        string ime, sigurnosniKod;
        DateTime otvaranjeŠtanda;
        bool aktivnost;
        double ukupniPromet;

        #endregion

        #region Properties

        public string Ime 
        { 
            get => ime;
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new InvalidOperationException("Neispravno uneseno ime prodavača!");
                ime = value;
            }
        }
        public DateTime OtvaranjeŠtanda 
        { 
            get => otvaranjeŠtanda;
            set
            {
                if (Math.Abs((DateTime.Now - value).TotalDays) < 30)
                    throw new InvalidOperationException("Štand nije moguće registrovati dok ne prođe barem mjesec dana!");
                else if (Math.Abs((DateTime.Now - value).TotalDays) > 365)
                    throw new InvalidOperationException("Štand nije moguće registrovati - protekao period dozvoljene registracije!");
                otvaranjeŠtanda = value;
            }
        }
        public bool Aktivnost { get => aktivnost; set => aktivnost = value; }
        public double UkupniPromet { get => ukupniPromet; }

        #endregion

        #region Konstruktor

        public Prodavač(string ime, string sigurnosnaŠifra, DateTime otvaranje, double dosadašnjiPromet)
        {
            Ime = ime;
            sigurnosniKod = sigurnosnaŠifra;
            OtvaranjeŠtanda = otvaranje;
            aktivnost = true;
            if (dosadašnjiPromet < 0)
                throw new InvalidOperationException("Promet ne može biti negativan!");
            ukupniPromet = dosadašnjiPromet;
        }

        #endregion

        #region Metode

        /// <summary>
        /// Metoda koja registruje promet prodavača.
        /// Ukoliko se poslana šifra ne poklapa sa sigurnosnim kodom prodavača, potrebno je baciti izuzetak.
        /// Ukoliko je poslani period duži od mjesec dana a ukupni promet je jednak 0 KM,
        /// potrebno je označiti prodavača neaktivnim.
        /// Ukoliko je poslani period kraći od dva dana a registrovani promet veći od 1,000 KM,
        /// potrebno je baciti izuzetak o neispravno unesenim podacima.
        /// U suprotnom je potrebno povećati ukupni promet za proslijeđenu sumu prometa.
        /// </summary>
        public void RegistrujPromet(string šifra, double promet, DateTime početak, DateTime kraj)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
