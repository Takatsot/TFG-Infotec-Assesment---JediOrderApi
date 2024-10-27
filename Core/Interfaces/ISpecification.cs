﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISpecification<T> 
    {
        Expression<Func<T,bool>>? Criteria { get; }
        Expression<Func<T,Object>>? OrderBy { get; }
        Expression<Func<T,Object>>? OrderByDescending { get; }
        int Take { get; }
        int Skip { get; }   
        bool IsPagingEnabled { get; }
        IQueryable<T> ApplyCriteria(IQueryable<T> query);
    }
}
