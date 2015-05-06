using SozialHeap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SozialHeap.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            SozialheapEntities db = new SozialheapEntities();

            var groups = (from item in db.Users
                          select item).ToList();

            foreach(var item in groups)
            {
                var kl = item;
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}