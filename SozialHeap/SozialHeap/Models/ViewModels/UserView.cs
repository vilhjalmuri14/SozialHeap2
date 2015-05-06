using SozialHeap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sozialheap.Models.ViewModels
{
    public class UserView
    {
        public bool following { get; set; }
        public int notifications { get; set; }
        public List<Post> notificationList { get; set; }
        public User currentUser { get; set; }
        public User user { get; set; }
        public List<Post> postList { get; set; }
        public List<User> userList { get; set; }
        public List<Group> groupList { get; set; }
        public List<Answer> topRatedAnswer { get; set; }
    }
}