using Sozialheap.Models;
using Sozialheap.Models.ViewModels;
using Sozialheap.Services;
using SozialHeap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sozialheap.Controllers
{
    public class GroupController : Controller
    {
        SozialService service = new SozialService();

        [HttpPost]
        public ActionResult CreateGroup([Bind(Include = "postID, title")]Group form)
        {
            if(form.groupName == "" || form.userID == "")
            {
                // Sensitive fields missing!
            }

            form.dateCreated = DateTime.Now;
            
            service.CreateGroup(form);

            return View("ViewGroup/"+form.groupID);
        }

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
            ViewBag.Notfications = 89;
        
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

                return View(v);
            }
        }
    }
}