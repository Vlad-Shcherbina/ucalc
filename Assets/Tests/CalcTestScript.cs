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
        public void Basic()
        {
            Calc calc = new Calc();
            calc.Receive("123");
            Assert.AreEqual("123", calc.Display);
            calc.Receive("+");
            Assert.AreEqual("123", calc.Display);
            calc.Receive("45");
            Assert.AreEqual("45", calc.Display);
            calc.Receive("=");
            Assert.AreEqual("168", calc.Display);
        }

        [Test]
        public void BackspaceWorksOnlyAfterInput()
        {
            Calc calc = new Calc();
            calc.Receive("123<");
            Assert.AreEqual("12", calc.Display);
            calc.Receive("+<");
            Assert.AreEqual("12", calc.Display);
            calc.Receive("456<");
            Assert.AreEqual("45", calc.Display);
            calc.Receive("=");
            Assert.AreEqual("57", calc.Display);
            calc.Receive("<");
            Assert.AreEqual("57", calc.Display);
        }

        [Test]
        public void Independent()
        {
            Calc calc = new Calc();
            calc.Receive("1+2=");
            Assert.AreEqual("3", calc.Display);
            calc.Receive("40-10=");
            Assert.AreEqual("30", calc.Display);
        }

        [Test]
        public void UseResult()
        {
            Calc calc = new Calc();
            calc.Receive("10-1=");
            Assert.AreEqual("9", calc.Display);
            calc.Receive("-2=");
            Assert.AreEqual("7", calc.Display);
            calc.Receive("-3=");
            Assert.AreEqual("4", calc.Display);
        }

        [Test]
        public void Overflow()
        {
            Calc calc = new Calc();
            calc.Receive("99999999+1=");
            Assert.AreEqual("overflow", calc.Display);
            calc.Receive("C");
            Assert.AreEqual("0", calc.Display);

            calc.Receive("0-9999999=");
            Assert.AreEqual("-9999999", calc.Display);
            calc.Receive("-1=");
            Assert.AreEqual("overflow", calc.Display);
        }

        [Test]
        public void Chain()
        {
            Calc calc = new Calc();
            calc.Receive("10+5-1=");
            Assert.AreEqual("14", calc.Display);

            calc = new Calc();
            calc.Receive("10+5-1+2+");
            Assert.AreEqual("16", calc.Display);
        }

        [Test]
        public void RepeatEq()
        {
            Calc calc = new Calc();
            calc.Receive("2+=");
            Assert.AreEqual("4", calc.Display);
            calc.Receive("=");
            Assert.AreEqual("6", calc.Display);
            calc.Receive("=");
            Assert.AreEqual("8", calc.Display);

            calc = new Calc();
            calc.Receive("2-=");
            Assert.AreEqual("0", calc.Display);
            calc.Receive("=");
            Assert.AreEqual("-2", calc.Display);
            calc.Receive("=");
            Assert.AreEqual("-4", calc.Display);
        }

        [Test]
        public void Double()
        {
            Calc calc = new Calc();
            calc.Receive("2+=");
            Assert.AreEqual("4", calc.Display);
            calc.Receive("+=");
            Assert.AreEqual("8", calc.Display);
            calc.Receive("+=");
            Assert.AreEqual("16", calc.Display);
        }

        [Test]
        public void ReapplySameOp()
        {
            Calc calc = new Calc();
            calc.Receive("10-2=");
            Assert.AreEqual("8", calc.Display);
            calc.Receive("100=");
            Assert.AreEqual("98", calc.Display);
            calc.Receive("1000=");
            Assert.AreEqual("998", calc.Display);
        }

        [Test]
        public void Fuzz()
        {
            var rnd = new System.Random(42);
            string choices = "1290C<+-=";
            Calc calc = new Calc();
            for (int i = 0; i < 1000000; i++)
            {
                calc.Receive(choices[rnd.Next(0, choices.Length)]);
            }
        }
    }
}
