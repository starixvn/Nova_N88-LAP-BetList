namespace Fanex.Athena.Common.Utility.Helpers
{
    using System.Globalization;

    public static class NumberHelper
    {
        private const int EvenNumber = 2;

        private static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");

        public static string DecFormat(decimal value, byte digit = 2)
        {
            var format = "N" + digit.ToString();
            return value.ToString(format, DefaultCulture.NumberFormat);
        }

        public static decimal ToPercentage(this decimal value)
        {
            const int PercentageNumber = 100;
            return value * PercentageNumber;
        }

        public static bool IsEvenNumber(this decimal value)
        {
            return value % EvenNumber == 0;
        }

        public static bool IsEvenNumber(this int value)
        {
            return value % EvenNumber == 0;
        }

        public static int ToInt(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}