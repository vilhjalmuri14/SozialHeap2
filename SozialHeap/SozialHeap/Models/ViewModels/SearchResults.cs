using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SozialHeap.Models.ViewModels
{
    public class SearchResults
    {
        public List<User> users { get; set; }
        public List<Post> Posts { get; set; }
    }
}