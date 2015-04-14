// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultLinkActivator.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Provides functionality for creating links using an appropriate constructor or member properties.
    /// </summary>
    /// <typeparam name="T">The type of link to create.</typeparam>
    public class DefaultLinkActivator<T> : ILinkActivator<T> where T : LinkTemplate
    {
        /// <summary>
        /// A delegate for creating the link instance using the constructor or member properties.
        /// </summary>
        private static Func<string, IUriTemplate, ITargetAttributes, T> createLinkUsingType;

        /// <summary>
        /// A custom delegate for creating the link instance.
        /// </summary>
        private readonly Func<string, IUriTemplate, ITargetAttributes, T> createLinkUsingCustomDelegate;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLinkActivator{T}"/> class.
        /// </summary>
        public DefaultLinkActivator() : this(createLink: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultLinkActivator{T}"/> class.
        /// </summary>
        /// <param name="createLink">A delegate for creating the instance.</param>
        public DefaultLinkActivator(Func<string, IUriTemplate, ITargetAttributes, T> createLink)
        {
            this.createLinkUsingCustomDelegate = createLink;
        }

        /// <summary>
        /// Creates an instance of the link.
        /// </summary>
        /// <param name="relationType">The type of link relation to create.</param>
        /// <param name="target">The target of the link.</param>
        /// <param name="attributes">The attributes of the link target.</param>
        /// <returns>A new link instance.</returns>
        public T Create(string relationType, IUriTemplate target, ITargetAttributes attributes)
        {
            var createLink = this.createLinkUsingCustomDelegate ??
                createLinkUsingType ??
                (createLinkUsingType = CreateDelegateUsingConstructor() ?? CreateDelegateUsingInitializer());

            return createLink == null ? null : createLink(relationType, target, attributes);
        }

        /// <summary>
        /// Creates an instance of the link using the type's constructor.
        /// </summary>
        /// <returns>A delegate for creating the link instance.</returns>
        private static Func<string, IUriTemplate, ITargetAttributes, T> CreateDelegateUsingConstructor()
        {
            var parameters = new[] { typeof(string), typeof(IUriTemplate), typeof(ITargetAttributes) };
            var constructor = typeof(T).GetConstructor(parameters);

            if (constructor == null)
            {
                return null;
            }

            var newExpression = Expression.New(constructor, parameters.Select(Expression.Parameter));

            return Expression.Lambda<Func<string, IUriTemplate, ITargetAttributes, T>>(
                newExpression,
                newExpression.Arguments.OfType<ParameterExpression>()).Compile();
        }

        /// <summary>
        /// Creates an instance of the link using the type's properties.
        /// </summary>
        /// <returns>A delegate for creating the link instance.</returns>
        private static Func<string, IUriTemplate, ITargetAttributes, T> CreateDelegateUsingInitializer()
        {
            var constructor = typeof(T).GetConstructor(Type.EmptyTypes);

            if (constructor == null)
            {
                return null;
            }

            try
            {
                var assignments = GetAssignments(new[] { "RelationType", "Target", "TargetAttributes" });

                var init = Expression.MemberInit(Expression.New(constructor), assignments);

                return Expression.Lambda<Func<string, IUriTemplate, ITargetAttributes, T>>(
                    init,
                    init.Bindings.OfType<ParameterExpression>()).Compile();
            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a collection of expressions that represent assigning a value to the specified properties.
        /// </summary>
        /// <param name="propertyNames">The names of the properties for which to generate assignments.</param>
        /// <returns>A collection of expressions that represent assigning values to the specified properties.</returns>
        private static IEnumerable<MemberBinding> GetAssignments(params string[] propertyNames)
        {
            foreach (var property in propertyNames.Select(p => typeof(T).GetProperty(
                p,
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)))
            {
                if (property == null || !property.CanWrite)
                {
                    throw new InvalidOperationException();
                }

                yield return Expression.Bind(property, Expression.Parameter(property.PropertyType));
            }
        }
    }
}