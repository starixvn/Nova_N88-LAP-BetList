// -----------------------------------------------------------------------
// <copyright file="CashOutBetListDetailMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.BetList.Model.MetaModel
{
    using System.Collections.Generic;

    public class CashOutBetListDetailMetaModel
    {
        public string BaseCurrency { get; set; }

        public long TransId { get; set; }

        public long RefNo { get; set; }

        public int BetType { get; set; }

        public long BetdaqId { get; set; }

        public string AgentListXml { get; set; }

        public bool IsCashOut { get; set; }

        public bool HasPlayerComm { get; set; }
    }
}