using PracticeFive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticeFive.Controllers
{
    public class RescueTraceListController : Controller
    {
        SaveAnimalsEntities sadb = new SaveAnimalsEntities();
        // GET: TraceListMember
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Tracelist()
        {
            var trackList = sadb.FollowRescue.AsEnumerable()
                   .Where(x => x.FollowMemberID == Convert.ToInt32(Session["UserID"]))
                   .ToList();
            return View(trackList);
        }
        public ActionResult DeleteFollow(int id)
        {
            FollowRescue followDelete = sadb.FollowRescue.FirstOrDefault(p => p.FollowID == id);
            if (followDelete != null)
            {
                sadb.FollowRescue.Remove(followDelete);
                sadb.SaveChanges();
            }
            return RedirectToAction("TraceList");
        }
    }
}