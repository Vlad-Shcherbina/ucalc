using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CalcTestScript
    {
        [Test]
        public void Stuff()
        {
            Calc calc = new Calc();
            calc.receive("123");
            Assert.AreEqual("123", calc.display);
        }
    }
}
