namespace Fanex.Athena.Result.Model.MetaModel
{
    using System;
    using System.Web.Script.Serialization;

    [Serializable]
    public class GetSportResultMetaModel : ResultMetaModel
    {
        [ScriptIgnore]
        public string ProductCodeList { get; set; }
    }
}