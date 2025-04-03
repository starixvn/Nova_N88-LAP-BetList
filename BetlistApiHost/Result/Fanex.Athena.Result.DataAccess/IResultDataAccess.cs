namespace Fanex.Athena.Result.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Model.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Fanex.Result.Core.VirtualRacing.DTO;
    using Model.MetaModel;

    [System.CLSCompliant(false)]
    public interface IResultDataAccess
    {
        Task<IList<Outright>> GetOutRightAsync(ResultMetaModel metal);

        #region Filter

        Task<IList<Sport>> GetSportsAsync(DateTime dateValue, int licenseeId);

        Task<IList<League>> GetLeaguesAsync(DateTime dateValue, int sportId);

        #endregion Filter

        #region Report

        Task<IList<T>> GetMatchesAsync<T>(ResultMetaModel metalModel);

        Task<IList<Racing>> GetVirtualSportMatchesAsync(ResultMetaModel metalModel);

        #endregion Report

        #region Popup

        Task<IList<T>> GetPopUpMatchesAsync<T>(ResultPopupMetaModel metaModel);

        Task<IList<Parlay>> GetPopUpParalayMatchAsync(long refNo);

        Task<IList<Parlay>> GetPopUpVS5ParalayMatchAsync(long refNo);

        Task<Sportsbook> GetSportsbookGameDetailAsync(int matchId);

        Task<IList<T>> GetPopUpMatches3rdAsync<T>(int matchId);

        Task<IList<VirtualRacing>> GetPopUpVSNewRacingAsync(int eventId, DateTime eventDate);

        Task<Sportsbook> GetPopUpSingleMatchParlayAsync(int matchId, string marketId);

        #endregion Popup
    }
}