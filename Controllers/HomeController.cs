using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TilausDBApp.Models;

namespace TilausDBApp.Controllers
{
    public class HomeController : Controller
    {

        //public partial class Logins
        //{
        //    public int LoginId { get; set; }
        //    [Required(ErrorMessage = "Anna käyttäjätunnus!")]
        //    public string UserName { get; set; }
        //    [DataType(DataType.Password)]
        //    [Required(ErrorMessage = "Anna salasana!")]
        //    public string PassWord { get; set; } KOMMEEENTTTTTTTTTTTTTT
        //    public string LoginErrorMessage { get; set; }
  
        //}

        public ActionResult Index()
        {
            TilausDBEntities1 db = new TilausDBEntities1();
                try
                {
                    if (Session["UserName"] == null)
                    {
                        ViewBag.LoggedStatus = "OUT";
                    }
                    else ViewBag.LoggedStatus = "IN";
                }
                finally
                {
                    db.Dispose();
                }
                return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            TilausDBEntities1 db = new TilausDBEntities1();
            try
            {
                if (Session["UserName"] == null)
                {
                    ViewBag.LoggedStatus = "OUT";
                }
                else ViewBag.LoggedStatus = "IN";
            }
            finally
            {
                db.Dispose();
            }
            return View();

        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            TilausDBEntities1 db = new TilausDBEntities1();
            try
            {
                if (Session["UserName"] == null)
                {
                    ViewBag.LoggedStatus = "OUT";
                }
                else ViewBag.LoggedStatus = "IN";
            }
            finally
            {
                db.Dispose();
            }
            return View();
        }

        public ActionResult Login()

        {

            TilausDBEntities1 db = new TilausDBEntities1();
            try
            {
                if (Session["UserName"] == null)
                {
                    ViewBag.LoggedStatus = "OUT";
                }
                else ViewBag.LoggedStatus = "IN";
            }
            finally
            {
                db.Dispose();
            }
            return View();

        }



        [HttpPost]

        public ActionResult Authorize(Logins LoginModel)

        {

            TilausDBEntities1 db = new TilausDBEntities1();
            



            //Haetaan käyttäjän/Loginin tiedot annetuilla tunnustiedoilla tietokannasta LINQ -kyselyllä

            var LoggedUser = db.Logins.SingleOrDefault(x => x.UserName == LoginModel.UserName && x.PassWord == LoginModel.PassWord);



            if (LoggedUser != null)

            {

                ViewBag.LoginMessage = "Successfull login";

                ViewBag.LoggedStatus = "In";

                Session["UserName"] = LoggedUser.UserName;

                return RedirectToAction("Index", "Home"); //Tässä määritellään mihin onnistunut kirjautuminen johtaa --> Home/Index

            }

            else

            {

                ViewBag.LoginMessage = "Login unsuccessfull";

                ViewBag.LoggedStatus = "Out";


                //LoginModel.LoginErrorMessage = "Tuntematon käyttäjätunnus tai salasana.";

                return View("Login", LoginModel);

            }

        }



        public ActionResult LogOut()

        {

            Session.Abandon();

            ViewBag.LoggedStatus = "Out";

            return RedirectToAction("Index", "Home"); //Uloskirjautumisen jälkeen pääsivulle

        }
    }
}