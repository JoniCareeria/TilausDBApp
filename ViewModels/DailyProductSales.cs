﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TilausDBApp.ViewModels
{
    public class DailyProductSales

    {

        public string OrderDate { get; set; }

        public string ProductName { get; set; }



        public float? DailySales { get; set; }



        public decimal? UnitPrice { get; set; }

        public short? Quantity { get; set; }

        public float? Discount { get; set; }



        public float? ProductSales { get; set; }

    }
}