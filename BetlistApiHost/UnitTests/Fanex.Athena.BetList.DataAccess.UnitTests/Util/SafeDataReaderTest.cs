namespace Fanex.Athena.BetList.DataAccess.UnitTests.Util
{
    using System;
    using System.Data;
    using DataAccess.Util;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class SafeDataReaderTest
    {
        private readonly string _existingColumn = FakeHelper.Create<string>();

        private readonly string _nonExistingColumn = FakeHelper.Create<string>();

        private ISafeDataReader _safeDataReader;

        private IDataReader _dataReader;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _dataReader = Substitute.For<IDataReader>();

            var columns = new[] { _existingColumn, _nonExistingColumn };
            _dataReader.GetName(0).Returns(_existingColumn);
            _dataReader.GetName(1).Returns(_nonExistingColumn + "NotExist");

            _dataReader.FieldCount.Returns(columns.Length);

            _safeDataReader = new SafeDataReader(_dataReader, columns);
        }

        [TearDown]
        public void TearDown()
        {
            _dataReader.ClearReceivedCalls();
        }

        [Test]
        public void Read_WhenCall_CallDataReaderMethod()
        {
            // Action
            _safeDataReader.Read();

            // Assert
            _dataReader.Received(1).Read();
        }

        [TestCase(null, false)]
        [TestCase(false, false)]
        [TestCase(true, true)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(-1, false)]
        public void GetBool_WhenCall_ReturnCorrecData(object value, bool expected)
        {
            // Arrange
            _dataReader[_existingColumn].Returns(value);

            // Action
            var result = _safeDataReader.GetBool(_existingColumn);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void GetBool_DataIsDBNull_ReturnFalse()
        {
            // Arrange
            _dataReader[_existingColumn].Returns(Convert.DBNull);

            // Action
            var result = _safeDataReader.GetBool(_existingColumn);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetBool_InvaidData_ReturnFalse()
        {
            // Arrange
            var data = FakeHelper.Create<string>();
            _dataReader[_existingColumn].Returns(data);

            // Action
            var result = _safeDataReader.GetBool(_existingColumn);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void GetValue_IsExistingColumn_ReturnDataFromColumn()
        {
            // Arrange
            var data = FakeHelper.Create<string>();
            _dataReader[_existingColumn].Returns(data);

            // Action
            var result = _safeDataReader.GetValue(_existingColumn);

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        [Test]
        public void GetValue_IsNotExistingColumn_ReturnDefaultValue()
        {
            // Action
            var result = _safeDataReader.GetValue(_nonExistingColumn);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetString_IsExistingColumn_ReturnDataFromColumn()
        {
            // Arrange
            var data = FakeHelper.Create<int>();
            _dataReader[_existingColumn].Returns(data);

            // Action
            var result = _safeDataReader.GetString(_existingColumn);

            // Assert
            Assert.That(result, Is.EqualTo(data.ToString()));
        }

        [Test]
        public void GetString_IsNotExistingColumn_ReturnDefaultValue()
        {
            // Action
            var result = _safeDataReader.GetString(_nonExistingColumn);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetInt_IsExistingColumn_ReturnDataFromColumn()
        {
            // Arrange
            var data = FakeHelper.Create<int>();
            _dataReader[_existingColumn].Returns(data.ToString());

            // Action
            var result = _safeDataReader.GetInt(_existingColumn);

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        [Test]
        public void GetInt_IsNotExistingColumn_ReturnDefaultValue()
        {
            // Action
            var result = _safeDataReader.GetInt(_nonExistingColumn);

            // Assert
            Assert.That(result, Is.Zero);
        }

        [Test]
        public void GetLong_IsExistingColumn_ReturnDataFromColumn()
        {
            // Arrange
            var data = FakeHelper.Create<long>();
            _dataReader[_existingColumn].Returns(data.ToString());

            // Action
            var result = _safeDataReader.GetLong(_existingColumn);

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        [Test]
        public void GetLong_IsNotExistingColumn_ReturnDefaultValue()
        {
            // Action
            var result = _safeDataReader.GetLong(_nonExistingColumn);

            // Assert
            Assert.That(result, Is.Zero);
        }

        [Test]
        public void GetNullableLong_IsExistingColumn_ReturnDataFromColumn()
        {
            // Arrange
            var data = FakeHelper.Create<long>();
            _dataReader[_existingColumn].Returns(data);

            // Action
            var result = _safeDataReader.GetNullableLong(_existingColumn);

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        [Test]
        public void GetNullableLong_IsNotExistingColumn_ReturnDefaultValue()
        {
            // Action
            var result = _safeDataReader.GetNullableLong(_nonExistingColumn);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetNullableLong_DataIsNull_ReturnNull()
        {
            // Arrange
            _dataReader[_existingColumn].Returns(DBNull.Value);

            // Action
            var result = _safeDataReader.GetNullableLong(_existingColumn);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetDateTime_IsExistingColumn_ReturnDataFromColumn()
        {
            // Arrange
            var data = FakeHelper.Create<DateTime>();
            var defaultData = FakeHelper.Create<DateTime>();
            _dataReader[_existingColumn].Returns(data);

            // Action
            var result = _safeDataReader.GetDateTime(_existingColumn, defaultData);

            // Assert
            Assert.That(result.ToString("MM-dd-yyyy hh:mm:ss"), Is.EqualTo(data.ToString("MM-dd-yyyy hh:mm:ss")));
        }

        [Test]
        public void GetDateTime_IsNotExistingColumn_ReturnDefaultValue()
        {
            // Arrange
            var defaultData = FakeHelper.Create<DateTime>();

            // Action
            var result = _safeDataReader.GetDateTime(_nonExistingColumn, defaultData);

            // Assert
            Assert.That(result, Is.EqualTo(defaultData));
        }

        [Test]
        public void GetByte_IsExistingColumn_ReturnDataFromColumn()
        {
            // Arrange
            var data = FakeHelper.Create<byte>();
            _dataReader[_existingColumn].Returns(data.ToString());

            // Action
            var result = _safeDataReader.GetByte(_existingColumn);

            // Assert
            Assert.That(result, Is.EqualTo(data));
        }

        [Test]
        public void GetByte_IsNotExistingColumn_ReturnDefaultValue()
        {
            // Action
            var result = _safeDataReader.GetByte(_nonExistingColumn);

            // Assert
            Assert.That(result, Is.Zero);
        }
    }
}