// -----------------------------------------------------------------------
// <copyright file="ColossusBetDetailMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class ColossusBetDetailMetaModel
    {
        public int UserId { get; set; }

        public int CustId { get; set; }

        public long RefNo { get; set; }

        public string BaseCurrency { get; set; }

        public long BetCheck { get; set; }

        public string AgentListXml { get; set; }
    }
}