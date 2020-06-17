using System.Collections.Generic;

namespace CodeShell.Data.Helpers
{
    /// <summary>
    /// An object used to transmit the load options using ajax 
    /// </summary>
    public class LoadOpts
    {
        public string SearchTerm { get; set; }
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
        /// <summary>
        /// DESC : for descending &lt;br/&gt;
        /// ASC : for ascending
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// json string of a <see cref="PropertyFilter"/> array
        /// </summary>
        /// <example>
        /// <code>
        /// [ 
        ///     { MemberName : "NAME" , FilterType : "string" , Value1 : "Ahm" , Value2:"" , Ids : [] },
        ///     { MemberName : "AGE" , FilterType : "int" , Value1 : "15" , Value2:"30" , Ids : [] }
        /// ]
        /// </code>
        /// </example>
        public List<PropertyFilter> Filters { get; set; }
        /// <summary>
        /// An object used to transmit the load options using ajax 
        /// </summary>
        public LoadOpts() { }
        /// <summary>
        /// An object used to transmit the load options using ajax 
        /// </summary>
        /// <param name="show"></param>
        public LoadOpts(int show)
        {

            Showing = show;
            OrderProperty = "ID";

        }

        public GetOptions<T> GetOptionsFor<T>() where T : class
        {
            return (new ExpressionGenerator<T>()).ToModelGetOptions(this);
        }
    }
}
