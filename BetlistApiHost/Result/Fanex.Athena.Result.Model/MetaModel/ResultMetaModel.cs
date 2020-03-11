// -----------------------------------------------------------------------
// <copyright file="ResultMetalModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Result.Model.MetaModel
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Script.Serialization;

    /// <summary>
    /// ResultMetalModel class.
    /// </summary>
    [Serializable]
    public class ResultMetaModel
    {
        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public bool IsOutRight { get; set; }

        [Required]
        public bool IsExportExcel { get; set; }

        [Required]
        public int SportId { get; set; }

        [Required]
        public int LeagueId { get; set; }

        [ScriptIgnore]
        public int LicenseeId { get; set; }

        [ScriptIgnore]
        public string LanguageCode { get; set; }

        [ScriptIgnore]
        public int UserId { get; set; }
    }
}