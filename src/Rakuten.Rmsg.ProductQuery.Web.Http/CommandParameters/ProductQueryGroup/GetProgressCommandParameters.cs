//------------------------------------------------------------------------------
// <copyright file="GetProgressCommandParameters.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
namespace Rakuten.Rmsg.ProductQuery.Web.Http.Commands
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// The parameters for the <see cref="GetProgressCommand"/> class.
    /// </summary>
    internal class GetProgressCommandParameters
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetProgressCommandParameters"/> class.
        /// </summary>
        /// <param name="id">The unique identifier for the product query group.</param>
        /// <param name="year">The year of the point in time.</param>
        /// <param name="month">The month of the point in time.</param>
        /// <param name="day">The day of the point in time.</param>
        /// <param name="hour">The hour of the point in time.</param>
        /// <param name="minute">The minute of the point in time.</param>
        public GetProgressCommandParameters(
            string id, 
            string year,
            string month, 
            string day, 
            string hour,
            string minute)
        {
            Contract.Requires(day != null);
            Contract.Requires(hour != null);
            Contract.Requires(id != null);
            Contract.Requires(month != null);
            Contract.Requires(minute != null);
            Contract.Requires(year != null);

            // Try and convert the identifier to a GUID.
            Guid parsedId;

            if (!Guid.TryParse(id, out parsedId))
            {
                throw new InvalidGuidException("product query identifier", id);
            }

            this.Id = parsedId;

            var dateString = string.Format(
                "{0}/{1}/{2} {3}:{4}",
                day.PadLeft(2, '0'),
                month.PadLeft(2, '0'),
                year.PadLeft(4, '0'),
                hour.PadLeft(2, '0'),
                minute.PadLeft(2, '0'));
            DateTime date;

            if (!DateTime.TryParseExact(dateString, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                throw new InvalidDateException("date portion", string.Format("{0}/{1}/{2}/{3}/{4}", year, month, day, hour, minute));
            }

            this.DateTime = date;
        }

        /// <summary>
        /// Gets the point in time that the progress is determined.
        /// </summary>
        public DateTime DateTime { get; private set; }

        /// <summary>
        /// Gets the unique identifier of the product query group.
        /// </summary>
        public Guid Id { get; private set; }
    }
}