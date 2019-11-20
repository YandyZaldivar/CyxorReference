/*
  { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/>
  Copyright (C) 2017  Yandy Zaldivar

  This program is free software: you can redistribute it and/or modify
  it under the terms of the GNU Affero General Public License as
  published by the Free Software Foundation, either version 3 of the
  License, or (at your option) any later version.

  This program is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU Affero General Public License for more details.

  You should have received a copy of the GNU Affero General Public License
  along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Linq;
using System.Reflection;
using System.Linq.Expressions;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

namespace Cyxor.Controllers
{
    using Models;
    using Networking;

    public abstract class MasterController : Controller
    {
        Master master;
        protected new Master Node => master ?? (master = base.Node as Master);

        MasterConnection masterConnection;
        protected new MasterConnection Connection
            => masterConnection ?? (masterConnection = base.Connection as MasterConnection);

        /*
        protected virtual TApiModel FromModel<TApiModel, TModel>(TModel model)
            => model != null ? Node.Mapper.Map<TModel, TApiModel>(model, opts
                => opts.ConfigureMap(AutoMapper.MemberList.None)) : default;

        protected virtual TModel ToModel<TApiModel, TModel>(TApiModel apiModel)
            => apiModel != null ? Node.Mapper.Map<TApiModel, TModel>(apiModel, opts
                => opts.ConfigureMap(AutoMapper.MemberList.None)) : default;

        //protected virtual IEnumerable<TApiModel> FromModels<TApiModel, TModel>(IEnumerable<TModel> models)
        //    => models != null ? Node.Mapper.Map<IEnumerable<TModel>, IEnumerable<TApiModel>>(models, opts =>
        //    opts.ConfigureMap(AutoMapper.MemberList.None)) : default(IEnumerable<TApiModel>);

        protected virtual IEnumerable<TApiModel> FromModels<TApiModel, TModel>(IEnumerable<TModel> models)
            => models != null ? Node.Mapper.Map<IEnumerable<TModel>, IEnumerable<TApiModel>>(models) : default(IEnumerable<TApiModel>);
        */


        //protected IQueryable<TSource> Query<TSource>(IQueryable<TSource> queryable, ListApiModel model, int pageSize = -1)
        //    where TSource: class
        //{
        //    queryable = Include(queryable, model); // TODO: Dar la posibilidad de listar recursivamente cada include
        //    queryable = Filter(queryable, model);
        //    queryable = Order(queryable, model);
        //    queryable = Paginate(queryable, model);

        //    return queryable;
        //}

        protected IQueryable<TModel> Read<TModel>(IQueryable<TModel> query, ReadApiModel readApiModel, int pageSize = -1)
            where TModel : class
        {
            query = Include(query, readApiModel); // TODO: Dar la posibilidad de listar recursivamente cada include
            query = Filter(query, readApiModel);
            query = Order(query, readApiModel);
            query = Paginate(query, readApiModel);

            return query;
        }

        protected IQueryable<TSource> Include<TSource>(IQueryable<TSource> query, ReadApiModel readApiModel)
            where TSource : class
        {
            if (readApiModel.Includes is IEnumerable<string> includes)
                foreach (var include in includes)
                    query = query.Include(include);

            return query;
        }

        protected IQueryable<TSource> Filter<TSource>(IQueryable<TSource> query, ReadApiModel readApiModel)
        {
            if (((readApiModel.Filters?.Count ?? 0) > 0))
                return query.Where(ExpressionHelper.Filter<TSource>(readApiModel.Filters));

            return query;
        }

        protected IQueryable<TSource> Order<TSource>(IQueryable<TSource> query, ReadApiModel readApiModel)
        {
            var i = 0;

            if (readApiModel.Orders is IEnumerable<OrderByApiModel> orders)
                foreach (var order in orders)
                    query = ExpressionHelper.OrderBy(query, order.PropertyName, i++, order.Descending);

            return query;
        }

        protected IQueryable<TSource> Paginate<TSource>(IQueryable<TSource> query, ReadApiModel readApiModel, int pageSize = -1)
        {
            var take = readApiModel.Take;

            if (pageSize != -1 && take > pageSize)
                take = pageSize;

            switch (readApiModel)
            {
                case ReadApiModel list when (list.Skip == -1 && list.Take == -1): return query;
                case ReadApiModel list when (list.Skip == -1): return query.Take(take);
                case ReadApiModel list when (list.Take == -1): return query.Skip(readApiModel.Skip);
                default: return query.Skip(readApiModel.Skip).Take(take);
            }
        }

        static class ExpressionHelper
        {
            const string OrderByExpressionParameterName = "p";

            const string OrderByMethodName = nameof(Queryable.OrderBy);
            const string OrderByDescendingMethodName = nameof(Queryable.OrderByDescending);
            const string ThenByMethodName = nameof(Queryable.ThenBy);
            const string ThenByDescendingMethodName = nameof(Queryable.ThenByDescending);

            static readonly MethodInfo ContainsMethod = typeof(string).GetMethod(nameof(string.Contains));
            static readonly MethodInfo StartsWithMethod = typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) });
            static readonly MethodInfo EndsWithMethod = typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) });

            public static Expression<Func<T, bool>> Filter<T>(List<FilterApiModel> filters)
            {
                if ((filters?.Count ?? 0) == 0)
                    return null;

                var parameter = Expression.Parameter(typeof(T), nameof(T));
                var expression = GetExpression(parameter, filters[0]);

                if (filters.Count > 1)
                    for (var i = 1; i < filters.Count; i++)
                    {
                        var newExpression = GetExpression(parameter, filters[i]);

                        // TODO: Redesign to add support for 'AndAlso' and 'OrElse' operators
                        if (filters[i].Operator == FilterOperator.And)
                            expression = Expression.And(expression, newExpression);
                        else
                            expression = Expression.Or(expression, newExpression);
                    }

                Expression GetExpression(ParameterExpression param, FilterApiModel filter)
                {
                    var constant = default(Expression);
                    var constantRange = default(Expression);
                    var member = Expression.Property(param, filter.PropertyName);

                    constant = Expression.Constant(filter.Value);
                    constantRange = Expression.Constant(filter.ValueRange);

                    if (member.Type.GetTypeInfo().IsPrimitive)
                    {
                        constant = Expression.ConvertChecked(constant, member.Type);
                        constantRange = Expression.ConvertChecked(constantRange, member.Type);
                    }

                    switch (filter.Comparison)
                    {
                        case FilterComparison.Equal: return Expression.Equal(member, constant);
                        case FilterComparison.GreaterThan: return Expression.GreaterThan(member, constant);
                        case FilterComparison.GreaterThanOrEqual: return Expression.GreaterThanOrEqual(member, constant);
                        case FilterComparison.LessThan: return Expression.LessThan(member, constant);
                        case FilterComparison.LessThanOrEqual: return Expression.LessThanOrEqual(member, constant);
                        case FilterComparison.NotEqual: return Expression.NotEqual(member, constant);
                        case FilterComparison.Contains: return Expression.Call(member, ContainsMethod, constant);
                        case FilterComparison.StartsWith: return Expression.Call(member, StartsWithMethod, constant);
                        case FilterComparison.EndsWith: return Expression.Call(member, EndsWithMethod, constant);

                        case FilterComparison.InRange: return Expression.And(Expression.GreaterThanOrEqual(member, constant), Expression.LessThanOrEqual(member, constantRange));

                        default: return null;
                    }
                }

                return Expression.Lambda<Func<T, bool>>(expression, parameter);
            }

            //// TODO: This if for converting a property name to a (p => p.Property) expression
            //static Expression<Func<T, object>> ToLambda<T>(string propertyName)
            //{
            //    var parameter = Expression.Parameter(typeof(T));
            //    var property = Expression.Property(parameter, propertyName);
            //    var propAsObject = Expression.Convert(property, typeof(object));

            //    return Expression.Lambda<Func<T, object>>(propAsObject, parameter);
            //}

            public static IQueryable<T> OrderBy<T>(IQueryable<T> query, string propertyName, int orderIteration, bool descending = false, IComparer<object> comparer = null)
            {
                var methodName = default(string);

                if (orderIteration == 0)
                    methodName = descending ? OrderByDescendingMethodName : OrderByMethodName;
                else
                    methodName = descending ? ThenByDescendingMethodName : ThenByMethodName;

                var param = Expression.Parameter(typeof(T), OrderByExpressionParameterName);

                var body = propertyName.Split('.').Aggregate<string, Expression>(param, Expression.PropertyOrField);

                var arguments = new[] { typeof(T), body.Type };

                var expression = default(Expression);

                if (comparer == null)
                    expression = Expression.Call(typeof(Queryable), methodName, arguments, query.Expression, Expression.Lambda(body, param));
                else
                    expression = Expression.Call(typeof(Queryable), methodName, arguments, query.Expression, Expression.Lambda(body, param), Expression.Constant(comparer));

                return query.Provider.CreateQuery<T>(expression);
            }
        }
    }
}
/* { Cyxor } - .NET Core Backend Framework <http://www.cyxor.com/> */
