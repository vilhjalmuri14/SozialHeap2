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
        SozialService service = new SozialService(null);
        
        /// <summary>
        /// Webservice to create a question
        /// </summary>
        /// <param name="form">form of Post</param>
        /// <returns>view of the Group that holds the Question/Post</returns>
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateQuestion([Bind(Include = "groupID,categoryID,name,body")]Post form)
        {
            var group = service.GetAllGroups();
            if (form.groupID < 1 || form.name == null || form.body == null)
            {
                ViewBag.Message = "You cannot create Question without a title or question!";
                return View("QuestionHelper");
            }

            // We have valid input, lets insert
            form.scoreCounter = 0;
            // form.PostCategory = 1;
            form.dateCreated = DateTime.Now;
            // ApplicationUser u = User.Identity.
            form.userID = User.Identity.GetUserId();
            form.viewCount = 0;

            service.CreatePost(form);

            return RedirectToAction("ViewGroup/"+form.groupID, "Group", "");
        }

        /// <summary>
        /// Webservice to create Answer
        /// </summary>
        /// <param name="form">Answer object from form</param>
        /// <returns>view of question</returns>
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult CreateAnswer([Bind(Include = "postID, title, body")]Answer form)
        {
            if (form.postID < 1 || form.title == null || form.body == null)
            {
                // input is not valid, return errormessage
                ViewBag.Message = "You cannot create Answer without a title or body!";
                return View("QuestionHelper");
            }

            // We have valid input, lets insert
            form.scoreCounter = 0;
            form.seenByOwner = false;
            form.userID = User.Identity.GetUserId();
            form.dateCreated = DateTime.Now;
            service.CreateAnswer(form);

            return RedirectToAction("ViewQuestion/"+form.postID, "Question", form.postID);
        }

        /// <summary>
        /// Webservice to like post
        /// </summary>
        /// <param name="id">id of post</param>
        /// <returns>view post/question</returns>
        [Authorize]
        public ActionResult LikePost(int id)
        {
            Post post = service.getPost(id);
            User currentUser = service.GetUserById(User.Identity.GetUserId());
            if (post != null && currentUser != null && post.Users.Contains(currentUser) == false)
            {
                service.LikePost(currentUser, post);
            }

            return RedirectToAction("ViewQuestion/" + id);
        }

        [Authorize]
        public ActionResult LikePostAjax(int id)
        {
            Post post = service.getPost(id);
            User currentUser = service.GetUserById(User.Identity.GetUserId());
            if (post != null && currentUser != null && post.Users.Contains(currentUser) == false)
            {
                
                service.LikePost(currentUser, post);
                return Content(post.Users.Count().ToString(), "text/plain");
            }
            return Content("can't like", "text/plain");
   }

        /// <summary>
        /// Webservice to unlike post
        /// </summary>
        /// <param name="id">postID to unlike</param>
        /// <returns>view of post/question</returns>
        [Authorize]
        public ActionResult UnLikePost(int id)
        {
            Post post = service.getPost(id);
            User currentUser = service.GetUserById(User.Identity.GetUserId());
            if (post != null && currentUser != null && post.Users.Contains(currentUser) == true)
            {
                service.UnLikePost(currentUser, post);
            }
            return RedirectToAction("ViewQuestion/" + id);
        }

        [Authorize]
        public ActionResult UnLikePostAjax(int id)
        {
            Post post = service.getPost(id);
            User currentUser = service.GetUserById(User.Identity.GetUserId());
            if (post != null && currentUser != null && post.Users.Contains(currentUser) == true)
            {
                service.UnLikePost(currentUser, post);
                return Content(post.Users.Count().ToString(), "text/plain");
            }
            return Content("can't unlike", "text/plain");
        }

        /// <summary>
        /// View question by desired questionID
        /// </summary>
        /// <param name="id">questionID</param>
        /// <returns>view of the question</returns>
        public ActionResult ViewQuestion(int? id)
        {
            if (id != null)
            {
                
                PostView model = new PostView();

                int new_id = id ?? default(int);
                model.currentPost = service.getPost(new_id);
                
                // insert statistics
                Utils.LogAction(User.Identity.GetUserName(), Request.UserHostAddress, "ViewQuestion/" + new_id);

                if (User.Identity.IsAuthenticated)
                {
                    // set user information for logged in user
                    User currentUser = service.GetUserById(User.Identity.GetUserId());
                    if(currentUser == model.currentPost.User)
                    {
                        service.AcknowledgeNotifications(model.currentPost);
                    }
                    model.notificationList = service.getUnreadPostsByUser(currentUser);
                    model.LikedPost = service.DidUserLikePost(currentUser, model.currentPost);
                    ViewBag.notifications = model.notificationList.Count();
                }
                if (model.currentPost != null)
                {
                    // post found
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
                ViewBag.Message = "Question not found";
                return View("Error");
            }
            ViewBag.Message = "You must select a valid question.";
            return View("Error");
        }

    }
}