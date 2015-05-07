using Sozialheap.Models.ViewModels;
using Sozialheap.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        public ActionResult UserQuery(string q)
        {
            var users = service.GetAllUsers().ToList();

            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }
}
