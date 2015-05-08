using Sozialheap.Models.ViewModels;
using Sozialheap.Models;
using Sozialheap.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Sozialheap.Controllers
{
    public class UserController : Controller
    {
        SozialService service = new SozialService();

        
        public ActionResult ViewUser(string id)
        {
            UserView model = new UserView();

            model.user = service.GetUserByUsername(id);
            model.postList = service.getPostbyId(model.user.userID);
            model.following = service.isFollowing(service.GetUserById(User.Identity.GetUserId()), model.user);
           
            return View(model);
        }

        public ActionResult Feed()
        {
            // TODO: create the view!!

            UserView model = new UserView();
            model.groupList = service.GetAllGroups();

            return View(model);
        }

        public ActionResult UserFeed()
        {
            return View();
        }

        public ActionResult UserQuery(string term)
        {
            var users = service.GetUsersByQuery(term);

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult StartFollowing(string id)
        {
            SozialHeap.Models.User userToFollow = service.GetUserByUsername(id);
            SozialHeap.Models.User currentUser = service.GetUserById(User.Identity.GetUserId());
            service.StartFollowingUser(currentUser, userToFollow);

            return RedirectToAction("ViewUser/" + userToFollow.userName);
        }

        [Authorize]
        public ActionResult StopFollowing(string id)
        {
            SozialHeap.Models.User userToStopFollow = service.GetUserByUsername(id);
            SozialHeap.Models.User currentUser = service.GetUserById(User.Identity.GetUserId());
            service.StopFollowingUser(currentUser, userToStopFollow);

            return RedirectToAction("ViewUser/" + userToStopFollow.userName);
        }
    }
}
