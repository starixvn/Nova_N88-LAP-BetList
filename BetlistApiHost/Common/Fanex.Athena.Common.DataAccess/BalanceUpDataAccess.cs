namespace Fanex.Athena.Common.DataAccess
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Common.Extensions;
    using Interfaces;
    using Logging;
    using Model.DTO;
    using Utility.Services.Interfaces;

    public class BalanceUpDataAccess : BaseDataAccess, IBalanceUpDataAccess
    {
        private const int CacheTimeoutSeconds = 60;

        private const string BalanceUpCacheKey = "BETLISTAPI_CACHE_BALANCEUP";

        private readonly IMemoryCacheService _cache;

        [CLSCompliant(false)]
        public BalanceUpDataAccess(IMemoryCacheService cache)
        {
            _cache = cache;
        }

        public BalanceUp GetBalanceUp()
        {
            var balanceup = _cache.Get<BalanceUp>(BalanceUpCacheKey);

            if (balanceup != null)
            {
                return balanceup;
            }

            balanceup = GetBalanceUpDayFromDb();
            _cache.Set(BalanceUpCacheKey, balanceup, CacheTimeoutSeconds);
            return balanceup;
        }

        private BalanceUp GetBalanceUpDayFromDb()
        {
            const string UspName = "Lrf_SelBalanceUpDay";

            using (var db = this.GetObjectDb(UspName))
            {
                var balanceUp = new BalanceUp();
                using (var reader = db.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        balanceUp.BalanceUpDay = reader.ToDateTime("balanceupday");
                        balanceUp.MoveBetsDay = reader.ToDateTime("movebetsday");
                    }

                    return balanceUp;
                }
            }
        }
    }
}