using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pijaca;
using System;
using System.Collections.Generic;

namespace TestProjectG3
{
    [TestClass]
    public class TestClassTrznica
    {

        [TestMethod]
        public void TestZatvoriSveNeaktivneStandove()
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

    [TestClass]
    public class TestClassProdavac
    {
        //Adna Husičić
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRegistrujPrometIzuzetak1()
        {
            Prodavač p = new Prodavač("prodavac", "000", DateTime.Parse("01/07/2021"), 0);
            p.RegistrujPromet("123", 0, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRegistrujPrometIzuzetak2()
        {
            Prodavač p = new Prodavač("prodavac", "000", DateTime.Parse("01/07/2021"), 0);
            p.RegistrujPromet("000", 900, DateTime.Now.AddDays(-5), DateTime.Now);
            p.RegistrujPromet("000", 900, DateTime.Now.AddDays(-1), DateTime.Now);
            p.RegistrujPromet("000", 3000, DateTime.Now.AddDays(-5), DateTime.Now);
            p.RegistrujPromet("000", 3000, DateTime.Now.AddDays(-1), DateTime.Now);
        }

        [TestMethod]
        public void TestRegistrujPrometAktivnost()
        {
            Prodavač prodavac1 = new Prodavač("prodavac", "000", DateTime.Parse("01/07/2021"), 0);
            Prodavač prodavac2 = new Prodavač("prodavac", "001", DateTime.Parse("01/07/2021"), 0);
            Assert.IsTrue(prodavac1.Aktivnost);

            prodavac1.RegistrujPromet("000", 0, DateTime.Parse("01/07/2021"), DateTime.Parse("01/09/2021"));
            prodavac2.RegistrujPromet("001", 1000, DateTime.Parse("01/07/2021"), DateTime.Parse("10/07/2021"));
            Assert.IsFalse(prodavac1.Aktivnost);
            Assert.IsTrue(prodavac2.Aktivnost);
            Assert.AreEqual(prodavac2.UkupniPromet, 1000);

        }
    }

    [TestClass]
    public class TestClassProizvod
    {

    }

    [TestClass]
    public class TestClassKupovina
    {

    }

    [TestClass]
    public class TestClassStand
    {

    }
}
