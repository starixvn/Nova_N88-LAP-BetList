namespace Fanex.Athena.Common.Utility.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Interfaces;

    public class WebClientService : IWebClientService
    {
        public string UploadString(Uri address, string parameters)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                return webClient.UploadString(address, parameters);
            }
        }

        public string DownloadString(string address)
        {
            using (WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                return webClient.DownloadString(address);
            }
        }

        public byte[] DownloadData(string address)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadData(address);
            }
        }

        public string UploadString(Uri address, string parameters, IDictionary<HttpRequestHeader, string> headers)
        {
            using (WebClient webClient = new WebClient())
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        webClient.Headers[header.Key] = header.Value;
                    }
                }

                return webClient.UploadString(address, parameters);
            }
        }
    }
}
