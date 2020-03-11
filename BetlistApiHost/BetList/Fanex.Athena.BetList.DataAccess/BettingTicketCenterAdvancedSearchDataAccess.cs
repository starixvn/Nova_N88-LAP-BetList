namespace Fanex.Athena.BetList.DataAccess
{
    using System;
    using System.Collections;
    using System.Linq;
    using System.Threading.Tasks;
    using BetList.Model.DTO;
    using BetList.Model.MetaModel;
    using Common.DataAccess.Interfaces;
    using Common.Utility.Services.Interfaces;
    using Data;
    using Fanex.Common;
    using Interfaces;
    using Newtonsoft.Json;

    public class BettingTicketCenterAdvancedSearchDataAccess : BaseBetListDataAccess, IBettingTicketCenterAdvancedSearchDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;
        private readonly ITempFileService _tempFileService;

        public BettingTicketCenterAdvancedSearchDataAccess(IBalanceUpDataAccess balanceUpDataAccess,
            ITempFileService tempFileService, ICurrencyDataAccess currencyService) : base(currencyService)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _tempFileService = tempFileService;
        }

        public async Task<GroupTicketResult> GetBetListForMultipleCustomersAsync(
            AdvancedSearchBetListMetaModel metaModel,
            int rowPerGroup)
        {
            const string UspKey = "Lrf_BetList_Licensee";
            var groupData = new GroupTicketResult
            {
                RowPerGroup = rowPerGroup
            };
            var currencyList = await this.GetCurrencyListAsync();

            using (var db = GetObjectDb(UspKey))
            {
                AddAdvancedSearchParameters(db, metaModel);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    var count = 0;
                    var ticket = new TicketResult();

                    var columns = GetColumns();
                    var safeReader = this.GetSafeDataReader(reader, columns.Select(x => x.Value).ToList());

                    while (reader.Read())
                    {
                        if (count == rowPerGroup)
                        {
                            SaveDataToFile(groupData, ticket);
                            count = 0;
                            ticket = new TicketResult();
                        }

                        ticket.MainTicketList.Add(MapTicket(safeReader, columns, currencyList));
                        count++;
                    }

                    if (ticket.MainTicketList.Any())
                    {
                        SaveDataToFile(groupData, ticket);
                    }
                }

                var outSqlParams = db.GetOutputParams();
                ReadOutputParams(groupData, outSqlParams);

                return groupData;
            }
        }

        public async Task<TicketResult> GetBetListForMultipleCustomersAsync(AdvancedSearchBetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_Licensee";

            var ticketResult = new TicketResult();
            var currencyList = await this.GetCurrencyListAsync();

            using (var db = GetObjectDb(UspKey))
            {
                AddAdvancedSearchParameters(db, metaModel);

                using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                {
                    ticketResult.MainTicketList.AddRange(ReadBetListTicket(reader, currencyList));
                }

                var outSqlParams = db.GetOutputParams();
                ticketResult.SumStake = Converter.ToDecimal(outSqlParams["sumstake"]);
                ticketResult.SumWinLoss = Converter.ToDecimal(outSqlParams["sumwinlost"]);
                ticketResult.SumLicenseeWL = Converter.ToDecimal(outSqlParams["sumlicenseewl"]);
                ticketResult.SumComm = Converter.ToDecimal(outSqlParams["sumcomm"]);
                ticketResult.SumPlayerComm = Converter.ToDecimal(outSqlParams["sumplayercomm"]);
                ticketResult.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
            }

            return ticketResult;
        }

        public async Task<int> GetTotalRowForMultipleCustomersAsync(AdvancedSearchBetListMetaModel metaModel)
        {
            const string UspKey = "Lrf_BetList_Licensee_TotalRows";

            using (var db = GetObjectDb(UspKey))
            {
                var balanceUp = _balanceUpDataAccess.GetBalanceUp();
                db.SetParameterValue("AgentList", metaModel.AgentListXml);
                db.SetParameterValue("FromDate", metaModel.FromDate);
                db.SetParameterValue("ToDate", metaModel.ToDate);
                db.SetParameterValue("MoveBetsDay", balanceUp.MoveBetsDay);
                db.SetParameterValue("SportType", metaModel.SportId);
                db.SetParameterValue("BetType", metaModel.BetTypeId);
                db.SetParameterValue("LeagueId", metaModel.LeagueId);
                db.SetParameterValue("MatchId", metaModel.MatchId);
                db.SetParameterValue("StatusIdList", metaModel.Status);
                db.SetParameterValue("Username", metaModel.UserName);
                db.SetParameterValue("LiveIndicator", metaModel.LiveStatus);
                db.SetParameterValue("Product", metaModel.ProductCodeList);

                db.SetParameterValue("TotalRows", null);

                await db.ExecuteNonQueryAsync().ConfigureAwait(false);

                var outSqlParams = db.GetOutputParams();
                return Converter.ToInt(outSqlParams["totalrows"]);
            }
        }

        private static void ReadOutputParams(GroupTicketResult groupData, Hashtable outSqlParams)
        {
            groupData.SumStake = Converter.ToDecimal(outSqlParams["sumstake"]);
            groupData.SumWinLoss = Converter.ToDecimal(outSqlParams["sumwinlost"]);
            groupData.SumLicenseeWL = Converter.ToDecimal(outSqlParams["sumlicenseewl"]);
            groupData.SumComm = Converter.ToDecimal(outSqlParams["sumcomm"]);
            groupData.SumPlayerComm = Converter.ToDecimal(outSqlParams["sumplayercomm"]);
            groupData.TotalRows = Converter.ToInt(outSqlParams["totalrows"]);
        }

        private static void AddSortParameter(IObjectDb db, string sortBy, string sortOrder)
        {
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                db.SetParameterValue("Sort", sortBy + " " + sortOrder);
            }
        }

        private void SaveDataToFile(GroupTicketResult groupData, TicketResult ticket)
        {
            var data = JsonConvert.SerializeObject(ticket);
            var dataFileName = $"{Guid.NewGuid().ToString()}.json";
            _tempFileService.Save(dataFileName, data);
            groupData.DataPath.Add(dataFileName);
            ticket.MainTicketList.Clear();
            ticket.MainTicketList.TrimExcess();
        }

        private void AddAdvancedSearchParameters(IObjectDb db, AdvancedSearchBetListMetaModel metaModel)
        {
            var balanceUp = _balanceUpDataAccess.GetBalanceUp();
            db.SetParameterValue("AgentList", metaModel.AgentListXml);
            db.SetParameterValue("FromDate", metaModel.FromDate);
            db.SetParameterValue("ToDate", metaModel.ToDate);
            db.SetParameterValue("MoveBetsDay", balanceUp.MoveBetsDay);
            db.SetParameterValue("SportType", metaModel.SportId);
            db.SetParameterValue("BetType", metaModel.BetTypeId);
            db.SetParameterValue("LeagueId", metaModel.LeagueId);
            db.SetParameterValue("MatchId", metaModel.MatchId);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            db.SetParameterValue("StatusIdList", metaModel.Status);
            db.SetParameterValue("LiveIndicator", metaModel.LiveStatus);
            db.SetParameterValue("Username", metaModel.UserName);
            db.SetParameterValue("PageSize", metaModel.IsExport ? int.MaxValue - 1 : metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.IsExport ? 1 : metaModel.PageIndex);
            db.SetParameterValue("Product", metaModel.ProductCodeList);
            db.SetParameterValue("ConvertToBaseCur", metaModel.IsConvertAll);

            db.SetParameterValue("TotalRows", null);
            db.SetParameterValue("SumStake", null);
            db.SetParameterValue("SumWinLost", null);
            db.SetParameterValue("SumLicenseeWL", null);
            db.SetParameterValue("SumComm", null);
            db.SetParameterValue("SumPlayerComm", null);
            AddSortParameter(db, metaModel.SortBy, metaModel.SortOrder);
        }
    }
}