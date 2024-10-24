using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TilausDBApp.ViewModels
{
    public class CategorySalesClass
    {
        public long rowid { get; set; }
        public string Nimi { get; set; }
        public Nullable<decimal> ProductSales { get; set; }

        public string WeekdayName { get; set; }
    }
}