namespace Fanex.Athena.Result.Controller.UnitTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Fanex.Result.Core.Base;
    using Fanex.Result.Core.NumberGame.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class ResultReportControllerTest
    {
        [Test]
        public async Task Index_ValidModelState_ResultContentIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _resultService.HtmlParser(Arg.Any<IList<Sportsbook>>(), null).ReturnsForAnyArgs(HtmlResult);

                // Action
                var result = await controller.Index(new ResultMetaModel()).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.EqualTo(HtmlResult));
            }
        }

        [Test]
        public async Task Index_IsOutRight_ResultContentIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const int SportId = 1;
                var metaModel = new ResultMetaModel
                {
                    SportId = SportId,
                    IsOutRight = true
                };
                var dataList = new List<Outright>
                {
                    new Outright
                    {
                        SportType = SportId
                    }
                };
                _resultService.GetOutRightAsync(metaModel).Returns(dataList);
                _resultService.HtmlParser(Arg.Any<IList<Outright>>(), Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var result = await controller.Index(metaModel).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.EqualTo(HtmlResult));
            }
        }

        [TestCase((int)SportType.VirtualHorseRacing)]
        [TestCase((int)SportType.VirtualGreyhounds)]
        [TestCase((int)SportType.VirtualMotorRacing)]
        [TestCase((int)SportType.VirtualCycling)]
        [TestCase((int)SportType.VirtualSpeedway)]
        public async Task IndexPost_VirtualSport_ResultContentIsCorrect(int sportId)
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ResultMetaModel
                {
                    SportId = sportId,
                    IsOutRight = false
                };
                var dataList = new List<Racing> { new Racing() };
                _resultService.GetVirtualSportMatchesAsync(metaModel).Returns(dataList);
                _resultService.HtmlParser(Arg.Any<IList<Racing>>(), Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var result = await controller.Index(metaModel).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.EqualTo(HtmlResult));
            }
        }

        [Test]
        public async Task IndexPost_NumberGame_ResultContentIsCorrect()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new ResultMetaModel
                {
                    SportId = (int)SportType.NumberGame,
                    IsOutRight = false
                };
                var dataList = new List<NumberGame> { new NumberGame() };
                _resultService.GetNumberGameMatchesAsync(metaModel).Returns(dataList);
                _resultService.HtmlParser(Arg.Any<IList<NumberGame>>(), Arg.Any<ResultSetting>()).Returns(HtmlResult);

                // Action
                var result = await controller.Index(metaModel).ConfigureAwait(false);

                // Assert
                Assert.That(result, Is.EqualTo(HtmlResult));
            }
        }
    }
}