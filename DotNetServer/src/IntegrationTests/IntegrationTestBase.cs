using System;
using System.Data;
using System.Reflection;
using Common.Enumerations;
using Common.Helpers;
using Common.Service.Impl;
using Core.Commands;
using Core.Domain.Model;
using Core.ReadWrite;
using Core.ReadWrite.Attribute;
using Core.ReadWrite.Base;
using Dto.ApiResponses;
using IntegrationTests.DataAccess;
using NUnit.Framework;
using WebApp.Initialization;

namespace IntegrationTests
{
    /// <summary>
    ///     This is the base class for Integration
    /// </summary>
    [TestFixture]
    public abstract class IntegrationTestBase
    {
        [SetUp]
        public virtual void Setup()
        {
            RunOnce(() =>
            {
                ClientEndPoint.Initialize();
                _unitOfWork = ClientEndPoint.Container.GetInstance<IUnitOfWork>();
                _sqlExtension = new SqlExtension(ConfigProvider.GetDatabaseConfig().GetConnectionString());
            });

            RunAlways(() =>
            {
                ResetSystemTime();
                _unitOfWork.Begin();
                _sqlExtension.ResetDatabase();
            });
        }

        [TearDown]
        public virtual void Teardown()
        {
            if (_unitOfWork == null) return;
            _unitOfWork.Commit();
        }

        protected readonly DateTime Today = SystemTime.Now().Date;

        //ASK MM for why we dont use static here.
        private readonly object _lock = new object();
        private bool _alreadyRun;

        protected bool SetUpForDataLoader;
        private SqlExtension _sqlExtension;
        private IUnitOfWork _unitOfWork;


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

        protected static void AssertObjectsMatch(object obj1, object obj2)
        {
            Assert.AreNotSame(obj1, obj2);

            var propertyInfos = obj1.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            foreach (var info in propertyInfos)
            {
                var value1 = info.GetValue(obj1, null);
                var value2 = info.GetValue(obj2, null);

                Assert.AreEqual(value1, value2, string.Format("Property {0} doesn't match", info.Name));
            }
        }

        protected T GetInstance<T>()
        {
            return ClientEndPoint.Container.GetInstance<T>();
        }

        protected void Persist(params Entity[] entities)
        {
            foreach (var entity in entities)
            {
                var spName = string.Format("usp_{0}Insert", GetTableName(entity.GetType()));
                _sqlExtension.ExecuteCore(spName, entity.To, command => command.ExecuteNonQuery());
            }
        }

        public T LoadById<T>(Guid id) where T : Entity, new()
        {
            var spName = string.Format("usp_{0}Select", GetTableName(typeof(T)));

            T entity = null;

            _sqlExtension.ExecuteCore(spName,
                command => command.Parameters.Add("@Id", SqlDbType.UniqueIdentifier).Value = id, command =>
                {
                    var dataReader = command.ExecuteReader(CommandBehavior.Default);
                    if (dataReader.Read())
                    {
                        entity = new T();
                        entity.From(dataReader);
                    }
                    dataReader.Close();
                });

            return entity;
        }

        private static string GetTableName(Type type)
        {
            var attrs = type.GetCustomAttributes(typeof(EntityNameAttribute), true);
            var tableNameAttr = attrs[0] as EntityNameAttribute;
            if (tableNameAttr == null)
                throw new CustomAttributeFormatException("Missing EntityNameAttribute in " + type.Name);
            return tableNameAttr.Value;
        }

        protected void ProcessCommand<TCommand>(TCommand command, Guid userId) where TCommand : ICommand
        {
            var validationResult = command.Validate();

            if (!validationResult.IsValid)
            {
                throw new Exception(validationResult.ToString());
            }

            IWebApiResponse response;

            var processor = ClientEndPoint.Container.GetInstance<ICommandProcessor<TCommand>>();
            processor.Process(command, userId, out response);

            try
            {
                _unitOfWork.Commit();
            }
            catch
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public AppUser GetPersistedSiteUser(string email = "temp@abc.com", string mobile = "9898989898")
        {
            var cryptographer = ClientEndPoint.Container.GetInstance<Cryptographer>();
            var salt = cryptographer.CreateSalt();

            var contact = new Contact
            {
                Id = GuidComb.New(),
                Name = "Name",
                Email = email,
                Mobile = mobile
            };

            var user = new AppUser
            {
                Id = contact.Id,
                PasswordHash = cryptographer.ComputeHash(mobile + salt),
                PasswordSalt = salt,
                UserStatus = UserStatus.Active
            };

            Persist(contact, user);
            return user;
        }
    }
}