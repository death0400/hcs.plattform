using System.Linq.Expressions;

namespace System.Linq.Expressions
{
    public static class ReplaceParameterExtensions
    {
        class ParameterReplacer : ExpressionVisitor
        {
            public ParameterExpression Source;
            public System.Linq.Expressions.Expression Target;
            public ParameterReplacer(ParameterExpression source, System.Linq.Expressions.Expression target)
            {
                this.Source = source;
                this.Target = target;
            }
            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == Source ? Target : base.VisitParameter(node);
            }
        }
        public static T ReplaceParameter<T>(this T expression, ParameterExpression source, System.Linq.Expressions.Expression target) where T : System.Linq.Expressions.Expression
        {
            return (T)new ParameterReplacer(source, target).Visit(expression);
        }
    }
}