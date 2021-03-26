using Fanex.Athena.BetList.Model.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    public interface IIPDataAccess
    {
        IList<IPRangeEntity> GetVNIPs();
    }
}
