using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlineSuperMartket.Models
{
    public class signupforSellerPartial
    {
    }

    [MetadataType(typeof(user))]
    public partial class user
    {
        public bool isVendor { get; set; }
    }
}