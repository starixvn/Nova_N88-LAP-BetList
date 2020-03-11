namespace Fanex.Athena.BetList.Service.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Constant;
    using DataAccess.Interfaces;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Interfaces;
    using Model.DTO;
    using Model.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class BettingTicketCenterServiceTest
    {
        private readonly IDictionary<int, string> _currencyList = new Dictionary<int, string> { { 20, "UUS" } };

        private IBettingTicketCenterDataAccess _bettingTicketCenterDataAccess;

        private IBettingTicketCenterMultiCustomerDataAccess _bettingTicketCenterMultiCustomerDataAccess;

        private IBetListHTMLBuilder _betlistHtmlBuilder;

        private ITicketHelper _ticketHelper;

        private IBettingTicketCenterService _bettingTicketCenterService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _bettingTicketCenterDataAccess = Substitute.For<IBettingTicketCenterDataAccess>();
            _bettingTicketCenterMultiCustomerDataAccess = Substitute.For<IBettingTicketCenterMultiCustomerDataAccess>();
            _betlistHtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            _ticketHelper = Substitute.For<ITicketHelper>();
            _bettingTicketCenterService = new BettingTicketCenterService(
                _bettingTicketCenterDataAccess,
                _bettingTicketCenterMultiCustomerDataAccess,
                _betlistHtmlBuilder
                );
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _bettingTicketCenterDataAccess = null;
            _bettingTicketCenterMultiCustomerDataAccess = null;
            _bettingTicketCenterService = null;
        }
        [Test]
        public async Task GetBetListForMultipleCustomersAsync_GetDataSavedInFile_CallDataAccess()
        {
            // Arrange
            var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>();
            var currencyList = FakeHelper.Create<IDictionary<int, string>>();
            var rowPerGroup = FakeHelper.Create<int>();

            // Action
            await _bettingTicketCenterService.GetBetListForMultipleCustomersAsync(metaModel, rowPerGroup, currencyList);

            // Assert
            Assert.Multiple(() =>
            {
                _bettingTicketCenterMultiCustomerDataAccess.Received(1).GetBetListForMultipleCustomersAsync(metaModel, rowPerGroup, currencyList);
            });
        }

        [Test]
        public void GetBetListForMultipleCustomersAsync_WhenCall_CallDataAccess()
        {
            // Arrange
            var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>();
            var currencyList = FakeHelper.Create<IDictionary<int, string>>();

            // Action
            _bettingTicketCenterService.GetBetListForMultipleCustomersAsync(metaModel, currencyList);

            // Assert
            _bettingTicketCenterMultiCustomerDataAccess.Received().GetBetListForMultipleCustomersAsync(metaModel, currencyList);
        }

        [Test]
        public void GetTotalRowForMultipleCustomersAsync_WhenCall_CallDataAccess()
        {
            // Arrange
            var metaModel = FakeHelper.Create<MultipleCustomersBetListMetaModel>();

            // Action
            _bettingTicketCenterService.GetTotalRowForMultipleCustomersAsync(metaModel);

            // Assert
            _bettingTicketCenterMultiCustomerDataAccess.Received().GetTotalRowForMultipleCustomersAsync(metaModel);
        }

        [Test]
        public void GetBetListForSingleCustomerAsync_WhenCall_CallDataAccess()
        {
            // Arrange
            var metaModel = FakeHelper.Create<SingleCustomersBetListMetaModel>();
            var currencyList = FakeHelper.Create<IDictionary<int, string>>();

            // Action
            _bettingTicketCenterService.GetBetListForSingleCustomerAsync(metaModel, currencyList);

            // Assert
            _bettingTicketCenterDataAccess.Received().GetBetListForSingleCustomerAsync(metaModel, currencyList);
        }

        [Test]
        public void GetBetListByRefnoAsync_WhenCall_CallDataAccess()
        {
            // Arrange
            var metaModel = FakeHelper.Create<RefNoBetListMetaModel>();
            var currencyList = FakeHelper.Create<IDictionary<int, string>>();

            // Action
            _bettingTicketCenterService.GetBetListByRefnoAsync(metaModel, currencyList);

            // Assert
            _bettingTicketCenterDataAccess.Received().GetBetListByRefnoAsync(metaModel, currencyList);
        }

        [Test]
        public void GetRunningBetListAsync_WhenCall_CallDataAccess()
        {
            // Arrange
            var metaModel = FakeHelper.Create<BaseBettingTicketMetaModel>();
            var currencyList = FakeHelper.Create<IDictionary<int, string>>();

            // Action
            _bettingTicketCenterService.GetRunningBetListAsync(metaModel, currencyList);

            // Assert
            _bettingTicketCenterDataAccess.Received().GetRunningBetListAsync(metaModel, currencyList);
        }

        [Test]
        public void BuildSumWinLoss_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var ticket = new TicketResult();
            var isAllowedCustomerComm = FakeHelper.Create<bool>();

            // Action
            var result = _bettingTicketCenterService.BuildSumWinLoss(ticket, isAllowedCustomerComm);

            // Assert
            Assert.That(result, Is.Not.Empty);
        }


        [TestCase(false, "", "EUR", "1/1/0001", "1/1/0001", "Betting Ticket Center")]
        [TestCase(false, "EUR", "", "1/1/2001", "12/1/2002", "Betting Ticket Center, 01/01/2001-->12/01/2002, Base Currency: EUR")]
        [TestCase(true, "", null, "1/1/2001", "12/1/2002", "Betting Ticket Center, 01/01/2001-->12/01/2002")]
        [TestCase(true, "EUR", "USD", "1/1/2001", "12/1/2002", "Betting Ticket Center, 01/01/2001-->12/01/2002, Base Currency: EUR")]
        [TestCase(true, "EUR", "", "1/1/2001", "12/1/2002", "Betting Ticket Center, 01/01/2001-->12/01/2002, Base Currency: EUR")]
        public void GetTitle_WhenCall_ReturnCorrectTitle(bool isConvert, string baseCurrency, string baseCurrencyName, string fromDate, string toDate, string expectedTitle)
        {
            // Arrange
            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Parse(fromDate),
                ToDate = DateTime.Parse(toDate),
                BaseCurrency = baseCurrency,
                BaseCurrencyName = baseCurrencyName,
                IsConvertAll = isConvert
            };

            // Action
            var title = _bettingTicketCenterService.GetTitle(metaModel);

            // Assert
            Assert.That(title, Is.EqualTo(expectedTitle));
        }
    }
}