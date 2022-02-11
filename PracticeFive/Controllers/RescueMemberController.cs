using Newtonsoft.Json;
using PracticeFive.Models;
using PracticeFive.ViewModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticeFive.Controllers
{
    public class RescueMemberController : Controller
    {
        SaveAnimalsEntities sadb = new SaveAnimalsEntities();

        // GET: Rescue
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Member");
        }


        public ActionResult Rescue()
        {
            var rescuePositionCity = sadb.tPosition.Where(m => m.positionBelong == 0).ToList();
            ViewBag.City = new SelectList(rescuePositionCity, "positionID", "positionPosition");
            
            var rescuePositionCountry = sadb.tPosition.Where(m => m.positionBelong != 0).ToList();
            ViewBag.Country = new SelectList(rescuePositionCountry, "positionID", "positionPosition");

            var rescueSpecies = sadb.tSpecies.ToList();
            ViewBag.Species = new SelectList(rescueSpecies, "SpeciesID", "SpeciesName");

            return View();
        }

        [HttpPost]
        public ActionResult Rescue(tRescue pRescue)
        {
            var rescuePositionCity = sadb.tPosition.Where(m => m.positionBelong == 0).ToList();
            ViewBag.City = new SelectList(rescuePositionCity, "positionID", "positionPosition");

            var rescuePositionCountry = sadb.tPosition.Where(m => m.positionBelong != 0).ToList();
            ViewBag.Country = new SelectList(rescuePositionCountry, "positionID", "positionPosition");

            var rescueSpecies = sadb.tSpecies.ToList();
            ViewBag.Species = new SelectList(rescueSpecies, "SpeciesID", "SpeciesName");

            //var selectList = new List<SelectListItem>()
            //        {
            //            new SelectListItem {Text="text-1", Value="value-1" },
            //            new SelectListItem {Text="text-2", Value="value-2" },
            //            new SelectListItem {Text="text-3", Value="value-3" },
            //        };

            ////預設選擇哪一筆
            //selectList.Where(q => q.Value == "value-2").First().Selected = true;

            //ViewBag.SelectList = selectList;


            pRescue.RescueTitle = Request.Form["selectorTitle"];
            pRescue.ResCueDone = Request.Form["selectorResCueDone"];
            pRescue.RescueMemberID = Convert.ToInt32(Session["UserID"]);
            pRescue.Created_At = DateTime.Now;

            string fileName = "";
            if (pRescue.upImg != null && pRescue.upImg.ContentLength > 0)
            {
                fileName = DateTime.Now.ToString("yyMMdd") + pRescue.upImg.FileName;
                var path = Path.Combine(Server.MapPath("~/UpImg"), fileName);
                pRescue.upImg.SaveAs(path);
            }
            pRescue.RescuePictures = fileName;
            //Debug.WriteLine(pRescue.RescuePosition);
            //Debug.WriteLine(pRescue.RescueSpecies);
            sadb.tRescue.Add(pRescue);
            sadb.SaveChanges();
            return RedirectToAction("List", "RescueMember");
        }

        [HttpPost]
        public ActionResult getCountry(string id)
        {
            var City = Convert.ToInt32(id);
            List<SelectListItem> list = (from p in sadb.tPosition
                                         where p.positionBelong == City
                                         select new SelectListItem
                                         {
                                             Value = p.positionID.ToString(),
                                             Text = p.positionPosition
                                         }).ToList();

            string result = string.Empty;

            if (list == null)
            {
                return Json(result);
            }
            else
            {
                result = JsonConvert.SerializeObject(list);
                return Json(result);
            }
        }

        public ActionResult Edit(int id)
        {
            var rescuePositionCity = sadb.tPosition.Where(m => m.positionBelong == 0).ToList();
            ViewBag.City = new SelectList(rescuePositionCity, "positionID", "positionPosition");

            var rescuePositionCountry = sadb.tPosition.Where(m => m.positionBelong != 0).ToList();
            ViewBag.Country = new SelectList(rescuePositionCountry, "positionID", "positionPosition");

            var rescueSpecies = sadb.tSpecies.ToList();
            ViewBag.Species = new SelectList(rescueSpecies, "SpeciesID", "SpeciesName");

            tRescue rescueDetails = sadb.tRescue.FirstOrDefault(p => p.RescueID == id);

            return View(rescueDetails);
        }

        [HttpPost]
        public ActionResult Edit(tRescue pRescue)
        {
            var rescuePositionCity = sadb.tPosition.Where(m => m.positionBelong == 0).ToList();
            ViewBag.City = new SelectList(rescuePositionCity, "positionID", "positionPosition");

            var rescuePositionCountry = sadb.tPosition.Where(m => m.positionBelong != 0).ToList();
            ViewBag.Country = new SelectList(rescuePositionCountry, "positionID", "positionPosition");

            var rescueSpecies = sadb.tSpecies.ToList();
            ViewBag.Species = new SelectList(rescueSpecies, "SpeciesID", "SpeciesName");

            pRescue.RescueTitle = Request.Form["selector"].ToString();
            pRescue.RescueMemberID = Convert.ToInt32(Session["UserID"]);
            pRescue.Created_At = DateTime.Now;

            if (pRescue.upImg != null && pRescue.upImg.ContentLength > 0)
            {
                string fileName = "";
                fileName = DateTime.Now.ToString("yyMMdd") + pRescue.upImg.FileName;
                var path = Path.Combine(Server.MapPath("~/UpImg"), fileName);
                pRescue.upImg.SaveAs(path);
                pRescue.RescuePictures = fileName;
            }
            else
            {
                pRescue.RescuePictures = pRescue.RescuePictures;
            }

            sadb.Entry(pRescue).State = System.Data.Entity.EntityState.Modified;
            sadb.SaveChanges();

            return RedirectToAction("List", "RescueMember");
        }


        public ActionResult List(string RescueTitlesearch)
        {
            var rescueTitleCatageory = from d in sadb.tRescue select d.RescueTitle;
            var rescueTitleList = new List<string>();

            rescueTitleList.AddRange(rescueTitleCatageory.Distinct());
            ViewBag.rescueTitleList = rescueTitleList;


            var rescueList = from m in sadb.tRescue.OrderByDescending(sadb => sadb.Created_At) select m;

            if (!string.IsNullOrEmpty(RescueTitlesearch))
            {
                rescueList = rescueList.Where(x => x.RescueTitle == RescueTitlesearch);
            }
            return View(rescueList);
        }


        public ActionResult More(int? id)
        {
            tRescue rescueDetails = sadb.tRescue.FirstOrDefault(p => p.RescueID == id);

            if (rescueDetails == null)
            {
                return RedirectToAction("List", "Rescue");
            }
            else
            {
                return View(rescueDetails);
            }
        }

        public ActionResult Delete(int id)
        {
            tRescue rescueDelete = sadb.tRescue.FirstOrDefault(p => p.RescueID == id);
            if (rescueDelete != null)
            {
                sadb.tRescue.Remove(rescueDelete);
                sadb.SaveChanges();
            }
            return RedirectToAction("List", "RescueMember");
        }

        [HttpPost]
        public ActionResult AddRescueComment(AddRescueComment comment)
        {
            tRescue rescuecomment = sadb.tRescue.FirstOrDefault(p => p.RescueID == comment.RescueID);

            if (rescuecomment != null)
            {
                tComment RescueComment = new tComment();
                RescueComment.CommentContent = Request.Form["Content"];
                RescueComment.CommentMemberID = Convert.ToInt32(Session["UserID"]);
                RescueComment.CommentRescueID = rescuecomment.RescueID;
                RescueComment.Created_At = DateTime.Now;
                sadb.tComment.Add(RescueComment);
                sadb.SaveChanges();
            }
            return RedirectToAction("More", "RescueMember");
            //TODO:無法顯示回原本的這筆More
        }
        public ActionResult AddtoFollowrescue(FollowRescue member,int id)
        {
            tRescue rescue = sadb.tRescue.FirstOrDefault(p => p.RescueID == id);
       
            if (rescue != null)
            {
                sadb.FollowRescue.Add(new FollowRescue() { FollowMemberID = Convert.ToInt32(Session["UserID"]), FollowResueID = id });
            }
            sadb.SaveChanges();
            return RedirectToAction("List", "RescueMember");
        }
        public ActionResult Rescuelist()
        {
            var Rescuelist = sadb.tRescue.AsEnumerable()
                   .Where(x => x.RescueMemberID == Convert.ToInt32(Session["UserID"]))
                   .ToList();
            return View(Rescuelist);
        }
    }
}