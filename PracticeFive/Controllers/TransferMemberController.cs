using Newtonsoft.Json;
using PracticeFive.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticeFive.Controllers
{
    public class TransferMemberController : Controller
    {
        SaveAnimalsEntities sadb = new SaveAnimalsEntities();

        // GET: TransferMember
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Transfer()
        {
            var transferPositionCity = sadb.tPosition.Where(m => m.positionBelong == 0).ToList();
            ViewBag.City = new SelectList(transferPositionCity, "positionID", "positionPosition");

            var transferPositionCountry = sadb.tPosition.Where(m => m.positionBelong != 0).ToList();
            ViewBag.Country = new SelectList(transferPositionCountry, "positionID", "positionPosition");

            var transferSpecies = sadb.tSpecies.ToList();
            ViewBag.Species = new SelectList(transferSpecies, "SpeciesID", "SpeciesName");

            return View();
        }

        [HttpPost]
        public ActionResult Transfer(tTransfer pTransfer)
        {
            var transferPositionCity = sadb.tPosition.Where(m => m.positionBelong == 0).ToList();
            ViewBag.City = new SelectList(transferPositionCity, "positionID", "positionPosition");

            var transferPositionCountry = sadb.tPosition.Where(m => m.positionBelong != 0).ToList();
            ViewBag.Country = new SelectList(transferPositionCountry, "positionID", "positionPosition");

            var transferSpecies = sadb.tSpecies.ToList();
            ViewBag.Species = new SelectList(transferSpecies, "SpeciesID", "SpeciesName");

            //var selectList = new List<SelectListItem>()
            //        {
            //            new SelectListItem {Text="text-1", Value="value-1" },
            //            new SelectListItem {Text="text-2", Value="value-2" },
            //            new SelectListItem {Text="text-3", Value="value-3" },
            //        };

            ////預設選擇哪一筆
            //selectList.Where(q => q.Value == "value-2").First().Selected = true;

            //ViewBag.SelectList = selectList;


            pTransfer.TransferDone = Request.Form["selectorTransferDone"].ToString();
            pTransfer.TransferNeuter = Request.Form["selectorTransferNeuter"].ToString();
            pTransfer.TransferGender = Request.Form["selectorTransferGender"].ToString();
            pTransfer.TransferMemberID = Convert.ToInt32(Session["UserID"]);
            pTransfer.Created_At = DateTime.Now;

            if (pTransfer.upImg != null && pTransfer.upImg.ContentLength > 0)
            {
                var fileName = Path.GetFileName(pTransfer.upImg.FileName);
                var path = Path.Combine(Server.MapPath("~/UpImg"), fileName);
                pTransfer.upImg.SaveAs(path);
            }

            pTransfer.TransferPictures = pTransfer.upImg.FileName;
            //Debug.WriteLine(pTransfer.RescuePosition);
            //Debug.WriteLine(pTransfer.RescueSpecies);
            sadb.tTransfer.Add(pTransfer);
            sadb.SaveChanges();
            return RedirectToAction("List", "TransferMember");
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
            var transferPositionCity = sadb.tPosition.Where(m => m.positionBelong == 0).ToList();
            ViewBag.City = new SelectList(transferPositionCity, "positionID", "positionPosition");

            var transferPositionCountry = sadb.tPosition.Where(m => m.positionBelong != 0).ToList();
            ViewBag.Country = new SelectList(transferPositionCountry, "positionID", "positionPosition");

            var transferSpecies = sadb.tSpecies.ToList();
            ViewBag.Species = new SelectList(transferSpecies, "SpeciesID", "SpeciesName");

            tTransfer transferDetails = sadb.tTransfer.FirstOrDefault(p => p.TransferID == id);

            return View(transferDetails);
        }

        [HttpPost]
        public ActionResult Edit(tTransfer pTransfer)
        {
            var rescuePositionCity = sadb.tPosition.Where(m => m.positionBelong == 0).ToList();
            ViewBag.City = new SelectList(rescuePositionCity, "positionID", "positionPosition");

            var rescuePositionCountry = sadb.tPosition.Where(m => m.positionBelong != 0).ToList();
            ViewBag.Country = new SelectList(rescuePositionCountry, "positionID", "positionPosition");

            var rescueSpecies = sadb.tSpecies.ToList();
            ViewBag.Species = new SelectList(rescueSpecies, "SpeciesID", "SpeciesName");

            pTransfer.TransferDone = Request.Form["selectorTransferDone"].ToString();
            pTransfer.TransferNeuter = Request.Form["selectorTransferNeuter"].ToString();
            pTransfer.TransferGender = Request.Form["selectorTransferGender"].ToString();
            pTransfer.TransferMemberID = Convert.ToInt32(Session["UserID"]);
            pTransfer.Created_At = DateTime.Now;

            if (pTransfer.upImg != null && pTransfer.upImg.ContentLength > 0)
            {
                var fileName = Path.GetFileName(pTransfer.upImg.FileName);
                var path = Path.Combine(Server.MapPath("~/UpImg"), fileName);
                pTransfer.upImg.SaveAs(path);
                pTransfer.TransferPictures = pTransfer.upImg.FileName;
            }
            else
            {
                pTransfer.TransferPictures = pTransfer.TransferPictures;
            }

            sadb.Entry(pTransfer).State = System.Data.Entity.EntityState.Modified;
            sadb.SaveChanges();

            return RedirectToAction("List", "TransferMember");
        }


        public ActionResult List(string TransferSpeciesSearch/*string TransferSpeciesSearch*//*string RescueTitlesearch*/)
        {
            var SpeciesCatageory = from d in sadb.tSpecies select d.SpeciesName;
            ViewBag.SpeciesList = new List<string>(SpeciesCatageory);

            //var TransferSpecies = sadb.tSpecies.ToList();
            //ViewBag.Species = new SelectList(TransferSpecies, "SpeciesID", "SpeciesName");

            var TransferList = from m in sadb.tTransfer.OrderByDescending(sadb => sadb.Created_At) select m;

            if (!string.IsNullOrEmpty(TransferSpeciesSearch))
            {
                var SpeciesChooseID = sadb.tSpecies.FirstOrDefault(m => m.SpeciesName == TransferSpeciesSearch).SpeciesID;
                TransferList = TransferList.Where(x => x.TransferSpecies == SpeciesChooseID);
            }
            return View(TransferList);

        }


        public ActionResult More(int id)
        {
            tTransfer TransferDetails = sadb.tTransfer.FirstOrDefault(p => p.TransferID == id);

            if (TransferDetails == null)
            {
                return RedirectToAction("List", "TransferMember");
            }
            else
            {
                return View(TransferDetails);
            }
        }

        public ActionResult Delete(int id)
        {
            tTransfer transferDelete = sadb.tTransfer.FirstOrDefault(p => p.TransferID == id);
            if (transferDelete != null)
            {
                sadb.tTransfer.Remove(transferDelete);
                sadb.SaveChanges();
            }
            return RedirectToAction("List", "TransferMember");
        }
    }
}