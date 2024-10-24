using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TilausDBApp.ViewModels
{
    public class DailyProductSales

    {
        public long rowid { get; set; }
        public string Nimi { get; set; }
        public string Tilauspvm { get; set; }

        public string WeekdayName { get; set; }

    }
}