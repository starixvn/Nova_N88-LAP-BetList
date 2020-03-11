namespace Fanex.Athena.BetList.Model.MetaModel
{
    using System.Collections.Generic;

    public class PlayerAdminBetListMetaModel : ICommonValueMetaModel
    {
        public int CustId { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        public string AgentListXml { get; set; }

        public string Currency { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
        public string BaseCurrency { get; set; } = string.Empty;
    }
}
