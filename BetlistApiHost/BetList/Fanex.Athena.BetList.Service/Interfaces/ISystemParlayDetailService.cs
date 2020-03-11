namespace Fanex.Athena.BetList.Service.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.BetList.Core.Entities;

    [CLSCompliant(false)]
    public interface ISystemParlayDetailService
    {
        Task<string> BuildHtmlContent(SystemParlayDetailMetaModel metaModel, ITicketHelper ticketHelper, string viewPath);
    }
}