using NUnit.Framework;

namespace Template.Tests
{
    [TestFixture]
    public class GetAutoTestWorkingTests
    {
        [Test]
        public void SomePassingTest()
        {
            Assert.AreEqual(5, 5);
        }

        [Test]
        public void SomeFailingTest()
        {
            Assert.Greater(5, 7);
        }
    }
}
