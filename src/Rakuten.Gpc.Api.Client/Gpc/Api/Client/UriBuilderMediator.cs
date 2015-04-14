// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UriBuilderMediator.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Api.Client
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Locates and calls a <see cref="IUriBuilder"/> instance from the given <see cref="IUriParameters"/>.
    /// </summary>
    public class UriBuilderMediator : IUriBuilderMediator
    {
        /// <summary>
        /// The cached set of <see cref="IUriBuilder"/> implementations keyed to the type of 
        /// <see cref="IUriParameters"/> implemented.
        /// </summary>
        private readonly Dictionary<Type, IUriBuilder> constructors;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriBuilderMediator"/> class.
        /// </summary>
        /// <param name="constructors">
        /// The collection of <see cref="IUriBuilder"/> implementations to use.
        /// </param>
        public UriBuilderMediator(IEnumerable<IUriBuilder> constructors)
        {
            if (constructors == null)
            {
                throw new ArgumentNullException("constructors");
            }

            this.constructors = new Dictionary<Type, IUriBuilder>();

            foreach (IUriBuilder constructor in constructors)
            {
                Type constructorType = constructor.GetType();
                Type[] constructorInterfaces = constructorType.GetInterfaces();
                Type[] parameterTypes = constructorInterfaces[0].GetGenericArguments();

                this.constructors.Add(parameterTypes[0], constructor);
            }
        }

        /// <summary>
        /// Locates and calls the <see cref="IUriBuilder"/> implementation with the specified 
        /// <see cref="IUriParameters"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the parameters to use to locate the <see cref="IUriBuilder"/> implementation.
        /// </typeparam>
        /// <param name="context">The context in which the URI should be constructed.</param>
        /// <param name="parameters">The parameters required to build the URI.</param>
        /// <returns>A URI built using the specified parameters.</returns>
        public Uri Build<T>(IApiClientContext context, T parameters) where T : IUriParameters
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (EqualityComparer<T>.Default.Equals(parameters, default(T)))
            {
                throw new ArgumentException("Parameters cannot be empty.", "parameters");
            }

            Type parameterType = parameters.GetType();

            return this.constructors[parameterType].Build(context, parameters);
        }
    }
}