using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pijaca;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TestProjectG3
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test1GenerisanjeSifreDomaci()
        {
          
        }

        [TestMethod]
        public void TestGenerisanjeSifreStrani()
        {

        }

        [TestMethod]
        public void Test3ZatvoriSveNeaktivneStandove()
        {
            //Dinija Seferovic
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prvi", "1234", new DateTime(2021, 1, 31), 80);
            prodavac1.Aktivnost = false;
            Prodavač prodavac2 = new Prodavač("Drugi", "1235", new DateTime(2021, 2, 10), 50);
            prodavac2.Aktivnost = true;
            Prodavač prodavac3 = new Prodavač("Treci", "1236", new DateTime(2021, 5, 1), 2000);
            prodavac3.Aktivnost = true;
            Prodavač prodavac4 = new Prodavač("Cetvrti", "1237", new DateTime(2021, 8, 1), 200000);
            prodavac4.Aktivnost = false;
            Proizvod proizvod = new Proizvod(Namirnica.Meso, "Piletina", 20, DateTime.Now, 10, false);

            trznica.RadSaProdavačima(prodavac1, "Dodavanje");
            trznica.RadSaProdavačima(prodavac2, "Dodavanje");
            trznica.RadSaProdavačima(prodavac3, "Dodavanje");
            trznica.RadSaProdavačima(prodavac4, "Dodavanje");

            List<Proizvod> listaProizvoda = new List<Proizvod>() { proizvod };

            trznica.OtvoriŠtand(prodavac1, listaProizvoda, new DateTime(2021, 12, 15));
            trznica.OtvoriŠtand(prodavac2, listaProizvoda, new DateTime(2021, 5, 20));
            trznica.OtvoriŠtand(prodavac3, listaProizvoda, new DateTime(2022, 1, 20));
            trznica.OtvoriŠtand(prodavac4, listaProizvoda, new DateTime(2022, 3, 21));
            
            trznica.ZatvoriSveNeaktivneŠtandove();

            Štand stand3 = new Štand(prodavac3, new DateTime(2022, 1, 20), listaProizvoda);
            Štand stand4 = new Štand(prodavac4, new DateTime(2022, 3, 21), listaProizvoda);
            List<Štand> trazeniStandovi = new List<Štand>() { stand3, stand4 };

            Assert.AreEqual(2, trznica.Štandovi.Count);
            Assert.AreEqual(stand3.Prodavač.Ime, trznica.Štandovi[0].Prodavač.Ime);
            Assert.AreEqual(stand4.Prodavač.Ime, trznica.Štandovi[1].Prodavač.Ime);

        }

    }
}
