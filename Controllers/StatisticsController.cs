using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TilausDBApp.Models;
using TilausDBApp.ViewModels;

namespace TilausDBApp.Controllers
{
    public class StatisticsController : Controller
    {
        private TilausDBEntities1 db = new TilausDBEntities1();
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CategorySales()
        {
           
            string categoryNameList;
            string categorySalesList;
            List<CategorySalesClass> CategorySalesList = new List<CategorySalesClass>();

            var categorySalesData = from cs in db.ProductSales10Best_forAllTimes
                                    select cs;

            foreach (ProductSales10Best_forAllTimes product10 in categorySalesData)
            {
                CategorySalesClass OneSalesRow = new CategorySalesClass();
                OneSalesRow.Nimi = product10.Nimi;
                OneSalesRow.ProductSales = (int)product10.ProductSales;
                CategorySalesList.Add(OneSalesRow);
            }

            categoryNameList = "'" + string.Join("','", CategorySalesList.Select(n => n.Nimi).ToList()) + "'";
            categorySalesList = string.Join(",", CategorySalesList.Select(n => n.ProductSales).ToList());

            ViewBag.tuoteNimi = categoryNameList;
            ViewBag.tuoteProductSales = categorySalesList;


            return View();
        }



        private string GetFinnishWeekday(DateTime date)
        {
            var culture = new System.Globalization.CultureInfo("fi-FI");
            return culture.DateTimeFormat.GetDayName(date.DayOfWeek);
        }

        public ActionResult _ProductSalesPerDate(string tuoteNimi)

        {

            //if (String.IsNullOrEmpty(productName)) productName = "Lakkalikööri";  //debuggausta varten



            List<DailyProductSales> dailyproductsalesList = new List<DailyProductSales>();



            var orderSummary = from pds in db.ProductsDailySales

                               where pds.Nimi == tuoteNimi

                               orderby pds.Tilauspvm



                               select new DailyProductSales

                               {

                                   Tilauspvm = SqlFunctions.DateName("year", pds.Tilauspvm) + "." + SqlFunctions.DateName("MM", pds.Tilauspvm) + "." + SqlFunctions.DateName("day", pds.Tilauspvm),


                                   WeekdayName = pds.Tilauspvm.HasValue ? GetFinnishWeekday(pds.Tilauspvm.Value) : string.Empty,
                                   //DailySales = (float)pds.DailySales,

                                   Nimi = pds.Nimi

                               };

            return Json(orderSummary, JsonRequestBehavior.AllowGet);

        }
    }
}