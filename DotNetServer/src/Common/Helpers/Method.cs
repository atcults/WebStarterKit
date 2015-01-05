using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Helpers
{
    public class Method
    {
        public static string Of<T>(Expression<Func<T, object>> expr)
        {
            return FindMethod(expr).Name;
        }

        public static string Of(LambdaExpression lambdaExpression)
        {
            return FindMethod(lambdaExpression).Name;
        }

        private static MethodInfo FindMethod(LambdaExpression lambdaExpression)
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
                    case ExpressionType.Call:
                        return ((MethodCallExpression)expressionToCheck).Method;
                    default:
                        done = true;
                        break;
                }
            }
            return null;
        }
    }
}