using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Helpers
{
    public static class Property
    {
        public static string Of<T>(Expression<Func<T, object>> expr)
        {
            return FindProperty(expr).Name;
        }

        public static string Of(LambdaExpression lambdaExpression)
        {
            return FindProperty(lambdaExpression).Name;
        }

        private static PropertyInfo FindProperty(LambdaExpression lambdaExpression)
        {
            Expression expressionToCheck = lambdaExpression;

            var done = false;

            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;
                        break;
                    case ExpressionType.Lambda:
                        expressionToCheck = lambdaExpression.Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var propertyInfo = ((MemberExpression)expressionToCheck).Member as PropertyInfo;
                        return propertyInfo;
                    default:
                        done = true;
                        break;
                }
            }

            return null;
        }

    }
}