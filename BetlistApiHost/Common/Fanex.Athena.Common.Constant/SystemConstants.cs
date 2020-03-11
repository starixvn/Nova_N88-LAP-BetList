namespace Fanex.Athena.Common.Constant
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class SystemConstants
    {
        public const int SheetSizeExcel = 65000;

        public const int MaxDataTotalRow = int.MaxValue - 1000;

        public static readonly ReadOnlyCollection<string> BaseCurrencyList = new ReadOnlyCollection<string>(
            new List<string> { "EUR", "USD", "GBP", "MYR", "CNY", "THB", "IDR", "INH" });
    }
}
