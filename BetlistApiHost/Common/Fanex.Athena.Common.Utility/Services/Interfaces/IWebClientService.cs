using System;
using System.Collections.Generic;
using System.Net;

namespace Fanex.Athena.Common.Utility.Services.Interfaces
{
    public interface IWebClientService
    {
        string UploadString(Uri address, string parameters);

        string DownloadString(string address);

        byte[] DownloadData(string address);

        string UploadString(Uri address, string parameters, IDictionary<HttpRequestHeader, string> headers);
    }
}