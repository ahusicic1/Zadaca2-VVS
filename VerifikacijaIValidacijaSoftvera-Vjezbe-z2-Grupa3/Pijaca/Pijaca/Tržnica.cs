using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pijaca
{
    public class Tržnica
    {
        #region Atributi

        List<Prodavač> prodavači;
        List<Štand> štandovi;
        double ukupniPrometPijace;

        #endregion

        #region Properties

        public List<Prodavač> Prodavači { get => prodavači;}
        public List<Štand> Štandovi { get => štandovi; }
        public double UkupniPrometPijace { get => ukupniPrometPijace; }

        #endregion

        #region Konstruktor

        public Tržnica()
        {
            prodavači = new List<Prodavač>();
            štandovi = new List<Štand>();
            ukupniPrometPijace = 0.0;
        }

        #endregion

        #region Metode

        public void RadSaProdavačima(Prodavač p, string opcija)
        {
            if (p == null)
                throw new ArgumentNullException("Morate unijeti informacije o prodavaču!");

            List<Prodavač> postojeći = prodavači.FindAll(prod => prod.Ime == p.Ime);
            if (opcija == "Dodavanje")
            {
                if (postojeći.Count > 0)
                    throw new InvalidOperationException("Nemoguće dodati prodavača kad već postoji registrovan!");
                else
                    prodavači.Add(p);
            }
            else if (opcija == "Izmjena" || opcija == "Brisanje")
            {
                if (postojeći.Count < 1)
                    throw new FormatException("Nemoguće izmijeniti tj. obrisati prodavača koji nije registrovan!");
                else
                {
                    prodavači.RemoveAll(prod => postojeći.Contains(prod));
                    if (opcija == "Izmjena")
                        prodavači.Add(p);
                }
            }
            else
                throw new InvalidOperationException("Unijeli ste nepoznatu opciju!");
        }

        public void OtvoriŠtand(Prodavač p, List<Proizvod> pr, DateTime rok)
        {
            if (!prodavači.Contains(p))
                throw new ArgumentException("Prodavač nije registrovan!");
            if (štandovi.Find(š => š.Prodavač == p) != null)
                throw new FormatException("Prodavač može imati samo jedan štand!");
            Štand štand = new Štand(p, rok, pr);
            štandovi.Add(štand);
        }

        /// <summary>
        /// Metoda koja zatvara sve štandove čiji prodavači nemaju aktivan promet.
        /// Potrebno je pronaći sve prodavače koji nisu aktivni i sve njihove štandove,
        /// kao i sve štandove čiji je rok zakupa istekao.
        /// Ukoliko je ukupni promet nekog prodavača koji nije aktivan veći od 100,000 KM
        /// i nije mu istekao zakup, neće biti obrisan i biti će mu data još jedna prilika.
        /// U suprotnom, potrebno je obrisati sve štandove koji ispunjavaju gore navedene
        /// uslove.
        /// U slučaju da ne postoji nijedan registrovan prodavač i/ili štand, potrebno je samo
        /// završiti rad metode.
        /// </summary>
        public void ZatvoriSveNeaktivneŠtandove()
        {
            throw new NotImplementedException();
        }

        public void IzvršavanjeKupovina(Štand š, List<Kupovina> kupovine, string sigurnosniKod)
        {
            Štand štand = štandovi.Find(št => št.Prodavač == š.Prodavač);
            if (štand == null)
                throw new ArgumentException("Unijeli ste štand koji nije registrovan!");

            DateTime najranijaKupovina = kupovine[0].DatumKupovine, najkasnijaKupovina = kupovine[0].DatumKupovine;
            double ukupanPromet = 0;

            foreach (var kupovina in kupovine)
            {
                if (kupovina.DatumKupovine < najranijaKupovina)
                    najranijaKupovina = kupovina.DatumKupovine;
                if (kupovina.DatumKupovine > najkasnijaKupovina)
                    najkasnijaKupovina = kupovina.DatumKupovine;
                ukupanPromet += kupovina.UkupnaCijena;

                štand.RegistrujKupovinu(kupovina);
            }

            Prodavač prodavač = štand.Prodavač;
            prodavač.RegistrujPromet(sigurnosniKod, ukupanPromet, najranijaKupovina, najkasnijaKupovina);
        }

        public void DodajTipskeNamirnice(Namirnica vrsta, bool svi = false)
        {
            throw new NotImplementedException();
        }

        public void NaručiProizvode(Štand štand, List<Proizvod> proizvodi, List<int> količine, List<DateTime> rokovi, bool svi = false)
        {
            if (proizvodi.Count != količine.Count || proizvodi.Count != rokovi.Count)
                throw new ArgumentException("Pogrešan unos parametara!");

            for (int i = 0; i < proizvodi.Count; i++)
            {
                if (!svi)
                {
                    Proizvod pr = štand.Proizvodi.Find(p => p.ŠifraProizvoda == proizvodi[i].ŠifraProizvoda);
                    if (pr == null)
                        throw new ArgumentException("Nemoguće naručiti proizvod - nije registrovan na štandu!");
                    
                    pr.NaručiKoličinu(količine[i], rokovi[i]);
                }
                else
                {
                    foreach (var š in štandovi)
                    {
                        var pr = š.Proizvodi.Find(p => p.ŠifraProizvoda == proizvodi[i].ŠifraProizvoda);
                        if (pr != null)
                            pr.NaručiKoličinu(količine[i], rokovi[i]);
                    }
                }
            }
        }

        public void IzvršiInspekciju(IInspekcija inspekcija)
        {
            int brojNeispravnihŠtandova = 0;

            foreach (Štand š in štandovi)
            {
                if (!inspekcija.ŠtandIspravan(š))
                    brojNeispravnihŠtandova++;
            }

            if (brojNeispravnihŠtandova > 10)
            {
                štandovi.Clear();
                prodavači.Clear();
                ukupniPrometPijace = 0;
            }
        }

        #endregion
    }
}
