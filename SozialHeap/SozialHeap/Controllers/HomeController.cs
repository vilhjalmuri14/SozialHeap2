using SozialHeap.Utils;
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

        /// <summary>
        /// First view of page
        /// </summary>
        /// <returns>Index view</returns>
        public ActionResult Index()
        {
            // start by logging the visit (statistcs)
            Utils.Utils.LogAction(User.Identity.GetUserName(), Request.UserHostAddress, "Home/Index");

            // create the model from ViewModel and fetch data
            FrontPageView model = new FrontPageView();
            model.Groups = service.GetAllGroups().Take(5).ToList();
            model.Users = service.GetAllUsers();
            model.Posts = service.getRecentPosts();

            if (User.Identity.IsAuthenticated)
            {
                // user logged int
                ViewBag.isLoggedIn = true;
                model.notificationList = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId()));
                ViewBag.notifications = model.notificationList.Count();
                User currUser = service.GetUserById(User.Identity.GetUserId());
                model.recentFromUsers = service.getRecentByFollowingUsers(User.Identity.GetUserId()).Take(5);
                model.recentGroups = service.getRecentFollowingGroups(User.Identity.GetUserId()).Take(5);
            }
            else
            {
                // user not logged in
                ViewBag.isLoggedIn = false;
                model.recentFromUsers = new List<Post>();
                model.recentGroups = new List<Post>();
            }
            return View(model);
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

        /// <summary>
        /// Search feature, accessible from all pages of the system.
        /// </summary>
        /// <param name="term">search string</param>
        /// <returns>JSON list of all words found</returns>
        public ActionResult FindTopic(string term)
        {
            //get the wordlist
            var users = Utils.Utils.getKeywords(term);

            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}