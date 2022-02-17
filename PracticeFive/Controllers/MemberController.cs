using PracticeFive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PracticeFive.Controllers
{
    public class MemberController : Controller
    {
        SaveAnimalsEntities sadb = new SaveAnimalsEntities();
        // GET: Member
        public ActionResult Index()
        {
            return View("Index", "_IndexMember");
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Profilo(int? id)
        {
            id = Convert.ToInt32(Session["UserID"]);
            if (id == null)
            {
                return RedirectToAction("Login", "Home");
            }
            else 
            {
                tMember profilo = sadb.tMember.FirstOrDefault(p => p.MemberID == id);
                return View(profilo);
            }
        }
        public ActionResult MemberEdit(int id)
        {
            tMember profiloedit = sadb.tMember.FirstOrDefault(p => p.MemberID == id);
            return View(profiloedit);
        }

        [HttpPost]
        public ActionResult MemberEdit(tMember pMember)
        {
            string oldpassword= Request.Form["oldpassword"];
            string newpassword1= Request.Form["newpassword1"];
            string newpassword2= Request.Form["newpassword2"];
            string oldpasswordsql = pMember.MemberPassword;
            if (oldpassword == oldpasswordsql)
            {
                if (newpassword1 == newpassword2)
                {
                    pMember.MemberPassword = Request.Form["newpassword1"];
                    pMember.MemberID = Convert.ToInt32(Session["UserID"]);
                    pMember.Created_At = DateTime.Now;
                    sadb.Entry(pMember).State = System.Data.Entity.EntityState.Modified;
                    sadb.SaveChanges();
                    return Content("<script>alert('修改成功！請重新登入！');window.location.href='/Home/Login';</script>");
                }
                else
                {
                    return Content("<script>alert('兩次新密碼不同');history.go(-1);</script>");
                }
            }
            else
            {
                return Content("<script>alert('舊密碼輸入錯誤');history.go(-1);</script>");
            }

        }

    }
}