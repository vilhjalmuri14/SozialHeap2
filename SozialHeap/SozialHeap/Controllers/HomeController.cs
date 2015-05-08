using Sozialheap.Models.ViewModels;
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
            FeedView v = new FeedView();
            return View("Index");
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