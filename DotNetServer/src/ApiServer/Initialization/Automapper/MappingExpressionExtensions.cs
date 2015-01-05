using AutoMapper;

/*
 * CreateMap<Source, Destination>()
            .IgnoreAllUnmapped()
            .ForMember(d => d.Text, o => o.MapFrom(s => s.Name))
            .ForMember(d => d.Value, o => o.MapFrom(s => s.Id));
 */

namespace WebApp.Initialization.Automapper
{
    public static class MappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDest> IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}