using System;
using System.Linq.Expressions;
using Common.Base;

namespace Common.Extensions
{
    public static class ExpressionHelper
    {
        public static object Evaluate(LambdaExpression expression, object target)
        {
            if (target == null) return null;

            var func = expression.Compile();

            object result = null;
            
            try
            {
                result = func.DynamicInvoke(target);
            }
            catch (Exception exception)
            {
                Logger.Log(LogType.Error, typeof(ExpressionHelper), "Expression Helper", exception);
            }

            return result;
        }
    }
}