namespace Fanex.Athena.Result.Service.Interfaces
{
    using System;
    using Model.DTO.Keno;
    using Model.MetaModel.Keno;

    public interface IKenoApi
    {
        KenoExtendedToken ExtendToken(KenoResultMetaModel setting, string userId);

        string CreateKenoResultAddress(KenoResultMetaModel setting, string userId, DateTime resultDate);
    }
}