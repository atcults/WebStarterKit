using AutoMapper;
using NUnit.Framework;
using WebApp.Initialization.Automapper;

namespace UnitTests
{
    public class AutoMapperConfigurationTester : TestBase
    {
        [Test]
        public void EnsureAutomapperConfigIsValid()
        {
            AutoMapperConfiguration.Configure();
            Mapper.AssertConfigurationIsValid();
        }
    }
}