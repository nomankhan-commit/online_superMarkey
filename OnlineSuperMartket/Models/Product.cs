//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OnlineSuperMartket.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            this.Discounts = new HashSet<Discount>();
            this.statements = new HashSet<statement>();
        }
    
        public int Product_ID { get; set; }
        public Nullable<int> sellorID { get; set; }
        public string sellorName { get; set; }
        public int category_ID { get; set; }
        public Nullable<int> brand_ID { get; set; }
        public string Product_name { get; set; }
        public string Product_disc { get; set; }
        public Nullable<int> Product_code { get; set; }
        public int whole_sale_price { get; set; }
        public int retail_price { get; set; }
        public Nullable<int> stock { get; set; }
        public string imgPath { get; set; }
        public System.DateTime create_at { get; set; }
        public Nullable<System.DateTime> update_at { get; set; }
        public Nullable<bool> is_active { get; set; }
    
        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Discount> Discounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<statement> statements { get; set; }
    }
}
