using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SozialHeap.Models.ViewModels
{
    public class AllUsersView
    {
        public int notifications { get; set; }
        public List<Post> notificationList { get; set; }
        public User currentUser { get; set; }
        public List<User> usersList { get; set; }
    }
}