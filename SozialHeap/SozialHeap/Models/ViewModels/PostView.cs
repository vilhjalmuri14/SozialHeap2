using SozialHeap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sozialheap.Models.ViewModels
{
    public class PostView
    {
        public int notfications { get; set; }

        public List<Post> notificationList { get; set; }

        public User currentUser { get; set; }

        public Post currentPost { get; set; }
        public List<Answer> answerList { get; set; }
    }
}