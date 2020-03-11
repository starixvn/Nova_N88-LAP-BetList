namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using Model.DTO;
    using NUnit.Framework;

    [TestFixture]
    public abstract class BaseControllerHelperTest
    {
        protected void AssertClearData(TicketResult ticketResult)
        {
            Assert.Multiple(() =>
            {
                Assert.That(ticketResult.MainTicketList, Is.Null);
                Assert.That(ticketResult.TicketDataList, Is.Null);
            });
        }
    }
}