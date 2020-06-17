using CodeShell.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace CodeShell.Data.Collections
{
    public static class CollectionExtensions
    {
        public static Expression<Func<T, bool>> Combine<T>(this Expression<Func<T, bool>> exp, Expression<Func<T, bool>> exp2)
        {
            ParameterExpression para = Expression.Parameter(typeof(T));
            BinaryExpression combinedExpression = Expression.MakeBinary(ExpressionType.And, exp, exp2);
            return Expression.Lambda<Func<T, bool>>(combinedExpression, para);
        }

        public static List<T> ToListWith<T>(this IQueryable<T> q, GetOptions<T> opts) where T : class
        {

            ExpressionGenerator<T> gen = new ExpressionGenerator<T>();
            //Expression<Func<T, bool>> fil = null;
            if (opts.Filters != null)
            {
                //fil = (Expression<Func<T, bool>>)opts.Filters[0];
                for (int i = 0; i < opts.Filters.Count; i++)
                {
                    Expression<Func<T, bool>> e = (Expression<Func<T, bool>>)opts.Filters[i];
                    q = q.Where(e);
                }
                
            }

            if (!string.IsNullOrEmpty(opts.OrderProperty))
                q = gen.SortWith(q, opts.OrderProperty, opts.Direction);

            if (opts.Showing > 0)
                q = q.Skip(opts.Skip).Take(opts.Showing);

            return q.ToList();

        }
        public static LoadResult LoadWith<T>(this IQueryable<T> q, GetOptions<T> opts) where T : class
        {
            LoadResult res = new LoadResult();
            
            ExpressionGenerator<T> gen = new ExpressionGenerator<T>();

            if (opts == null)
                opts = new GetOptions<T>();

            //Expression<Func<T, bool>> fil = null;
            if (opts.Filters != null)
            {
                //fil = (Expression<Func<T, bool>>)opts.Filters[0];
                for (int i = 0; i < opts.Filters.Count; i++)
                {
                    Expression<Func<T, bool>> e = (Expression<Func<T, bool>>)opts.Filters[i];
                    //fil = fil.Combine(e);
                    q = q.Where(e);
                }
                
                
            }
            
                res.TotalCount = q.Count(d => true);
            


            if (!string.IsNullOrEmpty(opts.OrderProperty))
                q = gen.SortWith(q, opts.OrderProperty, opts.Direction);

            if (opts.Showing > 0)
                q = q.Skip(opts.Skip).Take(opts.Showing);

            res.List = q.ToList();
            return res;
        }
    }
}
