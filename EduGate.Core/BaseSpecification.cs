﻿using EduGate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EduGate.Core
{
    public class BaseSpecification<T> : ISpecification<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> InCludes { get; set; } = new List<Expression<Func<T, object>>>();

        public BaseSpecification()
        {
            
        }

        public BaseSpecification(Expression<Func<T, bool>> _criteria)
        {
            Criteria = _criteria;   
        }
    }
}
