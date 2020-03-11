namespace Fanex.Athena.BetList.Service.UnitTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Model.DTO;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.Athena.BetList.Model.ViewModel;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Fanex.Athena.Common.Utility.Services.Interfaces;
    using Fanex.Athena.UnitTest.Common.Helpers;
    using Fanex.BetList.Core.App_GlobalResources;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using NUnit.Framework;

    public class SystemParlayDetailServiceTest
    {
        private ISystemParlayDetailDataAccess _dataAccess;

        private ISystemParlayDetailService _service;

        private ITemplateBuilder _templateBuilder;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<ISystemParlayDetailDataAccess>();
            _templateBuilder = Substitute.For<ITemplateBuilder>();
            _service = new SystemParlayDetailService(_dataAccess, _templateBuilder);
        }

        private static IEnumerable StatusTestCaseValue
        {
            get
            {
                yield return new TestCaseData(new { StatusId = BetStatusId.Won, Expected = CoreBetList.lblWon });
                yield return new TestCaseData(new { StatusId = BetStatusId.Lost, Expected = CoreBetList.lblLose });
                yield return new TestCaseData(new { StatusId = BetStatusId.Draw, Expected = CoreBetList.draw });
                yield return new TestCaseData(new { StatusId = BetStatusId.Running, Expected = CoreBetList.lblRunning });
                yield return new TestCaseData(new { StatusId = BetStatusId.Waiting, Expected = CoreBetList.lblWaiting });
                yield return new TestCaseData(new { StatusId = BetStatusId.Reject, Expected = CoreBetList.reject });
                yield return new TestCaseData(new { StatusId = BetStatusId.Void, Expected = CoreBetList.voided });
                yield return new TestCaseData(new { StatusId = BetStatusId.Refund, Expected = CoreBetList.refund });
            }
        }

        [Test]
        public async Task BuildHtmlContent_WhenCall_BuilderCalledWithCorrectViewModel()
        {
            // Arrange
            var metaModel = FakeHelper.Create<SystemParlayDetailMetaModel>();
            DateTime groupDate1 = new DateTime(2019, 10, 20), groupDate2= new DateTime(2019, 10, 22);
            const int Serial1 = 1, Serial2 = 2, Serial3 = 3;
            const string ViewPath = "ViewPath", ResultContent = "ResultContent";
            var ticketHelper = Substitute.For<ITicketHelper>();

            var ticket1 = new SystemParlayDetailTicket { Serial = Serial1, WinLostDate = groupDate1, WinLost = FakeHelper.Create<decimal>() };
            var ticket2 = new SystemParlayDetailTicket { Serial = Serial2, WinLostDate = groupDate1, WinLost = FakeHelper.Create<decimal>() };
            var ticket3 = new SystemParlayDetailTicket { Serial = Serial3, WinLostDate = groupDate2, WinLost = FakeHelper.Create<decimal>() };

            var serial1 = FakeHelper.Create<SystemParlayDetailSerial>(x => x.Serial, 1);
            var serial3 = FakeHelper.Create<SystemParlayDetailSerial>(x => x.Serial, 3);
            var serial4 = FakeHelper.Create<SystemParlayDetailSerial>(x => x.Serial, 4);

            var fakeData = new SystemParlayDetailData
            {
                Outstanding = FakeHelper.Create<decimal>(),
                TicketList = new List<SystemParlayDetailTicket> { ticket1, ticket2, ticket3 },
                SerialList = new List<SystemParlayDetailSerial> { serial1, serial3, serial4 }
            };
            _dataAccess.GetData(metaModel).Returns(Task.FromResult(fakeData));

            SystemParlayDetailViewModel viewModel = null;
            _templateBuilder.BuildContent(ViewPath, Arg.Any<SystemParlayDetailViewModel>())
                .Returns(x =>
                {
                    viewModel = x.ArgAt<SystemParlayDetailViewModel>(1);
                    return ResultContent;
                });

            // Action
            var result = await _service.BuildHtmlContent(metaModel, ticketHelper, ViewPath);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result, Is.EqualTo(ResultContent));

                Assert.That(viewModel.DataList.First(x => x.Key == groupDate1).Count(), Is.EqualTo(fakeData.TicketList.Count(x => x.WinLostDate == groupDate1)));
                Assert.That(viewModel.DataList.First(x => x.Key == groupDate2).Count(), Is.EqualTo(fakeData.TicketList.Count(x => x.WinLostDate == groupDate2)));

                Assert.That(viewModel.DataList.Count, Is.EqualTo(2));
                Assert.That(viewModel.Total, Is.EqualTo(fakeData.TicketList.Sum(x => x.WinLost)));
                Assert.That(viewModel.Outstanding, Is.EqualTo(fakeData.Outstanding));
            });
        }

        [Test]
        public async Task BuildHtmlContent_WhenCall_ReturnCorrectMatchList()
        {
            // Arrange
            var metaModel = FakeHelper.Create<SystemParlayDetailMetaModel>();

            var ticket = FakeHelper.Create<SystemParlayDetailTicket>();
            var serial = FakeHelper.Create<SystemParlayDetailSerial>(x => x.Serial, ticket.Serial);

            var fakeData = new SystemParlayDetailData
            {
                Outstanding = FakeHelper.Create<decimal>(),
                TicketList = new List<SystemParlayDetailTicket> { ticket },
                SerialList = new List<SystemParlayDetailSerial> { serial }
            };
            _dataAccess.GetData(metaModel).Returns(Task.FromResult(fakeData));

            var ticketHelper = Substitute.For<ITicketHelper>();
            ticketHelper.GetTeamNameById(Arg.Any<object>()).Returns(x => x[0].ToString());

            SystemParlayDetailViewModel viewModel = null;
            _templateBuilder.BuildContent(Arg.Any<string>(), Arg.Any<SystemParlayDetailViewModel>())
                .Returns(x =>
                {
                    viewModel = x.ArgAt<SystemParlayDetailViewModel>(1);
                    return string.Empty;
                });

            // Action
            await _service.BuildHtmlContent(metaModel, ticketHelper, FakeHelper.Create<string>());
            var matchList = viewModel.DataList.ElementAt(0).SelectMany(x => x.MatchList).First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(matchList.HomeTeam, Is.EqualTo(serial.HomeId.ToString()));
                Assert.That(matchList.AwayTeam, Is.EqualTo(serial.AwayId.ToString()));
            });
        }

        [TestCaseSource(nameof(StatusTestCaseValue))]
        public async Task BuildHtmlContent_WhenCall_ReturnCorrectBetListStatusName(dynamic testCaseData)
        {
            // Arrange
            var metaModel = FakeHelper.Create<SystemParlayDetailMetaModel>();
            var ticketHelper = Substitute.For<ITicketHelper>();

            var ticket = FakeHelper.Create<SystemParlayDetailTicket>(x => x.StatusId, (string)testCaseData.StatusId);
            var serial = FakeHelper.Create<SystemParlayDetailSerial>(x => x.Serial, ticket.Serial);

            var fakeData = new SystemParlayDetailData
            {
                Outstanding = FakeHelper.Create<decimal>(),
                TicketList = new List<SystemParlayDetailTicket> { ticket },
                SerialList = new List<SystemParlayDetailSerial> { serial }
            };
            _dataAccess.GetData(metaModel).Returns(Task.FromResult(fakeData));         

            _templateBuilder.BuildContent(Arg.Any<string>(), Arg.Any<SystemParlayDetailViewModel>()).Returns(string.Empty);

            var expected = (string)testCaseData.Expected;

            // Action
            await _service.BuildHtmlContent(metaModel, ticketHelper, FakeHelper.Create<string>());

            // Assert
            Assert.Multiple(() =>
            {
                _templateBuilder.Received().BuildContent(
                    Arg.Any<string>(),
                    Arg.Is<SystemParlayDetailViewModel>(x => x.DataList.First().First().Status == expected));
            });
        }
    }
}