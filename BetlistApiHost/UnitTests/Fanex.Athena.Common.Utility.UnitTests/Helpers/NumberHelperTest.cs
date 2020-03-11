namespace Fanex.Athena.Common.Utility.UnitTests.Helpers
{
    using System;
    using NUnit.Framework;
    using Utility.Helpers;

    [TestFixture]
    public class NumberHelperTest
    {
        [TestCase(0, "0.00")]
        [TestCase(2.5, "2.50")]
        [TestCase(2.53, "2.53")]
        [TestCase(2.534, "2.53")]
        [TestCase(2.535, "2.54")]
        [TestCase(2.5361, "2.54")]
        [TestCase(2, "2.00")]
        [TestCase(-1, "-1.00")]
        [TestCase(-2.5, "-2.50")]
        public void DecFormat_OneArgumentWithDecimalType_ReturnFormattedNumberWithTwoDigits(object value, string expected)
        {
            // Arrange
            var decimalNumber = Convert.ToDecimal(value);

            // Action
            var result = NumberHelper.DecFormat(decimalNumber);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(0, "0.000")]
        [TestCase(2.5, "2.500")]
        [TestCase(2.53, "2.530")]
        [TestCase(2.534, "2.534")]
        [TestCase(2.5354, "2.535")]
        [TestCase(2.5366, "2.537")]
        [TestCase(2, "2.000")]
        [TestCase(-1, "-1.000")]
        [TestCase(-2.5, "-2.500")]
        public void DecFormat_TwoArgumentsWithDecimalType_ReturnFormattedNumberWithThreeDigits(object value, string expected)
        {
            // Arrange
            var decimalNumber = Convert.ToDecimal(value);
            byte digit = 3;

            // Action
            var result = NumberHelper.DecFormat(decimalNumber, digit);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}