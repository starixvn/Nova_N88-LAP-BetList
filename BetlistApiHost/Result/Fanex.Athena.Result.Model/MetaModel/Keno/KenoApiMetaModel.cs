// -----------------------------------------------------------------------
namespace Fanex.Athena.Result.Model.MetaModel.Keno
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings", Justification = "Reviewed. Suppression is OK here.")]
    public class KenoResultMetaModel
    {
        public string ResultUrl { get; set; }

        public string SiteID { get; set; }

        public string Vendor { get; set; }

        public string VenderKey { get; set; }

        public string AuthorizedUrl { get; set; }

        public string ExpireTokenUrl { get; set; }

        public string ExtendTokenUrl { get; set; }

        public string KickOutUrl { get; set; }

        public string Language { get; set; }

        public int VendorProduct { get; set; }
    }
}
