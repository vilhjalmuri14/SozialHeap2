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

        public ActionResult UserQuery(string userName)
        {
            var users = service.GetUsersByQuery(userName);

            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult StartFollowing(string userName)
        {
            SozialHeap.Models.User userToFollow = service.GetUserByUsername(userName);
            SozialHeap.Models.User currentUser = service.GetUserById(User.Identity.GetUserId());
            return null;
        }
    }
}
