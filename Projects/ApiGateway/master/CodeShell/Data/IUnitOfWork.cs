using CodeShell.Data.EntityFramework;
using CodeShell.Data.Helpers;
using CodeShell.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ChangeLists GetChangeSet();
        void EnableJsonLoading();
        IUserRepository UserRepository { get; }
        IRepository GetRepository(Type t);
        IRepository<T> RepositoryFor<T>() where T : class, IModel;
        T GetRepository<T>() where T : class, IRepository;
        T Get<T>(long id) where T : class, IModel;
        SubmitResult SaveChanges(string successMessage = null,string faileMessage=null);
    }
}
