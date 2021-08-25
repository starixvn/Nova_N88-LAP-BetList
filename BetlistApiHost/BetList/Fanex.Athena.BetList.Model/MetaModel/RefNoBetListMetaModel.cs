using System;

namespace Fanex.Athena.BetList.Model.MetaModel
{
    public class RefNoBetListMetaModel : BaseBettingTicketMetaModel
    {
        public long RefNo { get; set; }

        public DateTime? LastXDays { get; set; }

        public override bool IsValid => RefNo > 0;
    }
}