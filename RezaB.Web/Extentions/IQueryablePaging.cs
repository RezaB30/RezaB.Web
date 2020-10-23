using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RezaB.Web.Extentions
{
    public static class IQueryablePaging
    {
        public static IQueryable<T> PageData<T>(this IQueryable<T> query, int page, int rowCount)
        {
            return query.Skip(page * rowCount).Take(rowCount);
        }
    }
}