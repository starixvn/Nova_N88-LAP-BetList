namespace Fanex.Athena.Result.Service
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;
    using Common.Model.DTO;
    using DataAccess;
    using Fanex.Result.Core;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.Happy5.DTO;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Interfaces;
    using Model.MetaModel;

    [CLSCompliant(false)]
    public class ResultService : IResultService
    {
        private readonly IResultDataAccess _dataAccess;

        private readonly IResultNumberGameDataAccess _numberGameDataAccess;

        public ResultService(IResultDataAccess dataAccess, IResultNumberGameDataAccess numberGameDataAccess)
        {
            this._dataAccess = dataAccess;
            _numberGameDataAccess = numberGameDataAccess;
        }

        public async Task<IList<Outright>> GetOutRightAsync(ResultMetaModel metal)
        {
            return await this._dataAccess.GetOutRightAsync(metal).ConfigureAwait(false);
        }

        public async Task<IList<Sport>> GetSportsAsync(DateTime dateValue, int licenseeId)
        {
            return await this._dataAccess.GetSportsAsync(dateValue, licenseeId).ConfigureAwait(false);
        }

        public async Task<IList<League>> GetLeaguesAsync(DateTime dateValue, int sportId)
        {
            return await this._dataAccess.GetLeaguesAsync(dateValue, sportId).ConfigureAwait(false);
        }

        #region ResultEngine wrapping

        public MemoryStream ExcelParser<T>(IList<T> dataList, ResultSetting setting)
        {
            return ResultEngine.ExcelParser<T>(dataList, setting);
        }

        public string HtmlGameDetailParser(dynamic data, ResultSetting setting)
        {
            return ResultEngine.HtmlGameDetailParser(data, setting);
        }

        public string HtmlParser<T>(IList<T> dataList, ResultSetting setting)
        {
            return ResultEngine.HtmlParser(dataList, setting);
        }

        #endregion ResultEngine wrapping

        #region Report

        public async Task<IList<Sportsbook>> GetMatchesAsync(ResultMetaModel metalModel)
        {
            return await this._dataAccess.GetMatchesAsync<Sportsbook>(metalModel).ConfigureAwait(false);
        }

        public async Task<IList<NumberGame>> GetNumberGameMatchesAsync(ResultMetaModel metalModel)
        {
            return await this._numberGameDataAccess.GetNumberGameMatchesAsync(metalModel).ConfigureAwait(false);
        }

        public async Task<IList<Happy5>> GetHappy5MatchesAsync(ResultMetaModel metalModel)
        {
            return await this._numberGameDataAccess.GetHappy5MatchesAsync(metalModel).ConfigureAwait(false);
        }

        public async Task<IList<Racing>> GetVirtualSportMatchesAsync(ResultMetaModel metalModel)
        {
            return await this._dataAccess.GetVirtualSportMatchesAsync(metalModel).ConfigureAwait(false);
        }

        #endregion Report

        #region Popup

        [System.CLSCompliant(false)]
        public async Task<Sportsbook> GetSportsbookGameDetailAsync(int matchId)
        {
            return await this._dataAccess.GetSportsbookGameDetailAsync(matchId).ConfigureAwait(false);
        }

        public async Task<IList<Sportsbook>> GetPopUpMatchAsync(ResultPopupMetaModel metalModel)
        {
            metalModel.RaceNo = null;
            return await this._dataAccess.GetPopUpMatchesAsync<Sportsbook>(metalModel).ConfigureAwait(false);
        }

        public async Task<IList<Racing>> GetPopUpVirtualSportMatchAsync(ResultPopupMetaModel metalModel)
        {
            return await this._dataAccess.GetPopUpMatchesAsync<Racing>(metalModel).ConfigureAwait(false);
        }

        public async Task<IList<Parlay>> GetPopUpParlayMatchAsync(long refNo)
        {
            return await this._dataAccess.GetPopUpParalayMatchAsync(refNo).ConfigureAwait(false);
        }

        public async Task<Sportsbook> GetPopUpSingleMatchParlayAsync(int matchId, string marketId)
        {
            return await this._dataAccess.GetPopUpSingleMatchParlayAsync(matchId, marketId).ConfigureAwait(false);
        }

        public async Task<IList<NumberGame>> GetPopUpNumberGameMatchsAsync(int matchId)
        {
            return await this._numberGameDataAccess.GetPopUpNumberGameMatchAsync(matchId).ConfigureAwait(false);
        }

        public async Task<IList<Happy5>> GetPopUpHappy5MatchesAsync(int matchId)
        {
            return await this._numberGameDataAccess.GetPopUpHappy5MatchAsync(matchId).ConfigureAwait(false);
        }

        #endregion Popup
    }
}