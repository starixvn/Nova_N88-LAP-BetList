using System;

namespace Fanex.Athena.BetList.Model.MetaModel
{
    public interface IBalanceUpMetaModel
    {
        DateTime MoveBetsDay { get; set; }

        DateTime BalanceUpDay { get; set; }
    }
}