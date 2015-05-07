using Sozialheap.Models;
using Sozialheap.Models.ViewModels;
using Sozialheap.Services;
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
    }
}