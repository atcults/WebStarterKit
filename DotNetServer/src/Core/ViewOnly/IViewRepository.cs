using System;
using System.Collections.Generic;
using Core.ViewOnly.Base;
using Dto.ApiRequests;

namespace Core.ViewOnly
{
    public interface IViewRepository<T> where T : IView
    {
        //Prevent passing all data.
        IEnumerable<T> FetchAll();
        IEnumerable<T> Fetch(Sql sql);
        T GetById(Guid id);
        T GetByKey(string keyProperty, object keyValue);
        T[] GetAllFor(string keyProperty, object keyValue);
        Page<T> SearchBySpecification(SimpleSearch specification);
    }
}