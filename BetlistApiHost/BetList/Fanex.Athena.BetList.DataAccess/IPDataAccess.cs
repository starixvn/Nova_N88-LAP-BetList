using Fanex.Athena.BetList.DataAccess.Interfaces;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.Common.DataAccess;
using Fanex.Common.Extensions;
using System;
using System.Collections.Generic;

namespace Fanex.Athena.BetList.DataAccess
{
    public class IPDataAccess : BaseDataAccess, IIPDataAccess
    {
        public Tuple<IList<IPV4RangeEntity>, IList<IPV6RangeEntity>> GetVNIPs()
        {
            const string UspKey = "Lrf_GetAllIPVN";

            var ipVNList = new List<IPV4RangeEntity>();
            var ipV6VNList = new List<IPV6RangeEntity>();

            using (var db = GetObjectDb(UspKey))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ipRange = new IPV4RangeEntity
                        {
                            IPFrom = reader.ToLong("IPFrom"),
                            IPTo = reader.ToLong("IPTo")
                        };
                        ipVNList.Add(ipRange);
                    }

                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            var ipV6Range = new IPV6RangeEntity
                            {
                                IPFrom = reader.ToString("IPFrom"),
                                IPTo = reader.ToString("IPTo")
                            };
                            ipV6VNList.Add(ipV6Range);
                        }
                    }
                }
            }

            return Tuple.Create<IList<IPV4RangeEntity>, IList<IPV6RangeEntity>>(ipVNList, ipV6VNList);
        }
    }
}