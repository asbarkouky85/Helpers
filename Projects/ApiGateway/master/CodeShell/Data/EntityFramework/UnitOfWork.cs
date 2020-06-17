using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using CodeShell.Data.Helpers;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using CodeShell.Security;
using CodeShell.Globalization;

namespace CodeShell.Data.EntityFramework
{
    /// <summary>
    /// Unit of work is a container for all the repositories
    /// </summary>
    public abstract class UnitOfWork : IUnitOfWork
    {

        /// <summary>
        /// holds instances of all requested repositories throughout the object life
        /// </summary>
        protected Dictionary<Type, IRepository> reposes;

        /// <summary>
        /// holds instances of all repositories requested with models throughout the object life
        /// </summary>
        protected Dictionary<Type, IRepository> modelReposes;

        /// <summary>
        /// This unit of work uses <see cref="System.Data.Entity.DbContext"/>
        /// </summary>
        protected DbContext DbContext { get; set; }


        /// <summary>
        /// An event handler for when a database log is to be written
        /// </summary>
        public Action<string> Log
        {
            set { DbContext.Database.Log = value; }
        }

        public virtual Action<ChangeLists> OnBeforeSave { get { return null; } }
        public virtual Action<ChangeLists> OnSaveSuccess { get { return null; } }
        public virtual Action<Exception> OnExceptionHandling { get { return null; } }
        public abstract IUserRepository UserRepository { get; }

        public void NoTracking()
        {
            DbContext.Configuration.LazyLoadingEnabled = false;
            DbContext.Configuration.ProxyCreationEnabled = false;
        }

        public ChangeLists GetChangeSet()
        {
            IEnumerable<DbEntityEntry> entries = DbContext.ChangeTracker.Entries();
            return new ChangeLists
            {
                Updated = entries.Where(d => d.State == EntityState.Modified).Select(d=>d.Entity).ToList(),
                Added = entries.Where(d => d.State == EntityState.Added).Select(d => d.Entity).ToList(),
                Deleted = entries.Where(d => d.State == EntityState.Deleted).Select(d => d.Entity).ToList()
            };
        }


        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="con"></param>
        public UnitOfWork(DbContext con = null)
        {
            DbContext = con;
            reposes = new Dictionary<Type, IRepository>();
            modelReposes = new Dictionary<Type, IRepository>();
        }
        /// <summary>
        /// Sets <see cref="DbContextConfiguration.ProxyCreationEnabled"/> to false, to prevent errors on json serialization
        /// </summary>
        public void EnableJsonLoading()
        {
            DbContext.Configuration.ProxyCreationEnabled = false;
            DbContext.Configuration.LazyLoadingEnabled = false;
            DbContext.Configuration.AutoDetectChangesEnabled = false;

        }

        /// <summary>
        /// Gets a <see cref="IRepository"/> for the type of repository provided using a string, the repository should only be instantiated
        /// once for each <see cref="IRepository"/> type
        /// </summary>
        /// <remarks>This methods looks for the repository in the properties of the unit of work using the reporsitory name
        /// so make sure to add the repository class to your <see cref="IUnitOfWork"/> class as a property with the name you call
        /// </remarks>
        /// <param name="t"></param>
        /// <returns></returns>
        public T GetRepository<T>() where T : class, IRepository
        {
            Type indx = typeof(T);
            if (!reposes.ContainsKey(indx))
                reposes[indx] = (T)Activator.CreateInstance(typeof(T), DbContext);

            return (T)reposes[indx];
        }


        /// <summary>
        /// Gets an object of type T using primary key
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get<T>(long id) where T : class, IModel
        {
            return DbContext.Set<T>().Where(d => d.Id == id).FirstOrDefault();
        }


        /// <summary>
        /// Gets a <see cref="IRepository"/> for the type of repository provided, the repository should only be instantiated
        /// once for each <see cref="IRepository"/> type
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public IRepository GetRepository(Type t)
        {
            if (!t.GetInterfaces().Contains(typeof(IRepository)))
                throw new Exception(t.Name + " does not implement IRepository");
            Type indx = t;
            if (!reposes.ContainsKey(indx))
                reposes[indx] = (IRepository)Activator.CreateInstance(t, DbContext);

            return reposes[indx];
        }


        /// <summary>
        /// Gets a <see cref="IRepository"/> for the type of repository provided using a string, the repository should only be instantiated
        /// once for each <see cref="IRepository"/> type
        /// </summary>
        /// <remarks>This methods looks for the repository in the properties of the unit of work using the reporsitory name
        /// so make sure to add the repository class to your <see cref="IUnitOfWork"/> class as a property with the name you call
        /// </remarks>
        /// <param name="t"></param>
        /// <returns></returns>
        public IRepository GetRepository(string t)
        {
            string name = (!t.Contains("Repository")) ? t + "Repository" : t;
            PropertyInfo p = GetType().GetProperty(name);

            if (p == null)
                throw new Exception("no such repository " + name + "Repository in Unit of work " + GetType().Name);

            return p.GetValue(this) as IRepository;
        }

        /// <summary>
        /// Gets a <see cref="IRepository"/> for the type of repository provided, the repository should only be instantiated
        /// once for each <see cref="IRepository"/> type
        /// </summary>
        /// <typeparam name="T">Type of a class that implements <see cref="IRepository"/></typeparam>
        /// <returns><see cref="Repository{T}"/></returns>
        public IRepository<T> RepositoryFor<T>() where T : class, IModel
        {
            Type indx = typeof(T);
            if (!modelReposes.ContainsKey(indx))
                modelReposes[indx] = new Repository<T>(DbContext);

            return (IRepository<T>)modelReposes[indx];

        }


        /// <summary>
        /// Attempts to submit changes to the data source
        /// </summary>
        /// <returns>if success <see cref="SubmitResult.Code"/> is 0</returns>
        public virtual SubmitResult SaveChanges(string successMessage = null, string failMessage = null)
        {
            SubmitResult res = new SubmitResult();
            successMessage = successMessage == null ? Strings.Word(MessageIds.success_message) : successMessage;
            failMessage = failMessage == null ? Strings.Word(MessageIds.fail_message) : failMessage;

            ChangeLists lst = null;

            if (OnBeforeSave != null || OnSaveSuccess!=null)
            {
                lst = GetChangeSet();
                OnBeforeSave?.Invoke(lst);
            }
                
            try
            {
                int rows = DbContext.SaveChanges();

                OnSaveSuccess?.Invoke(lst);

                res = new SubmitResult
                {
                    AffectedRows = rows,
                    Code = 0,
                    Message = successMessage
                };
            }
            catch (DbEntityValidationException dbEx)
            {
                OnExceptionHandling?.Invoke(dbEx);
                var msg = string.Empty;
                
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        msg += Environment.NewLine +
                               string.Format("Property: {0} Error: {1}", validationError.PropertyName,
                                   validationError.ErrorMessage);
                    }
                }
                res = new SubmitResult
                {
                    Message = failMessage,
                    Code = 102,
                };
                res.SetException(dbEx);
                res.ExceptionMessage = msg;
            }
            catch (SqlException ex)
            {
                OnExceptionHandling?.Invoke(ex);
                res = new SubmitResult
                {
                    Code = ex.Number,
                    Message = failMessage
                };
                res.SetException(ex);
            }
            catch (Exception ex)
            {
                OnExceptionHandling?.Invoke(ex);
                res = new SubmitResult
                {
                    Code = 101,
                    Message = failMessage
                };
                res.SetException(ex);
            }
            return res;
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
