// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="GenericExtensions.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.WebJob
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Provides a useful set of generic extensions.
    /// </summary>
    internal static class GenericExtensions
    {
        /// <summary>
        /// Sets the value of the specified property on the <paramref name="instance"/> where the property was found to
        /// not already have a value.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type"/> of the instance.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="lambda">An expression declaring the property.</param>
        /// <param name="value">The value to assign to the specified property.</param>
        public static void SetIfNullOrEmpty<T>(this T instance, Expression<Func<T, object>> lambda, object value)
        {
            var memberSelectorExpression = lambda.Body as MemberExpression;

            if (memberSelectorExpression == null)
            {
                return;
            }

            var property = memberSelectorExpression.Member as PropertyInfo;

            if (property == null)
            {
                return;
            }

            if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException("Only string properties are aupported.");
            }

            var currentValue = property.GetValue(instance) as string;

            if (string.IsNullOrEmpty(currentValue))
            {
                property.SetValue(instance, value);
            }
        }
    }
}