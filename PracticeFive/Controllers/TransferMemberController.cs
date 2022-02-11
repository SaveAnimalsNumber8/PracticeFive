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

    }
}