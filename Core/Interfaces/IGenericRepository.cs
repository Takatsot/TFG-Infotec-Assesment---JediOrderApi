﻿using Core.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T?> GetEntityWithSpec(ISpecification<T> specification);
        Task<IReadOnlyList<T>> ListAsync(ISpecification<T> specification);
        void Add (T entity);    
        void Update (T entity);
        void Delete (T entity); 
        Task<bool>SaveAllAsync();
        bool Exists(int id);
        Task<int> CountAsync(ISpecification<T>spec);
    }
}
