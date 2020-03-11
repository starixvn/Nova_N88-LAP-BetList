using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.Common.DataAccess.Interfaces;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Common.Extensions;

namespace Fanex.Athena.Common.DataAccess
{
    public class CurrencyDataAccess : BaseDataAccess, ICurrencyDataAccess
    {
        private const int CacheTime1DayInSecond = 86400;

        private const string CacheKey = "_LAP_BETLIST_CACHE_CURRENCY_";

        private readonly IMemoryCacheService _cache;

        public CurrencyDataAccess(IMemoryCacheService memoryCache)
        {
            _cache = memoryCache;
        }

        public async Task<IDictionary<int, string>> GetCurrencyListAsync()
        {
            var currencyList = _cache.Get<IDictionary<int, string>>(CacheKey);

            if (currencyList != null)
            {
                return currencyList;
            }
            currencyList = await GetCurrencyListFromDbAsync().ConfigureAwait(false);
            if (currencyList.Keys.Any())
            {
                _cache.Set(CacheKey, currencyList, CacheTime1DayInSecond);
            }
            return currencyList;
        }

        private async Task<IDictionary<int, string>> GetCurrencyListFromDbAsync()
        {
            const string UspName = "Lrf_CurrencySel";
            var result = new Dictionary<int, string>();
            using (var db = this.GetObjectDb(UspName))
            {
                using (var reader = await db.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        result.Add(reader.ToInt("Id"), reader["Name"].ToString());
                    }
                    return result;
                }
            }
        }
    }
}
