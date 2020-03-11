namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System.Threading.Tasks;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.UnitTests;
    using Model.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class SingleMatchParlayDetailDataAccessTest : BaseDataAccessHelperTest<SingleMatchParlayDetailDataAccess>
    {
        private const string GetBetBuilderDetailTicketSp = "Lrf_BetList_BetBuilder_Detail";

        [Test]
        public async Task GetSingleMatchParlayDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetBetBuilderDetailTicketSp)).Returns(ObjectDb);
            var metaModel = FakeHelper.Create<SingleMatchParlayDetailMetaModel>();

            Reader.Read().Returns(false);

            // Action
            await DataAccess.GetSingleMatchParlayDetail(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("TransId", metaModel.TransId);
                ObjectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
            });
        }

        [Test]
        public async Task GetSingleMatchParlayDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetBetBuilderDetailTicketSp)).Returns(ObjectDb);
            var metaModel = FakeHelper.Create<SingleMatchParlayDetailMetaModel>();

            Reader.Read().Returns(true, false);

            var expectedResutl = FakeHelper.Create<SingleMatchParlayDetail>();
            MapReaderData(expectedResutl);

            // Action
            var result = await DataAccess.GetSingleMatchParlayDetail(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].SelectionType, Is.EqualTo(expectedResutl.SelectionType));
                Assert.That(result[0].Result, Is.EqualTo(expectedResutl.Result));
            });
        }
    }
}