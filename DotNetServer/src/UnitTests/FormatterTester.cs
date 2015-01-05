using Common.Helpers;
using NUnit.Framework;

namespace UnitTests
{
    public class FormatterTester : TestBase
    {
        [Test]
        public void CheckMobile()
        {
            Assert.AreEqual(Formatter.MobileNumber("9723012600"), true);
            Assert.AreEqual(Formatter.MobileNumber("09723012600"), false);
            Assert.AreEqual(Formatter.MobileNumber("+919723012600"), false);
            Assert.AreEqual(Formatter.MobileNumber("97230126000"), false);
        }
    }
}