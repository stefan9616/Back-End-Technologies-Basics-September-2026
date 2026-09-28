using ArrayTools;
using FluentAssertions;

namespace ArrayToolsTest
{
    [TestFixture]
    public class ArraySearchTests
    {
        [Test]
        public void CheckArrayFindElement()
        {
            int[] numbers = { 1, 3, 5, 2, 4 };
            int valueToFind = 2;
            int expected = 3;

            int result = ArraySearch.FindIndex(numbers, valueToFind);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void CheckArrayNotFindElement()
        {
            int[] numbers = { 1, 3, 5, 2, 4 };
            int valueToFind = 20;
            int expected = -1;

            int result = ArraySearch.FindIndex(numbers, valueToFind);
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}