using System;
using AutoMapper;
using Common.Helpers;
using Core.ViewOnly;
using Core.Views;

namespace WebApp.Initialization.Automapper.Resolvers
{
    public class AppUserCodeToIdResolver : ValueResolver<string, Guid?>
    {
        private readonly IViewRepository<AppUserView> _viewRepository;

        public AppUserCodeToIdResolver(IViewRepository<AppUserView> viewRepository)
        {
            _viewRepository = viewRepository;
        }

        protected override Guid? ResolveCore(string source)
        {
            var view = Formatter.EmailId(source)
                ? _viewRepository.GetByKey(Property.Of<AppUserView>(x => x.Email), source)
                : _viewRepository.GetByKey(Property.Of<AppUserView>(x => x.Mobile), source);
            return view == null ? (Guid?) null : view.Id;
        }
    }
}