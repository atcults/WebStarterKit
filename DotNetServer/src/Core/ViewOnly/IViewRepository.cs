using System;
using System.Collections.Generic;
using Core.ViewOnly.Base;
using Dto.ApiRequests;

namespace Core.ViewOnly
{
    public interface IViewRepository<TViewModel>
    {
        //Prevent passing all data.
        IEnumerable<TViewModel> FetchAll();
        IEnumerable<TViewModel> Fetch(Sql sql);
        TViewModel GetById(Guid id);
        TViewModel GetByKey(string keyProperty, object keyValue);
        TViewModel[] GetAllFor(string keyProperty, object keyValue);
        Page<TViewModel> SearchBySpecification(SimpleSearch specification);
    }
}