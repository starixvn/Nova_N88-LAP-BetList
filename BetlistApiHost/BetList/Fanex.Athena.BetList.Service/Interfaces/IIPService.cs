using Fanex.Athena.BetList.Model.DTO;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.BetList.Service.Interfaces
{
    [CLSCompliant(false)]
    public interface IIPService
    {
        bool IsVNIP(string ipAddress);
    }
}