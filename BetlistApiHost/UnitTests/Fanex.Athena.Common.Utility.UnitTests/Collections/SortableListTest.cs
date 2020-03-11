namespace Fanex.Athena.Common.Utility.UnitTests.Collections
{
    using System.Collections.Generic;
    using ClassTest;
    using NUnit.Framework;
    using Utility.Collections;

    [TestFixture]
    public class SortableListTest
    {
        [Test]
        public void Sort_AscendingDirection_ListSortedCorrectly()
        {
            // Arrange
            var firstObject = new Person { Id = 1, Name = "Walter" };
            var secondObject = new Person { Id = 2, Name = "Ralph" };
            var dataList = new SortableList<Person> { firstObject, secondObject };

            // Action
            dataList.Sort("Name", true);

            // Assert
            Assert.That(firstObject, Is.EqualTo(dataList[1]));
            Assert.That(secondObject, Is.EqualTo(dataList[0]));
        }

        [Test]
        public void Sort_DescendingDirection_ListSortedCorrectly()
        {
            // Arrange
            var firstObject = new Person { Id = 1, Name = "Walter" };
            var secondObject = new Person { Id = 2, Name = "Ralph" };
            var dataList = new SortableList<Person> { firstObject, secondObject };

            // Action
            dataList.Sort("Name", false);

            // Assert
            Assert.That(firstObject, Is.EqualTo(dataList[0]));
            Assert.That(secondObject, Is.EqualTo(dataList[1]));
        }
    }
}