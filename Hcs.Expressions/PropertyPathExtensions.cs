using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Linq.Expressions
{
    public static class PropertyPathExtensions
    {
        public static IReadOnlyList<MemberInfo> GetPropertyVistPath(this LambdaExpression expression)
        {
            var visitor = new PropertyVisitor();
            visitor.Visit(expression.Body);
            visitor.Path.Reverse();
            return visitor.Path;
        }

        private class PropertyVisitor : ExpressionVisitor
        {
            internal readonly List<MemberInfo> Path = new List<MemberInfo>();

            protected override Expression VisitMember(MemberExpression node)
            {
                if (!(node.Member is PropertyInfo))
                {
                    throw new ArgumentException("The path can only contain properties", nameof(node));
                }

                this.Path.Add(node.Member);
                return base.VisitMember(node);
            }
        }
    }
}
