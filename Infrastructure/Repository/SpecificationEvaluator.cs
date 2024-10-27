﻿using Core.Interfaces;
using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> query, ISpecification<T> spec)
        {
            if (spec.Criteria != null) { query = query.Where(spec.Criteria);}

            if (spec.OrderBy != null) { query = query.OrderBy(spec.OrderBy); }

            if (spec.OrderByDescending != null) { query = query.OrderBy(spec.OrderByDescending); }

            if (spec.IsPagingEnabled) {query = query.Skip(spec.Skip).Take(spec.Take);}

            return query;
        }      
    }
}
