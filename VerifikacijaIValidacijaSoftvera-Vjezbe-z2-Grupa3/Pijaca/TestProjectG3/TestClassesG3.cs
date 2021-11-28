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
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRadSaProdavačimaProdavačNull()
        {
            Tržnica trznica = new Tržnica();
            trznica.RadSaProdavačima(null, "Dodavanje");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRadSaProdavačimaDodavanjePostojećeg()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prodavač", "1234", new DateTime(2021, 1, 31), 0);
           
            trznica.RadSaProdavačima(prodavac1, "Dodavanje");
            trznica.RadSaProdavačima(prodavac1, "Dodavanje");
        }

        [TestMethod]
        public void TestRadSaProdavačima()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prodavač", "1234", new DateTime(2021, 1, 31), 0);

            Assert.AreEqual(trznica.Prodavači.Count, 0);
            trznica.RadSaProdavačima(prodavac1, "Dodavanje");
            Assert.AreEqual(trznica.Prodavači.Count, 1);

            prodavac1.Ime = "Neko";
            trznica.RadSaProdavačima(prodavac1, "Izmjena");
            Assert.AreNotEqual(trznica.Prodavači[0].Ime, "Prodavač");
            StringAssert.Contains(trznica.Prodavači[0].Ime, "Neko");

            trznica.RadSaProdavačima(prodavac1, "Brisanje");
            Assert.AreEqual(trznica.Prodavači.Count, 0);

        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestRadSaProdavačimaBrisanjeIzuzetak()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prodavač", "1234", new DateTime(2021, 1, 31), 0);
            trznica.RadSaProdavačima(prodavac1, "Brisanje");
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestRadSaProdavačimaIzmjenaIzuzetak()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prodavač", "1234", new DateTime(2021, 1, 31), 0);
            trznica.RadSaProdavačima(prodavac1, "Izmjena");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRadSaProdavačimaNepoznataOpcija()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prodavač", "1234", new DateTime(2021, 1, 31), 0);
            trznica.RadSaProdavačima(prodavac1, "Test");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestOtvoriŠtandIzuzetak1()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prodavač", "1234", new DateTime(2021, 1, 31), 0);
            trznica.OtvoriŠtand(prodavac1, new List<Proizvod>(), new DateTime(2022, 12, 31));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestOtvoriŠtandIzuzetak2()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prodavač", "1234", new DateTime(2021, 1, 31), 0);
            trznica.RadSaProdavačima(prodavac1, "Dodavanje");
            Štand štand = new Štand(prodavac1, new DateTime(2022, 12, 31));
            trznica.OtvoriŠtand(prodavac1, new List<Proizvod>(), new DateTime(2022, 12, 31));
            trznica.OtvoriŠtand(prodavac1, new List<Proizvod>(), new DateTime(2022, 12, 31));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestIzvršavanjeKupovinaIzuzetak1()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prodavač", "1234", new DateTime(2021, 1, 31), 0);
            Štand štand = new Štand(prodavac1, new DateTime(2022, 12, 31));
      
            trznica.IzvršavanjeKupovina(štand, new List<Kupovina>(), "");
        }

        [TestMethod]
        public void TestIzvršavanjeKupovina()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prodavač", "1234", new DateTime(2021, 1, 31), 0);
            Štand štand = new Štand(prodavac1, new DateTime(2022, 12, 31));
        
            Proizvod kupus = new Proizvod(Namirnica.Povrće, "kupus", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod paradajz = new Proizvod(Namirnica.Povrće, "paradajz", 3, new DateTime(2021, 3, 31), 2, false);
            Kupovina k1 = new Kupovina(kupus, 1);
            Kupovina k2 = new Kupovina(paradajz, 2);

            List<Kupovina> kupovine = new List<Kupovina>() {  k2 };
            trznica.RadSaProdavačima(prodavac1, "Dodavanje");
            trznica.OtvoriŠtand(prodavac1, new List<Proizvod>(), new DateTime(2022, 3, 31));
            trznica.IzvršavanjeKupovina(štand, kupovine, "1234");
            Assert.AreEqual(trznica.Prodavači[0].UkupniPromet, 4);

            Kupovina k3 = new Kupovina(paradajz, 2);
            kupovine.Add(k3);
            kupovine.Add(k1);
            trznica.IzvršavanjeKupovina(štand, kupovine, "1234");
            Assert.AreEqual(trznica.Prodavači[0].UkupniPromet, 14);
        }

        [TestMethod]
        public void TestDodajTipskeNamirnice()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("ProdavačA", "1234", new DateTime(2021, 1, 31), 0);
            Prodavač prodavac2 = new Prodavač("ProdavačB", "4567", new DateTime(2021, 1, 31), 0);
            Prodavač prodavac3 = new Prodavač("ProdavačC", "6789", new DateTime(2021, 1, 31), 0);
            Štand štand1 = new Štand(prodavac1, new DateTime(2022, 12, 31));
            Štand štand2 = new Štand(prodavac2, new DateTime(2022, 12, 31));
            Štand štand3 = new Štand(prodavac3, new DateTime(2022, 12, 31));

            Proizvod kupus = new Proizvod(Namirnica.Povrće, "kupus", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod paradajz = new Proizvod(Namirnica.Povrće, "paradajz", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod jabuka = new Proizvod(Namirnica.Voće, "jabuka", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod meso= new Proizvod(Namirnica.Meso, "meso", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod zitarica = new Proizvod(Namirnica.Žitarica, "zitarica", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod poluproizvod = new Proizvod(Namirnica.Poluproizvod, "poluproizvod", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod ostalo = new Proizvod(Namirnica.Ostalo, "ostalo", 3, new DateTime(2021, 3, 31), 2, false);

            trznica.RadSaProdavačima(prodavac1, "Dodavanje");
            trznica.RadSaProdavačima(prodavac2, "Dodavanje");
            trznica.RadSaProdavačima(prodavac3, "Dodavanje");
            trznica.OtvoriŠtand(prodavac1, new List<Proizvod>() { meso, paradajz, jabuka}, new DateTime(2022, 3, 31));
            trznica.OtvoriŠtand(prodavac2, new List<Proizvod>() { poluproizvod}, new DateTime(2022, 3, 31));
            trznica.OtvoriŠtand(prodavac3, new List<Proizvod>() { kupus, jabuka}, new DateTime(2022, 3, 31));

            List<Proizvod> proizvodi = new List<Proizvod>() { kupus, paradajz, jabuka, meso, zitarica, poluproizvod, ostalo };
            trznica.DodajTipskeNamirnice(Namirnica.Meso, true);
            Assert.IsTrue(trznica.Štandovi[0].Proizvodi.Find(p => p.VrstaNamirnice == Namirnica.Meso) != null);

            trznica.DodajTipskeNamirnice(Namirnica.Voće, true);
            Assert.IsTrue(trznica.Štandovi[0].Proizvodi.Find(p => p.Ime == "Šljiva") != null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDodajTipskeNamirniceIzuzetak1()
        {
            Tržnica trznica = new Tržnica();
            trznica.DodajTipskeNamirnice(Namirnica.Ostalo, false);

        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDodajTipskeNamirniceIzuzetak2()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("ProdavačA", "1234", new DateTime(2021, 1, 31), 0);
            Štand štand1 = new Štand(prodavac1, new DateTime(2022, 12, 31));
            Proizvod meso = new Proizvod(Namirnica.Meso, "meso", 3, new DateTime(2021, 3, 31), 2, false);
            trznica.RadSaProdavačima(prodavac1, "Dodavanje");
           
            trznica.OtvoriŠtand(prodavac1, new List<Proizvod>() { meso}, new DateTime(2022, 3, 31));
            trznica.DodajTipskeNamirnice(Namirnica.Meso, true);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDodajTipskeNamirniceIzuzetak3()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("ProdavačA", "1234", new DateTime(2021, 1, 31), 0);
            Štand štand1 = new Štand(prodavac1, new DateTime(2022, 12, 31));
            Proizvod jabuka = new Proizvod(Namirnica.Voće, "jabuka", 3, new DateTime(2021, 3, 31), 2, false);
            trznica.RadSaProdavačima(prodavac1, "Dodavanje");

            trznica.OtvoriŠtand(prodavac1, new List<Proizvod>() { jabuka }, new DateTime(2022, 3, 31));
            trznica.DodajTipskeNamirnice(Namirnica.Voće, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNaruciProizvodeIzuzetak1()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("ProdavačA", "1234", new DateTime(2021, 1, 31), 0);
            Štand štand1 = new Štand(prodavac1, new DateTime(2022, 12, 31));
            Proizvod jabuka = new Proizvod(Namirnica.Voće, "jabuka", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod zitarica = new Proizvod(Namirnica.Žitarica, "zitarica", 3, new DateTime(2021, 3, 31), 2, false);

            trznica.NaručiProizvode(štand1, new List<Proizvod>() { jabuka, zitarica }, new List<int>() { 1 }, new List<DateTime>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNaruciProizvodeIzuzetak2()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("ProdavačA", "1234", new DateTime(2021, 1, 31), 0);
            Štand štand1 = new Štand(prodavac1, new DateTime(2022, 12, 31));
            Proizvod jabuka = new Proizvod(Namirnica.Voće, "jabuka", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod zitarica = new Proizvod(Namirnica.Žitarica, "zitarica", 3, new DateTime(2021, 3, 31), 2, false);

            trznica.NaručiProizvode(štand1, new List<Proizvod>() { jabuka, zitarica }, new List<int>() { 1 ,1}, new List<DateTime>());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestNaruciProizvodeIzuzetak3()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("ProdavačA", "1234", new DateTime(2021, 1, 31), 0);
            Štand štand1 = new Štand(prodavac1, new DateTime(2022, 12, 31));
            Proizvod jabuka = new Proizvod(Namirnica.Voće, "jabuka", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod zitarica = new Proizvod(Namirnica.Žitarica, "zitarica", 3, new DateTime(2021, 3, 31), 2, false);

            trznica.NaručiProizvode(štand1, new List<Proizvod>() { jabuka, zitarica }, new List<int>() { 1, 1 }, new List<DateTime>() { new DateTime(2021, 1, 31), new DateTime(2021, 1, 31) });
       
        }

        [TestMethod]
        public void TestNaruciProizvode1()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("ProdavačA", "1234", new DateTime(2021, 1, 31), 0);
            //Štand štand1 = new Štand(prodavac1, new DateTime(2022, 12, 31));
            Proizvod jabuka = new Proizvod(Namirnica.Voće, "jabuka", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod zitarica = new Proizvod(Namirnica.Žitarica, "zitarica", 3, new DateTime(2021, 3, 31), 2, false);
            trznica.RadSaProdavačima(prodavac1, "Dodavanje");

            trznica.OtvoriŠtand(prodavac1, new List<Proizvod>() { jabuka, zitarica }, new DateTime(2022, 3, 31));
            trznica.NaručiProizvode(trznica.Štandovi[0], new List<Proizvod>() { jabuka, zitarica }, new List<int>() { 1, 1 }, new List<DateTime>() { DateTime.Now.AddDays(3), DateTime.Now.AddDays(3) });


            Assert.AreEqual(trznica.Štandovi.Count, 1);
            Assert.AreEqual(trznica.Štandovi[0].Proizvodi.Count, 2);
            Assert.AreEqual(trznica.Štandovi[0].Proizvodi[0].OčekivanaKoličina, 1);
            Assert.AreEqual(trznica.Štandovi[0].Proizvodi[0].DatumOčekivaneKoličine.Day, DateTime.Now.AddDays(3).Day);
            Assert.AreEqual(trznica.Štandovi[0].Proizvodi[0].DatumOčekivaneKoličine.Month, DateTime.Now.AddDays(3).Month);
            Assert.AreEqual(trznica.Štandovi[0].Proizvodi[0].DatumOčekivaneKoličine.Year, DateTime.Now.AddDays(3).Year);
        }
    

        [TestMethod]
        public void TestNaruciProizvode2()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("ProdavačA", "1234", new DateTime(2021, 1, 31), 0);
            //Štand štand1 = new Štand(prodavac1, new DateTime(2022, 12, 31));
            Proizvod jabuka = new Proizvod(Namirnica.Voće, "jabuka", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod zitarica = new Proizvod(Namirnica.Žitarica, "zitarica", 3, new DateTime(2021, 3, 31), 2, false);
            trznica.RadSaProdavačima(prodavac1, "Dodavanje");

            trznica.OtvoriŠtand(prodavac1, new List<Proizvod>() { jabuka, zitarica }, new DateTime(2022, 3, 31));
            trznica.NaručiProizvode(trznica.Štandovi[0], new List<Proizvod>() { jabuka, zitarica }, new List<int>() { 1, 1 }, new List<DateTime>() { DateTime.Now.AddDays(3), DateTime.Now.AddDays(3) }, true);

            Assert.AreEqual(trznica.Štandovi.Count, 1);
            Assert.AreEqual(trznica.Štandovi[0].Proizvodi.Count, 2);
            Assert.AreEqual(trznica.Štandovi[0].Proizvodi[0].OčekivanaKoličina, 1);
            Assert.AreEqual(trznica.Štandovi[0].Proizvodi[1].OčekivanaKoličina, 1);
        }

        [TestMethod]
        public void IzvršiInspekciju1()
        {
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("prodA", "1234", new DateTime(2021, 1, 31), 0);
            List<Prodavač> prodavaciNeispravni = new List<Prodavač>();
            for(int i=0; i<12; i++)
            {
                string naziv = "Prodavač" + i.ToString();
                string broj = "1" + i.ToString();
                prodavaciNeispravni.Add(new Prodavač(naziv, broj, new DateTime(2021, 1, 31), 0));
            }

            List<Štand> standovi = new List<Štand>();
       

            Štand štand1 = new Štand(prodavac1, new DateTime(2022, 12, 31));
            Inspekcija inspekcija = new Inspekcija();

            Proizvod kupus = new Proizvod(Namirnica.Povrće, "kupus", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod paradajz = new Proizvod(Namirnica.Povrće, "paradajz", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod jabuka = new Proizvod(Namirnica.Voće, "jabuka", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod meso = new Proizvod(Namirnica.Meso, "meso", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod zitarica = new Proizvod(Namirnica.Žitarica, "zitarica", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod poluproizvod = new Proizvod(Namirnica.Poluproizvod, "poluproizvod", 3, new DateTime(2021, 3, 31), 2, false);
            Proizvod ostalo = new Proizvod(Namirnica.Ostalo, "ostalo", 3, new DateTime(2021, 3, 31), 2, false);

            trznica.RadSaProdavačima(prodavac1, "Dodavanje");
       
            trznica.OtvoriŠtand(prodavac1, new List<Proizvod>() { meso, paradajz, jabuka }, new DateTime(2022, 3, 31));
  

            Kupovina k2 = new Kupovina(paradajz, 2);

            List<Kupovina> kupovine = new List<Kupovina>() { k2 };
            trznica.IzvršavanjeKupovina(trznica.Štandovi[0], kupovine, "1234");
            
            Assert.IsFalse(trznica.Štandovi.Count== 0);
            Assert.IsFalse(trznica.Prodavači.Count==0);
            for (int i = 1; i < 12; i++)
            {
                 trznica.RadSaProdavačima(prodavaciNeispravni[i], "Dodavanje");
                trznica.OtvoriŠtand(prodavaciNeispravni[i], new List<Proizvod>() { meso, paradajz, jabuka }, new DateTime(2022, 3, 31));
            }
            trznica.IzvršiInspekciju(inspekcija);
            Assert.IsTrue(trznica.UkupniPrometPijace == 0);
            Assert.AreEqual(trznica.Štandovi.Count, 0);
            Assert.AreEqual(trznica.Prodavači.Count, 0);


        }




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
        static IEnumerable<object[]> ProizvodiNeispravniCSV
        {
            get
            {
                return UcitajProizvodeCSV();
            }
        }

        static IEnumerable<object[]> ProizvodiIspravniXML
        {
            get
            {
                return UcitajProizvodeXML();
            }
        }

        [TestMethod]
        [DynamicData("ProizvodiNeispravniCSV")]
        [ExpectedException(typeof(FormatException))]
        public void TestKonstruktoraProizvodCSV(string ime, int kolicina, DateTime datum, double cijena)
        {
            Proizvod proizvod = new Proizvod(Namirnica.Voće, ime, kolicina, datum, cijena, true);
        }

        [TestMethod]
        [DynamicData("ProizvodiIspravniXML")]
        public void TestKonstruktoraProizvodXML(string ime, int kolicina, DateTime datum, double cijena, bool domaci)
        {
            Proizvod proizvod = new Proizvod(Namirnica.Povrće, ime, kolicina, datum, cijena, domaci);
            Assert.AreEqual(proizvod.CijenaProizvoda, cijena);
            Assert.AreEqual(proizvod.KoličinaNaStanju, kolicina);
            Assert.AreEqual(proizvod.Certifikat387, domaci);
            StringAssert.StartsWith(proizvod.Ime, ime);
            StringAssert.EndsWith(proizvod.Ime, ime);
        }

        public static IEnumerable<object[]> UcitajProizvodeCSV()
        {
            using (var reader = new StreamReader("proizvodiNeispravni.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var rows = csv.GetRecords<dynamic>();
                foreach (var row in rows)
                {
                    var values = ((IDictionary<String, Object>)row).Values;
                    var elements = values.Select(elem => elem.ToString()).ToList();
                    yield return new object[] { elements[0], Convert.ToInt32(elements[1]), DateTime.Parse(elements[2]), Convert.ToDouble(elements[3]) };
                }
            }
        }

        public static IEnumerable<object[]> UcitajProizvodeXML()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("proizvodiIspravni.xml");
            foreach (XmlNode node in doc.DocumentElement.ChildNodes)
            {
                List<string> elements = new List<string>();
                foreach (XmlNode innerNode in node)
                {
                    elements.Add(innerNode.InnerText);
                }
                yield return new object[] { elements[0], Convert.ToInt32(elements[1]), DateTime.Parse(elements[2]), Convert.ToDouble(elements[3]), Convert.ToBoolean(elements[4]) };
            }
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestDatumOcekivaneKolicineIzuzetak()
        {
            Proizvod paradajz = new Proizvod(Namirnica.Povrće, "paradajz", 3, new DateTime(2021, 3, 31), 2, false);
            paradajz.NaručiKoličinu(10, new DateTime(2021, 1, 1));
        }


        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestOcekivanaKolicinaIzuzetak1()
        {
            Proizvod paradajz = new Proizvod(Namirnica.Povrće, "paradajz", 3, new DateTime(2021, 3, 31), 2, false);
            paradajz.NaručiKoličinu(-5, DateTime.Now.AddMonths(3));
        }
        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestOcekivanaKolicinaIzuzetak2()
        {
            Proizvod paradajz = new Proizvod(Namirnica.Povrće, "paradajz", 3, new DateTime(2021, 3, 31), 2, false);
            paradajz.NaručiKoličinu(15, DateTime.Now.AddMonths(3));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestCertifikatIzuzetak1()
        {
            Proizvod paradajz = new Proizvod(Namirnica.Povrće, "paradajz", 3, new DateTime(2021, 3, 31), 2, true);
            paradajz.Certifikat387 = false;
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TestCertifikatIzuzetak2()
        {
            Proizvod paradajz = new Proizvod(Namirnica.Povrće, "paradajz", 3, new DateTime(2021, 3, 31), 2, false);
            paradajz.Certifikat387 = true;
        }

        [TestMethod]
        public void TestCertifikat()
        {
            Proizvod paradajz = new Proizvod(Namirnica.Povrće, "paradajz", 3, new DateTime(2021, 3, 31), 2, true);
            paradajz.Certifikat387 = true;
        }

        [TestMethod]
        public void TestGenerisanjeSifreDomaciIStrani()
        {
                //Mirza Kadrić
            Proizvod p = new Proizvod(Namirnica.Povrće, "Luk", 20, DateTime.Now, 1.5, true);
            StringAssert.StartsWith(p.ŠifraProizvoda, "387");

            p = new Proizvod(Namirnica.Voće, "Jagoda", 30, DateTime.Now, 2, false);
            StringAssert.StartsWith(p.ŠifraProizvoda, "111");
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
