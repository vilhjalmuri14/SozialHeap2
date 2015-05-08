using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SozialHeap.Models.ViewModels
{
    public class SimpleUser
    {
        public string userName{ get; set; }
        public string value { get; set; }

        public SimpleUser(string un, string uid)
        {
            userName = un;
            value = uid;
        }
    }
}