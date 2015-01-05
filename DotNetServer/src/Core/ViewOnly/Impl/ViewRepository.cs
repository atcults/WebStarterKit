using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Service.Impl;
using Core.ViewOnly.Attribute;
using Core.ViewOnly.Base;
using Dto.ApiRequests;

namespace Core.ViewOnly.Impl
{
    public class ViewRepository<TViewModel> : IViewRepository<TViewModel> where TViewModel : IView
    {
        public TViewModel GetById(Guid id)
        {
            return GetAllFor("Id", id).SingleOrDefault();
        }

        public TViewModel GetByKey(string keyProperty, object keyValue)
        {
            return GetAllFor(keyProperty, keyValue).SingleOrDefault();
        }

        public IEnumerable<TViewModel> FetchAll()
        {
            return GetDatabse().Fetch<TViewModel>(All());
        }

        public IEnumerable<TViewModel> Fetch(Sql sql)
        {
            return GetDatabse().Fetch<TViewModel>(sql);
        }

        public Page<TViewModel> SearchBySpecification(SimpleSearch specification)
        {
            var query = All();
            specification.BuildQuery(query);
            if (specification.Page == 0) specification.Page = 1;
            if (specification.PageSize == 0) specification.PageSize = 50;
            return GetDatabse().Page<TViewModel>(specification.Page, specification.PageSize, query.SqlFinal, query.Arguments);
        }

        public TViewModel[] GetAllFor(string keyName, object keyVal)
        {
            return Fetch(All().Where(keyName + " = @0 ", keyVal)).ToArray();
        }

        private static Database GetDatabse()
        {
            return new Database(ConfigProvider.GetDatabaseConfig().GetConnectionString(), "System.Data.SqlClient");
        }

        public Page<TViewModel> Page(long page, long itemsPerPage, Sql sql)
        {
            return GetDatabse().Page<TViewModel>(page, itemsPerPage, sql.SqlFinal, sql.Arguments);
        }

        protected Sql All()
        {
            var sql = Sql.Builder.Select("*").From(GetViewName());
            return sql;
        }

        protected Sql Top(int rows = 50)
        {
            var sql = Sql.Builder.Select(string.Format("top {0} *", rows)).From(GetViewName());
            return sql;
        }

        private static string GetViewName()
        {
            var viewModelType = typeof (TViewModel);
            var attrs = viewModelType.GetCustomAttributes(typeof (ViewNameAttribute), true);
            if (attrs.Length == 0) return viewModelType.Name;
            var tableNameAttr = attrs[0] as ViewNameAttribute;
            if (tableNameAttr == null)
                throw new CustomAttributeFormatException("Missing PetaPoco's ViewNameAttribute in " + viewModelType.Name);
            return tableNameAttr.Value;
        }
    }
}