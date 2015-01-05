using System;
using Common;
using Common.Helpers;
using NUnit.Framework;
using Rhino.Mocks;
using StructureMap;

namespace UnitTests
{
    [TestFixture]
    public class TestBase
    {
        private static readonly IContainer Container = new Container();

        [SetUp]
        public virtual void SetUp()
        {
            RunOnce(() => Container.Configure(x => x.Scan(y =>
            {
                y.AssemblyContainingType<CommonRegistry>();
                y.LookForRegistries();
            })));

            RunAlways(ResetSystemTime);
        }


        [TearDown]
        public virtual void TearDown()
        {
        }

        protected readonly DateTime StartDay = SystemTime.Now().Date.AddMonths(-1);
        protected readonly DateTime EndDay = SystemTime.Now().Date.AddMonths(2);
        protected readonly DateTime Today = SystemTime.Now().Date;

        private readonly object _lock = new object();
        private bool _alreadyRun;

        private void RunOnce(Action action)
        {
            if (_alreadyRun) return;
            lock (_lock)
            {
                if (_alreadyRun) return;
                action();
                _alreadyRun = true;
            }
        }

        private static void RunAlways(Action action)
        {
            action();
        }

        protected void StubDateTime(DateTime now)
        {
            SystemTime.Now = () => now;
        }

        private static void ResetSystemTime()
        {
            SystemTime.Now = () => DateTime.Now;
        }

        protected static T S<T>(params object[] argumentsForConstructor) where T : class
        {
            return MockRepository.GenerateStub<T>(argumentsForConstructor);
        }
    }
}