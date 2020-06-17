using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CodeShell.Data.Helpers
{
    public enum SortDir { ASC, DESC }
    /// <summary>
    /// Object defining conditions for <see cref="IRepository.Find(LoadOptions)"/>
    /// </summary>
    public class GetOptions<T> : GetOptions where T : class
    {
        
        /// <summary>
        /// Object defining conditions for <see cref="IRepository.Find(GetOptions)"/>
        /// </summary>
        /// <summary>
        /// To facilitate the creation of filter expressions according to the type of {T}
        /// </summary>
        /// <param name="expression"></param>
        public void AddFilter(Expression<Func<T, bool>> expression)
        {
            if (Filters == null)
                Filters = new List<Expression>();
            Filters.Add(expression);
            
        }

        
    }
}
