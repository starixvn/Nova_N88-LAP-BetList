// -----------------------------------------------------------------------
// <copyright file="IResultDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Result.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Result.Core.Happy5.DTO;
    using Fanex.Result.Core.NumberGame.DTO;
    using Model.MetaModel;

    [System.CLSCompliant(false)]
    public interface IResultNumberGameDataAccess
    {
        Task<IList<NumberGame>> GetNumberGameMatchesAsync(ResultMetaModel metalModel);

        Task<IList<Happy5>> GetHappy5MatchesAsync(ResultMetaModel metalModel);

        Task<IList<NumberGame>> GetPopUpNumberGameMatchAsync(int matchId);

        Task<IList<Happy5>> GetPopUpHappy5MatchAsync(int matchId);
    }
}