
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
    public class QuestionController : Controller
    {
        SozialService service = new SozialService();
        
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
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

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateAnswer([Bind(Include = "postID, title, body")]Answer form)
        {
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

        [Authorize]
        public ActionResult LikePost(int id)
        {
            Post post = service.getPost(id);
            User currentUser = service.GetUserById(User.Identity.GetUserId());
            service.LikePost(currentUser, post);

            return RedirectToAction("ViewQuestion/" + id);
        }

        [Authorize]
        public ActionResult UnLikePost(int id)
        {
            Post post = service.getPost(id);
            User currentUser = service.GetUserById(User.Identity.GetUserId());
            service.UnLikePost(currentUser, post);

            return RedirectToAction("ViewQuestion/" + id);
        }

        public ActionResult ViewQuestion(int? id)
        {
            if (id != null)
            {
                PostView model = new PostView();

                int new_id = id ?? default(int);
                model.currentPost = service.getPost(new_id);
                if (User.Identity.IsAuthenticated)
                {
                    User currentUser = service.GetUserById(User.Identity.GetUserId());
                    model.notificationList = service.getUnreadPostsByUser(currentUser);
                    model.LikedPost = service.DidUserLikePost(currentUser, model.currentPost);
                    ViewBag.notifications = model.notificationList.Count();
                }
                if (model.currentPost != null)
                {
                    model.answerList = service.GetAnswerById(model.currentPost.postID);
                    ViewBag.postID = (int)id;
                    ViewBag.timeSince = Utils.TimeSince(model.currentPost.dateCreated);
                    ViewBag.ansTime = new string[model.answerList.Count];
                    for (int i = 0; i < model.answerList.Count; i++ )
                    {
                        ViewBag.ansTime[i] = Utils.TimeSince(model.answerList[i].dateCreated);
                    }
                    return View(model);
                }
                return View("Error");
            }
            return View("Error");
        }

        public static string FormatTime(DateTime? date)
        {
            return Utils.TimeSince(date);
        }
    }
}