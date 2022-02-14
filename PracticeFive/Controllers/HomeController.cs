using PracticeFive.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PracticeFive.Controllers
{
    public class HomeController : Controller
    {
        SaveAnimalsEntities sadb = new SaveAnimalsEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View("Index", "_IndexVer2"); // 套用 _IndexVer2.cshtml
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string MemberAccount, string MemberPassword)
        {
            // 依帳密取得會員並指定給member
            var member = sadb.tMember
                .Where(m => m.MemberAccount == MemberAccount && m.MemberPassword == MemberPassword)
                .FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member == null)
            {
                return Content("<script>alert('帳密錯誤');history.go(-1);</script>");
            }
            //使用Session變數記錄歡迎詞
            Session["UserID"] = member.MemberID;
            FormsAuthentication.RedirectFromLoginPage(MemberAccount, true);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(tMember pMember)
        {
            // 依帳密取得會員並指定給member
            var member = sadb.tMember
                .Where(m => m.MemberAccount == pMember.MemberAccount)
                .FirstOrDefault();
            //若member為null，表示會員未註冊
            if (member != null)
            {
                return Content("<script>alert('已有人註冊');history.go(-1);</script>");
            }

            pMember.Created_At = DateTime.Now;

            sadb.tMember.Add(pMember);
            sadb.SaveChanges();

            return RedirectToAction("Login", "Home");
        }
    }
}