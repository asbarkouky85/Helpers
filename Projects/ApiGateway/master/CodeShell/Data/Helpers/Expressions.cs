using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CodeShell.Data.Helpers
{
    public static class Expressions
    {

        public static Expression<Func<T, object>> Property<T>(string property) where T : class
        {
            var par = Expression.Parameter(typeof(T));
            var prop = Expression.Property(par, property);

            return Expression.Lambda<Func<T, object>>(prop, par);
        }

        public static Expression<Func<T, string>> PropertyString<T>(string property) where T : class
        {
            var par = Expression.Parameter(typeof(T));
            var prop = Expression.Property(par, property);
            var propcon = Expression.Convert(prop, typeof(string));

            return Expression.Lambda<Func<T, string>>(propcon, par);
        }

        public static Expression<Func<T, long>> PropertyInt<T>(string property) where T : class
        {
            var par = Expression.Parameter(typeof(T));
            var prop = Expression.Property(par, property);
            var propcon = Expression.Convert(prop, typeof(long));

            return Expression.Lambda<Func<T, long>>(propcon, par);
        }

        public static Expression<Func<T, DateTime>> PropertyDate<T>(string property) where T : class
        {
            var par = Expression.Parameter(typeof(T));
            var prop = Expression.Property(par, property);
            var propcon = Expression.Convert(prop, typeof(DateTime));

            return Expression.Lambda<Func<T, DateTime>>(propcon, par);
        }

        public static Expression<Func<T, decimal>> PropertyDecimal<T>(string property) where T : class
        {
            var par = Expression.Parameter(typeof(T));
            var prop = Expression.Property(par, property);
            var propcon = Expression.Convert(prop, typeof(decimal));

            return Expression.Lambda<Func<T, decimal>>(propcon, par);
        }

        public static Expression<Func<T, double>> PropertyDouble<T>(string property) where T : class
        {
            var par = Expression.Parameter(typeof(T));
            var prop = Expression.Property(par, property);
            var propcon = Expression.Convert(prop, typeof(double));

            return Expression.Lambda<Func<T, double>>(propcon, par);
        }

        public static Expression<Func<T, bool>> StringContains<T>(string property, string value) where T : IModel
        {
            return StringMethod<T>("Contains", property, value);
        }

        public static Expression<Func<T, bool>> StringMethod<T>(string methodName, string property, string value) where T : IModel
        {
            var method = typeof(string).GetMethod(methodName, new[] { typeof(string) });
            if (method != null)
            {
                ParameterExpression paramExpr = Expression.Parameter(typeof(T));
                MemberExpression memExpr = Expression.Property(paramExpr, property);
                ConstantExpression cExpr = Expression.Constant(value, typeof(string));
                MethodCallExpression mExpr = Expression.Call(memExpr, method, cExpr);

                return Expression.Lambda<Func<T, bool>>(mExpr, paramExpr);
            }
            return x => true;
        }

        public static Expression<Func<T, bool>> Unique<T>(T obj, string property) where T : class, IModel
        {
            long oid = obj.Id;

            PropertyInfo pi = typeof(T).GetProperty(property);

            ParameterExpression paramExpr = Expression.Parameter(typeof(T));

            MemberExpression uniquePropEx = Expression.Property(paramExpr, property);
            ConstantExpression valueEx = Expression.Constant(pi.GetValue(obj, null), pi.PropertyType);

            MemberExpression memExpr2 = Expression.Property(paramExpr, "OID");
            ConstantExpression cExpr2 = Expression.Constant(oid, typeof(long));

            BinaryExpression ex = Expression.MakeBinary(ExpressionType.Equal, uniquePropEx, valueEx);
            BinaryExpression ex2 = Expression.MakeBinary(ExpressionType.NotEqual, memExpr2, cExpr2);

            BinaryExpression ex3 = Expression.MakeBinary(ExpressionType.And, ex, ex2);

            return Expression.Lambda<Func<T, bool>>(ex3, paramExpr);
        }
    }
}
