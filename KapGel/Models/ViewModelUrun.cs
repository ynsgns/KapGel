using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KapGel.Models.EntityFramework;

namespace KapGel.Models
{
    public class ViewModelUrun 
    {
        public int Id  { get; set; }
        public string productName { get; set; }
        public int stockNumber { get; set; }
        public Nullable<byte> discountRate { get; set; }
        public Nullable<byte> productPoint { get; set; }
        public int price { get; set; }
        public string productPicture { get; set; }


    }
}