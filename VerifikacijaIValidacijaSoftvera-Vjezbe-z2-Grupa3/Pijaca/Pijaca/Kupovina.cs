using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pijaca
{
    public class Kupovina
    {
        #region Atributi

        Proizvod proizvod;
        int količina;
        double ukupnaCijena, popust;
        DateTime datumKupovine;

        #endregion

        #region Properties

        public Proizvod Proizvod { get => proizvod; }
        public int Količina { get => količina; }
        public double UkupnaCijena { get => ukupnaCijena; }
        public double Popust { get => popust; }
        public DateTime DatumKupovine { get => datumKupovine; }

        #endregion

        #region Konstruktor

        public Kupovina(Proizvod prod, int kol)
        {
            proizvod = prod;
            količina = kol;
            ukupnaCijena = proizvod.CijenaProizvoda * količina;
            popust = (ukupnaCijena * 10) % 20;
            datumKupovine = DateTime.Now;
        }

        #endregion
    }
}
