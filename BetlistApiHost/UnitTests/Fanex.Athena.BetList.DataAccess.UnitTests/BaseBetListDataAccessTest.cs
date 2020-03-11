namespace Fanex.Athena.BetList.DataAccess.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;

    using Athena.BetList.DataAccess;
    using Athena.BetList.Model.DTO;

    using Common.DataAccess.UnitTests;

    using Fanex.Athena.Common.DataAccess.Interfaces;

    using NSubstitute;

    using NUnit.Framework;

    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

    [TestFixture]
    public class BaseBetListDataAccessTest : BaseDataAccessHelperTest<BaseBetListDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<BaseBetListDataAccess>(CurrencyDataAccess);
        }


        public void ReadLrfTicket_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var expected = GetFakeTicket();
            var dataList = GetFakeBetListData(expected);
            Reader.Read().Returns(true, false);
            ConfigureReader(dataList);

            // Action
            var result = DataAccess.ReadBetListTicket(Reader, UserCurrencyList)[0];

            // Assert
            Assert.That(result, Tobe.SameProperties(expected));
        }

        private static IDictionary<string, object> GetFakeBetListData(LrfTicket ticket)
        {
            var playerCommCol = "playercomm";
            var commCol = "comm";
            var licenseeWlCol = "licenseewl";
            var winlostCol = "winlost";
            var beteffectiveCol = "beteffective";
            var stakeCol = "stake";
            var actualStakeCol = "actual_stake";
            var originalStakeCol = "originalstake";

            var dataList = new Dictionary<string, object>
            {
                { playerCommCol, ticket.PlayerComm },
                { commCol, ticket.Comm },
                { licenseeWlCol, ticket.LicenseeWL },
                { winlostCol, ticket.Winlost },
                { beteffectiveCol, ticket.BetEffective },
                { stakeCol, ticket.Stake },
                { actualStakeCol, ticket.ActualStake },
                { originalStakeCol, ticket.OriginalStake }
            };

            AddStringData(dataList, ticket);
            AddBooleanData(dataList, ticket);
            AddDateTimeData(dataList, ticket);
            AddNumbericData(dataList, ticket);

            return dataList;
        }

        private static void AddStringData(Dictionary<string, object> dataList, LrfTicket ticket)
        {
            dataList.Add("username", ticket.UserName);
            dataList.Add("refno", ticket.RefNo);
            dataList.Add("matchcode", ticket.MatchCode);
            dataList.Add("status", ticket.Status);
            dataList.Add("betteam", ticket.BetTeam);
            dataList.Add("betstatus", ticket.EventStatus);
            dataList.Add("showtime", ticket.ShowTime);
            dataList.Add("oddstype", ticket.OddsType);
            dataList.Add("nickname", ticket.ExternalId);
            dataList.Add("race", ticket.Race);
            dataList.Add("betfrom", ticket.BetFrom);
            dataList.Add("betcheck", ticket.BetCheck);
        }

        private static void AddBooleanData(Dictionary<string, object> dataList, LrfTicket ticket)
        {
            dataList.Add("liveindicator", ticket.IsLive);
            dataList.Add("isneutral", ticket.IsNeutral);
        }

        private static void AddDateTimeData(Dictionary<string, object> dataList, LrfTicket ticket)
        {
            dataList.Add("winlostdate", ticket.WinlostDate);
            dataList.Add("transdate", ticket.TransDate);
            dataList.Add("eventdate", ticket.EventDate);
        }

        private static void AddNumbericData(Dictionary<string, object> dataList, LrfTicket ticket)
        {
            dataList.Add("sporttype", ticket.SportTypeId);
            dataList.Add("bettype", ticket.BetTypeId);
            dataList.Add("leagueid", ticket.LeagueId);
            dataList.Add("awayid", ticket.AwayId);
            dataList.Add("homeid", ticket.HomeId);
            dataList.Add("matchid", ticket.MatchId);
            dataList.Add("custid", ticket.CustId);
            dataList.Add("currencyid", ticket.CurrencyId);
            dataList.Add("livehomescore", ticket.LiveHomeScore);
            dataList.Add("liveawayscore", ticket.LiveAwayScore);
            dataList.Add("SpecialLevel", ticket.SpecialLevel);
            dataList.Add("StatusId", ticket.StatusId);

            dataList.Add("transid", ticket.TransId);
            dataList.Add("betid", ticket.BetId);

            dataList.Add("odds", ticket.Odds);
            dataList.Add("modds", ticket.MasterOdds);
            dataList.Add("hdp1", ticket.Handicap1);
            dataList.Add("hdp2", ticket.Handicap2);
            dataList.Add("transdesc", ticket.TransDesc);
            dataList.Add("currency", ticket.Currency);

            dataList.Add("betdaqid", ticket.BetdaqId);
            dataList.Add("extrainfo", ticket.ExtraInfo);
            dataList.Add("originalbettype", ticket.OriginalBetType);
            dataList.Add("oddsspread", ticket.OddsSpread);
            dataList.Add("SettlementStatus", ticket.SettlementStatus);
        }

        private void ConfigureReader(IDictionary<string, object> dataList)
        {
            var index = 0;

            Reader.FieldCount.Returns(dataList.Count);

            foreach (var item in dataList)
            {
                Reader.GetName(index).Returns(item.Key);
                Reader[item.Key].Returns(item.Value);
                index++;
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private LrfTicket GetFakeTicket()
        {
            return new LrfTicket
            {
                UserName = FakeHelper.Create<string>(),
                RefNo = "RefNoTest",
                MatchCode = "MatchCodeTest",
                Status = "StatusTest",
                BetTeam = "BetTeamTest",
                EventStatus = "EventStatusTest",
                ShowTime = "ShowTimeTest",
                OddsType = "OddsTypeTest",
                ExternalId = "ExternalIdTest",
                Race = "RaceTest",
                BetFrom = "BetFromTest",
                BetCheck = "BetCheckTest",
                IsLive = FakeHelper.Create<bool>(),
                IsNeutral = FakeHelper.Create<bool>(),
                SportTypeId = FakeHelper.Create<int>(),
                BetTypeId = FakeHelper.Create<int>(),
                LeagueId = 3,
                HomeId = 4,
                AwayId = 5,
                MatchId = 6,
                CustId = 7,
                CurrencyId = CurrencyId,
                LiveHomeScore = 9,
                LiveAwayScore = 10,
                SpecialLevel = 11,
                StatusId = 12,
                TransId = 100,
                BetId = 101,
                WinlostDate = new DateTime(2016, 8, 10),
                TransDate = new DateTime(2016, 8, 17),
                EventDate = new DateTime(2016, 8, 11),
                PlayerComm = 200.1m,
                Comm = 200.2m,
                LicenseeWL = 200.3m,
                Winlost = 200.4m,
                BetEffective = 200.5m,
                Stake = 200.6m,
                ActualStake = 200.7m,
                Odds = 200.8m,
                MasterOdds = 200.9m,
                Handicap1 = 200.11m,
                Handicap2 = 200.12m,
                TransDesc = "TransDescTest",
                Currency = CurrencyName,
                BetdaqId = 4534,
                ExtraInfo = "sdfsdf",
                OriginalStake = 3434.23m,
                OriginalBetType = 234,
                OddsSpread = 934.23m,
                SettlementStatus = 23
            };
        }
    }
}