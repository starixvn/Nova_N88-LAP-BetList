// -----------------------------------------------------------------------
// <copyright file="ResultDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Result.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Common.DataAccess;
    using Common.DataAccess.Extensions;
    using Fanex.Common.Extensions;
    using Fanex.Result.Core.Happy5.DTO;
    using Fanex.Result.Core.NumberGame.DTO;
    using Model.MetaModel;

    [CLSCompliant(false)]
    public class ResultNumberGameDataAccess : BaseDataAccess, IResultNumberGameDataAccess
    {
        public async Task<IList<NumberGame>> GetNumberGameMatchesAsync(ResultMetaModel metalModel)
        {
            const string UspName = "Lrf_Result_MatchSelNumberGameByLeague";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                leagueid = metalModel.LeagueId,
                sporttype = metalModel.SportId,
                isshowall = true
            };

            using (var db = this.GetObjectDb(UspName))
            {
                using (var reader = await db.ExecuteReaderAsync(uspParams).ConfigureAwait(false))
                {
                    return ReadNumberGameMatches(reader);
                }
            }
        }

        public async Task<IList<Happy5>> GetHappy5MatchesAsync(ResultMetaModel metalModel)
        {
            const string UspName = "Lrf_Result_MatchSelNumberGameByLeague";
            var uspParams = new
            {
                fromdate = metalModel.FromDate,
                leagueid = metalModel.LeagueId,
                sporttype = metalModel.SportId,
                isshowall = true
            };

            using (var db = this.GetObjectDb(UspName))
            {
                using (var reader = await db.ExecuteReaderAsync(uspParams).ConfigureAwait(false))
                {
                    return ReadHappy5Matches(reader);
                }
            }
        }

        public async Task<IList<NumberGame>> GetPopUpNumberGameMatchAsync(int matchId)
        {
            const string UspName = "Lrf_BetList_SelMatchResultNumberGame";
            var uspParams = new
            {
                matchId
            };

            using (var db = this.GetObjectDb(UspName))
            {
                using (var reader = await db.ExecuteReaderAsync(uspParams).ConfigureAwait(false))
                {
                    return ReadNumberGameMatches(reader);
                }
            }
        }

        public async Task<IList<Happy5>> GetPopUpHappy5MatchAsync(int matchId)
        {
            const string UspName = "Lrf_BetList_SelMatchResultNumberGame";
            var uspParams = new
            {
                matchId
            };

            using (var db = this.GetObjectDb(UspName))
            {
                using (var reader = await db.ExecuteReaderAsync(uspParams).ConfigureAwait(false))
                {
                    return ReadHappy5Matches(reader);
                }
            }
        }

        private static IList<NumberGame> ReadNumberGameMatches(IDataReader reader)
        {
            var dataList = new List<NumberGame>();

            while (reader.Read())
            {
                var matchId = reader.ToInt("matchid");
                var seq = reader.ToInt("seq");
                var number = reader.ToInt("number");

                var existMatch = dataList.SingleOrDefault(i => i.MatchId == matchId);
                if (existMatch != null)
                {
                    existMatch.Seg.Add(seq);
                    existMatch.Ball.Add(number);
                }
                else
                {
                    const string IsRefundColumn = "isrefund";
                    dataList.Add(
                        new NumberGame
                        {
                            LeagueId = reader.ToInt("leagueid"),
                            MatchId = matchId,
                            EventStatus = reader.ToString("eventstatus"),
                            EventDate = DateTime.Parse(reader.ToString("eventdate")),
                            IsPending = reader.HasColumn(IsRefundColumn) && reader.ToString(IsRefundColumn) != "0",
                            KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                            MatchCode = reader.ToString("matchcode"),
                            Seg = new List<int> { seq },
                            Ball = new List<int> { number }
                        });
                }
            }

            return dataList.OrderBy(x => x.LeagueId).ThenBy(x => x.MatchId).ToList();
        }

        private static IList<Happy5> ReadHappy5Matches(IDataReader reader)
        {
            var dataList = new List<Happy5>();

            while (reader.Read())
            {
                var matchId = reader.ToInt("matchid");
                var seq = reader.ToInt("seq");
                var number = reader.ToInt("number");

                var existMatch = dataList.SingleOrDefault(i => i.MatchId == matchId);
                if (existMatch != null)
                {
                    existMatch.Seq.Add(seq);
                    existMatch.Ball.Add(number);
                }
                else
                {
                    const string IsRefundColumn = "isrefund";
                    dataList.Add(
                        new Happy5
                        {
                            LeagueId = reader.ToInt("leagueid"),
                            MatchId = matchId,
                            EventStatus = reader.ToString("eventstatus"),
                            EventDate = DateTime.Parse(reader.ToString("eventdate")),
                            IsPending = reader.HasColumn(IsRefundColumn) && reader.ToString(IsRefundColumn) != "0",
                            KickOffTime = DateTime.Parse(reader.ToString("kickofftime")),
                            MatchCode = reader.ToString("matchcode"),
                            Seq = new List<int> { seq },
                            Ball = new List<int> { number }
                        });
                }
            }

            return dataList.OrderBy(x => x.LeagueId).ThenBy(x => x.MatchId).ToList();
        }
    }
}