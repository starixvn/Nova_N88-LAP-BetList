using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Fanex.Athena.BetList.DataAccess;
using Fanex.Athena.Common.DataAccess.Interfaces;
using Fanex.Athena.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Common.DataAccess.UnitTests
{
    public class BaseDataAccessHelperTest<T> where T : class
    {
        protected const int CurrencyId = 123;

        protected const string CurrencyName = "aaaaaaaaa";
        protected ICurrencyDataAccess CurrencyDataAccess;

        protected IObjectDb ObjectDb { get; set; }

        protected IDataReader Reader { get; set; }

        protected T DataAccess { get; set; }

        protected IDictionary<int, string> UserCurrencyList { get; } = new Dictionary<int, string>
        {
            { CurrencyId, CurrencyName }, { 1, "USD" }
        };

        [OneTimeSetUp]
        public virtual void FixtureSetUp()
        {
            CurrencyDataAccess = Substitute.For<ICurrencyDataAccess>();
            CurrencyDataAccess.GetCurrencyListAsync().Returns(Task.FromResult(UserCurrencyList));

            ObjectDb = Substitute.For<IObjectDb>();
            SetUpDataAccess();
        }

        [OneTimeTearDown]
        public virtual void FixtureTearDown()
        {
            CurrencyDataAccess = null;
            ObjectDb = null;
            DataAccess = null;
        }

        [SetUp]
        public virtual void SetUp()
        {

            ObjectDb.ClearReceivedCalls();
            Reader = Substitute.For<IDataReader>();
            ObjectDb.ExecuteReader().Returns(Reader);
            ObjectDb.ExecuteReader(Arg.Any<object>()).Returns(Reader);
            ObjectDb.ExecuteReaderAsync().Returns(Reader);
            ObjectDb.ExecuteReaderAsync(Arg.Any<object>()).Returns(Reader);
        }

        [TearDown]
        public virtual void TearDown()
        {
          
            DataAccess.ClearReceivedCalls();
            Reader = null;
        }

        protected virtual void SetUpDataAccess()
        {

            DataAccess = Substitute.ForPartsOf<T>();

        }

        protected void MapReaderData(object readerDataMapping)
        {
            DataAccessTestHelper.MapReaderData(Reader, readerDataMapping);
        }
    }
}