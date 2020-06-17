using CodeShell.Data.Helpers;
using CodeShell.Text;
using CodeShell.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Data
{
    public class ExpressionGenerator<T> : IExpressionGenerator where T : class
    {
        protected ParameterExpression ParamExpression { get { return Expression.Parameter(typeof(T)); } }

        public Expression GetRangeFilter(string propertyName, int start, int end)
        {
            Expression mem = MemberExpression.Property(ParamExpression, propertyName);
            BinaryExpression greaterExp = null;
            BinaryExpression smaller = null;

            if (!start.Equals(0))
            {
                ConstantExpression sExp = Expression.Constant(start);
                greaterExp = Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, mem, sExp);
            }

            if (!end.Equals(0))
            {
                ConstantExpression eExp = Expression.Constant(end);
                smaller = Expression.MakeBinary(ExpressionType.LessThanOrEqual, mem, eExp);
            }

            return Combine(greaterExp, smaller);
        }

        public Expression GetRangeFilter(string propertyName, DateTime start, DateTime end)
        {

            Expression mem = MemberExpression.Property(ParamExpression, propertyName);

            BinaryExpression greaterExp = null;
            BinaryExpression smaller = null;

            UnaryExpression uExp = Expression.Convert(mem, typeof(DateTime));

            if (start != DateTime.MinValue)
            {
                ConstantExpression sExp = Expression.Constant(start, typeof(DateTime));
                greaterExp = Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, uExp, sExp);
            }

            if (end != DateTime.MaxValue)
            {
                ConstantExpression eExp = Expression.Constant(end, typeof(DateTime));
                smaller = Expression.MakeBinary(ExpressionType.LessThanOrEqual, uExp, eExp);
            }

            return Combine(greaterExp, smaller);
        }

        public Expression GetRangeFilter(string propertyName, decimal start, decimal end)
        {
            Expression mem = MemberExpression.Property(ParamExpression, propertyName);
            BinaryExpression greaterExp = null;
            BinaryExpression smaller = null;

            if (!start.Equals(0))
            {
                ConstantExpression sExp = Expression.Constant(start);
                greaterExp = Expression.MakeBinary(ExpressionType.GreaterThanOrEqual, mem, sExp);
            }

            if (!end.Equals(0))
            {
                ConstantExpression eExp = Expression.Constant(end);
                smaller = Expression.MakeBinary(ExpressionType.LessThanOrEqual, mem, eExp);
            }

            return Combine(greaterExp, smaller);
        }

        Expression Combine(BinaryExpression greaterExp, BinaryExpression smaller)
        {
            if (greaterExp == null && smaller != null)
            {
                return Expression.Lambda<Func<T, bool>>(smaller, ParamExpression);
            }
            else if (greaterExp != null && smaller == null)
            {
                return Expression.Lambda<Func<T, bool>>(greaterExp, ParamExpression);
            }
            else if (greaterExp != null && smaller != null)
            {
                BinaryExpression combinedExpression = Expression.MakeBinary(ExpressionType.And, greaterExp, smaller);
                return Expression.Lambda<Func<T, bool>>(combinedExpression, ParamExpression);
            }
            else
                return null;
        }

        public Expression GetStringContainsFilter(string propertyName, string str)
        {
            ParameterExpression pExp = Expression.Parameter(typeof(T));
            MemberExpression mExp = MemberExpression.Property(pExp, propertyName);

            MethodInfo inf = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            ConstantExpression cExp = Expression.Constant(str, typeof(string));
            MethodCallExpression mcExp = Expression.Call(mExp, inf, cExp);

            return Expression.Lambda<Func<T, bool>>(mcExp, pExp);
        }
        
        public Expression GetReferenceContainedFilter(string propertyName, IEnumerable<long> ids)
        {
            Expression mem = MemberExpression.Property(ParamExpression, propertyName);
            var method = typeof(Enumerable).GetMethod("Contains", new[] { typeof(int) });

            ConstantExpression cExpr = Expression.Constant(ids, typeof(IEnumerable<long>));
            MethodCallExpression mExpr = Expression.Call(typeof(Enumerable), "Contains", new[] { mem.Type }, cExpr, mem);

            return Expression.Lambda<Func<T, bool>>(mExpr, ParamExpression);
        }
        /// <summary>
        /// creates the sorting expression with the property name and applies it to the queryable
        /// </summary>
        /// <param name="q"></param>
        /// <param name="propertyName"></param>
        /// <param name="dir"></param>
        /// <returns></returns>
        public IQueryable<T> SortWith(IQueryable<T> q, string propertyName, SortDir dir)
        {
            PropertyInfo prop = typeof(T).GetProperty(propertyName);

            if (prop == null)
                throw new Exception("no such property " + propertyName + " in class " + typeof(T).Name);

            if (prop.PropertyType == typeof(string))
            {
                var exp = Expressions.PropertyString<T>(propertyName);
                if (dir == SortDir.ASC) q = q.OrderBy(exp); else q = q.OrderByDescending(exp);
            }
            else if (prop.PropertyType == typeof(double))
            {
                var exp = Expressions.PropertyDouble<T>(propertyName);
                if (dir == SortDir.ASC) q = q.OrderBy(exp); else q = q.OrderByDescending(exp);
            }
            else if (prop.PropertyType.IsDecimalType())
            {
                var exp = Expressions.PropertyDecimal<T>(propertyName);
                if (dir == SortDir.ASC) q = q.OrderBy(exp); else q = q.OrderByDescending(exp);
            }
            else if (prop.PropertyType.IsIntgerType())
            {
                var exp = Expressions.PropertyInt<T>(propertyName);
                if (dir == SortDir.ASC) q = q.OrderBy(exp); else q = q.OrderByDescending(exp);
            }
            else if (prop.PropertyType == typeof(DateTime))
            {
                var exp = Expressions.PropertyDate<T>(propertyName);
                if (dir == SortDir.ASC) q = q.OrderBy(exp); else q = q.OrderByDescending(exp);
            }
            else if (prop.PropertyType == typeof(Nullable<DateTime>))
            {
               
                    var exp = Expressions.PropertyDate<T>(propertyName);
                    if (dir == SortDir.ASC) q = q.OrderBy(exp); else q = q.OrderByDescending(exp);
               
            }
            else
            {
                var exp = Expressions.Property<T>(propertyName);
                if (dir == SortDir.ASC) q = q.OrderBy(exp); else q = q.OrderByDescending(exp);
            }
            return q;
        }
        
        public List<Expression> ToFilterExpressions(IEnumerable<PropertyFilter> fs)
        {
            List<Expression> exs = new List<Expression>();
            foreach (PropertyFilter f in fs)
            {
                switch (f.FilterType)
                {
                    case "string":
                        exs.Add(GetStringContainsFilter(f.MemberName, f.Value1));
                        break;
                    case "decimal":
                        exs.Add(GetRangeFilter(f.MemberName, decimal.Parse(f.Value1), decimal.Parse(f.Value2)));
                        break;
                    case "int":
                        exs.Add(GetRangeFilter(f.MemberName, int.Parse(f.Value1), int.Parse(f.Value2)));
                        break;
                    case "date":
                        exs.Add(GetRangeFilter(f.MemberName, DateTime.Parse(f.Value1), DateTime.Parse(f.Value2)));
                        break;
                    case "reference":
                        exs.Add(GetReferenceContainedFilter(f.MemberName, f.Ids));
                        break;
                }
            }

            return exs;
        }

        /// <summary>
        /// Converts LoadOptionsPost to LoadOptions of T
        /// </summary>
        /// <param name="posted"></param>
        /// <returns></returns>
        public GetOptions<T> ToModelGetOptions(LoadOpts posted)
        {
            GetOptions<T> opts = new GetOptions<T>();
            opts.Skip = posted.Skip;
            opts.Showing = posted.Showing;
            opts.SearchTerm = posted.SearchTerm;
            SortDir dir;
            if (Enum.TryParse(posted.Direction, out dir))
                opts.Direction = dir;

            if (string.IsNullOrEmpty(posted.OrderProperty) && opts.Showing != 0)
                throw new Exception("OrderProperty must be set if Showing is not 0");
            opts.OrderProperty = posted.OrderProperty;

            if (posted.Filters != null)
            {
                opts.Filters = ToFilterExpressions(posted.Filters);
            }
            return opts;
        }
        
    }
}
