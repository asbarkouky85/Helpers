using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using CodeShell.Data.Helpers;

namespace CodeShell.Data
{
    public interface IRepository
    {
        int Count();
        
        IEnumerable All();
        
        void InsertObject(object ob);
        void UpdateObject(object ob);
        void DeleteObject(object ob);
        
    }

    
}
