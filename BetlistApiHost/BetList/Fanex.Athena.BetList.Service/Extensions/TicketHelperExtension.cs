namespace Fanex.Athena.BetList.Service.Extensions
{
    using System;
    using Fanex.Athena.Common.Constant;
    using Fanex.Athena.Resources;
    using Fanex.BetList.Core.Entities;

    public static class TicketHelperExtension
    {
        [CLSCompliant(false)]
        public static string GetValidOddsTypeNameById(this ITicketHelper ticketHelper, object oddsTypeId)
        {
            string oddsType = oddsTypeId?.ToString().Trim();

            if (oddsType == null || oddsType == string.Empty || oddsType == "0")
            {
                return string.Empty;
            }

            if (oddsType == OddsType.Hongkong)
            {
                return ViewResources.HKOdds;
            }

            return ticketHelper.GetOddsTypeNameById(oddsTypeId);
        }
    }
}