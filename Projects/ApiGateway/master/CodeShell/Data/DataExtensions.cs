using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Data
{
    public static class DataExtensions
    {
        public static IEnumerable<TR> MapList<T,TR>(this List<T> lst) where T : class where TR : class
        {
            Expression<Func<T, TR>> expression = QueryMapper.GetExpression<T, TR>();
            return lst.Select(expression.Compile()).ToList();
        }

        public static IEnumerable<TR> MapList<T, TR>(this IEnumerable<T> lst) where T : class where TR : class
        {
            Expression<Func<T, TR>> expression = QueryMapper.GetExpression<T, TR>();
            if(lst is IQueryable<T>)
                ((IQueryable<T>)lst).Select(expression).ToList();
            return lst.Select(expression.Compile()).ToList();
        }

        public static IEnumerable<TR> MapList<T, TR>(this IQueryable<T> lst) where T : class where TR : class
        {
            return lst.Select(QueryMapper.GetExpression<T, TR>()).ToList();
        }

        public static void AppendProperties(this IModel model, object ob, IEnumerable<string> ignore = null)
        {
            ignore = ignore == null ? new List<string>() : ignore;
            PropertyInfo[] props = ob.GetType().GetProperties();
            Dictionary<string, PropertyInfo> modelProps = model.GetType()
                .GetProperties()
                .Where(
                    d => d.Name != "Id" &&
                    (d.PropertyType == typeof(string) || !typeof(IEnumerable).IsAssignableFrom(d.PropertyType)) &&
                    !ignore.Contains(d.Name)
                )
                .ToDictionary(d => d.Name);

            foreach (PropertyInfo inf in props)
            {
                if (modelProps.ContainsKey(inf.Name))
                {
                    object v = inf.GetValue(ob);
                    modelProps[inf.Name].SetValue(model, v);
                }
            }
        }
    }
}
