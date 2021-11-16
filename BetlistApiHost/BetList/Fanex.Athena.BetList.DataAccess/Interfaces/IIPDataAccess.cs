using Fanex.Athena.BetList.Model.DTO;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    public interface IIPDataAccess
    {
        Tuple<IList<IPV4RangeEntity>, IList<IPV6RangeEntity>> GetVNIPs();
    }
}