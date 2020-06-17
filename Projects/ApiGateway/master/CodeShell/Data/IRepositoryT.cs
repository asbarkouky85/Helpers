using CodeShell.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Data
{
    public interface IRepository<T> : IRepository where T : class, IModel
    {
        TValue GetValue<TValue>(object id, Expression<Func<T, TValue>> ex);

        T FindSingle(object id);
        T FindSingle(Expression<Func<T, bool>> expression);
        TR FindSingleAs<TR>(object id) where TR : class;
        TR FindSingleAs<TR>(Expression<Func<T, bool>> expression) where TR : class;

        void Insert(T obj);
        void Update(T obj);
        void Delete(T obj);

        List<T> Find(Expression<Func<T, bool>> exp);
        LoadResult Find(GetOptions<T> opts);

        List<TR> FindAs<TR>(Expression<Func<T, TR>> exp, Expression<Func<T, bool>> cond = null, GetOptions<TR> opts = null) where TR : class;
        LoadResult FindAs<TR>(Expression<Func<T, TR>> exp, GetOptions<TR> opts, Expression<Func<T, bool>> cond = null) where TR : class;

        int Count(Expression<Func<T, bool>> exp);

        bool Exist(Expression<Func<T, bool>> exp);
    }
}
