namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System.Collections;
    using System.Threading.Tasks;
    using Athena.BetList.Model.DTO;
    using Athena.BetList.Model.MetaModel;
    using Fanex.BetList.Core.Entities;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    public partial class BetListDataAccessTest
    {
        [TestCase("Lrf_BetList_RNG_Detail")]
        [TestCase("Lrf_BetList_LicenseeBonus_Detail")]
        public async Task GetBetListData_CallSpDoesNotHaveOutPutParam_SumDataToGetSummaryResult(string uspName)
        {
            // Arrange
            var expectResult = new LrfTicket
            {
                Winlost = 1,
                BetEffective = 2,
                LicenseeWL = 3,
                Comm = 4,
                PlayerComm = 5
            };
            DataAccess.GetObjectDb(Arg.Is(uspName)).Returns(ObjectDb);
            MapReaderData(new
            {
                winlost = expectResult.Winlost,
                beteffective = expectResult.BetEffective,
                licenseewl = expectResult.LicenseeWL,
                comm = expectResult.Comm,
                playercomm = expectResult.PlayerComm
            });
            Reader.NextResult().Returns(false);
            Reader.Read().Returns(true, false, false);
            SetUpColumnsForReader(5, new[] { "winlost", "beteffective", "licenseewl", "comm", "playercomm" });

            ObjectDb.GetOutputParams().Returns(x => { return null; });

            var metaModel = new BetListMetaModel { BaseCurrency = string.Empty, UspKey = uspName };

            // Action
            var result = await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.Zero);
                Assert.That(result.SumWinLoss, Is.EqualTo(expectResult.Winlost));
                Assert.That(result.SumBetEffective, Is.EqualTo(expectResult.BetEffective));
                Assert.That(result.SumLicenseeWL, Is.EqualTo(expectResult.LicenseeWL));
                Assert.That(result.SumComm, Is.EqualTo(expectResult.Comm));
                Assert.That(result.SumPlayerComm, Is.EqualTo(expectResult.PlayerComm));
            });
        }

        [TestCase("Lrf_BetList_SelWinLossByDate_Mem")]
        [TestCase("Lrf_BetList_SelWinloss_Mem")]
        public async Task GetBetListData_CallSpHaveOutPutParam_GetSummaryFromOutputParams(string uspName)
        {
            // Arrange
            const string BaseCurrency = "USD";
            var expectSummaryResult = new Hashtable
            {
                { "totalrows", 6 },
                { "sumwinlost", 7 },
                { "sumbeteffective", 8 },
                { "sumlicenseewl", 9 },
                { "sumcomm", 10 },
                { "sumplayercomm", 11 }
            };
            DataAccess.GetObjectDb(Arg.Is(uspName)).Returns(ObjectDb);
            MapReaderData(new
            {
                winlost = 1,
                beteffective = 2,
                licenseewl = 3,
                comm = 4,
                playercomm = 5
            });
            Reader.NextResult().Returns(false);
            Reader.Read().Returns(true, false, false);
            SetUpColumnsForReader(5, new[] { "winlost", "beteffective", "licenseewl", "comm", "playercomm" });

            ObjectDb.GetOutputParams().Returns(expectSummaryResult);

            var metaModel = new BetListMetaModel { BaseCurrency = BaseCurrency, UspKey = uspName };

            // Action
            var result = await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            AssertTicketResult(result, expectSummaryResult);
        }

        [TestCase("Lrf_BetList_SelWinLossByDate_Mem")]
        public async Task GetBetListData_CallSpHaveOnlyOriginalOutPutParam_GetSummaryFromOriginalOutputParams(string uspName)
        {
            // Arrange
            const string BaseCurrency = "usd";
            var expectSummaryResult = new Hashtable
            {
                { "totalrows", 6 },
                { "sumwinlost", 12 },
                { "sumbeteffective", 13 },
                { "sumlicenseewl", 14 },
                { "sumcomm", 15 },
                { "sumplayercomm", 16 },
            };
            DataAccess.GetObjectDb(Arg.Is(uspName)).Returns(ObjectDb);
            MapReaderData(new
            {
                winlost = 1,
                beteffective = 2,
                licenseewl = 3,
                comm = 4,
                playercomm = 5
            });
            Reader.NextResult().Returns(false);
            Reader.Read().Returns(true, false, false);
            SetUpColumnsForReader(5, new[] { "winlost", "beteffective", "licenseewl", "comm", "playercomm" });

            ObjectDb.GetOutputParams().Returns(expectSummaryResult);

            var metaModel = new BetListMetaModel { BaseCurrency = BaseCurrency, UspKey = uspName };

            // Action
            var result = await DataAccess.GetBetListData(metaModel).ConfigureAwait(false);

            // Assert
            AssertTicketResult(result, expectSummaryResult);
        }

        private static void AssertTicketResult(TicketResult result, Hashtable expectSummaryResult)
        {
            Assert.Multiple(() =>
            {
                Assert.That(result.TotalRows, Is.EqualTo(expectSummaryResult["totalrows"]));
                Assert.That(result.SumWinLoss, Is.EqualTo(expectSummaryResult["sumwinlost"]));
                Assert.That(result.SumBetEffective, Is.EqualTo(expectSummaryResult["sumbeteffective"]));
                Assert.That(result.SumLicenseeWL, Is.EqualTo(expectSummaryResult["sumlicenseewl"]));
                Assert.That(result.SumComm, Is.EqualTo(expectSummaryResult["sumcomm"]));
                Assert.That(result.SumPlayerComm, Is.EqualTo(expectSummaryResult["sumplayercomm"]));
            });
        }

        private void SetUpColumnsForReader(int numberOfCols, string[] cols)
        {
            Reader.FieldCount.Returns(numberOfCols);

            for (var i = 0; i < numberOfCols; i++)
            {
                Reader.GetName(i).Returns(cols[i]);
            }
        }
    }
}