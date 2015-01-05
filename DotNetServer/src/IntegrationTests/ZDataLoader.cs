using NUnit.Framework;

namespace IntegrationTests
{
    /// <summary>
    ///     This class inserts all initial records to run applicatin so we can have some data while running in test
    ///     environment.
    ///     When you run Click To Build; this is the last test that run so that all predefined data will be set in database.
    /// </summary>
    public class ZDataLoader : IntegrationTestBase
    {
        /// <summary>
        ///     The Category Dataloader makes application understand that this is the last test to run.
        /// </summary>
        [Test, Explicit, Category("DataLoader")]
        public void LoadData()
        {
            var adminUser = GetPersistedSiteUser();
            Persist(adminUser);
        }
    }
}