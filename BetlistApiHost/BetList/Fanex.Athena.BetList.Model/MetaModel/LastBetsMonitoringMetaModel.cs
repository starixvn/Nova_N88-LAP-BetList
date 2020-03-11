// -----------------------------------------------------------------------
// <copyright file="LastBetsMonitoringMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.BetList.Model.MetaModel
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class LastBetsMonitoringMetaModel
    {
        private const int DefaultPageSize = 10;

        public LastBetsMonitoringMetaModel()
        {
            PageSize = DefaultPageSize;
        }

        public int SportId { get; set; }

        public int LeagueId { get; set; }

        public int MatchId { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BetType { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Level { get; set; }

        public int LiveStatus { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string OddsType { get; set; }

        public int TimeZone { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string Currency { get; set; }

        [DisplayFormat(ConvertEmptyStringToNull = false)]
        public string BasedCurrency { get; set; }

        public int StakeFrom { get; set; }

        public int StakeTo { get; set; }

        public int PageSize { get; set; }

        public long TransId { get; set; }

        public int BetFrom { get; set; }

        public string AgentListXml { get; set; }

    }
}