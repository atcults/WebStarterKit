using System;
using System.Collections.Generic;
using System.Dynamic;
using Core.ViewOnly;
using Core.Views;
using NUnit.Framework;
using SmartFormat;
using WebApp.Initialization;

namespace IntegrationTests.ViewModel
{
    public class SmartFormatterTester : IntegrationTestBase
    {
        [Test]
        public void TryMappingWithSmartFormatter()
        {
            var member = GetPersistedSiteUser();

            var staticData = new Dictionary<string, object>
            {
                {"TokenHash", "123"},
                {"ContactId", member.Id}
            };

            var viewData = new Dictionary<string, string>
            {
                {"ContactView", "Id:ContactId"}
            };

            dynamic data = new ExpandoObject();

            var expando = (IDictionary<string, object>) data;
            expando["User"] = GetInstance<IViewRepository<AppUserView>>().GetById(member.Id);

            foreach (var k in staticData.Keys)
            {
                expando[k] = staticData[k];
            }

            foreach (var k in viewData.Keys)
            {
                var viewType = "Core.Views." + k + ", Core";

                var openType = typeof(IViewRepository<>); //generic open type

                var type = openType.MakeGenericType(Type.GetType(viewType));
                dynamic viewRepository = ClientEndPoint.Container.GetInstance(type); //should get your ConcreteABuilder 

                var parts = viewData[k].Split(':');

                if(parts[1].Contains(".")) throw new NotImplementedException("To be implement");

                expando[k] = viewRepository.GetByKey(parts[0], staticData[parts[1]]);
            }
            
            var specificCulture = System.Globalization.CultureInfo.CreateSpecificCulture("en-us");
            var mailBody = Smart.Format(specificCulture, "Dear {User:{Name}}, {ContactView:{Mobile}}, Your password token is {TokenHash}.", data);
            var anotherBody = Smart.Format(specificCulture, "Dear {User.Name}, {ContactView.Mobile}, Your password token is {TokenHash}.", data);
            Assert.AreEqual(mailBody, "Dear Name, 9898989898, Your password token is 123.");
        }
    }
}
