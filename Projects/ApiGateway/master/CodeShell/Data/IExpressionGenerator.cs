﻿using CodeShell.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CodeShell.Data
{
    public interface IExpressionGenerator
    {
        
        /// <summary>
        /// converts property filter objects to filtering expressions use on an IQueryable
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        List<Expression> ToFilterExpressions(IEnumerable<PropertyFilter> f);

        /// <summary>
        /// creates an expression for finding records with the value of the between two decimal numbers
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="from">if 0 minimum is ignored</param>
        /// <param name="to">if 0 maximum is ignored</param>
        /// <returns></returns>
        Expression GetRangeFilter(string propertyName, decimal from, decimal to);
        /// <summary>
        /// creates an expression for finding records with the value of the between two dates
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="from">if DateTime.MinValue minimum is ignored</param>
        /// <param name="to">if DateTime.MaxValue maximum is ignored</param>
        /// <returns></returns>
        Expression GetRangeFilter(string propertyName, DateTime from, DateTime to);
        /// <summary>
        /// creates an expression for finding records with the value of the between two intgers
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="from">if 0 minimum is ignored</param>
        /// <param name="to">if 0 maximum is ignored</param>
        /// <returns></returns>
        Expression GetRangeFilter(string propertyName, int from, int to);
        /// <summary>
        /// creates an expression for finding records with the value of the property contains str
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="str">the string to filter by</param>
        /// <returns></returns>
        Expression GetStringContainsFilter(string propertyName, string str);
        /// <summary>
        /// creates an expression for finding records with the value of the property is contained in the ids
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="values">list of ids from the referenced table</param>
        /// <returns></returns>
        Expression GetReferenceContainedFilter(string propertyName, IEnumerable<long> values);
    }
}
