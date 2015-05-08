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
            if(form.groupName == "")
            {
                // Sensitive fields missing!
                return RedirectToAction("Index");
            }
            form.userID = User.Identity.GetUserId();
            form.dateCreated = DateTime.Now;
            
            service.CreateGroup(form);

            return View("~/ViewGroup/"+form.groupID);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditGroup([Bind(Include = "groupID, postID, title")]Group form)
        {
            if (form.groupName == "" || form.userID == "")
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
                return RedirectToAction("Error");
            }
            else
            {
                ViewBag.groupID = (int)id;
                v.notifications = 01;
                v.notificationList = null;
                v.postList = service.getPosts((int)id);
                v.group = service.GetGroupById((int)id);
                v.group.Users = service.GetUsersByGroup((int)id, 1);
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
            User currentUser = service.GetUserById(User.Identity.GetUserId());
            service.StartFollowingGroup(currentUser, group);

            return RedirectToAction("ViewGroup/" + id);
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