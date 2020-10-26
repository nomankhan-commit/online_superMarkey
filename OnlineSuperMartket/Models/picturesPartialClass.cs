using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineSuperMartket.Models
{
    public class picturesPartialClass
    {
    }
    [MetadataType(typeof(Product))]
    public partial class Product
    {
        public HttpPostedFileBase ImageFile { get; set; }
        public int user_id { get; set; }
    }
}

