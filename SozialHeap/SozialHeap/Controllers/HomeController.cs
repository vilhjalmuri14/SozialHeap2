using Sozialheap.Services;
using SozialHeap.Models;
using SozialHeap.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SozialHeap.Controllers
{
    public class HomeController : Controller
    {
        SozialService service = new SozialService();

        public ActionResult Index()
        {
            FrontPageView model = new FrontPageView();
            model.Groups = service.GetAllGroups().Take(5).ToList();
            model.Users = service.GetAllUsers();
            model.Posts = service.getRecentPosts();
            if (User.Identity.IsAuthenticated)
            {

                model.notificationList = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId()));
                ViewBag.notifications = model.notificationList.Count();
            }
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
        
        
        public ActionResult Search(string id)
        {
            if(id == null)
            { 
                return RedirectToAction("Index");
            }
            SearchResults model = new SearchResults();
            model.Posts = service.findPostByString(id);
            model.users = service.findUsersByString(id);
            if(User.Identity.IsAuthenticated)
            {
                ViewBag.notifications = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId())).Count();
            }
            return View(model);

        }
    }
}