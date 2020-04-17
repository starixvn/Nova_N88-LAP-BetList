namespace Fanex.Athena.Result.Controller
{
    using System;
    using System.Web.Mvc;
    using Common.Utility.Services.Interfaces;
    using Logging;
    using Model.Const;
    using Model.DTO.Keno;
    using Model.MetaModel.Keno;
    using Service.Interfaces;

    public class ResultKenoReportController : BaseResultController
    {
        private readonly ILogger _logger;

        private readonly IAppSettings _appSettings;

        private readonly IKenoApi _kenoApi;

        public ResultKenoReportController(ILogger logger, IAppSettings appSetting, IKenoApi kenoApi)
        {
            _logger = logger;
            _appSettings = appSetting;
            _kenoApi = kenoApi;
        }

        [HttpPost]
#pragma warning disable S3995 // URI return values should not be strings
        public string GetKenoResultUrl(int vendorProductId, string userId)
#pragma warning restore S3995 // URI return values should not be strings
        {
            try
            {
                return _kenoApi.CreateKenoResultAddress(GetKenoSetting(vendorProductId), userId, DateTime.Now);
            }
            catch (Exception exception)
            {
                _logger.ErrorAsync(exception.Message, exception);

                return string.Empty;
            }
        }

        [HttpPost]
        public JsonResult KeepSession(int vendorProductId, string userId)
        {
            try
            {
                var result = _kenoApi.ExtendToken(GetKenoSetting(vendorProductId), userId);
                return Json(result);
            }
            catch (Exception exception)
            {
                _logger.ErrorAsync(exception.Message, exception);

                return Json(new KenoExtendedToken
                {
                    ErrorCode = 1,
                    ErrorMsg = exception.Message
                });
            }
        }

        private KenoResultMetaModel GetKenoSetting(int vendorProductId)
        {
            return new KenoResultMetaModel
            {
                Language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName,
                SiteID = _appSettings.GetValue("KenoSiteID"),
                AuthorizedUrl = _appSettings.GetValue("KenoAuthorizedUrl"),
                ExpireTokenUrl = _appSettings.GetValue("KenoExpireTokenUrl"),
                ExtendTokenUrl = _appSettings.GetValue("KenoExtendTokenUrl"),
                VenderKey = _appSettings.GetValue("KenoVenderKey"),
                Vendor = _appSettings.GetValue("KenoVendor"),
                ResultUrl = _appSettings.GetValue(GetResultUrl(vendorProductId)),
                KickOutUrl = Url.Action("AccessDenied", "Utility", null, Request.Url.Scheme),
                VendorProduct = vendorProductId
            };
        }

        private static string GetResultUrl(int vendorProduct)
        {
            string resultUrl = "";

            switch (vendorProduct)
            {
                case ResultVendorProductTypes.Lottery:
                    resultUrl = "LotteryResultUrl";
                    break;
                case ResultVendorProductTypes.Table:
                    resultUrl = "TableResultUrl";
                    break;
                default:
                    resultUrl = "KenoResultUrl";
                    break;
            }

            return resultUrl;
        }
    }
}