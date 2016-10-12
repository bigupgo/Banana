using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Banana.Core.Base
{
    public class ParameterReplacer : ExpressionVisitor
    {
        public ParameterReplacer(System.Linq.Expressions.ParameterExpression paramExpr)
        {
            this.ParameterExpression = paramExpr;
        }

        public Expression Replace(Expression expr)
        {
            return this.Visit(expr);
        }

        protected override Expression VisitParameter(System.Linq.Expressions.ParameterExpression p)
        {
            return this.ParameterExpression;
        }

        public System.Linq.Expressions.ParameterExpression ParameterExpression { get; private set; }
    }
}
