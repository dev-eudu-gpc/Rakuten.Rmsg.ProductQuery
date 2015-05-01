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
    using Rakuten.Rmsg.ProductQuery.Configuration;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Commands;
    using Rakuten.Rmsg.ProductQuery.Web.Http.Links;

    /// <summary>
    /// Defines endpoints for creating and obtaining information for product query groups.
    /// </summary>
    public class ProductQueryGroupController : ApiController
    {
        /// <summary>
        /// The context under which this instance is operating.
        /// </summary>
        private readonly IApiContext context;

        /// <summary>
        /// A command that gets an image representing the progress of a given group at a given point in time.
        /// </summary>
        private readonly ICommand<GetProgressCommandParameters, Task<Stream>> getProgressCommand;

        /// <summary>
        /// A link representing the canonical location for the status of a product
        /// query group at a particular point in time.
        /// </summary>
        private readonly ProductQueryMonitorLink monitorLink;
        ////private readonly ILink monitorLink;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductQueryGroupController"/> class.
        /// </summary>
        /// <param name="context">The context in which this instance is running.</param>
        /// <param name="getProgressCommand">
        /// A command that gets an image representing the progress of a given group at a given point in time.
        /// </param>
        /// <param name="monitorUriTemplate">
        /// A link template representing the canonical location for
        /// that status of a product query at a particular point in time
        /// </param>
        internal ProductQueryGroupController(
            IApiContext context,
            ICommand<GetProgressCommandParameters, Task<Stream>> getProgressCommand,
            IUriTemplate monitorUriTemplate)
        {
            Contract.Requires(context != null);
            Contract.Requires(getProgressCommand != null);
            Contract.Requires(monitorUriTemplate != null);

            this.context = context;
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
        /// <param name="hour">The hour of the point in time.</param>
        /// <param name="minute">The minute of the point in time.</param>
        /// <returns>The progress of the query group as an image.</returns>
        [Route("product-query-group/{id}/status/{year}/{month}/{day}/{hour}/{minute}")]
        public async Task<IHttpActionResult> GetProgressAsync(string id, string year, string month, string day, string hour, string minute)
        {
            Contract.Requires(day != null);
            Contract.Requires(hour != null);
            Contract.Requires(id != null);
            Contract.Requires(minute != null);
            Contract.Requires(month != null);
            Contract.Requires(year != null);

            // Construct the parameters for getting the progress of the product query group.
            // Validation of those parameters occurs in the constructor.
            var parameters = new GetProgressCommandParameters(id, year, month, day, hour, minute);

            if (parameters.DateTime > DateTime.UtcNow.AddSeconds(0 - this.context.ProgressMapIntervalInSeconds))
            {
                // The requested date is too recent, send a redirection to a monitor link
                // for the current time but also ask the user to wait for a specified
                // interval before following the link.
                var serverTime = DateTime.UtcNow;

                Link location = ((ProductQueryMonitorLink)this.monitorLink)
                    .ForId(id.ToString())
                    .ForYear(serverTime.Year.ToString("00"))
                    .ForMonth(serverTime.Month.ToString("00"))
                    .ForDay(serverTime.Day.ToString("00"))
                    .ForHour(serverTime.Hour.ToString("00"))
                    .ForMinute(serverTime.Minute.ToString("00"))
                    .ToLink(true);

                return new SeeOtherResult(
                    new Uri(location.Target, UriKind.Relative),
                    this.context.ProgressMapIntervalInSeconds,
                    this.Request);
            }
            else
            {
                // Get an image representation of the progress map and return it.
                return new ImageResult(await this.getProgressCommand.Execute(parameters), this.Request);
            }
        }
   }
}