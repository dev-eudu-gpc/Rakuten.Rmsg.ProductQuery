// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Statistic.cs" company="Rakuten">
//     Copyright (c) Rakuten. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Rakuten.Gpc.Resources
{
    using System.Xml.Serialization;

    /// <summary>
    /// Represents a collection of tracked system metrics.
    /// </summary>
    [XmlType("statistic")]
    public class Statistic : Resource
    {
        /// <summary>
        /// Gets or sets the event to which this metric represents.
        /// </summary>
        [XmlElement("event")]
        public string Event { get; set; }

        /// <summary>
        /// Gets or sets the value of the metric.
        /// </summary>
        [XmlElement("value")]
        public int Value { get; set; }
    }
}
