using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CDS.Core;

namespace CDS.Core.Tests
{
    [TestClass]
    public class ValueWithUnitTests
    {
        public TestContext TestContext { get; set; }


        [TestMethod]
        public void CreateValueWithUnitFromString()
        {
            var v1 = new ValueWithUnit("-3333.0 mL");
            var v2 = new ValueWithUnit("-3,333.0mL ");
            var v3 = new ValueWithUnit("-3,333mL");

            Assert.AreEqual(v1, v2);
            Assert.AreEqual(v1, v3);
            Assert.AreEqual(-3333.0, v1.Value);
            Assert.AreEqual("mL", v1.Unit);

            TestContext.WriteLine($"Created Value: {v1}");
        }

        [TestMethod]
        public void CrateValueWithUnitWithoutUnitsFromString()
        {
            var v1 = new ValueWithUnit("-3333.0 ");
            var v2 = new ValueWithUnit("-3,333.0 ");
            var v3 = new ValueWithUnit("-3,333");

            Assert.AreEqual(v1, v2);
            Assert.AreEqual(v1, v3);
            Assert.AreEqual(-3333.0, v1.Value);
            Assert.IsNull(v1.Unit);

            TestContext.WriteLine($"Created Value: {v1}");
        }

        [TestMethod]
        public void ConvertUnits()
        {
            var v1 = new ValueWithUnit("-3333.0 mL");
            var v2 = v1.Convert("L");
            var v3 = v1.Convert(null);
            var v4 = v1.Convert(" ");

            Assert.AreEqual(v2.Unit, "L");
            Assert.IsNull(v3.Unit);
            Assert.AreEqual(v3, v4);
        }

        [TestMethod]
        public void ConvertVolumeValue()
        {
            var v1 = new VolumeValue("2 L");
            var v2 = v1.Convert("mL");

            Assert.AreEqual(2000, v2.Value);
            Assert.AreEqual("mL", v2.Unit);
            Assert.AreEqual(v1, v2);
        }

        [TestMethod]
        public void ConvertTimeValue()
        {
            var v1 = new TimeValue("2h");
            var v2 = v1.Convert("min");

            Assert.AreEqual(120, v2.Value);
            Assert.AreEqual("min", v2.Unit);
            Assert.AreEqual(v1, v2);
        }
    }
}
