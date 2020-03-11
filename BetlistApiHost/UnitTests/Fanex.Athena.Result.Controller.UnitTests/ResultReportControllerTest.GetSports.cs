namespace Fanex.Athena.Result.Controller.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using System.Web.Script.Serialization;
    using Common.Model.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class ResultReportControllerTest
    {
        [Test]
        public async Task GetSports_IsOutRight_ReturnOutrightResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int SportId = 99;
                var dataList = new List<Outright>
                {
                    new Outright { SportType = SportId }
                };
                var metaModel = new GetSportResultMetaModel
                {
                    SportId = SportId,
                    IsOutRight = true
                };
                _resultService.GetOutRightAsync(metaModel).Returns(dataList);

                // Action
                var result = (await controller.GetSports(metaModel)) as JsonResult;

                // Assert
                Assert.That(
                        new JavaScriptSerializer().Serialize(result.Data),
                        Is.EqualTo(@"{""Sports"":[{""SportId"":99,""SportName"":""""}],""Leagues"":[{""LeagueId"":0,""LeagueName"":"""",""IsOutRight"":false}]}"));
            }
        }

        [Test]
        public async Task GetSports_SportIdIsZero_ReturnNormalSportsResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int SportId = 99;
                var dataList = new List<Sport>
                {
                    new Sport { SportId = SportId }
                };
                var metaModel = new GetSportResultMetaModel
                {
                    SportId = 0,
                    FromDate = new DateTime(2017, 1, 1),
                    LicenseeId = 123,
                    ProductCodeList = "SB"
                };
                _resultService.GetSportsAsync(metaModel.FromDate, metaModel.LicenseeId).Returns(dataList);

                // Action
                var result = await controller.GetSports(metaModel) as JsonResult;

                // Assert
                Assert.That(
                        new JavaScriptSerializer().Serialize(result.Data),
                        Is.EqualTo(@"{""Sports"":[{""SportId"":99,""SportName"":null}],""Leagues"":[]}"));
            }
        }

        [Test]
        public async Task GetSports_SportIdIsNotZero_ReturnCorrectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int SportId = 99;
                var dataList = new List<League>
                {
                    new League()
                };
                var metaModel = new GetSportResultMetaModel
                {
                    SportId = SportId,
                    FromDate = new DateTime(2017, 1, 1)
                };
                _resultService.GetLeaguesAsync(metaModel.FromDate, SportId).Returns(dataList);

                // Action
                var result = await controller.GetSports(metaModel) as JsonResult;

                // Assert
                Assert.That(
                        new JavaScriptSerializer().Serialize(result.Data),
                        Is.EqualTo(@"{""Leagues"":[{""LeagueId"":0,""LeagueName"":null,""IsOutRight"":false}]}"));
            }
        }
    }
}