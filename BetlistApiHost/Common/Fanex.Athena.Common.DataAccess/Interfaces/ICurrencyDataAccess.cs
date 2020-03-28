using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.Common.DataAccess.Interfaces
{
    public interface ICurrencyDataAccess
    {
        Task<IDictionary<int, string>> GetCurrencyListAsync();

    }
}
