using SozialHeap.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SozialHeap.Models.ViewModels
{
    public class FrontPageView
    {
        public List<Group> Groups { get; set; }
        public List<User> Users { get; set; }
        public List<Post> Posts { get; set; }
        public List<Post> notificationList { get; set; }
        public int notifications { get; set; }
    }
}