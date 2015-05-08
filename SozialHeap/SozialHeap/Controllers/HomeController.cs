using Sozialheap.Services;
using SozialHeap.Models;
using SozialHeap.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SozialHeap.Controllers
{
    public class HomeController : Controller
    {
        SozialService service = new SozialService();

        public ActionResult Index()
        {
            FrontPageView model = new FrontPageView();
            model.Groups = service.GetAllGroups();
            model.Users = service.GetAllUsers();
            model.Posts = service.getRecentPosts();

            return View(model);
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