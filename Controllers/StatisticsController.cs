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


        public ActionResult ViikonpaivaTilaukset()

        {

            string viikonPva = "";

            string kappalemaara = "";



            List<ViikonpaivaTilaukset> vkopvmTilaukset = new List<ViikonpaivaTilaukset>();



            using (var db = new TilausDBEntities1())

            {



                var tilauspvdata = (from tpd in db.ViikonpaivaTilaukset

                                    select tpd).ToList();



                foreach (ViikonpaivaTilaukset vkopmvTilaukset in tilauspvdata)

                {

                    ViikonpaivaTilaukset YksiTilausRivi = new ViikonpaivaTilaukset();

                    YksiTilausRivi.Viikonpaiva = vkopmvTilaukset.Viikonpaiva;

                    YksiTilausRivi.TilaustenMaara = (int)vkopmvTilaukset.TilaustenMaara;

                    vkopvmTilaukset.Add(YksiTilausRivi);

                }

                db.Database.Connection.Close();

            }



            viikonPva = "'" + string.Join("','", vkopvmTilaukset.Select(n => n.Viikonpaiva).ToList()) + "'";

            kappalemaara = string.Join(",", vkopvmTilaukset.Select(n => n.TilaustenMaara.ToString()).ToList());



            ViewBag.viikonPaiva = viikonPva;

            ViewBag.kappaleMaara = kappalemaara;



            return View();

        }

    }
}