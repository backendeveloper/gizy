﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Gizy.Extensions;

namespace Gizy.Filtering
{
    public static class FilteringExtensions
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, params QueryParameter[] queryParameters)
        {
            if (!queryParameters.HasElements()) return source;
            foreach (QueryParameter parameter in queryParameters)
            {
                if (parameter.Value == null) continue;
                ParameterExpression param = Expression.Parameter(typeof(T), "p");

                Type parameterType = Expression.Property(param, parameter.Name).Type;

                if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    throw new NotImplementedException("Query parameter binding for nullable types are not implemented yet.");
                }
                Expression<Func<T, bool>> exp = Expression.Lambda<Func<T, bool>>(Expression.Equal(
                        Expression.Property(param, parameter.Name),
                        Expression.Constant(parameter.Value)
                    ),
                    param
                );

                source = source.Where(exp.Compile());
            }

            return source;
        }
    }
}