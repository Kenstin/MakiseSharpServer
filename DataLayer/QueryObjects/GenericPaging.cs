using System;
using System.Linq;

namespace DataLayer.QueryObjects
{
    public static class GenericPaging
    {
        public static IQueryable<T> Page<T>(
            this IQueryable<T> query,
            int pageNum, int pageSize)
        {
            if (pageSize == 0)
                throw new ArgumentOutOfRangeException
                    (nameof(pageSize), "pageSize cannot be zero.");

            if (pageNum != 0)
                query = query
                    .Skip(pageNum * pageSize);

            return query.Take(pageSize);
        }
    }
}
