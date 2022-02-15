namespace Fanex.Athena.Result.Controller.UnitTests
{
    using Fanex.Athena.Result.Controller.MetaModel;
    using Fanex.Athena.Result.Controller.ResultPopup;
    using Fanex.Result.Core.ThirdParty;
    using Fanex.Result.Core.ThirdParty.Models;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class VoidbridgeResultPopupControllerTest
    {
        [Test]
        public void Index_WhenCall_ReturnResultFromApi()
        {
            // Arrange
            var metaModel = FakeHelper.Create<ThirdPartyMetaModel>();
            var resultApi = Substitute.For<IThirdPartyResult>();
            var request = new ResultRequest(metaModel.CustId, metaModel.TransId3rd, metaModel.GameId3rd, metaModel.CurrencyId, metaModel.SiteId, metaModel.LanguageCode)
            {
                BetTypeId = metaModel.BetTypeId
            };

            using (var controller = new VoidbridgeResultPopupController(resultApi))
            {
                // Action
                var result = controller.Index(metaModel);

                // Assert
                resultApi.Received().GetResultGET(Arg.Is<ResultRequest>(rq => rq.CustId == request.CustId
                    && rq.TranId3rd == request.TranId3rd
                    && rq.GameId3rd == request.GameId3rd
                    && rq.SiteId == request.SiteId));
            }
        }
    }
}