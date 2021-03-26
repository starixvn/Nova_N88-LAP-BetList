using Fanex.Athena.BetList.Model.DTO;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.BetList.Service.Interfaces
{
    [CLSCompliant(false)]
    public interface IIPService
    {
        IList<IPRangeEntity> GetIPVN();
        bool IsVNIP(string ipAddress);
    }
}
