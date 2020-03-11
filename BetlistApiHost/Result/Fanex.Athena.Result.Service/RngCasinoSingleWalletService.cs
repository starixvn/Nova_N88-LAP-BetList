using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Net;
using System.Web;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.Athena.Resources;
using Fanex.Athena.Result.Service.Interfaces;
using Fanex.Logging;
using Newtonsoft.Json;

namespace Fanex.Athena.Result.Service
{
    public class RngCasinoSingleWalletService : IRNGCasinoSingleWalletService
    {
        private static readonly IDictionary<string, string> MapLanguages = new Dictionary<string, string>
            {
                { "en-US", "en" },
                { "zh-TW", "ch" },
                { "zh-CN", "cs" },
                { "zh-Hans", "zhcn" }
            };

        private readonly ILogger _logger;

        private readonly IAppSettings _appSettings;

        private readonly IWebClientService _webClientService;

        [CLSCompliant(false)]
        public RngCasinoSingleWalletService(ILogger logger, IAppSettings appSettings, IWebClientService webClientService)
        {
            _logger = logger;
            _appSettings = appSettings;
            _webClientService = webClientService;
        }

        public dynamic GetResultInfo(string refNo)
        {
            var authenticatedToken = GetRngCasinoServerToken();

            if (string.IsNullOrEmpty(authenticatedToken))
            {
                return new
                {
                    Success = false,
                    Result = new
                    {
                        ErrorCode = 1,
                        ErrorDescription = ViewResources.ErrorProcessingMsg
                    }
                };
            }

            var authenticatedTokenInJson = JsonConvert.DeserializeObject<dynamic>(authenticatedToken);
            var result = authenticatedTokenInJson.Result;
            var isSuccess = result.ErrorCode == 0;

            return new
            {
                Success = isSuccess,
                Result = isSuccess
                    ? GetRngCasinoPopupAddress(result.Token.ToString(), refNo)
                    : new
                    {
                        ErrorCode = result.ErrorCode.ToString(),
                        ErrorDescription = result.ErrorDescription.ToString()
                    }
            };
        }

        private static string EncryptQueryString(string parameters, string encryptedKey)
        {
            return IBCCrypt.IBCCrypt.Encrypt(parameters, encryptedKey);
        }

        private static string GetLanguageCode()
        {
            var languageCode = CultureInfo.CurrentUICulture.IetfLanguageTag;
            string result;
            if (!MapLanguages.TryGetValue(languageCode, out result))
            {
                return "en";
            }

            return result;
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed.")]
        private string GetRngCasinoServerToken()
        {
            try
            {
                var uri = new Uri(_appSettings.GetValue("RNGCasinoGetServerTokenAPIUrl"));
                var paramString = CreateServerTokenParameters();

                return _webClientService.UploadString(
                    uri,
                    paramString,
                    new Dictionary<HttpRequestHeader, string>
                    {
                        { HttpRequestHeader.ContentType, "application/json" }
                    });
            }
            catch (Exception exception)
            {
                _logger.ErrorAsync(exception.Message, exception);
            }

            return string.Empty;
        }

        private string CreateServerTokenParameters()
        {
            var timeStamp = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");
            var login = _appSettings.GetValue("RNGCasinoLogin");
            var password = _appSettings.GetValue("RNGCasinoPassword");
            var siteId = int.Parse(_appSettings.GetValue("RNGCasinoSiteId"));
            var seq = _appSettings.GetValue("RNGCasinoSeq");
            var vendorId = int.Parse(_appSettings.GetValue("RNGCasinoVendorId"));

            var auth = new { Login = login, Password = password, SiteId = siteId };
            var call = new { Seq = seq, VendorId = vendorId };

            var param = new
            {
                TimeStamp = timeStamp,
                Auth = auth,
                Call = call
            };

            return JsonConvert.SerializeObject(param);
        }

        private string GetRngCasinoPopupAddress(string token, string refNo)
        {
            var queryString = GetQueryString(token, refNo);
            var encryptedKey = _appSettings.GetValue("RNGCasinoEncryptedKey");
            var encryptParam = EncryptQueryString(queryString, encryptedKey);
            var paramEncode = HttpUtility.UrlEncode(encryptParam);
            var popupUrl = _appSettings.GetValue("RNGCasinoResultPopupUrl");

            return string.Format(popupUrl, paramEncode);
        }

        private string GetQueryString(string token, string refNo)
        {
            var languageCode = GetLanguageCode();
            var template = _appSettings.GetValue("RNGCasinoTemplate");
            var role = _appSettings.GetValue("RNGCasinoRole");

            var queryString = $"Token={token}&RefID={refNo}&Lang={languageCode}&Template={template}&Role={role}";

            return queryString;
        }
    }
}