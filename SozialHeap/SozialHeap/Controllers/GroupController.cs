using Sozialheap.Models;
using Sozialheap.Models.ViewModels;
using Sozialheap.Services;
using SozialHeap.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Sozialheap.Controllers
{
    public class GroupController : Controller
    {
        SozialService service = new SozialService();

        [Authorize]
        [HttpPost]
        public ActionResult CreateGroup([Bind(Include = "groupName, description")]Group form)
        {
            if(form.groupName == null)
            {
                // Sensitive fields missing!
                ViewBag.Message = "You tried to post a nameless group!";
                return View("GroupHelper");
            }
            form.userID = User.Identity.GetUserId();
            form.dateCreated = DateTime.Now;
            
            service.CreateGroup(form);
            if(form.groupID > 0)
            {
                return View("~/Group/ViewGroup/" + form.groupID);

            }
            else
            {
                ViewBag.Message = "Could not create group, check if its name already exists!";
                return View("GroupHelper");
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditGroup([Bind(Include = "groupID, postID, title")]Group form)
        {
            if (form.groupName == null || form.userID == null)
            {
                // Sensitive fields missing!
                return View("Error");
            }

            service.EditGroup(form);

            return View("ViewGroup/" + form.groupID);
        }

        public ActionResult GetGroup(int? id)
        {
            int new_id = id ?? default(int);
            SingleGroupView model = new SingleGroupView();

            model.group = service.GetGroupById(new_id);
            if (User.Identity.IsAuthenticated)
            {
                model.notificationList = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId()));
                ViewBag.notifications = model.notificationList.Count();
            }
        
            return View(model);
        }

        public ActionResult ViewGroup(int? id)
        {
            SingleGroupView v = new SingleGroupView();
            
            if(!id.HasValue)
            {
                ViewBag.Message = "No id given on requested group.";
                return View("Error");
            }
            else
            {
                v.group = service.GetGroupById((int)id);
                if(v.group == null)
                {
                    ViewBag.Message = "Invalid group request!";
                    return View("Error");
                }
                ViewBag.groupID = (int)id;
                v.notifications = 01;
                v.notificationList = null;
                v.group.Users = v.group.Users.OrderByDescending(x=>x.score).ToList();
                v.postList = service.getPosts((int)id);
                //v.group.Users = service.GetUsersByGroup(v.group);
                if (User.Identity.IsAuthenticated)
                {
                    v.following = service.isFollowingGroup(service.GetUserById(User.Identity.GetUserId()), v.group);
                    v.notificationList = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId()));
                    ViewBag.notifications = v.notificationList.Count();
                }
                return View(v);
            }
        }

        public ActionResult Index()
        {
            AllGroupView model = new AllGroupView();
            model.groupList = service.GetAllGroups();
            if (User.Identity.IsAuthenticated)
            {

                model.notificationList = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId()));
                ViewBag.notifications = model.notificationList.Count();
            }
            return View(model);
        }

        [Authorize]
        public ActionResult StartFollowing(int id)
        {
            Group group = service.GetGroupById(id);
            if(group != null)
            {
                User currentUser = service.GetUserById(User.Identity.GetUserId());
                service.StartFollowingGroup(currentUser, group);

                return RedirectToAction("ViewGroup/" + id);
            }
            return View("GroupHelper");
        }

        [Authorize]
        public ActionResult StopFollowing(int id)
        {
            Group group = service.GetGroupById(id);
            User currentUser = service.GetUserById(User.Identity.GetUserId());
            service.StopFollowingGroup(currentUser, group);

            return RedirectToAction("ViewGroup/" + id);
        }
    }
}