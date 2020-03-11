namespace Fanex.Athena.Excel.BetList.Builders.UnitTests
{
    using Athena.BetList.Excel.Builders;
    using Athena.BetList.Model.DTO;
    using Common.Constant;
    using Fanex.Athena.BetList.Model.MetaModel;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using NPOI.HSSF.UserModel;
    using NPOI.HSSF.Util;
    using NPOI.SS.UserModel;
    using NPOI.SS.Util;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    public class BettingTicketCenterExcelTest
    {
        private const int NumberOfColumns = 14;
        private const int ColumnTotalStart = 8;

        private ITicketHelper _ticketHelper;
        private IBetListExcelBuilder _builder;

        private ICellStyle _oddStyle;
        private ICellStyle _evenStyle;

        private IBettingTicketCenterExcelBuilder _excelBuilder;

        [SetUp]
        public void Setup()
        {
            _ticketHelper = Substitute.For<ITicketHelper>();
            _builder = Substitute.For<IBetListExcelBuilder>();

            _excelBuilder = Substitute.ForPartsOf<BettingTicketCenterExcelBuilder>(
                _builder,
                _ticketHelper);

            _oddStyle = Substitute.For<ICellStyle>();
            _oddStyle.FillBackgroundColor = 2;
            _builder.OddCellStyle.Returns(_oddStyle);

            _evenStyle = Substitute.For<ICellStyle>();
            _evenStyle.FillBackgroundColor = 1;
            _builder.EvenCellStyle.Returns(_evenStyle);

            _builder.CreateCellStyle().Returns(_oddStyle, _evenStyle, _oddStyle, _evenStyle);
            _builder.When(x => x.BuildWorksheet(Arg.Any<string>(), Arg.Any<short>())).DoNotCallBase();
        }

        [TearDown]
        public void TearDown()
        {
            _ticketHelper = null;
            _builder = null;
        }

        [Test]
        public void GetWorkbook_WhenCall_CallGetBetListFunctionFromBuilder()
        {
            // Action
            _excelBuilder.GetWorkbook();

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).GetBetList();
            });
        }

        [TestCase(true, true, true, true, NumberOfColumns)]
        [TestCase(false, false, false, false, NumberOfColumns - 4)]
        [TestCase(true, false, true, false, NumberOfColumns - 2)]
        [TestCase(false, true, false, true, NumberOfColumns - 2)]
        public void CreateSheet_WhenCall_BuildWorksheetWithCorrectParameters(
            bool showWinLoss,
            bool showLicenseeWL,
            bool showExtternalId,
            bool showSystemId,
            short expectedCols)
        {
            // Arrange
            var sheetName = FakeHelper.Create<string>();
            var option = new BettingTicketCenterOption
            {
                ShowWinLoss = showWinLoss,
                ShowLicenseeWl = showLicenseeWL,
                ShowExternalId = showExtternalId,
                ShowSystemId = showSystemId
            };

            // Action
            _excelBuilder.CreateSheet(sheetName, option);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).BuildWorksheet(sheetName, expectedCols);
            });
        }

        [TestCase(true, true, true, true, NumberOfColumns - 1)]
        [TestCase(false, false, false, false, NumberOfColumns - 5)]
        [TestCase(true, false, true, false, NumberOfColumns - 3)]
        [TestCase(false, true, false, true, NumberOfColumns - 3)]
        public void AddNoInfoRow_WhenCall_WorkCorrectly(
            bool showWinLoss,
            bool showLicenseeWL,
            bool showExtternalId,
            bool showSystemId,
            int expectedMergedLastCol)
        {
            // Arrange
            var titleStyle = Substitute.For<ICellStyle>();
            _builder.CreateCellStyle().Returns(titleStyle);

            // Action
            _excelBuilder.AddNoInfoRow(new BettingTicketCenterOption
            {
                ShowWinLoss = showWinLoss,
                ShowLicenseeWl = showLicenseeWL,
                ShowExternalId = showExtternalId,
                ShowSystemId = showSystemId
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(titleStyle.Alignment, Is.EqualTo(HorizontalAlignment.Center));

                _builder.Received(1).AddCell(
                    Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.NoInfo),
                    titleStyle);
                _builder.Received(1).MergeCell(
                    Arg.Is<CellRangeAddress>(x => x.FirstRow == 2 && x.LastRow == 2
                    && x.FirstColumn == 0 && x.LastColumn == expectedMergedLastCol));
            });
        }

        [TestCase(true, true, true, true, NumberOfColumns - 1)]
        [TestCase(false, false, false, false, NumberOfColumns - 5)]
        [TestCase(true, false, true, false, NumberOfColumns - 3)]
        [TestCase(false, true, false, true, NumberOfColumns - 3)]
        public void AddTitle_WhenCall_WorkCorrectly(
            bool showWinLoss,
            bool showLicenseeWL,
            bool showExtternalId,
            bool showSystemId,
            int expectedMergedLastCol)
        {
            // Arrange
            const string Title = "sddflghdlkfjghldjfg";
            var font = Substitute.For<IFont>();
            _builder.CreateFont().Returns(font);
            var titleStyle = Substitute.For<ICellStyle>();
            _builder.CreateCellStyle().Returns(titleStyle);

            // Action
            _excelBuilder.AddTitle(
                Title,
                new BettingTicketCenterOption
                {
                    ShowWinLoss = showWinLoss,
                    ShowLicenseeWl = showLicenseeWL,
                    ShowExternalId = showExtternalId,
                    ShowSystemId = showSystemId
                });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(font.Boldweight, Is.EqualTo((short)FontBoldWeight.Bold));
                Assert.That(font.Color, Is.EqualTo(HSSFColor.Orange.Index));

                Assert.That(titleStyle.VerticalAlignment, Is.EqualTo(VerticalAlignment.Center));
                titleStyle.Received(1).SetFont(font);

                _builder.Received(1).AddCell(
                    Arg.Is<HSSFRichTextString>(x => x.String == Title),
                    titleStyle);
                _builder.Received(1).MergeCell(
                    Arg.Is<CellRangeAddress>(x => x.FirstRow == 0 && x.LastRow == 0
                    && x.FirstColumn == 0 && x.LastColumn == expectedMergedLastCol));
            });
        }

        [TestCase(false, "Win Loss")]
        [TestCase(true, "Win Loss/Comm")]
        public void AddHeader_WhenCall_WorkCorrectly(bool isShowCustomerComm, string winLossHeader)
        {
            // Arrange
            var cellStyle = Substitute.For<ICellStyle>();
            _builder.CreateCellStyle().Returns(cellStyle);
            var captions = new[]
            {
                "#",
                ViewResources.ExternalId,
                ViewResources.SystemId,
                ViewResources.TransTime,
                ViewResources.Choice,
                ViewResources.Odds,
                ViewResources.Stake,
                winLossHeader,
                ViewResources.LicenseeWL + "/" + ViewResources.Comm,
                ViewResources.Status,
                ViewResources.Platform,
                ViewResources.Live
            };

            // Action
            _excelBuilder.AddHeader(new BettingTicketCenterOption
            {
                ShowWinLoss = true,
                ShowLicenseeWl = true,
                IsAllowedCustomerComm = isShowCustomerComm,
                ShowSystemId = true,
                ShowExternalId = true
            });

            // Assert
            Assert.Multiple(() =>
            {
                for (int i = 0; i < captions.Length; i++)
                {
                    _builder.Received(1).AddCell(
                        Arg.Is<HSSFRichTextString>(x => x.String == captions[i]),
                        cellStyle);
                }
            });
        }

        [Test]
        public void AddHeader_IsNotShownWinLossColumn_NoWinLossCellCreated()
        {
            // Arrange
            var cellStyle = Substitute.For<ICellStyle>();
            _builder.CreateCellStyle().Returns(cellStyle);

            // Action
            _excelBuilder.AddHeader(new BettingTicketCenterOption { ShowWinLoss = false, ShowLicenseeWl = true });

            // Assert
            _builder.DidNotReceive().AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.WinLoss),
                cellStyle);
        }

        [Test]
        public void AddHeader_IsNotShownLicenseeWLColumn_NoLicenseeWLCellCreated()
        {
            // Arrange
            var cellStyle = Substitute.For<ICellStyle>();
            _builder.CreateCellStyle().Returns(cellStyle);

            // Action
            _excelBuilder.AddHeader(new BettingTicketCenterOption { ShowWinLoss = true, ShowLicenseeWl = false });

            // Assert
            _builder.DidNotReceive().AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.LicenseeWL + "/" + ViewResources.Comm),
                cellStyle);
        }

        [TestCase(false, false, false, false, ColumnTotalStart - 2)]
        [TestCase(true, true, true, true, ColumnTotalStart)]
        [TestCase(false, true, false, true, ColumnTotalStart - 1)]
        [TestCase(true, false, true, false, ColumnTotalStart - 1)]
        public void AddFooter_WhenCall_WorkCorrectly(
            bool showExternalId,
            bool showSystemId,
            bool showWinLoss,
            bool showLicenseeWL,
            int expectedColumnTotalStart)
        {
            // Arrange
            const int ExpectedNonTotalColumn = 3;
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            var totalText = FakeHelper.Create<string>();
            var option = new BettingTicketCenterOption {
                ShowExternalId = showExternalId,
                ShowSystemId = showSystemId,
                ShowLicenseeWl = showLicenseeWL,
                ShowWinLoss = showWinLoss,
                ShowSumStake = true,
                IsAllowedCustomerComm = true
            };

            // Action
            _excelBuilder.AddFooter(0, 0, 0, 0, totalText, option);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(expectedColumnTotalStart).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == totalText), Arg.Any<ICellStyle>());
                _builder.Received(ExpectedNonTotalColumn).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == string.Empty), Arg.Any<ICellStyle>());
            });
        }

        [TestCase(45698, "45,698.00")]
        [TestCase(0.1, "0.10")]
        [TestCase(-654321.123, "-654,321.12")]
        public void AddFooter_ShowSumStake_AddSumStakeCell(decimal original, string expected)
        {
            // Arrange
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(original, 0, 0, 0, FakeHelper.Create<string>(), FakeHelper.Create<BettingTicketCenterOption>(x => x.ShowSumStake, true));

            // Assert
            _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == expected), Arg.Any<ICellStyle>());
        }

        [TestCase(0.996, "1.00")]
        [TestCase(0.2, "0.20")]
        [TestCase(-0.99, "-0.99")]
        public void AddFooter_ShowSumWinLoss_AddSumWinLossCell(decimal original, string expected)
        {
            // Arrange
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(0, original, 0, 0, FakeHelper.Create<string>(), FakeHelper.Create<BettingTicketCenterOption>(x => x.ShowWinLoss, true));

            // Assert
            _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == expected), Arg.Any<ICellStyle>());
        }

        [TestCase(0.996, 0, true, "1.00\n0.00")]
        [TestCase(0.8, 1.258, false, "0.80")]
        [TestCase(-0.99, 0.111, true, "-0.99\n0.11")]
        public void AddFooter_ShowSumPlayerComm_AddSumWinLossCell(decimal originalWL, decimal originalComm, bool isAllowedCustomerComm, string expected)
        {
            // Arrange
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(
                0,
                originalWL,
                originalComm,
                0,
                FakeHelper.Create<string>(),
                new BettingTicketCenterOption { ShowWinLoss = true, IsAllowedCustomerComm = isAllowedCustomerComm });

            // Assert
             _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == expected), Arg.Any<ICellStyle>());
        }

        [TestCase(12.345, "12.35")]
        [TestCase(0.5, "0.50")]
        [TestCase(-654321, "-654,321.00")]
        public void AddFooter_ShowSumLicenseeWL_AddSumLicenseeWLCell(decimal original, string expected)
        {
            // Arrange
            var lastRow = FakeHelper.Create<int>();
            _builder.LastContentRow.Returns(lastRow);
            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AddFooter(0, 0, 0, original, FakeHelper.Create<string>(), FakeHelper.Create<BettingTicketCenterOption>(x => x.ShowLicenseeWl, true));

            // Assert
            _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == expected), Arg.Any<ICellStyle>());
        }

        [Test]
        public void AssignRowData_IsShowWinLoss_BuildCustomerWLColumn()
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.Handicap,
                Status = BetStatus.Won,
                Winlost = 100m
            };
            tickets.MainTicketList.Add(ticket);

            _ticketHelper.Index.Returns(0);

            _excelBuilder.InitializeFonts();
            _excelBuilder.InitializeCellStyles();

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, 1, new BettingTicketCenterOption { ShowWinLoss = true });

            // Assert
            _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == "100.00"));
        }

        [Test]
        public void AssignRowData_IsShowLicenseeWls_BuildCustomerWLColumn()
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.Handicap,
                Status = BetStatus.Won,
                LicenseeWL = 200m
            };
            tickets.MainTicketList.Add(ticket);

            _ticketHelper.Index.Returns(0);

            _excelBuilder.InitializeFonts();
            _excelBuilder.InitializeCellStyles();

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, 1, new BettingTicketCenterOption { ShowLicenseeWl = true });

            // Assert
            _builder.Received(1).AddCell(Arg.Is<HSSFRichTextString>(x => x.String == "200.00"));
        }

        [TestCase("a", "China")]
        [TestCase("z", "China")]
        [TestCase("g", "Asia")]
        [TestCase("e", "Euro")]
        [TestCase("u", "Euro")]
        [TestCase("t", "DeepLink")]
        [TestCase("m", "Wap")]
        [TestCase("v", "BetfromOthers")]
        [TestCase("0", "NativeApp")]
        public void AssignRowData_AddPlatfromCell_IndexIsOdd_BuildCorrectCellWithOddCellStyle(string betFrom, string platformName)
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetFrom = betFrom,
                BetTypeId = (int)BetType.Handicap,
                Status = BetStatus.Won
            };
            tickets.MainTicketList.Add(ticket);

            _ticketHelper.Index.Returns(0);

            _excelBuilder.InitializeFonts();
            _excelBuilder.InitializeCellStyles();

            var expectedPlatformName = ViewResources.ResourceManager.GetString(platformName);

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, 1, FakeHelper.Create<BettingTicketCenterOption>(x => x.IsAllowedCustomerComm, true));

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == expectedPlatformName),
                Arg.Is<ICellStyle>(x => x.Alignment == HorizontalAlignment.Center
                && x.FillBackgroundColor == _evenStyle.FillBackgroundColor));
        }

        [TestCase("t", "DeepLink")]
        [TestCase("l", "Mobile")]
        [TestCase("r", "Tablet")]
        [TestCase("x", "NewAsia")]
        [TestCase("o", "MobileApp")]
        [TestCase("c", "MobileLiteVersion")]
        public void AssignRowData_AddPlatfromCell_IndexIsEven_BuildCorrectCellWithOddCellStyle(string betFrom, string platformName)
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetFrom = betFrom,
                BetTypeId = (int)BetType.Handicap,
                Status = BetStatus.Won
            };
            tickets.MainTicketList.Add(ticket);

            _ticketHelper.Index.Returns(1);

            _excelBuilder.InitializeFonts();
            _excelBuilder.InitializeCellStyles();

            var expectedPlatformName = ViewResources.ResourceManager.GetString(platformName);

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, 1, FakeHelper.Create<BettingTicketCenterOption>(x => x.IsAllowedCustomerComm, true));

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == expectedPlatformName),
                Arg.Is<ICellStyle>(x => x.Alignment == HorizontalAlignment.Center
                && x.FillBackgroundColor == _oddStyle.FillBackgroundColor));
        }

        [TestCase(BetType.MixParlay, LiveStatusTypes.Live, false)]
        [TestCase(BetType.MixParlay, LiveStatusTypes.Live, true)]
        [TestCase(BetType.Handicap, LiveStatusTypes.All, true)]
        [TestCase(BetType.CombinationMixParlay, LiveStatusTypes.Live, false)]
        [TestCase(BetType.CombinationMixParlay, LiveStatusTypes.Live, true)]
        public void AssignRowData_AddLiveCell_BuildLiveCell(BetType bettype, int liveHomeScore, bool isLive)
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetTypeId = (int)bettype,
                Status = BetStatus.Won,
                IsLive = isLive,
                LiveHomeScore = liveHomeScore
            };
            tickets.MainTicketList.Add(ticket);

            _ticketHelper.Index.Returns(2);

            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, 2, FakeHelper.Create<BettingTicketCenterOption>(x => x.IsAllowedCustomerComm, false));

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.Live),
                Arg.Any<ICellStyle>());
        }

        [TestCase(BetType.MixParlay, 0, false)]
        [TestCase(BetType.MixParlay, 0, true)]
        [TestCase(BetType.Handicap, LiveStatusTypes.All, false)]
        [TestCase(BetType.CombinationMixParlay, 0, false)]
        [TestCase(BetType.CombinationMixParlay, 0, true)]
        public void AssignRowData_AddLiveCell_BuildNonLiveCell(BetType bettype, int liveHomeScore, bool isLive)
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetTypeId = (int)bettype,
                Status = BetStatus.Won,
                IsLive = isLive,
                LiveHomeScore = liveHomeScore
            };
            tickets.MainTicketList.Add(ticket);

            _ticketHelper.Index.Returns(3);

            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, 2, FakeHelper.Create<BettingTicketCenterOption>(x => x.IsAllowedCustomerComm, false));

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.NoneLive),
                Arg.Any<ICellStyle>());
        }

        [Test]
        public void AssignRowData_AddLiveCell_BuildMixLiveAndNoneLiveCell()
        {
            // Arrange
            var tickets = new TicketResult();
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.MixParlay,
                Status = BetStatus.Won,
                LiveHomeScore = LiveStatusTypes.Mix
            };
            tickets.MainTicketList.Add(ticket);

            _ticketHelper.Index.Returns(4);

            _excelBuilder.InitializeFonts();

            // Action
            _excelBuilder.AssignRowData(tickets, ticket, 3, FakeHelper.Create<BettingTicketCenterOption>(x => x.IsAllowedCustomerComm, true));

            // Assert
            _builder.Received(1).AddCell(
                Arg.Is<HSSFRichTextString>(x => x.String == ViewResources.MixLiveAndNoneLive),
                Arg.Any<ICellStyle>());
        }
    }
}