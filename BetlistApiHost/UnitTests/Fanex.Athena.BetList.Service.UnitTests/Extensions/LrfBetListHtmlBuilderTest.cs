namespace Fanex.Athena.BetList.Service.UnitTests.Extensions
{
    using System.Collections;
    using System.Collections.Generic;
    using Common.Constant;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.Extensions;

    public class LrfBetListHtmlBuilderTest
    {
        private BetListHTMLBuilder _betListBuilder;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _betListBuilder = Substitute.ForPartsOf<BetListHTMLBuilder>();
        }

        [TestCase(BetType.BetTradeBought)]
        [TestCase(BetType.CashOutBought)]
        public void BuildLicenseeWL_BoughtTicket_PositiveValue_SeparateWLandComm(BetType betType)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = (int)betType,
                Status = BetStatus.Won,
                Comm = 1,
                LicenseeWL = 2
            };

            const string ExpectedHtml = "<div class ='lic-win-loss'>2.00<br/>1.00</div>";

            // Action
            var result = _betListBuilder.BuildLicenseeWL(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedHtml));
        }

        [Test]
        public void BuildLicenseeWL_RemainingRunningTicket_ShowHyphenCharacter()
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.Handicap,
                Status = BetStatus.Running,
                Comm = 1,
                LicenseeWL = 3
            };

            const string ExpectedHtml = "-";

            // Action
            var result = _betListBuilder.BuildLicenseeWL(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedHtml));
        }

        [TestCase(1, 3, "4.00")]
        [TestCase(-1, -3, "<span class='negative-number'>-4.00</span>")]
        public void BuildLicenseeWL_RemainingTicket_CombineWLandCom(decimal winloss, decimal comm, string expected)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.Handicap,
                Status = BetStatus.Won,
                Comm = comm,
                LicenseeWL = winloss
            };

            var expectedHtml = $"<div class='lic-win-loss'>{expected}</div>";

            // Action
            var result = _betListBuilder.BuildLicenseeWL(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(expectedHtml));
        }

        [Test]
        public void BuildBetEffective_WhenCall_ReturnCorrectFormat()
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetEffective = 3
            };

            const string ExpectedHtml = "<div class='underdog'>3.00</div>";

            // Action
            var result = _betListBuilder.BuildBetEffective(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedHtml));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void BuildCustomerWL_RunningTicket_ReturnHyphenCharacter(bool showPlayerComm)
        {
            // Arrange
            const string ExpectedResult = "-";

            var ticket = new LrfTicket
            {
                Winlost = 1,
                PlayerComm = 2,
                Status = BetStatus.Running,
            };

            // Action
            var result = _betListBuilder.BuildCustomerWL(ticket, showPlayerComm);

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedResult));
        }

        [TestCase(2, 1)]
        [TestCase(1, 2)]
        [TestCase(6, 5)]
        public void BuildCustomerWL_ShowPlayerComm_AppendPlayerCommToResult(decimal winloss, decimal playerComm)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                Winlost = winloss,
                PlayerComm = playerComm,
                Status = BetStatus.Won,
            };

            var expected = $"<div class='underdog'>{winloss.ToString("#,##0.00")}</div>"
                + $"<div class='underdog'>{playerComm.ToString("#,##0.00")}</div>";

            // Action
            var result = _betListBuilder.BuildCustomerWL(ticket, true);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        public void BuildCustomerWL_NoShowPlayerComm_DoNotAppendPlayerCommToResult(decimal winloss)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                Winlost = winloss,
                Status = BetStatus.Won,
            };

            var expected = $"<div class='underdog'>{winloss.ToString("#,##0.00")}</div>";

            // Action
            var result = _betListBuilder.BuildCustomerWL(ticket, false);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(true, "#cc0033", "Live")]
        [TestCase(false, "#000", "NoneLive")]
        public void BuildLiveStatus_IsNotParlayTicket_ReturnCorrectStatus(bool isLive, string expectedColor, string expectStatus)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = 0,
                IsLive = isLive
            };

            var expected = $"<label style='color:{expectedColor}'>{ViewResources.ResourceManager.GetString(expectStatus)}</label>";

            // Action
            var result = _betListBuilder.BuildLiveStatus(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(BetType.MixParlay, LiveStatusTypes.Live, "#cc0033", "Live")]
        [TestCase(BetType.MixParlay, LiveStatusTypes.Mix, "#0033ff", "MixLiveAndNoneLive")]
        [TestCase(BetType.MixParlay, LiveStatusTypes.NonLive, "#000", "NoneLive")]
        [TestCase(BetType.CombinationMixParlay, LiveStatusTypes.Live, "#cc0033", "Live")]
        [TestCase(BetType.CombinationMixParlay, LiveStatusTypes.Mix, "#0033ff", "MixLiveAndNoneLive")]
        [TestCase(BetType.CombinationMixParlay, LiveStatusTypes.NonLive, "#000", "NoneLive")]
        public void BuildLiveStatus_ParlayTicket_ReturnCorrectStatus(int betType, int status, string expectedColor, string expectStatus)
        {
            // Arrange
            var ticket = new LrfTicket
            {
                BetTypeId = betType,
                LiveHomeScore = status
            };

            var expected = $"<label style='color:{expectedColor}'>{ViewResources.ResourceManager.GetString(expectStatus)}</label>";

            // Action
            var result = _betListBuilder.BuildLiveStatus(ticket);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("running", BetStatus.Running)]
        [TestCase("Reject", BetStatus.Reject)]
        [TestCase("WON", BetStatus.Won)]
        [TestCase("lOse", BetStatus.Lose)]
        [TestCase("draW", BetStatus.Draw)]
        [TestCase("WaitIng", BetStatus.Waiting)]
        [TestCase("Void", "voided")]
        [TestCase("RefunD", BetStatus.Refund)]
        public void BuildSingleMatchParlaySelectionStatus_WhenCall_ReturnCorrectStatus(string status, string expectStatus)
        {
            // Arrange
            var sourceType = typeof(CoreBetList);
            var statusProp = sourceType.GetProperty(expectStatus);
            var expected = statusProp.GetValue(sourceType, null);

            // Action
            var result = _betListBuilder.BuildSingleMatchParlaySelectionStatus(status);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}