namespace Fanex.Athena.BetList.DataAccess.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Constant;
    using Data;
    using Model.MetaModel;

    [CLSCompliant(false)]
    public static class BetListParameterBuilder
    {
        private const string MoveBetDayColumn = "MoveBetsDay";
        private const string MatchIDColumn = "MatchId";
        private const string LiveIndicatorColumn = "LiveIndicator";
        private const string CustIdColumn = "CustId";
        private const string AgentListColumn = "AgentList";
        private const string BetTypeColumn = "BetType";

        private static readonly Dictionary<string, Action<IObjectDb, BetListMetaModel>> NonBaseCurrencySpNameActionMap =
            new Dictionary<string, Action<IObjectDb, BetListMetaModel>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Lrf_BetList_WinlostByLeague_Mem", (db, metalModel) => CreateWinlossByLeagueParameters(db, metalModel) },
            { "Lrf_BetList_MatchWL_Parlay", (db, metalModel) => CreateMatchWLParlayBetListParameters(db, metalModel) },
            { "Lrf_BetList_MatchWL_Outright", (db, metalModel) => CreateMatchWLOutRightBetListParameters(db, metalModel) },
            { "Lrf_BetList_MatchWL_Bettype", (db, metalModel) => CreateMatchWLBettypeBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinLossByDate_Mem", (db, metalModel) => CreateCustomerWlBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinloss_Mem", (db, metalModel) => CreateLicenseeWLBetlistParameters(db, metalModel) },
            { "Lrf_BetList_Forecast_SelByMatch", (db, metalModel) => CreateRunningBetListByForeCastMatchParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByBettype", (db, metalModel) => CreateRunningBetListByBettypePaprameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByMultipleBettypeMatch", (db, metalModel) => CreateMatchRunningBetLitByMultipleBettypeOfMatchParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByBettypeList", (db, metalModel) => CreateRunningBetListByMultipleBettypePaprameters(db, metalModel) },
            { "Lrf_TotalBets_Sel430", (db, metalModel) => CreateCorectScoreBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByBettypeMatch", (db, metalModel) => CreateMatchRunningBetLitByBettypeParameters(db, metalModel) },
            { "Lrf_BetList_CustomerList_Mem", (db, metalModel) => CreateCustomerListBetListParameters(db, metalModel) },
            { "Lrf_BetList_LicenseeBonus_Detail", (db, metalModel) => CreateBonusDetailParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossNumberGame", (db, metalModel) => CreateLiveReportNumberGameBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinLossByBetType", (db, metalModel) => CreateWinLossByBetTypeParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossByMatch", (db, metalModel) => CreateLiveReportBetListByMatchParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossParlay", (db, metalModel) => CreateLiveReportParlayBetlistParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossOutright", (db, metalModel) => CreateLiveReportOutRightBetlistParameters(db, metalModel) },
            { "Lrf_BetList_LiveReport_SelWinlossVHRacing", (db, metalModel) => CreateLiveReportBetListVHRacingParameters(db, metalModel) },
            { "Lrf_BetList_PlatformWinLoss_Mem", (db, metalModel) => CreatePlatformWinLossBetListParameters(db, metalModel) },
            { "Lrf_BetList_SelRunningByDate_Mem", (db, metalModel) => CreateRunningBetListParameters(db, metalModel) },
            { "Lrf_BetList_SelByDayAll_Mem", (db, metalModel) => CreateDailyStatementBetListParameters(db, metalModel) },
            { "Lrf_BetList_SelByDayAll_Mem_Old", (db, metalModel) => CreateDailyStatementBetListOldParameters(db, metalModel) },
            { "Lrf_BetList_RNG_Detail", (db, metalModel) => CreateCasinoDetailParameters(db, metalModel) },
            { "Lrf_BetList_LC_Detail", (db, metalModel) => CreateCasinoDetailParameters(db, metalModel) },

        };
        public static void CreateBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            NonBaseCurrencySpNameActionMap[metaModel.UspKey](db, metaModel);
        }

        public static void CreateBetListDateRangeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("FromDate", metaModel.FromDate);
            db.SetParameterValue("ToDate", metaModel.ToDate);
        }

        public static void CreateBetlistBUParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(MoveBetDayColumn, metaModel.MoveBetsDay);
            db.SetParameterValue("BalanceUpDay", metaModel.BalanceUpDay);
        }

        public static void CreateBetListPagingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue("PageSize", metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.PageIndex);
            db.SetParameterValue("totalrows", null);
        }

        private static void CreateMatchRunningBetLitByMultipleBettypeOfMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(BetTypeColumn, !string.IsNullOrEmpty(metaModel.MultipleBettype) ? metaModel.MultipleBettype : string.Empty);
            db.SetParameterValue(MatchIDColumn, metaModel.MatchId);
            db.SetParameterValue(LiveIndicatorColumn, metaModel.LiveIndicator ?? -1);
            db.SetParameterValue("betteam", metaModel.BetTeam);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateWinlossByLeagueParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as WinLossByLeagueBetListMetaModel;
            var products = metaModel.ProductCodeList.Split(',');
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(CustIdColumn, metaModel.CustId);
            db.SetParameterValue("LeagueID", metaModel.LeagueId);
            db.SetParameterValue(MatchIDColumn, metaModel.MatchId);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue("Odds", metaModel.Odds);
            db.SetParameterValue("FromDate", metaModel.FromDate);
            db.SetParameterValue("ToDate", metaModel.ToDate);
            db.SetParameterValue(MoveBetDayColumn, metaModel.MoveBetsDay);
            db.SetParameterValue("BalanceUpDay", metaModel.BalanceUpDay);
            db.SetParameterValue("ShowWin", metaModel.ShowWin);
            db.SetParameterValue("ShowLoss", metaModel.ShowLoss);
            db.SetParameterValue("ShowDraw", metaModel.ShowDraw);
            db.SetParameterValue("ShowSB", products.Contains(ProductCodes.SB));
            db.SetParameterValue("ShowBA", products.Contains(ProductCodes.BA));
        }

        private static void CreateRunningBetListByForeCastMatchParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(BetTypeColumn, metaModel.BetType);
            db.SetParameterValue(MatchIDColumn, metaModel.MatchId);
            db.SetParameterValue(LiveIndicatorColumn, metaModel.LiveIndicator ?? -1);
            db.SetParameterValue("betteam", metaModel.BetTeam);
            db.SetParameterValue("BetChoice", metaModel.BetChoice);
            db.SetParameterValue("Handicap", metaModel.Handicap);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateMatchRunningBetLitByBettypeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(BetTypeColumn, metaModel.BetType);
            db.SetParameterValue(MatchIDColumn, metaModel.MatchId);
            db.SetParameterValue(LiveIndicatorColumn, metaModel.LiveIndicator ?? -1);
            db.SetParameterValue("betteam", metaModel.BetTeam);
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateRunningBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(CustIdColumn, metaModel.CustId);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateProductListParameters(db, metaModel.ProductCodeList);
        }

        private static void CreateDailyStatementBetListOldParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(CustIdColumn, metaModel.CustId);
            db.SetParameterValue("fromdate", metaModel.FromDate);
            db.SetParameterValue("transtype", metaModel.TransType);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateDailyStatementBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(CustIdColumn, metaModel.CustId);
            db.SetParameterValue("fromdate", metaModel.FromDate);
            db.SetParameterValue("transtype", metaModel.TransType);
            db.SetParameterValue(MoveBetDayColumn, metaModel.MoveBetsDay);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateCustomerListBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(CustIdColumn, metaModel.CustId);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
            CreateProductListParameters(db, metaModel.ProductCodeList);
            CreateBetlistBUParameters(db, metaModel);
            if (!string.IsNullOrWhiteSpace(metaModel.SortBy))
            {
                db.SetParameterValue("Sort", metaModel.SortBy + " " + metaModel.SortOrder);
            }
        }

        private static void CreatePlatformWinLossBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(CustIdColumn, metaModel.CustId);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue("drawfilter", metaModel.DrawFilter);
            db.SetParameterValue("PlatformType", metaModel.PlatformType);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateProductListParameters(db, metaModel.ProductCodeList);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateMatchWLParlayBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(BetTypeColumn, metaModel.BetType);
            db.SetParameterValue(MoveBetDayColumn, metaModel.MoveBetsDay);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);

            var supportedProducts = new[] { ProductCodes.SB, ProductCodes.NG, ProductCodes.BA, ProductCodes.VS, ProductCodes.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateMatchWLOutRightBetListParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.SetParameterValue(MoveBetDayColumn, metaModel.MoveBetsDay);
            CreateBetListSortingParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);

            var supportedProducts = new[] { ProductCodes.SB, ProductCodes.BA };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateMatchWLBettypeBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(BetTypeColumn, metaModel.BetType);
            db.SetParameterValue(MatchIDColumn, metaModel.MatchId);
            db.SetParameterValue(LiveIndicatorColumn, metaModel.LiveIndicator ?? -1);
            db.SetParameterValue("Date", metaModel.FromDate);
            CreateBetListSortingParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);

            var supportedProducts = new[] { ProductCodes.SB, ProductCodes.NG, ProductCodes.BA, ProductCodes.VS, ProductCodes.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLiveReportBetListVHRacingParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;
            var defaultParams = new[] { "SUMWL", "SUMLicenseeWL" };

            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.SetParameterValue(MoveBetDayColumn, metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            db.SetParameterValue("SportType", metaModel.SportType);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
            foreach (var name in defaultParams)
            {
                db.SetParameterValue(name, null);
            }
        }

        private static void CreateLiveReportParlayBetlistParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.SetParameterValue(MoveBetDayColumn, metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            db.SetParameterValue(BetTypeColumn, metaModel.BetType);
            db.SetParameterValue(LiveIndicatorColumn, metaModel.LiveIndicator);
            var supportedProducts = new[] { ProductCodes.SB, ProductCodes.BA, ProductCodes.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLiveReportNumberGameBetlistParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;

            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.SetParameterValue(MoveBetDayColumn, metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            db.SetParameterValue(LiveIndicatorColumn, metaModel.LiveIndicator);
            db.SetParameterValue("SportType", metaModel.SportType);
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
        }

        private static void CreateLiveReportBetListByMatchParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(MatchIDColumn, metaModel.MatchId);
            AddLiveReportPagingParams(db, metaModel);
            db.SetParameterValue(LiveIndicatorColumn, metaModel.LiveIndicator);
            var supportedProducts = new[] { ProductCodes.SB, ProductCodes.BA, ProductCodes.VS, ProductCodes.VS2 };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLiveReportOutRightBetlistParameters(IObjectDb db, BetListMetaModel betlistMetaModel)
        {
            var metaModel = betlistMetaModel as LiveReportBetListMetaModel;
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            db.SetParameterValue(MoveBetDayColumn, metaModel.MoveBetsDay);
            AddLiveReportPagingParams(db, metaModel);
            var supportedProducts = new[] { ProductCodes.SB, ProductCodes.BA };
            CreateProductListParameters(db, metaModel.ProductCodeList, supportedProducts);
        }

        private static void CreateLicenseeWLBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(CustIdColumn, metaModel.CustId);
            db.SetParameterValue("drawfilter", metaModel.DrawFilter);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateProductListParameters(db, metaModel.ProductCodeList);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateCorectScoreBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateCustomerWlBetlistParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(CustIdColumn, metaModel.CustId);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
            CreateProductListParameters(db, metaModel.ProductCodeList);
        }

        private static void CreateRunningBetListByBettypePaprameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(BetTypeColumn, metaModel.BetType);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateRunningBetListByMultipleBettypePaprameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(BetTypeColumn, metaModel.MultipleBettype);
            CreateBetListPagingParameters(db, metaModel);
        }

        private static void CreateCasinoDetailParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue("refno", metaModel.TransId);
            db.SetParameterValue(CustIdColumn, metaModel.CustId);
            db.SetParameterValue("transtype", metaModel.TransType);
        }

        private static void CreateWinLossByBetTypeParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            CreateBetListSortingParameters(db, metaModel);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue(BetTypeColumn, metaModel.BetType);
            CreateBetListDateRangeParameters(db, metaModel);
            CreateBetlistBUParameters(db, metaModel);
            CreateBetListPagingParameters(db, metaModel);
            CreateSummaryOutputParameters(db);
        }

        private static void CreateBonusDetailParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue("TransId", metaModel.TransId);
            db.SetParameterValue("RefCode", metaModel.RefCode);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
        }

        #region Utility

        private static void AddLiveReportPagingParams(IObjectDb db, LiveReportBetListMetaModel metaModel)
        {
            db.SetParameterValue("PageSize", metaModel.IsExportExcel ? int.MaxValue - 1 : metaModel.PageSize);
            db.SetParameterValue("PageIndex", metaModel.IsExportExcel ? 1 : metaModel.PageIndex);
        }

        private static void CreateBetListSortingParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            if (!string.IsNullOrWhiteSpace(metaModel.SortBy))
            {
                db.SetParameterValue("Sort", metaModel.SortBy + " " + metaModel.SortOrder);
            }
        }

        private static void CreateSummaryOutputParameters(IObjectDb db)
        {
            var defaultParams = new[] { "SumWinlost", "SumLicenseeWL", "SumComm", "SumPlayerComm" };

            foreach (var name in defaultParams)
            {
                db.SetParameterValue(name, null);
            }
        }


        private static void CreateProductListParameters(IObjectDb db, string productCodeList, IEnumerable<string> supportedProducts = null)
        {
            if (string.IsNullOrWhiteSpace(productCodeList))
            {
                return;
            }

            IEnumerable<string> products = productCodeList.Split(',');

            if (supportedProducts != null)
            {
                products = products.Where(x => supportedProducts.Contains(x)).ToList();
            }

            foreach (var productCode in products)
            {
                db.SetParameterValue("show" + productCode, true);
            }
        }

        #endregion Utility
    }
}