using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.Common.DataAccess;
using Fanex.Common.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.BetList.DataAccess
{
    public class IPDataAccess : BaseDataAccess, IIPDataAccess
    {
        public IList<IPRangeEntity> GetVNIPs()
        {
            const string UspKey = "Lrf_GetAllIPVN";

            var ipVNList = new List<IPRangeEntity>();

            using (var db = GetObjectDb(UspKey))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ipRange = new IPRangeEntity
                        {
                            IPFrom = reader.ToLong("IPFrom"),
                            IPTo = reader.ToLong("IPTo")
                        };
                        ipVNList.Add(ipRange);
                    }
                }
            }

            return ipVNList.ToList();
        }
    }
}
