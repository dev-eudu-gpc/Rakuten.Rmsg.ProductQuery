//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rakuten.Rmsg.ProductQuery.Web.Http.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    
    public partial class rmsgProductQueryStatu
    {
        public rmsgProductQueryStatu()
        {
            this.rmsgProductQueries = new HashSet<rmsgProductQuery>();
        }
    
        public byte rmsgProductQueryStatusID { get; set; }
        public string name { get; set; }
    
        public virtual ICollection<rmsgProductQuery> rmsgProductQueries { get; set; }
    }
}
