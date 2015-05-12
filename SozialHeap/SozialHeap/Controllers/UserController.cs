﻿using Sozialheap.Models.ViewModels;
using Sozialheap.Models;
using Sozialheap.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SozialHeap.Models;

namespace Sozialheap.Controllers
{
    public class UserController : Controller
    {
        SozialService service = new SozialService();

        
        public ActionResult ViewUser(string id)
        {
            UserView model = new UserView();

            model.user = service.GetUserByUsername(id);
            if(model.user == null)
            {
                ViewBag.Message = "Requested user does not exists";
                return View("UserHelper");
            }
            model.user.Users1 = model.user.Users1.OrderByDescending(x => x.score).ToList();
            model.postList = service.getPostbyUserId(model.user.userID);
            model.following = service.isFollowingUser(service.GetUserById(User.Identity.GetUserId()), model.user);
            if (User.Identity.IsAuthenticated)
            {
                User currUser = service.GetUserById(User.Identity.GetUserId());
                model.notificationList = service.getUnreadPostsByUser(currUser);
                ViewBag.notifications = model.notificationList.Count();
                if(model.user == currUser)
                {
                    // Current user, display notifications!
                    ViewBag.isThisUser = true;
                    // mark all posts as read
                    //service.AcknowledgeNotifications(currUser);

                }
                else
                {
                    ViewBag.isThisUser = false;
                }
                ViewBag.isLoggedIn = true;
            }
            else
            {
                ViewBag.isLoggedIn = false;
            }
            return View(model);
        }

        public ActionResult Feed()
        {
            // TODO: create the view!!

            UserView model = new UserView();
            model.groupList = service.GetAllGroups();
            if (User.Identity.IsAuthenticated)
            {
                model.notificationList = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId()));
                ViewBag.notifications = model.notificationList.Count();
            }
            return View(model);
        }

        public ActionResult Index()
        {
            UserView model = new UserView();

            model.userList = service.GetAllUsers();
            ViewBag.Site = "Index";

            return View(model);
        }

        public ActionResult UserQuery(string term)
        {
            //var users = service.GetUsersByQuery(term);
            var users = service.getKeywords(term);

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult StartFollowing(string id)
        {
            SozialHeap.Models.User userToFollow = service.GetUserByUsername(id);
            SozialHeap.Models.User currentUser = service.GetUserById(User.Identity.GetUserId());
            if (currentUser != userToFollow)
            {
                service.StartFollowingUser(currentUser, userToFollow);
            }
            return RedirectToAction("ViewUser/" + userToFollow.userName);
        }

        [Authorize]
        public ActionResult StopFollowing(string id)
        {
            SozialHeap.Models.User userToStopFollow = service.GetUserByUsername(id);
            SozialHeap.Models.User currentUser = service.GetUserById(User.Identity.GetUserId());
            if (userToStopFollow != currentUser)
            {
                service.StopFollowingUser(currentUser, userToStopFollow);
            }
            return RedirectToAction("ViewUser/" + userToStopFollow.userName);
        }

        public ActionResult ShowFollowing(string id)
        {
            UserView model = new UserView();
            model.user = service.GetUserByUsername(id);
            ViewBag.Site = "Following";

            return View("index", model);
        }

        public ActionResult ShowFollowers(string id)
        {
            UserView model = new UserView();
            model.user = service.GetUserByUsername(id);
            ViewBag.Site = "Followers";

            return View("index", model);
        }
    }
}
