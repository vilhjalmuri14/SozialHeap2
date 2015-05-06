using Sozialheap.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SozialHeap.Models.ViewModels
{
    public class NarrowUser
    {
        public string userId { get; set; }
        public string userName { get; set; }

        public NarrowUser(string uid, string un)
        {
            userId = uid;
            userName = un;
        }
    }

    
}