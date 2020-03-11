// -----------------------------------------------------------------------
// <copyright file="ResultPopupMetalModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Result.Model.MetaModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// ResultPopupMetalModel class.
    /// </summary>
    [Serializable]
    public class ResultPopupMetaModel
    {
        [Required]
        public int MatchId
        {
            get;
            set;
        }

        [Required]
        public int Bettype { get; set; }

        public int? RaceNo { get; set; }

        [Required]
        public int SportType { get; set; }

        public int? IsOutRight { get; set; }

        public long? RefNo { get; set; }

        public DateTime? WinLossDate { get; set; }

        public long? BetId { get; set; }

        public int CustId { get; set; }

        public int BetCheck { get; set; }

        public int Handicap1 { get; set; }

        public int Handicap2 { get; set; }

        public string LanguageCode { get; set; }
    }
}