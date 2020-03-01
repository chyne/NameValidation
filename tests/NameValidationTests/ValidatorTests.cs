namespace NameValidationTests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NameValidation;
    using System.Collections.Generic;

    [TestClass]
    public class ValidatorTests
    {
        private static readonly IEnumerable<ICollection<string>> Alternates = new[]
        {
            new[] {"ROBERT", "ROB", "BOB"}
        };

        private readonly Validator _validator = new Validator(Alternates);
        private readonly Parser _parser = new Parser();
        private readonly RegistryParser _registryParser = new RegistryParser();

        [TestMethod]
        public void Test1()
        {
            var result = _validator.Validate(new [] {"PERRY"},
                _registryParser.Parse("C[PERRY]"),
                "PERRY");

            Assert.AreEqual(0, result[0].Distance);
        }

        [TestMethod]
        public void Test2()
        {
            var result = _validator.Validate(
                _parser.Parse("Bob M.", "Perry-Smith"),
                _registryParser.Parse("C[ROBERT] [MICHAEL]", "C[PERRYSMITH] [ ] A[PERRY] [SMITH]"));

            var given = result.BestGivenNameResults;
            var surname = result.BestSurnameResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Test3()
        {
            var result = _validator.Validate(
                _parser.Parse("Edith.M", "Perry"),
                _registryParser.Parse("C[EDITH] [MABEL]", "C[SMITH]"));

            Assert.AreEqual(4, result.GivenNameResults.Count);
            Assert.AreEqual(2, result.SurnameResults.Count);
        }

        [TestMethod]
        public void Test4()
        {
            var result = _validator.Validate(
                _parser.Parse("Edith.M", "Perry"),
                _registryParser.Parse("C[LINDA] [MABEL]", "C[SMITH]"));

            Assert.AreEqual(4, result.GivenNameResults.Count);
            Assert.AreEqual(2, result.SurnameResults.Count);
        }

        [TestMethod]
        public void Test5()
        {
            var result = _validator.Validate(
                _parser.Parse("M", "Perry"),
                _registryParser.Parse("C[LINDA] [MABEL]", "C[SMITH]"));

            Assert.AreEqual(2, result.GivenNameResults.Count);
            Assert.AreEqual(2, result.SurnameResults.Count);
        }
    }
}
