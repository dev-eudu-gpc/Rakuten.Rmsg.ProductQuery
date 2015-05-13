﻿//------------------------------------------------------------------------------
// <copyright file="GetProgressCommand.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Rakuten.Rmsg.ProductQuery.Configuration;

    /// <summary>
    /// A command for obtaining the progress of a given product query
    /// group at a given point in time.
    /// </summary>
    internal class GetProgressCommand : AsyncCommand<GetProgressCommandParameters, Stream>
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// A command that creates an images from a progress map.
        /// </summary>
        private readonly ICommand<CreateProgressImageCommandParameters, Task<Stream>> createImageCommand;

        /// <summary>
        /// A command that gets the progress map of a product query group
        /// from the database.
        /// </summary>
        private readonly ICommand<GetProgressDatabaseCommandParameters, Task<IQueryable<ProductQueryProgress>>> getProgressDatabaseCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetProgressCommand"/> class
        /// </summary>
        /// <param name="context">The context in which this instance is running.</param>
        /// <param name="createImageCommand">A command that creates an image from a progress map.</param>
        /// <param name="getProgressDatabaseCommand">A command that gets product query data from the database.</param>
        public GetProgressCommand(
            IApiContext context,
            ICommand<CreateProgressImageCommandParameters, Task<Stream>> createImageCommand,
            ICommand<GetProgressDatabaseCommandParameters, Task<IQueryable<ProductQueryProgress>>> getProgressDatabaseCommand)
        {
            Contract.Requires(context != null);
            Contract.Requires(createImageCommand != null);
            Contract.Requires(getProgressDatabaseCommand != null);

            this.context = context;
            this.createImageCommand = createImageCommand;
            this.getProgressDatabaseCommand = getProgressDatabaseCommand;
        }

        /// <summary>
        /// Gets the progress map for a given product query group at a given point in time
        /// </summary>
        /// <param name="parameters">The input parameters for creating the map.</param>
        /// <returns>The progress map as an image.</returns>
        public override async Task<Stream> ExecuteAsync(GetProgressCommandParameters parameters)
        {
            // Ensure that the product query group exists
            // Get the progress map from the database
            var progressMap = await this.getProgressDatabaseCommand.Execute(
                new GetProgressDatabaseCommandParameters(
                    parameters.Id,
                    parameters.DateTime));

            // Create and return the image
            return await this.createImageCommand.Execute(
                new CreateProgressImageCommandParameters(progressMap));
        }
    }
}