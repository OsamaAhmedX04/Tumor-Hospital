﻿using System.Linq.Expressions;

namespace TumorHospital.WebAPI.Specifications.Interface
{
    public interface ISpecification<T> where T : class
    {
        Expression<Func<T, bool>>? Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        Expression<Func<T, object>>? OrderBy { get; }
        Expression<Func<T, object>>? OrderByDescending { get; }
        Expression<Func<T, object>> Selector { get; }
    }
}
