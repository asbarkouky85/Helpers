using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CodeShell.Web.Razor.Core
{
    /// <summary>
    /// Used by <see cref="Forms.FormsExtensions.For{T}"/> to cope view data from helper to another
    /// </summary>
    public class ViewDataContainer : IViewDataContainer
    {
        /// <summary>
        /// 
        /// </summary>
        public ViewDataDictionary ViewData { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public ViewDataContainer(ViewDataDictionary v) 
        {
            ViewData = v;
        }
    }

    
}
