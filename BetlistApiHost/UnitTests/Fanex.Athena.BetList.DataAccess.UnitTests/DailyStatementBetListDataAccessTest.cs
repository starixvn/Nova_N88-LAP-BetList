namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Common.DataAccess.Interfaces;
    using Common.DataAccess.UnitTests;
    using Common.Model.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class DailyStatementBetListDataAccessTest : BaseDataAccessHelperTest<DailyStatementBetListDataAccess>
    {
        private const string UspName = "Lrf_Statement_Mem";

        private static readonly BalanceUp BalanceUp = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 1, 1),
            BalanceUpDay = new DateTime(2016, 2, 1)
        };

        private BetListMetaModel _metaModel;

        private static IEnumerable TestCasesValue
        {
            get
            {
                yield return new TestCaseData(new
                {
                    TransDesc = "Test",
                    TotalAmt = 200,
                    ExpectedTransType = "0",
                    ExpectedCreditDebit = 0,
                    TransType = "bal",
                    ExpectedTransTypeHasBetList = false,
                    AllowCustComm = true,
                    Commission = 100,
                    ExpectedBalance = 200, // =  TotalAmt
                    ExternalId = (string)null,
                    ExpectedExternalId = "0",
                    SystemId = (string)null,
                    ExpectedSystemId = "0"
                });
                yield return new TestCaseData(new
                {
                    TransDesc = "outstanding bet",
                    TotalAmt = 200,
                    ExpectedTransType = "3",
                    ExpectedCreditDebit = 0, // = TotalAmt
                    TransType = "no bal",
                    ExpectedTransTypeHasBetList = false,
                    AllowCustComm = true,
                    Commission = 100,
                    ExpectedBalance = 88.5,
                    ExternalId = (string)null,
                    ExpectedExternalId = "0",
                    SystemId = (string)null,
                    ExpectedSystemId = "0"
                });
                yield return new TestCaseData(new
                {
                    TransDesc = "betting statement",
                    TotalAmt = 200,
                    ExpectedTransType = "1",
                    ExpectedCreditDebit = 200, // = TotalAmt
                    TransType = "no bal",
                    ExpectedTransTypeHasBetList = false,
                    AllowCustComm = false,
                    Commission = 100,
                    ExpectedBalance = 200, // =  ExpectedCreditDebit
                    ExternalId = "2",
                    ExpectedExternalId = "2",
                    SystemId = "3",
                    ExpectedSystemId = "3"
                });
                yield return new TestCaseData(new
                {
                    TransDesc = "betting statement",
                    TotalAmt = 200,
                    ExpectedTransType = "1",
                    ExpectedCreditDebit = 200, // = TotalAmt
                    TransType = "gp",
                    ExpectedTransTypeHasBetList = true,
                    AllowCustComm = false,
                    Commission = 100,
                    ExpectedBalance = 0,
                    ExternalId = "4",
                    ExpectedExternalId = "4",
                    SystemId = "5",
                    ExpectedSystemId = "5"
                });
            }
        }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            _metaModel = FakeHelper.Create<BetListMetaModel>();
        }

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(BalanceUp);

            DataAccess = Substitute.ForPartsOf<DailyStatementBetListDataAccess>(
              balanceUpDataAccess);
        }

        [Test]
        public async Task GetDailyStatementData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);

            // Action
            await DataAccess.GetDailyStatementData(_metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("CustID", _metaModel.CustId);
                ObjectDb.Received(1).SetParameterValue("FromDate", _metaModel.FromDate);
                ObjectDb.Received(1).SetParameterValue("ToDate", _metaModel.ToDate);
                ObjectDb.Received(1).SetParameterValue("MoveBetsDay", BalanceUp.MoveBetsDay);
                ObjectDb.Received(1).SetParameterValue("BalanceUpDay", BalanceUp.BalanceUpDay);
                ObjectDb.Received(1).SetParameterValue("PageSize", _metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", _metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("totalrows", null);
                ObjectDb.Received(1).SetParameterValue("BaseCurrency", _metaModel.BaseCurrency);
            });
        }

        [TestCaseSource(nameof(TestCasesValue))]
        public async Task GetDailyStatementData_WhenCall_DataReturnedCorrectly(dynamic testCaseData)
        {
            // Arrange
            _metaModel.ExternalId = testCaseData.ExternalId;
            _metaModel.SystemId = testCaseData.SystemId;
            _metaModel.HasPlayerCom = testCaseData.AllowCustComm;
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            ConfigureReaderForGetDailyStatementData(testCaseData);

            var expected = GetDailyStatementData(_metaModel, testCaseData);

            // Action
            var result = await DataAccess.GetDailyStatementData(_metaModel).ConfigureAwait(false);
            var firstItem = result.ElementAt(0);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.TransType, Is.EqualTo(expected.TransType));
                Assert.That(firstItem.CustId, Is.EqualTo(expected.CustId));
                Assert.That(firstItem.ExternalId, Is.EqualTo(expected.ExternalId));
                Assert.That(firstItem.SystemId, Is.EqualTo(expected.SystemId));
                Assert.That(firstItem.FullStatementDate, Is.EqualTo(expected.FullStatementDate));
                Assert.That(firstItem.TransTypeHasBetList, Is.EqualTo(expected.TransTypeHasBetList));
                Assert.That(firstItem.Index, Is.EqualTo(expected.Index));
                Assert.That(firstItem.Date, Is.EqualTo(expected.Date));
                Assert.That(firstItem.Remark, Is.EqualTo(expected.Remark));
                Assert.That(firstItem.Turnover, Is.EqualTo(expected.Turnover));
                Assert.That(firstItem.CreditDebit, Is.EqualTo(expected.CreditDebit));
                Assert.That(firstItem.Commission, Is.EqualTo(expected.Commission));
                Assert.That(firstItem.Balance, Is.EqualTo(expected.Balance));
            });
        }

        private static DailyStatement GetDailyStatementData(BetListMetaModel metaModel, dynamic testCaseData)
        {
            return new DailyStatement
            {
                TransType = testCaseData.ExpectedTransType,
                CustId = metaModel.CustId,
                ExternalId = testCaseData.ExpectedExternalId,
                SystemId = testCaseData.ExpectedSystemId,
                FullStatementDate = "Friday <br/> 08/19/2016 02:04:02 PM",
                TransTypeHasBetList = testCaseData.ExpectedTransTypeHasBetList,
                Index = 1,
                Date = "08/19/2016",
                Remark = $"<span class='bl-underdog'>{testCaseData.TransType.ToUpper(CultureInfo.CurrentCulture)} <br/>- {testCaseData.TransDesc}</span>",
                Turnover = "<span class='bl-underdog'>11.50</span>",
                CreditDebit = $"<span class='bl-underdog'>{testCaseData.ExpectedCreditDebit:#,##0.00}</span>",
                Commission = $"<span class='bl-underdog'>{testCaseData.Commission:#,##0.00}</span>",
                Balance = $"<span class='bl-underdog'>{testCaseData.ExpectedBalance:#,##0.00}</span>",
            };
        }

        private void ConfigureReaderForGetDailyStatementData(dynamic testCaseData)
        {
            Reader["winlostdate"].Returns(new DateTime(2016, 8, 19, 14, 4, 2));
            Reader["transtype"].Returns((string)testCaseData.TransType);
            Reader["transdesc"].Returns((string)testCaseData.TransDesc);
            Reader["commission"].Returns((decimal)testCaseData.Commission);
            Reader["turnover"].Returns(11.5m);
            Reader["totalamt"].Returns((decimal)testCaseData.TotalAmt);
        }
    }
}