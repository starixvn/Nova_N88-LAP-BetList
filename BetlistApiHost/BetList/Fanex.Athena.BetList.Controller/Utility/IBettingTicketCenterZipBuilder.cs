using System.IO;
using System.Web;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using Ionic.Zip;

namespace Fanex.Athena.BetList.Controller.Utility
{
    public interface IBettingTicketCenterZipBuilder
    {
        MemoryStream BuildZip(HttpResponseBase response, GroupTicketResult ticketResult, string title, AdvancedSearchBetListMetaModel metaModel, string totalText);
    }
}