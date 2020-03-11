namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System.Threading.Tasks;
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.UnitTests;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public partial class BetListDataAccessTest : BaseDataAccessHelperTest<BetListDataAccess>
    {
        private const string GetMixParlayDetailTicketSp = "Lrf_BetList_SelParlayDetail";

        private readonly BetListMetaModel _metaModel;

        public BetListDataAccessTest()
        {
            _metaModel = FakeHelper.Create<BetListMetaModel>(x => x.ProductCodeList, "SB,NG,BA");
        }

        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<BetListDataAccess>(CurrencyDataAccess);
        }

        [Test]
        public async Task GetMixParlayDetailTicket_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetMixParlayDetailTicketSp)).Returns(ObjectDb);
            var metaModel = new MixParlayDetailMetaModel
            {
                TransId = 123,
                CustId = 232,
            };

            // Action
            await DataAccess.GetMixParlayDetailTicket(metaModel);

            // Assert
            ObjectDb.Received(1).SetParameterValue("TransId", metaModel.TransId);
            ObjectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
        }
    }
}