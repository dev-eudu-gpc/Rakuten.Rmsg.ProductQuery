//---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProblemActivator.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Web.Http.ExceptionHandling
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;

    using FastMember;

    /// <summary>
    /// Contains methods to create types of objects.
    /// </summary>
    public class ProblemActivator
    {
        /// <summary>
        /// The <see cref="IApiExceptionMetadataProvider"/> instance that will extract key information from the 
        /// <see cref="Exception"/> instance.
        /// </summary>
        private readonly IApiExceptionMetadataProvider provider;

        /// <summary>
        /// A cache of problem types to the <see cref="Func{T}"/> that will create an instance.
        /// </summary>
        private readonly Dictionary<Type, Func<object>> constructorCache;

        /// <summary>
        /// A cache of <see cref="TypeAccessor"/> by problem type.
        /// </summary>
        private readonly Dictionary<Type, TypeAccessor> instanceAccessorCache;

        /// <summary>
        /// A cache of <see cref="TypeAccessor"/> by exception type.
        /// </summary>
        private readonly Dictionary<Type, TypeAccessor> exceptionAccessorCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProblemActivator"/> class
        /// </summary>
        /// <param name="provider">
        /// The <see cref="IApiExceptionMetadataProvider"/> instance to retrieve attributed metadata from an 
        /// <see cref="Exception"/> instance.
        /// </param>
        public ProblemActivator(IApiExceptionMetadataProvider provider)
        {
            Contract.Requires(provider != null);

            this.constructorCache = new Dictionary<Type, Func<object>>();
            this.instanceAccessorCache = new Dictionary<Type, TypeAccessor>();
            this.exceptionAccessorCache = new Dictionary<Type, TypeAccessor>();
            this.provider = provider;
        }

        /// <summary>
        /// Creates and populates an instance of the specified type using that type's default constructor and the 
        /// exception instance passed.
        /// </summary>
        /// <param name="type">
        /// The type of object to create.
        /// </param>
        /// <param name="exception">
        /// The exception from which the new instance should be populated.
        /// </param>
        /// <param name="innerInstances">
        /// The inner problems to be assigned to the created instance.
        /// </param>
        /// <returns>
        /// A reference to the newly created object.
        /// </returns>
        public object CreateInstance(Type type, Exception exception, object[] innerInstances = null)
        {
            Contract.Requires(type != null);
            Contract.Requires(exception != null);

            object instance = this.CreateInstance(type);
            var metadata = this.provider.GetMetadataForException(exception);

            this.PopulateInstance(type, instance, exception, metadata, innerInstances);

            return instance;
        }

        /// <summary>
        /// Instantiates an instance of the specified type.
        /// </summary>
        /// <param name="type">
        /// The type of which an instance should be created.
        /// </param>
        /// <returns>
        /// The newly created instance.
        /// </returns>
        private object CreateInstance(Type type)
        {
            Func<object> creator;

            if (!this.constructorCache.TryGetValue(type, out creator))
            {
                var newExpression = Expression.New(type);
                creator = Expression.Lambda<Func<object>>(newExpression).Compile();
                this.constructorCache.Add(type, creator);
            }

            return creator();
        }

        /// <summary>
        /// Populates the passed instance with the values from the passed exception.
        /// </summary>
        /// <param name="type">
        /// The type of the instance to populate.
        /// </param>
        /// <param name="instance">
        /// The instance to populate.
        /// </param>
        /// <param name="exception">
        /// The <see cref="Exception"/> from which values should be taken.
        /// </param>
        /// <param name="metadata">
        /// The metadata associated with the <paramref name="exception"/> passed.
        /// </param>
        /// <param name="innerInstances">
        /// The inner problems to be assigned to the created instance.
        /// </param>
        private void PopulateInstance(
            Type type,
            object instance,
            Exception exception,
            ApiExceptionMetadata metadata,
            object[] innerInstances)
        {
            Type exceptionType = exception.GetType();

            TypeAccessor instanceAccessor = this.GetOrCreateTypeAccessor(type, this.instanceAccessorCache);
            TypeAccessor exceptionAccessor = this.GetOrCreateTypeAccessor(exceptionType, this.exceptionAccessorCache);

            Uri problemType = metadata.Type;
            if (problemType != null)
            {
                instanceAccessor[instance, "Type"] = problemType.ToString();
            }
            
            instanceAccessor[instance, "Title"] = metadata.Title;
            instanceAccessor[instance, "Detail"] = exception.Message;

            // TODO: [MM, 03/10] Is there a better clause for the below if statement?
            if (innerInstances != null)
            {
                instanceAccessor[instance, "InnerProblems"] = innerInstances;
            }

            var properties = exception.GetType().GetProperties(
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo property in properties)
            {
                instanceAccessor[instance, property.Name] = exceptionAccessor[exception, property.Name];
            }
        }

        /// <summary>
        /// Gets or creates a <see cref="TypeAccessor"/> instance for the type passed.
        /// </summary>
        /// <param name="type">
        /// Type type of object for which to create the accessor.
        /// </param>
        /// <param name="cache">
        /// The cached collection of accessor instances.
        /// </param>
        /// <returns>
        /// The accessor instance.
        /// </returns>
        private TypeAccessor GetOrCreateTypeAccessor(Type type, Dictionary<Type, TypeAccessor> cache)
        {
            TypeAccessor accessor;

            if (!cache.TryGetValue(type, out accessor))
            {
                accessor = TypeAccessor.Create(type);
                cache.Add(type, accessor);
            }

            return accessor;
        }
    }
}