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
using SozialHeap.Utils;

namespace Sozialheap.Controllers
{
    public class GroupController : Controller
    {
        SozialService service = new SozialService(null);

        /// <summary>
        /// Create a new group
        /// </summary>
        /// <param name="form">form with group information</param>
        /// <returns>view the group, or helper if it fails</returns>
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
            
            // creation
            service.CreateGroup(form);
            
            // check if it worked, if groupID is less then 1 then it failed.
            if(form.groupID > 0)
            {
                return RedirectToAction("ViewGroup/" + form.groupID, "Group", "");
            }
            else
            {
                ViewBag.Message = "Could not create group, check if its name already exists!";
                return View("GroupHelper");
            }
        }

        /// <summary>
        /// Edit a given group
        /// </summary>
        /// <param name="form">form filled group information</param>
        /// <returns>view the group or errormessage</returns>
        [Authorize]
        [HttpPost]
        public ActionResult EditGroup([Bind(Include = "groupID, description, photo")]Group form)
        {
            if (form.groupID < 1 || form.description == null)
            {
                // Sensitive fields missing!
                ViewBag.Message = "Your update request was missing sensitive data!";
                return View("Error");
            }

            Group actualGroup = service.GetGroupById(form.groupID);
            if(actualGroup.userID != User.Identity.GetUserId())
            {
                ViewBag.Message = "You must be the group owner to change its profile!";
                return View("Error");
            }

            // update
            service.EditGroup(form);

            return RedirectToAction("ViewGroup/" + form.groupID, "Group");
            
        }

        /// <summary>
        /// Get view of the given group
        /// </summary>
        /// <param name="id">groupID you want to view</param>
        /// <returns>view of requested group</returns>
        public ActionResult ViewGroup(int? id)
        {
            
            SingleGroupView v = new SingleGroupView();
            
            if(!id.HasValue)
            {
                // no id provided
                ViewBag.Message = "No id given on requested group.";
                return View("Error");
            }
            else
            {
                Utils.LogAction(User.Identity.GetUserName(), Request.UserHostAddress, "ViewGroup/" + (int)id);
                v.group = service.GetGroupById((int)id);
                if(v.group == null)
                {
                    ViewBag.Message = "Invalid group request!";
                    return View("Error");
                }
                ViewBag.groupID = (int)id;
                
                v.notificationList = null;
                v.group.Users = v.group.Users.OrderByDescending(x=>x.score).ToList();
                v.postList = service.getPosts((int)id);
                //v.group.Users = service.GetUsersByGroup(v.group);
                if (User.Identity.IsAuthenticated)
                {
                    User currentUser = service.GetUserById(User.Identity.GetUserId());
                    v.currentUser = currentUser;

                    if(v.group.userID == User.Identity.GetUserId())
                    {
                        // Owner! can edit
                        ViewBag.isOwner = true;
                        ViewBag.groupID = v.group.groupID;
                        ViewBag.groupDescription = v.group.description;
                        ViewBag.groupPhoto = v.group.photo;
                    }
                    v.following = service.isFollowingGroup(service.GetUserById(User.Identity.GetUserId()), v.group);
                    v.notificationList = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId()));
                    ViewBag.notifications = v.notificationList.Count();
                }
                return View(v);
            }
        }

        /// <summary>
        /// Get list of all groups
        /// </summary>
        /// <returns>List view of all groups</returns>
        public ActionResult Index()
        {
            Utils.LogAction(User.Identity.GetUserName(), Request.UserHostAddress, "Group");
            AllGroupView model = new AllGroupView();
            model.groupList = service.GetAllGroups();
            if (User.Identity.IsAuthenticated)
            {

                model.notificationList = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId()));
                ViewBag.notifications = model.notificationList.Count();
            }
            return View(model);
        }

        /// <summary>
        /// Webservice to start follow Group
        /// </summary>
        /// <param name="id">groupID of the Group you want to follow</param>
        /// <returns>view of the Group</returns>
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
        public ActionResult StartFollowingAjax(int id)
        {
            Group group = service.GetGroupById(id);
            if (group != null)
            {
                User currentUser = service.GetUserById(User.Identity.GetUserId());
                service.StartFollowingGroup(currentUser, group);

                return Content("followed", "text/plain");

            }
            return Content("can't follow", "text/plain");

        }

        /// <summary>
        /// Webservice to stop follow Group
        /// </summary>
        /// <param name="id">groupID to stop follow</param>
        /// <returns>view of the group</returns>
        [Authorize]
        public ActionResult StopFollowing(int id)
        {
            Group group = service.GetGroupById(id);
            User currentUser = service.GetUserById(User.Identity.GetUserId());
            service.StopFollowingGroup(currentUser, group);

            return RedirectToAction("ViewGroup/" + id);
        }

        [Authorize]
        public ActionResult StopFollowingAjax(int id)
        {
            Group group = service.GetGroupById(id);
            User currentUser = service.GetUserById(User.Identity.GetUserId());
            service.StopFollowingGroup(currentUser, group);
            return Content("unfollowed", "text/plain");

        }
        /// <summary>
        /// Get view of particular user
        /// </summary>
        /// <param name="id">id of desired user</param>
        /// <returns>view of the user</returns>
        public ActionResult ViewUsers(int? id)
        {
            if (!id.HasValue)
            {
                ViewBag.Message = "No id given on requested group.";
                return View("Error");
            }
            else
            {
                var group = service.GetGroupById((int)id);
                if (group == null)
                {
                    ViewBag.Message = "Invalid group request!";
                    return View("Error");
                }
                if (User.Identity.IsAuthenticated == true)
                {
                    ViewBag.notifications = service.getUnreadPostsByUser(service.GetUserById(User.Identity.GetUserId())).Count();
                }

                return View(group);
            }
        }
    }
}