﻿namespace AgileObjects.AgileMapper.Api.Configuration
{
    using System;
    using System.Linq.Expressions;
    using Members;

    /// <summary>
    /// Provides options for configuring a condition which must evaluate to true for the configuration to apply
    /// to mappings from and to the source and target types being configured.
    /// </summary>
    /// <typeparam name="TSource">The source type to which the configuration should apply.</typeparam>
    /// <typeparam name="TTarget">The target type to which the configuration should apply.</typeparam>
    public interface IConditionalMappingConfigurator<TSource, TTarget> : IRootMappingConfigurator<TSource, TTarget>
    {
        /// <summary>
        /// Configure a condition which must evaluate to true for the configuration to apply. The condition
        /// expression is passed a context object containing the current mapping's source and target objects.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>An IConditionalRootMappingConfigurator with which to complete the configuration.</returns>
        IConditionalRootMappingConfigurator<TSource, TTarget> If(
            Expression<Func<IMappingData<TSource, TTarget>, bool>> condition);

        /// <summary>
        /// Configure a condition which must evaluate to true for the configuration to apply. The condition
        /// expression is passed the current mapping's source and target objects.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>An IConditionalRootMappingConfigurator with which to complete the configuration.</returns>
        IConditionalRootMappingConfigurator<TSource, TTarget> If(
            Expression<Func<TSource, TTarget, bool>> condition);

        /// <summary>
        /// Configure a condition which must evaluate to true for the configuration to apply. The condition
        /// expression is passed the current mapping's source and target objects and the current enumerable 
        /// index, if applicable.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <returns>An IConditionalRootMappingConfigurator with which to complete the configuration.</returns>
        IConditionalRootMappingConfigurator<TSource, TTarget> If(
            Expression<Func<TSource, TTarget, int?, bool>> condition);
    }
}