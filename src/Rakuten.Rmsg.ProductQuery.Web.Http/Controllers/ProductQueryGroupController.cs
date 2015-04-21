//------------------------------------------------------------------------------
// <copyright file="ProductQueryGroupController.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Results;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// Defines endpoints for creating and obtaining information for product query groups.
    /// </summary>
    public class ProductQueryGroupController : ApiController
    {
        /// <summary>
        /// A command that gets an image representing the progress of a given group at a given point in time.
        /// </summary>
        private readonly ICommand<GetProductQueryGroupProgressCommandParameters, Task<Stream>> getProgressCommand;

        /// <summary>
        /// A link representing the canonical location for the status of a product
        /// query group at a particular point in time.
        /// </summary>
        private readonly ProductQueryMonitorLink monitorLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryGroupController"/> class.
        /// </summary>
        /// <param name="getProgressCommand">
        /// A command that gets an image representing the progress of a given group at a given point in time.
        /// </param>
        /// <param name="monitorUriTemplate">
        /// A link template representing the canonical location for
        /// that status of a product query at a particular point in time
        /// </param>
        public ProductQueryGroupController(
            ICommand<GetProductQueryGroupProgressCommandParameters, Task<Stream>> getProgressCommand,
            IUriTemplate monitorUriTemplate)
        {
            Contract.Requires(getProgressCommand != null);
            Contract.Requires(monitorUriTemplate != null);

            this.getProgressCommand = getProgressCommand;
            this.monitorLink = new ProductQueryMonitorLink(monitorUriTemplate);
        }

        /// <summary>
        /// Provides a progress map of given product query group at a given point in time
        /// </summary>
        /// <param name="id">The unique identifier of the product query group.</param>
        /// <param name="year">The year of the point in time.</param>
        /// <param name="month">The month of the point in time.</param>
        /// <param name="day">The day of the point in time.</param>
        /// <param name="time">The time of the point in time.</param>
        /// <returns>The progress of the query group as an image.</returns>
        [Route("product-query-group/{id}/status/{year}/{month}/{day}/{time}")]
        public async Task<IHttpActionResult> GetProgressAsync(Guid id, int year, int month, int day, int time)
        {
            Contract.Requires(id != null);
            Contract.Requires(year >= 1 && year <= 9999);
            Contract.Requires(month >= 1 && month <= 12);
            Contract.Requires(day >= 1 && day <= 31);

            // Window granularity
            int granularity = 60;

            // Reconstitute the date
            var date = new DateTime(year, month, day, (int)Math.Floor(time / 100d), time % 100, 0);

            if (date > DateTime.UtcNow.AddSeconds(0 - granularity))
            {
                // The requested date is too recent, send a redirection
                var serverTime = DateTime.UtcNow;

                var location = this.monitorLink
                    .ForId(id.ToString())
                    .ForYear(serverTime.Year.ToString())
                    .ForMonth(serverTime.Month.ToString())
                    .ForDay(serverTime.Day.ToString())
                    .ForTime(serverTime.ToString("HHmm"))
                    .ToLink(true);

                return new SeeOtherResult(
                    new Uri(location.Target, UriKind.Relative),
                    granularity,
                    this);
            }
            else
            {
                // Get an image representation of the progress map
                var image = await this.getProgressCommand.Execute(
                    new GetProductQueryGroupProgressCommandParameters(id, date));

                // Return a message containing the image
                return new ImageResult(image, this);
            }
        }
   }
}