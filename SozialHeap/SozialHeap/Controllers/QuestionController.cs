using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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
    public class QuestionController : Controller
    {
        SozialService service = new SozialService();
        
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateQuestion([Bind(Include = "groupID,categoryID,name,body")]Post form)
        {
            var group = service.GetAllGroups();
            if (form.groupID < 1 || form.name == "" || form.body == "")
            {
                ViewBag.Message = "You cannot create Question without a title or question!";
                return View();
            }
            
            // We have valid input, lets insert
            form.scoreCounter = 0;
//            form.PostCategory = 1;
            form.dateCreated = DateTime.Now;
           // ApplicationUser u = User.Identity.
            form.userID = User.Identity.GetUserId();
            form.viewCount = 0;

            service.CreatePost(form);

            return RedirectToAction("ViewGroup/"+form.groupID, "Group", "");
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateAnswer([Bind(Include = "postID, title, body")]Answer form)
        {
            var group = service.GetAllGroups();
            if (form.postID < 1 || form.title == "" || form.body == "")
            {
                ViewBag.Message = "You cannot create Answer without a title or body!";
                return View();
            }

            // We have valid input, lets insert
            form.scoreCounter = 0;
            form.seenByOwner = false;
            form.userID = User.Identity.GetUserId();
            form.dateCreated = DateTime.Now;
            service.CreateAnswer(form);

            return RedirectToAction("ViewQuestion/"+form.postID, "Question", form.postID);
        }

        [HttpGet]
        public ActionResult LikePost(int id, string username)
        {
            service.LikePost(id, username);

            return Json("1", JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewQuestion(int? id)
        {
            if (id != null)
            {
                PostView model = new PostView();

                int new_id = id ?? default(int);
                model.currentPost = service.getPost(new_id);
               
                if (model.currentPost != null)
                {
                    model.answerList = service.GetAnswerById(model.currentPost.postID);
                    ViewBag.postID = (int)id;
                    return View(model);
                }
                return View("Error");
            }
            return View("Error");
        }

    }
}