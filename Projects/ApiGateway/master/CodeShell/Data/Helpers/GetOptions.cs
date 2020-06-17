using System.Collections.Generic;
using System.Linq.Expressions;

namespace CodeShell.Data.Helpers
{
    public abstract class GetOptions
    {
        public string SearchTerm { get; set; }
        /// <summary>
        /// DESC : for descending &lt;br/&gt;
        /// ASC : for ascending
        /// </summary>
        public SortDir Direction { get; set; }
        /// <summary>
        /// List of Expression&lt;Func&lt;T,bool&gt;&gt;
        /// </summary>
        /// <example>
        /// <code>param => param.ID == 3</code>
        /// </example>
        public List<Expression> Filters { get; set; }
        /// <summary>
        /// starting from (Showing * PageNumber)
        /// </summary>
        public int Skip { get; set; }
        /// <summary>
        /// how many records to show
        /// </summary>
        public int Showing { get; set; }
        /// <summary>
        /// Property for ORDER BY
        /// </summary>
        public string OrderProperty { get; set; }

        public List<Expression> Sort { get; set; }
    }
}
