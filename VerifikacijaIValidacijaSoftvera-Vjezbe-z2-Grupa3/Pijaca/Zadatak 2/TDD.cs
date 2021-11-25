using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pijaca;
using System;
using System.Collections.Generic;

namespace Zadatak_2
{
    [TestClass]
    public class TDD
    {
        #region Zamjenski objekat

        [TestMethod]
        public void TestZamjenskiObjekat()
        {
            Tržnica pijaca = new Tržnica();

            for (int i = 0; i < 5; i++)
            {
                Prodavač p = new Prodavač("Ime " + (i + 1).ToString(), "šifra-" + (i + 1).ToString(),
                    DateTime.Now.AddDays(-50 - i), 500 + i);
                pijaca.RadSaProdavačima(p, "Dodavanje");
                pijaca.OtvoriŠtand(p, null, DateTime.Now.AddDays(50 + i));
            }

            for (int i = 5; i < 20; i++)
            {
                Prodavač p = new Prodavač("Ime " + (i + 1).ToString(), "šifra-" + (i + 1).ToString(),
                    DateTime.Now.AddDays(-50 - i), 500000 + i);
                pijaca.RadSaProdavačima(p, "Dodavanje");
                pijaca.OtvoriŠtand(p, null, DateTime.Now.AddDays(50 + i));
            }
            
            Inspekcija inspekcija = new Inspekcija();
            pijaca.IzvršiInspekciju(inspekcija);

            Assert.AreEqual(pijaca.Prodavači.Count, 0);
            Assert.AreEqual(pijaca.Štandovi.Count, 0);
        }

        #endregion

        #region TDD

        [TestMethod]
        public void DodavanjeMesnihProizvoda()
        {
            Prodavač p1 = new Prodavač("Prodavač mesa", "šifra-meso", DateTime.Parse("01/01/2021"), 100000);
            Tržnica pijaca = new Tržnica();
            pijaca.RadSaProdavačima(p1, "Dodavanje");
            Proizvod mesniProizvod = new Proizvod(Namirnica.Meso, "Salama", 10, DateTime.Now.AddDays(-7), 2.5, true);
            pijaca.OtvoriŠtand(p1, new List<Proizvod>() { mesniProizvod }, DateTime.Now.AddDays(60));
            Assert.AreEqual(pijaca.Prodavači.Count, 1);
            Assert.IsTrue(pijaca.Štandovi.Count == 1);
            Assert.IsTrue(pijaca.Štandovi[0].Proizvodi.Contains(mesniProizvod));
            
            pijaca.DodajTipskeNamirnice(Namirnica.Meso);

            Assert.AreEqual(pijaca.Štandovi.Count, 1);
            Assert.IsTrue(pijaca.Štandovi[0].Proizvodi.Contains(mesniProizvod));
            Assert.AreEqual(pijaca.Štandovi[0].Proizvodi.Count, 3);
            Assert.IsTrue(pijaca.Štandovi[0].Proizvodi[1].Ime == "Suho meso");
            Assert.IsTrue(pijaca.Štandovi[0].Proizvodi[2].Ime == "Pileća prsa");
        }

        [TestMethod]
        public void DodavanjeVoćaIBrisanje()
        {
            Prodavač p1 = new Prodavač("Prodavač mesa", "šifra-meso", DateTime.Parse("01/01/2021"), 100000);
            Tržnica pijaca = new Tržnica();
            pijaca.RadSaProdavačima(p1, "Dodavanje");
            Proizvod mesniProizvod = new Proizvod(Namirnica.Meso, "Salama", 10, DateTime.Now.AddDays(-7), 2.5, true);
            Proizvod voćniProizvod = new Proizvod(Namirnica.Voće, "Jabuka", 5, DateTime.Now.AddDays(-1), 1.5, true);
            pijaca.OtvoriŠtand(p1, new List<Proizvod>() { mesniProizvod, voćniProizvod }, DateTime.Now.AddDays(60));
            Assert.AreEqual(pijaca.Prodavači.Count, 1);
            Assert.IsTrue(pijaca.Štandovi.Count == 1);
            Assert.IsTrue(pijaca.Štandovi[0].Proizvodi.Contains(mesniProizvod));
            Assert.IsTrue(pijaca.Štandovi[0].Proizvodi.Contains(voćniProizvod));
            
            pijaca.DodajTipskeNamirnice(Namirnica.Voće, true);

            Assert.AreEqual(pijaca.Štandovi.Count, 1);
            Assert.IsFalse(pijaca.Štandovi[0].Proizvodi.Contains(mesniProizvod));
            Assert.IsTrue(pijaca.Štandovi[0].Proizvodi.Contains(voćniProizvod));
            Assert.AreEqual(pijaca.Štandovi[0].Proizvodi.Count, 3);
            Assert.IsTrue(pijaca.Štandovi[0].Proizvodi[1].Ime == "Šljiva");
            Assert.IsTrue(pijaca.Štandovi[0].Proizvodi[2].Ime == "Narandža");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NepodržaniTipDodavanja()
        {
            Prodavač p1 = new Prodavač("Prodavač mesa", "šifra-meso", DateTime.Parse("01/01/2021"), 100000);
            Tržnica pijaca = new Tržnica();
            pijaca.RadSaProdavačima(p1, "Dodavanje");
            Proizvod mesniProizvod = new Proizvod(Namirnica.Meso, "Salama", 10, DateTime.Now.AddDays(-7), 2.5, true);
            pijaca.OtvoriŠtand(p1, new List<Proizvod>() { mesniProizvod }, DateTime.Now.AddDays(60));

            pijaca.DodajTipskeNamirnice(Namirnica.Poluproizvod);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NemogućeDodavanjeUzBrisanje()
        {
            Prodavač p1 = new Prodavač("Prodavač mesa", "šifra-meso", DateTime.Parse("01/01/2021"), 100000);
            Tržnica pijaca = new Tržnica();
            pijaca.RadSaProdavačima(p1, "Dodavanje");
            Proizvod mesniProizvod = new Proizvod(Namirnica.Meso, "Salama", 10, DateTime.Now.AddDays(-7), 2.5, true);
            pijaca.OtvoriŠtand(p1, new List<Proizvod>() { mesniProizvod }, DateTime.Now.AddDays(60));

            pijaca.DodajTipskeNamirnice(Namirnica.Meso, true);
        }

        #endregion
    }
}
