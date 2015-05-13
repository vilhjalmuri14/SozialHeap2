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
        SozialService service = new SozialService(null);

        public ActionResult Index()
        {
            FrontPageView model = new FrontPageView();
            model.Groups = service.GetAllGroups().Take(5).ToList();
            model.Users = service.GetAllUsers();
            model.Posts = service.getRecentPosts();

            if (User.Identity.IsAuthenticated)
            {
                ViewBag.isLoggedIn = true;
                model.notificationList = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId()));
                ViewBag.notifications = model.notificationList.Count();
                User currUser = service.GetUserById(User.Identity.GetUserId());
                model.recentFromUsers = service.getRecentByFollowingUsers(User.Identity.GetUserId()).Take(5);
                model.recentGroups = service.getRecentFollowingGroups(User.Identity.GetUserId()).Take(5);
            }
            else
            {
                ViewBag.isLoggedIn = false;
               // model.recentFromUsers = new List<Post>();
               // model.recentGroups = new List<Group>();
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
        
        /// <summary>
        /// Search feature
        /// </summary>
        /// <param name="id">search string</param>
        /// <returns></returns>
        public ActionResult Search(string id)
        {
            if(id == null)
            { 
                return RedirectToAction("Index");
            }
            SearchResults model = new SearchResults();
            model.Posts = service.findPostByString(id);
            model.users = service.findUsersByString(id);
            model.groups = service.findGroupsByString(id);
            if(User.Identity.IsAuthenticated)
            {
                ViewBag.notifications = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId())).Count();
            }
            return View(model);
        }
    }
}