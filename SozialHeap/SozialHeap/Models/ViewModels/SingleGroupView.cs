using SozialHeap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sozialheap.Models.ViewModels
{
    public class SingleGroupView
    {
        public int notifications { get; set; }
        public List<Post> notificationList { get; set; }
        public User currentUser { get; set; }
        public Group group { get; set; }
        public List<Post> postList { get; set; }
    }
}