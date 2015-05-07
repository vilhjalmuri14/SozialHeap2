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
        public ActionResult CreateQuestion()
        {
            var group = service.GetAllGroups();

            ViewBag.Message = "Just testing :)";

            return View();
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
                    return View(model);
                }
                return View("Error");
            }
            return View("Error");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PostQuestion([Bind(Include = "groupID,userID,name,body")] Post p)
        {
            if(p.body == "" || p.name == "")
            {
                // no nulls with body or name
                p.userID = User.Identity.ToString();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
    }
}