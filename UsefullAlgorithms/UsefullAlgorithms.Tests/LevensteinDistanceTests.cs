using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UsefullAlgorithms.Helpers;

namespace UsefullAlgorithms.Tests
{
    [TestClass]
    public class LevensteinDistanceTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual(1, "ant".LevenhsteinDistance("aunt"));
            Assert.AreEqual(5, "Sam".LevenhsteinDistance("Samantha"));
            Assert.AreEqual(3, "clozapine".LevenhsteinDistance("olanzapine"));
            Assert.AreEqual(3, "flomax".LevenhsteinDistance("volmax"));
            Assert.AreEqual(3, "toradol".LevenhsteinDistance("tramadol"));
            Assert.AreEqual(3, "kitten".LevenhsteinDistance("sitting"));
        }
    }
}
