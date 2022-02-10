using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeFive.ViewModel
{
    public class AddRescueComment
    {
        public int RescueID { get; set; }
        public int CommentMemberID { get; set; }
        public string CommentContent { get; set; }
    }
}