using PracticeFive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PracticeFive.Controllers
{
    public class TransferController : Controller
    {
        // GET: Transfer
        SaveAnimalsEntities sadb = new SaveAnimalsEntities();

        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
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
                return RedirectToAction("List", "Transfer");
            }
            else
            {
                return View(TransferDetails);
            }
        }
    }
}