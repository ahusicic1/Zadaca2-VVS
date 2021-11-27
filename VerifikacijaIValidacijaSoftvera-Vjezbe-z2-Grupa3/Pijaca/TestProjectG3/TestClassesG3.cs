using CsvHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pijaca;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;

namespace TestProjectG3
{
    #region Testovi za klasu Trznica

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

    #endregion

    #region Testovi za klasu Prodavac

    [TestClass]
    public class TestClassProdavac
    {
        
        static IEnumerable<object[]> ProdavaciNeispravniCSV
        {
            get
            {
                return UcitajProdavaceCSV();
            }
        }

        static IEnumerable<object[]> ProdavaciIspravniXML
        {
            get
            {
                return UcitajProdavaceXML();
            }
        }

        [TestMethod]
        [DynamicData("ProdavaciNeispravniCSV")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestKonstruktoraProdavacaCSV(string ime, DateTime otvaranje, double promet)
        {
            Prodavač p = new Prodavač(ime, "100", otvaranje, promet);
        }

        [TestMethod]
        [DynamicData("ProdavaciIspravniXML")]
        public void TestKonstruktoraProdavacaXML(string ime, DateTime otvaranje, double promet)
        {
            Prodavač p = new Prodavač(ime, "110", otvaranje, promet);
            Assert.AreEqual(DateTime.Parse("01/09/2021"), p.OtvaranjeŠtanda);
        }


        public static IEnumerable<object[]> UcitajProdavaceCSV()
        {
            using (var reader = new StreamReader("prodavacineispravni.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<dynamic>();
                foreach (var row in rows)
                {
                    var values = ((IDictionary<String, Object>)row).Values;
                    var elements = values.Select(elem => elem.ToString()).ToList();
                    yield return new object[] { elements[0], DateTime.Parse(elements[1]), Convert.ToDouble(elements[2]) };
                }
            }
        }

        public static IEnumerable<object[]> UcitajProdavaceXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("prodavaciispravni.xml");
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                List<string> elements = new List<string>();
                foreach (XmlNode innerNode in node)
                {
                    elements.Add(innerNode.InnerText);
                }
                yield return new object[] { elements[0], DateTime.Parse(elements[1]), Convert.ToDouble(elements[2]) };
            }
        }


        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRegistrujPrometIzuzetak1()
        {
            //Adna Husičić
            Prodavač p = new Prodavač("prodavac", "000", DateTime.Parse("01/07/2021"), 0);
            p.RegistrujPromet("123", 0, DateTime.Now, DateTime.Now);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRegistrujPrometIzuzetak2()
        {
            //Adna Husičić
            Prodavač p = new Prodavač("prodavac", "000", DateTime.Parse("01/07/2021"), 0);
            p.RegistrujPromet("000", 900, DateTime.Now.AddDays(-5), DateTime.Now);
            p.RegistrujPromet("000", 900, DateTime.Now.AddDays(-1), DateTime.Now);
            p.RegistrujPromet("000", 3000, DateTime.Now.AddDays(-5), DateTime.Now);
            p.RegistrujPromet("000", 3000, DateTime.Now.AddDays(-1), DateTime.Now);
        }

        [TestMethod]
        public void TestRegistrujPrometAktivnost()
        {
            //Adna Husičić
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

    #endregion

    #region Testovi za klasu Stand

    [TestClass]
    public class TestClassStand
    {
        [TestMethod]
        public void TestRegistrujKupovinu()
        {
            Prodavač prodavac = new Prodavač("Prvi", "1234", new DateTime(2021, 1, 31), 80);
            Proizvod proizvod = new Proizvod(Namirnica.Povrće, "kupus", 3, new DateTime(2021, 3, 31), 2, false);
            Štand stand = new Štand(prodavac, new DateTime(2022, 1, 20), null);
            Kupovina kupovina = new Kupovina(proizvod, 2);
            List<Kupovina> trazenaLista = new List<Kupovina>() { kupovina };
            stand.RegistrujKupovinu(kupovina);
            Assert.AreEqual(trazenaLista[0].Proizvod.ŠifraProizvoda, stand.Kupovine[0].Proizvod.ŠifraProizvoda);
            Assert.AreEqual(trazenaLista.Count, stand.Kupovine.Count);
            

        }

    }

    #endregion

    #region Testovi za klasu Proizvod

    [TestClass]
    public class TestClassProizvod
    {
        [TestMethod]
        public void Test1GenerisanjeSifreDomaciIStrani()
        {
            //Mirza Kadrić
            Proizvod p = new Proizvod(Namirnica.Povrće, "Luk", 20, DateTime.Now, 1.5, true);
            Assert.AreEqual(p.ŠifraProizvoda, "387 - 1000 - 9");

            p = new Proizvod(Namirnica.Voće, "Jagoda", 30, DateTime.Now, 2, false);
            Assert.AreEqual(p.ŠifraProizvoda, "111 - 1001 - 5");
        }

    }

    #endregion

    #region Testovi za klasu Kupovina

    [TestClass]
    public class TestClassKupovina
    {
        [TestMethod]
        public void TestKupovinaKonstruktor()
        {
            Proizvod proizvod = new Proizvod(Namirnica.Voće, "jabuka", 3, new DateTime(2021, 3, 31), 2, false);
            Kupovina kupovina = new Kupovina(proizvod, 2);
            Assert.AreEqual(4, kupovina.UkupnaCijena);
            Assert.AreEqual(0, kupovina.Popust);
            Assert.AreEqual(2, kupovina.Količina);
            Assert.AreEqual(DateTime.Now.Date, kupovina.DatumKupovine.Date);
            StringAssert.Contains("jabuka", kupovina.Proizvod.Ime);
        }

    }

    #endregion


}
