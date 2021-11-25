using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pijaca
{
    public class Štand
    {
        #region Atributi

        Prodavač prodavač;
        List<Proizvod> proizvodi;
        List<Kupovina> kupovine;
        DateTime krajZakupa;

        #endregion

        #region Properties

        public Prodavač Prodavač { get => prodavač; set => prodavač = value; }
        public List<Proizvod> Proizvodi { get => proizvodi; set => proizvodi = value; }
        public List<Kupovina> Kupovine { get => kupovine; }
        public DateTime KrajZakupa { get => krajZakupa; }

        #endregion

        #region Konstruktor

        public Štand(Prodavač p, DateTime rok, List<Proizvod> pr = null)
        {
            Prodavač = p;
            if (pr != null)
                Proizvodi = pr;
            else
                Proizvodi = new List<Proizvod>();
            kupovine = new List<Kupovina>();
            krajZakupa = rok;
        }

        #endregion

        #region Metode

        public void RegistrujKupovinu(Kupovina k)
        {
            kupovine.Add(k);
        }

        #endregion
    }
}
