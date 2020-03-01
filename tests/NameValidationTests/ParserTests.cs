namespace NameValidationTests
{
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NameValidation;

    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void Test1()
        {
            var parser = new Parser();

            var result = parser.ParseGivenNames("Edith.M");

            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("EDITH", result[0]);
            Assert.AreEqual("M", result[1]);
        }

        [TestMethod]
        public void TestSenior1()
        {
            var parser = new Parser();

            var result = parser.ParseGivenNames("Bob Sr");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("BOB", result[0]);
        }

        [TestMethod]
        public void TestSenior2()
        {
            var parser = new Parser();

            var result = parser.ParseGivenNames("Bob Sr.");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("BOB", result[0]);
        }

        [TestMethod]
        public void TestSenior3()
        {
            var parser = new Parser();

            var result = parser.ParseGivenNames("Bob Snr");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("BOB", result[0]);
        }

        [TestMethod]
        public void TestSenior4()
        {
            var parser = new Parser();

            var result = parser.ParseGivenNames("Bob Snr.");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("BOB", result[0]);
        }

        [TestMethod]
        public void HyphenGiven()
        {
            var parser = new Parser();

            var result = parser.ParseGivenNames("mary-anne");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("MARYANNE", result[0]);
        }

        [TestMethod]
        public void HyphenSurname()
        {
            var parser = new Parser();

            var result = parser.ParseSurnames("mary-anne");

            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.Contains("MARY"));
            Assert.IsTrue(result.Contains("ANNE"));
            Assert.IsTrue(result.Contains("MARYANNE"));
        }

        [TestMethod]
        public void RegistryParse()
        {
            var parser = new RegistryParser();

            var result = parser.Parse("C[SMITH][] O[PERRY][]");

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(n => n.Name == "SMITH" && n.NameType == NameType.Current));
            Assert.IsTrue(result.Any(n => n.Name == "PERRY" && n.NameType == NameType.Original));
        }
    }
}