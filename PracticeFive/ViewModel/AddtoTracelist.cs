using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PracticeFive.ViewModel
{
    public class AddtoTracelist
    {
        public int RescueID { get; set; }
        public int TransferID { get; set; }
        public int FollowMemberID { get; set; }
    }
    public class TracelistViewModel
    {
        public int RescueID { get; set; }
        public int TransferID { get; set; }
        public string RescueEvent { get; set; }
        public string TransferName { get; set; }
        public System.DateTime Created_At { get; set; }

    }
       

}