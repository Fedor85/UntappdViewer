﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using UntappdViewer.Utils;

namespace UntappdViewer.Test
{
    [TestFixture]
    public class TestStringHelper
    {
        [Test]
        public void TestGetShortName()
        {
            List<string> fullNames = new List<string>();
            fullNames.Add("LIDSKAE Nulevachka (ЛІДСКАЕ Нулёвачка)");
            fullNames.Add("Golden Bumblebee Blackberry Ale (Черничный Эль)");
            fullNames.Add("Stary Melnik Iz Bochonka Myagkoe (Старый Мельник Из Бочонка Мягкое))");
            fullNames.Add("Seven Brewers (Семь Пивоваров) Cheshskoe Original (Чешское Оригинальное)");
            fullNames.Add("Lidski Gingerbread kvass (Квас Лидский Пряничный (медово-имбирный))");
            fullNames.Add("Paulaner Hefe-Weizen / Hefe-Weißbier Naturtrüb / Natural Wheat");
            fullNames.Add("Hoegaarden Wit / Blanche 0,0 %");
            fullNames.Add("Spaten München / Münchner Hell / (Premium Lager)");

            List<string> shortNames = new List<string>();
            foreach (string fullName in fullNames)
                shortNames.Add(StringHelper.GetShortName(fullName));

            Assert.AreEqual("LIDSKAE Nulevachka", shortNames[0]);
            Assert.AreEqual("Golden Bumblebee Blackberry Ale", shortNames[1]);
            Assert.AreEqual("Stary Melnik Iz Bochonka Myagkoe", shortNames[2]);
            Assert.AreEqual("Seven Brewers Cheshskoe Original", shortNames[3]);
            Assert.AreEqual("Lidski Gingerbread kvass", shortNames[4]);
            Assert.AreEqual("Paulaner Hefe-Weizen", shortNames[5]);
            Assert.AreEqual("Hoegaarden Wit", shortNames[6]);
            Assert.AreEqual("Spaten München", shortNames[7]);
        }

        [Test]
        public void TestGeBreakForLongName()
        {
            string name1 = "Imperial Rye Porter Aged In Jack Daniels Barrels Batch #2";
            string resultName11 = StringHelper.GeBreakForLongName(name1, 30, 5);
            Assert.AreEqual("Imperial Rye Porter Aged In\n     Jack Daniels Barrels Batch #2", resultName11);

            string resultName12 = StringHelper.GeBreakForLongName(name1, 30, 0);
            Assert.AreEqual("Imperial Rye Porter Aged In\nJack Daniels Barrels Batch #2", resultName12);

            string name2 = "LIDSKAE Nulevachka";
            string resultName2 = StringHelper.GeBreakForLongName(name2, 30, 5);
            Assert.AreEqual(name2, resultName2);
        }
    }
}