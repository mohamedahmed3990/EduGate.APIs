using EduGate.Core;
using EduGate.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Repositroy
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;

            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }


            query = spec.InCludes.Aggregate(query, (currentQuery, incluedExpression) => currentQuery.Include(incluedExpression));


            return query;
        }
    }
}
