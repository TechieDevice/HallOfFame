using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using HallOfFame_backend.DataBase;

namespace HallOfFame_Tests
{
    public class ServiceTestBase
    {
        protected ApplicationContext _applicationContext;

        [SetUp]
        public virtual void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>()
                .UseInMemoryDatabase("HallOfFameDB")
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            _applicationContext = new ApplicationContext(options);
        }

        [TearDown]
        public virtual void TearDown()
        {
            _applicationContext?.Database?.EnsureDeleted();
        }
    }
}