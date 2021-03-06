﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KapGel.Models.EntityFramework;

namespace KapGel.Models
{
    public class ViewModelUrun
    {
        public int Id { get; set; }
        public string productName { get; set; }
        public Nullable<int> stockNumber { get; set; }
        public Nullable<byte> discountRate { get; set; }
        public Nullable<byte> productPoint { get; set; }
        public Nullable<int> categoryId { get; set; }
        public decimal price { get; set; }
        public string productPicture { get; set; }
        public bool IsitApproved { get; set; }
        public int adet { get; set; }


    }
}