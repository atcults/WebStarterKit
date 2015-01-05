using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Common.Helpers
{
    public static class NameHelper
    {
        public static string BuildIdFrom<TModel>(Expression<Func<TModel, object>> expression)
        {
            return BuildIdFrom((Expression)expression);
        }

        public static string BuildNameFrom<TModel>(Expression<Func<TModel, object>> expression)
        {
            var expressionText = BuildNameFrom((Expression)expression);
            return expressionText;
        }

        public static string BuildNameFrom(Expression expression)
        {
            var expressionToCheck = expression;
            var tokens = new List<string>();

            var done = false;
            var accessedMember = false;

            while (!done)
            {
                if (expressionToCheck != null)
                    switch (expressionToCheck.NodeType)
                    {
                        case ExpressionType.Convert:

                            accessedMember = false;
                            expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;

                            break;
                        case ExpressionType.ArrayIndex:
                            var binaryExpression = (BinaryExpression)expressionToCheck;

                            var indexExpression = binaryExpression.Right;
                            var indexAction = Expression.Lambda(indexExpression).Compile();
                            var value = (int)indexAction.DynamicInvoke();

                            if (accessedMember)
                            {
                                tokens.Add(".");
                            }

                            tokens.Add(string.Format("[{0}]", value));

                            accessedMember = false;
                            expressionToCheck = binaryExpression.Left;

                            break;
                        case ExpressionType.Lambda:
                            var lambdaExpression = (LambdaExpression)expressionToCheck;
                            accessedMember = false;
                            expressionToCheck = lambdaExpression.Body;
                            break;
                        case ExpressionType.MemberAccess:
                            var memberExpression = (MemberExpression)expressionToCheck;

                            if (accessedMember)
                            {
                                tokens.Add(".");
                            }

                            tokens.Add(memberExpression.Member.Name);

                            if (memberExpression.Expression == null)
                            {
                                done = true;
                            }
                            else
                            {
                                accessedMember = true;
                                expressionToCheck = memberExpression.Expression;
                            }
                            break;
                        case ExpressionType.Call:
                            var callExpression = (MethodCallExpression)expressionToCheck;

                            var getItemArgumentExpression = callExpression.Arguments[0];
                            var itemAction = Expression.Lambda(getItemArgumentExpression).Compile();
                            var itemValue = (int)itemAction.DynamicInvoke();

                            if (accessedMember)
                            {
                                tokens.Add(".");
                            }

                            tokens.Add(string.Format("[{0}]", itemValue));

                            accessedMember = false;
                            expressionToCheck = callExpression.Object;

                            break;
                        default:
                            done = true;
                            break;
                    }
            }

            tokens.Reverse();

            var result = string.Join(string.Empty, tokens.ToArray());

            return result;
        }

        public static string BuildIdFrom(Expression expression)
        {
            var expressionToCheck = expression;
            var tokens = new List<string>();

            var done = false;
            var accessedMember = false;

            while (!done)
            {
                switch (expressionToCheck.NodeType)
                {
                    case ExpressionType.Convert:

                        accessedMember = false;
                        expressionToCheck = ((UnaryExpression)expressionToCheck).Operand;

                        break;
                    case ExpressionType.ArrayIndex:
                        var binaryExpression = (BinaryExpression)expressionToCheck;

                        var indexExpression = binaryExpression.Right;
                        var indexAction = Expression.Lambda(indexExpression).Compile();
                        var value = (int)indexAction.DynamicInvoke();

                        if (accessedMember)
                        {
                            tokens.Add("_");
                        }

                        tokens.Add(string.Format("_{0}_", value));

                        accessedMember = false;
                        expressionToCheck = binaryExpression.Left;

                        break;
                    case ExpressionType.Lambda:
                        var lambdaExpression = (LambdaExpression)expressionToCheck;
                        accessedMember = false;
                        expressionToCheck = lambdaExpression.Body;
                        break;
                    case ExpressionType.MemberAccess:
                        var memberExpression = (MemberExpression)expressionToCheck;

                        if (accessedMember)
                        {
                            tokens.Add("_");
                        }

                        tokens.Add(memberExpression.Member.Name);

                        if (memberExpression.Expression == null)
                        {
                            done = true;
                        }
                        else
                        {
                            accessedMember = true;
                            expressionToCheck = memberExpression.Expression;
                        }
                        break;
                    default:
                        done = true;
                        break;
                }
            }

            tokens.Reverse();

            var result = string.Join(string.Empty, tokens.ToArray());

            return result;
        }
    }
}