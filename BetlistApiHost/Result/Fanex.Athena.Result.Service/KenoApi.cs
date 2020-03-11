namespace Fanex.Athena.Result.Service
{
    using System;
    using System.Net;
    using System.Text;
    using System.Web;
    using System.Web.Script.Serialization;
    using Common.Utility.Services.Interfaces;
    using Interfaces;
    using Model.Const;
    using Model.DTO.Keno;
    using Model.MetaModel.Keno;

    public class KenoApi : IKenoApi
    {
        private const string KENOSERVICENAME = "KenoApi";
        private const string KENOGAMENAME = "Keno";
        private const string LOTTERYSERVICENAME = "LotteryApi";
        private const string LOTTERYGAMENAME = "Lottery";

        private readonly IWebClientService _webClient;

        public KenoApi(IWebClientService webClient)
        {
            _webClient = webClient;
        }

        public KenoExtendedToken ExtendToken(KenoResultMetaModel setting, string userId)
        {
            try
            {
                var extendedUrl = string.Format(setting.ExtendTokenUrl, setting.Vendor, setting.VenderKey, userId, setting.VendorProduct);
                var data = _webClient.DownloadString(extendedUrl);
                if (string.IsNullOrEmpty(data))
                {
                    throw new WebException(GetServiceName(setting.VendorProduct) + "Couldn't get authorized data from: " + extendedUrl);
                }

                var serialize = new JavaScriptSerializer();
                return serialize.Deserialize<KenoExtendedToken>(data);
            }
            catch (Exception exception)
            {
                throw new WebException(GetServiceName(setting.VendorProduct) + exception.Message);
            }
        }

        public string CreateKenoResultAddress(KenoResultMetaModel setting, string userId, DateTime resultDate)
        {
            var authorizedData = this.Authorize(setting, userId);
            if (authorizedData.ErrorCode > 0)
            {
                throw new WebException(GetServiceName(setting.VendorProduct) + authorizedData.ErrorMsg);
            }

            var kenoResultUrl = new StringBuilder(setting.ResultUrl);
            kenoResultUrl.Append(setting.ResultUrl.Contains("?") ? "&" : "?");
            kenoResultUrl.Append("Authorization=" + authorizedData.Data.Authorization);
            kenoResultUrl.Append("&cust=" + GetGameName(setting.VendorProduct));
            kenoResultUrl.Append("&siteID=" + setting.SiteID);
            kenoResultUrl.Append("&action=R");
            kenoResultUrl.Append("&lang=" + setting.Language);
            kenoResultUrl.Append("&date=" + resultDate.ToString("yyyyMMdd"));
            kenoResultUrl.Append("&redirectURL=" + HttpUtility.UrlEncode(setting.KickOutUrl));
            return kenoResultUrl.ToString();
        }

        private static string GetServiceName(int vendorProductId)
        {
            return vendorProductId == ResultVendorProductTypes.Keno ? KENOSERVICENAME : LOTTERYSERVICENAME;
        }

        private static string GetGameName(int vendorProductId)
        {
            return vendorProductId == ResultVendorProductTypes.Keno ? KENOGAMENAME : LOTTERYGAMENAME;
        }

        protected internal virtual KenoAuthorization Authorize(KenoResultMetaModel setting, string userId)
        {
            try
            {
                var authorizedUrl = string.Format(setting.AuthorizedUrl, setting.Vendor, setting.VenderKey, userId, setting.VendorProduct);
                var data = _webClient.DownloadString(authorizedUrl);
                if (string.IsNullOrEmpty(data))
                {
                    throw new WebException(GetServiceName(setting.VendorProduct) + "Couldn't get authorized data from: " + authorizedUrl, WebExceptionStatus.ReceiveFailure);
                }

                var serialize = new JavaScriptSerializer();
                return serialize.Deserialize<KenoAuthorization>(data);
            }
            catch (Exception exception)
            {
                throw new WebException(GetServiceName(setting.VendorProduct) + exception.Message, exception);
            }
        }
    }
}