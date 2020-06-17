using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using CodeShell.Data.Helpers;
using CodeShell.Data.Collections;

namespace CodeShell.Data.EntityFramework
{

    /// <summary>
    /// A repository is treated like a data source for a specific entity where you should be able to add, edit and delete and
    /// also retrieve lists with specific conditions
    /// </summary>
    /// <typeparam name="T">the physical or viewmodel type</typeparam>
    public class Repository<T> : IRepository<T> where T : class, IModel
    {
        #region members
        private DbSet _saver;
        private IQueryable<T> _loader;


        /// <summary>
        /// holds instances of all requested repositories throughout the object life
        /// </summary>
        protected Dictionary<Type, IRepository> reposes;
        /// <summary>
        /// DataContext
        /// </summary>
        protected DbContext DbContext { get; set; }


        /// <summary>
        /// The DbSet the Add, Edit and Delete uses, for a <see cref="IViewModel"/> will be the DbSet of the original Entity
        /// </summary>
        protected DbSet Saver
        {
            get
            {
                if (_saver == null)
                    _saver = GetSaver();
                return _saver;
            }
        }


        /// <summary>
        /// The main <see cref="Queryable"/> object all the repository data is loaded from
        /// </summary>
        protected IQueryable<T> Loader
        {
            get
            {
                if (_loader == null)
                    _loader = GetLoader();
                return _loader;
            }
        }


        #endregion


        /// <summary>
        /// DbContext is mandetory
        /// </summary>
        /// <param name="con">DbContext is mandetory</param>
        public Repository(DbContext con)
        {
            DbContext = con;
        }

        #region Protected
        /// <summary>
        /// Gets the DbSet the Add, Edit and Delete uses, for a <see cref="IViewModel"/> will be the DbSet of the original Entity
        /// </summary>
        /// <remarks>you can override this function change the behavior of the saving source</remarks>
        protected virtual DbSet GetSaver()
        {
            return DbContext.Set<T>();
        }

        /// <summary>
        /// Gets the main <see cref="Queryable"/> object all the repository data is loaded from
        /// </summary>
        /// <remarks>you can override this function change the behavior of the load function</remarks>
        protected virtual IQueryable<T> GetLoader()
        {
            return DbContext.Set<T>();
        }

        protected TR Repo<TR>() where TR : class, IRepository
        {
            if (reposes == null)
                reposes = new Dictionary<Type, IRepository>();
            Type indx = typeof(TR);
            if (!reposes.ContainsKey(indx))
                reposes[indx] = (TR)Activator.CreateInstance(typeof(TR), DbContext);

            return (TR)reposes[indx];
        }
        #endregion

        /// <summary>
        /// to retrieve all records from the data source with the repository entity
        /// </summary>
        /// <returns>should not be a queryable object</returns>
        public IEnumerable All()
        {
            return Loader.ToList();
        }

        public TValue GetValue<TValue>(object id, Expression<Func<T, TValue>> ex)
        {
            int mId = (int)id;
            return Loader.Where(d => d.Id == mId).Select(ex).FirstOrDefault();
        }

        /// <summary>
        /// to retrieve single object using the primary key
        /// </summary>
        /// <param name="id"></param>
        /// <returns>if not found returns null</returns>
        public T FindSingle(object id)
        {
            int intId = (int)id;
            return Loader.Where(d => d.Id == intId).FirstOrDefault();
        }

        public TR FindSingleAs<TR>(object id) where TR : class
        {
            int intId = (int)id;
            Expression<Func<T, TR>> exp = QueryMapper.GetExpression<T, TR>();
            return Loader.Where(d => d.Id == intId).Select(exp).FirstOrDefault();
        }



        /// <summary>
        /// set the object to be inserted when <see cref="UnitOfWork.SaveChanges"/> is called
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Insert(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("entity");
            Saver.Add(obj);
        }

        public void InsertObject(object ob)
        {
            Insert((T)ob);
        }


        /// <summary>
        /// set the object to be deleted when <see cref="UnitOfWork.SaveChanges"/> is called
        /// </summary>
        /// <param name="obj"></param>
        public virtual void Delete(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("entity");
            DbContext.Entry(obj).State = EntityState.Deleted;
        }
        public void DeleteObject(object ob)
        {
            Delete((T)ob);
        }

        public virtual void Update(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException("entity");
            DbContext.Entry(obj).State = EntityState.Modified;
        }

        public void UpdateObject(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("entity");
            Update((T)obj);
        }

        /// <summary>
        /// counts all records in repository
        /// </summary>
        /// <returns></returns>
        public int Count()
        {
            return Loader.Count(d => true);
        }

        /// <summary>
        /// counts records after filtering using the filter expression provided
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> exp)
        {
            return Loader.Where(exp).Count(d => true);
        }

        /// <summary>
        /// if records in the repository with the given createria exists
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public bool Exist(Expression<Func<T, bool>> exp)
        {
            return Loader.Any(exp);
        }

        /// <summary>
        /// to retrieve all records from the data source using conditions specified in the <see cref="LoadOptions"/> instance 
        /// </summary>
        /// <param name="exp"></param>
        /// <returns>should not be a queryable object</returns>
        public List<T> Find(Expression<Func<T, bool>> exp)
        {
            return Loader.Where(exp).ToList();
        }

        public LoadResult Find(GetOptions<T> opts)
        {
            return Loader.LoadWith(opts);
        }

        public List<TR> FindAs<TR>(Expression<Func<T, TR>> exp, Expression<Func<T, bool>> cond = null, GetOptions<TR> opts = null) where TR : class
        {
            var q = Loader;
            if (cond != null)
                q = q.Where(cond);

            if (opts != null)
                return q.Select(exp).ToListWith(opts);

            return q.Select(exp).ToList();
        }

        public LoadResult FindAs<TR>(Expression<Func<T, TR>> exp, GetOptions<TR> opts, Expression<Func<T, bool>> cond = null) where TR : class
        {
            var q = Loader;
            if (cond != null)
                q = q.Where(cond);

            return q.Select(exp).LoadWith(opts);
        }

        public T FindSingle(Expression<Func<T, bool>> expression)
        {
            return Loader.Where(expression).FirstOrDefault();
        }

        public TR FindSingleAs<TR>(Expression<Func<T, bool>> expression) where TR : class
        {

            Expression<Func<T, TR>> exp = QueryMapper.GetExpression<T, TR>();
            return Loader.Where(expression).Select(exp).FirstOrDefault();
        }
    }
}

