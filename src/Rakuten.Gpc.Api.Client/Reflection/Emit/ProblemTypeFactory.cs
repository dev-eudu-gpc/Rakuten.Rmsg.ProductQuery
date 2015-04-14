//---------------------------------------------------------------------------------------------------------------------
// <copyright file="ProblemTypeFactory.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//---------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Reflection.Emit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    using Rakuten.Gpc;

    /// <summary>
    /// Creates and populates dynamic types from an <see cref="IApiException"/> instance.
    /// </summary>
    public class ProblemTypeFactory
    {
        /// <summary>
        /// Constructs HTTP problem definitions from thrown exceptions to be serialized.
        /// </summary>
        private readonly ProblemBuilder builder;

        /// <summary>
        /// A cache of exception type to problem types.
        /// </summary>
        private readonly Dictionary<Type, Type> problemCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProblemTypeFactory"/> class
        /// </summary>
        /// <param name="builder">
        /// Constructs HTTP Problem types for serialization.
        /// </param>
        public ProblemTypeFactory(ProblemBuilder builder)
        {
            Contract.Requires(builder != null);

            this.builder = builder;
            this.problemCache = new Dictionary<Type, Type>();
        }

        /// <summary>
        /// Creates a dynamic type for the <see cref="IApiException"/> instance passed.
        /// </summary>
        /// <param name="exception">
        /// The instance from which the dynamic type should be modeled.
        /// </param>
        /// <returns>
        /// A dynamically created type of the <see cref="IApiException"/> instance passed.
        /// </returns>
        public Type Create(IApiException exception)
        {
            Contract.Requires(exception != null);

            return this.GetOrDefineType(exception);
        }

        /// <summary>
        /// Creates a dynamic type for the <see cref="IApiException"/> instance passed.
        /// </summary>
        /// <param name="type">
        /// The instance from which the dynamic type should be modeled.
        /// </param>
        /// <returns>
        /// A dynamically created type of the <see cref="IApiException"/> instance passed.
        /// </returns>
        public Type Create(Type type)
        {
            Contract.Requires(type != null);

            return this.GetOrDefineType(type);
        }

        /// <summary>
        /// Creates, or retrieves from cached types a dynamically created type to represent the specified exception.
        /// </summary>
        /// <param name="exception">
        /// The exception on which the dynamically created type should be based.
        /// </param>
        /// <returns>
        /// The dynamically defined type.
        /// </returns>
        private Type GetOrDefineType(IApiException exception)
        {
            Contract.Requires(exception != null);
            Contract.Requires(this.problemCache != null);

            Type type;

            Type exceptionType = exception.GetType();

            if (!this.problemCache.TryGetValue(exceptionType, out type))
            {
                type = this.builder.DefineProblem(exception);
                this.problemCache.Add(exceptionType, type);
            }

            return type;
        }

        /// <summary>
        /// Creates, or retrieves from cached types a dynamically created type to represent the specified exception.
        /// </summary>
        /// <param name="type">
        /// The exception on which the dynamically created type should be based.
        /// </param>
        /// <returns>
        /// The dynamically defined type.
        /// </returns>
        private Type GetOrDefineType(Type type)
        {
            Contract.Requires(type != null);
            Contract.Requires(this.problemCache != null);

            Type probemType;

            if (!this.problemCache.TryGetValue(type, out probemType))
            {
                probemType = this.builder.DefineProblem(type);
                this.problemCache.Add(type, probemType);
            }

            return probemType;
        }
    }
}