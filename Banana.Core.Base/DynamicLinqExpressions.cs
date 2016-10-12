using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Core.Base
{
    public static class DynamicLinqExpressions
    {
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            ParameterExpression expression;
            ParameterReplacer replacer = new ParameterReplacer(expression = Expression.Parameter(typeof(T), "candidate"));
            Expression left = replacer.Replace(one.Body);
            Expression right = replacer.Replace(another.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.And(left, right), new ParameterExpression[] { expression });
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> one, Expression<Func<T, bool>> another)
        {
            ParameterExpression expression;
            ParameterReplacer replacer = new ParameterReplacer(expression = Expression.Parameter(typeof(T), "candidate"));
            Expression left = replacer.Replace(one.Body);
            Expression right = replacer.Replace(another.Body);
            return Expression.Lambda<Func<T, bool>>(Expression.Or(left, right), new ParameterExpression[] { expression });
        }
    }
}
