namespace Fanex.Athena.BetList.DataAccess.Util
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Model.MetaModel;

    [CLSCompliant(false)]
    public static class BetListParameterBuilder
    {
        private const string MoveBetDayColumn = "MoveBetsDay";
        private const string CustIdColumn = "CustId";
        private const string AgentListColumn = "AgentList";

        private static readonly Dictionary<string, Action<IObjectDb, BetListMetaModel>> NonBaseCurrencySpNameActionMap =
            new Dictionary<string, Action<IObjectDb, BetListMetaModel>>(StringComparer.OrdinalIgnoreCase)
        {
            { "Lrf_BetList_SelWinLossByDate_Mem", (db, metalModel) => CreateCustomerWlBetlistParameters(db, metalModel) },
            { "Lrf_BetList_SelWinloss_Mem", (db, metalModel) => CreateLicenseeWLBetlistParameters(db, metalModel) },
            { "Lrf_BetList_LicenseeBonus_Detail", (db, metalModel) => CreateBonusDetailParameters(db, metalModel) },
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

        private static void CreateCasinoDetailParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(nameof(metaModel.BaseCurrency), metaModel.BaseCurrency);
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue("refno", metaModel.TransId);
            db.SetParameterValue(CustIdColumn, metaModel.CustId);
            db.SetParameterValue("transtype", metaModel.TransType);
        }

        private static void CreateBonusDetailParameters(IObjectDb db, BetListMetaModel metaModel)
        {
            db.SetParameterValue(AgentListColumn, metaModel.AgentListXml);
            db.SetParameterValue("TransId", metaModel.TransId);
            db.SetParameterValue("RefCode", metaModel.RefCode);
            db.SetParameterValue("BaseCurrency", metaModel.BaseCurrency);
        }

        #region Utility

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