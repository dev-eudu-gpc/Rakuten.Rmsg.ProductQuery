// ---------------------------------------------------------------------------------------------------------------------
// <copyright file="LinkRelationAttribute.cs" company="Rakuten">
//   Copyright (c) Rakuten. All rights reserved.
// </copyright>
// ---------------------------------------------------------------------------------------------------------------------
namespace Rakuten
{
    using System;

    /// <summary>Specifies the relation type of a link.</summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class LinkRelationAttribute : Attribute
    {
        /// <summary>Gets or sets the name of the link relation type.</summary>
        public string Name { get; set; }
    }
}