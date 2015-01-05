using System;
using AutoMapper;

namespace NSBus.Server.Initialization.Automapper
{
    public class AutoMapperConfiguration
    {
        public static void Configure(Func<Type, object> servicesConstructor = null)
        {
            Mapper.Initialize(x =>
                                  {
                                      if (servicesConstructor != null) 
                                          x.ConstructServicesUsing(servicesConstructor);
                                  });
        } 
    }
}