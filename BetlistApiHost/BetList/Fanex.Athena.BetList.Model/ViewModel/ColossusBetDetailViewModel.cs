namespace Fanex.Athena.BetList.Model.ViewModel
{
    using System.Collections.Generic;
    using DTO;

    public class ColossusBetDetailViewModel
    {
        public IEnumerable<ColossusBetDetail> DataList { get; set; }

        public decimal TotalSoldPercent { get; set; }

        public decimal TotalWinLoss { get; set; }

        public decimal TotalComm { get; set; }

        public decimal TotalSoldWinLoss { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3996:UriPropertiesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        public string IframeUrl { get; set; }
    }
}