﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pijaca;
using System;
using System.Collections.Generic;

namespace TestProjectG3
{
    [TestClass]
    public class UnitTest1
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

        [TestMethod]
        public void Test3ZatvoriSveNeaktivneStandove()
        {
            //Dinija Seferovic
            Tržnica trznica = new Tržnica();
            Prodavač prodavac1 = new Prodavač("Prvi", "1234", new DateTime(2021, 1, 31), 80);
            prodavac1.Aktivnost = false;
            Prodavač prodavac2 = new Prodavač("Drugi", "1235", new DateTime(2021, 2, 10), 50);
            prodavac2.Aktivnost = true;
            Proizvod proizvod = new Proizvod(Namirnica.Meso, "Piletina", 20, DateTime.Now, 10, false);
            trznica.RadSaProdavačima(prodavac1, "Dodavanje");
            trznica.RadSaProdavačima(prodavac2, "Dodavanje");
            List<Proizvod> listaProizvoda = new List<Proizvod>() { proizvod };
            trznica.OtvoriŠtand(prodavac1, listaProizvoda, new DateTime(2021, 12, 15));
            trznica.OtvoriŠtand(prodavac2, listaProizvoda, new DateTime(2021, 5, 20));
            trznica.ZatvoriSveNeaktivneŠtandove();
            CollectionAssert.AreEquivalent(trznica.Štandovi, new List<Štand>());

        }

    }
}
