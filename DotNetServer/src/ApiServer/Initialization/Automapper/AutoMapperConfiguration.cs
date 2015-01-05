using System;
using AutoMapper;

namespace WebApp.Initialization.Automapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure(Func<Type, object> servicesConstructor = null)
        {
            Mapper.Initialize(x =>
                                  {
                                      if (servicesConstructor != null) 
                                          x.ConstructServicesUsing(servicesConstructor);

                                      x.AddProfile<EntityToResponseProfile>();
                                      x.AddProfile<FormToCommandProfile>();
                                      x.AddProfile<ViewToResponseProfile>();
                                  });
        } 
    }
}