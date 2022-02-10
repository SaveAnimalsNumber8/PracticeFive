using Newtonsoft.Json;
using PracticeFive.Models;
using System;
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

            if (pRescue.upImg != null && pRescue.upImg.ContentLength > 0)
            {
                var fileName = Path.GetFileName(pRescue.upImg.FileName);
                var path = Path.Combine(Server.MapPath("~/UpImg"), fileName);
                pRescue.upImg.SaveAs(path);
            }

            pRescue.RescuePictures = pRescue.upImg.FileName;
            //Debug.WriteLine(pRescue.RescuePosition);
            //Debug.WriteLine(pRescue.RescueSpecies);
            sadb.tRescue.Add(pRescue);
            sadb.SaveChanges();
            return View();
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
                var fileName = Path.GetFileName(pRescue.upImg.FileName);
                var path = Path.Combine(Server.MapPath("~/UpImg"), fileName);
                pRescue.upImg.SaveAs(path);
                pRescue.RescuePictures = pRescue.upImg.FileName;            
            }
            else
            {
                pRescue.RescuePictures = pRescue.RescuePictures;
            }

            sadb.Entry(pRescue).State = System.Data.Entity.EntityState.Modified;
            sadb.SaveChanges();

            return RedirectToAction("List", "RescueMember");
        }


        public ActionResult List()
        {
            var rescueList = sadb.tRescue.OrderByDescending(sadb => sadb.Created_At).ToList();
            return View(rescueList);
        }


        public ActionResult More(int id)
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
    }
}