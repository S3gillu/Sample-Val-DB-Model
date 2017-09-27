using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using SampleValDBModel.Models;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace SampleValDBModel.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using (LoginDBContext db = new LoginDBContext())
            {
                return View(db.UserAccounts.ToList());
            }
        }
       
        public ActionResult Register()
        {
            return View();
        }
       /* public ActionResult Register(SampleValDBModel.Models.UserAccount model)
        {
            if (string.IsNullOrEmpty(model.UserName))
            {
                ModelState.AddModelError("UserName", "UserName is required");
            }
            if (string.IsNullOrEmpty(model.FirstName))
            {
                ModelState.AddModelError("FirstName", "FirstName is required");
            }
            if (string.IsNullOrEmpty(model.LastName))
            {
                ModelState.AddModelError("LastName", "LastName is required");
            }
            if (!string.IsNullOrEmpty(model.Email))
            {
                string emailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                                         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
                Regex re = new Regex(emailRegex);
                if (!re.IsMatch(model.Email))
                {
                    ModelState.AddModelError("Email", "Email is not valid");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Email is required");
            }
            

            if (ModelState.IsValid)
            {
                ViewBag.Name = model.UserName;
                ViewBag.Age = model.FirstName;
                ViewBag.Address = model.LastName;
                ViewBag.Email = model.Email;  
                ViewBag.Contact = model.DOB;
            }
            return View(model);
        }*/

















        [HttpPost]
       public ActionResult Register(UserAccount user)
        {
            if (ModelState.IsValid)
            {
                using (LoginDBContext db = new LoginDBContext())
                {
                    var get_user = db.UserAccounts.FirstOrDefault(p => p.UserName == user.UserName);
                    if (get_user == null)
                    {
                        user.Password = AESCryptography.Encrypt(user.Password);
                        user.ConfirmPassword = AESCryptography.Encrypt(user.ConfirmPassword);
                        db.UserAccounts.Add(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        ViewBag.Message = "UserName already exists" + user.UserName;
                        return View();
                    }
                }
                ModelState.Clear();
                ViewBag.Message = "Successfully Registered Mr. " +
                user.FirstName + " " + user.LastName;
            }
            return RedirectToAction("Login");
        }

        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserAccount user)
        {
            using (LoginDBContext db = new LoginDBContext())
            {
                var get_user = db.UserAccounts.Single(p => p.UserName == user.UserName
                && p.Password == user.Password);
                if (get_user != null)
                {
                    Session["UserId"] = get_user.UserID.ToString();
                    Session["UserName"] = get_user.UserName.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "UserName or Password does not match.");
                }

            }
            return View();
        }
        
        public ActionResult LoggedIn()
        {
            object obj = Session["UserId"];
            if (obj != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }

        }
        public ActionResult GetData()
        {
            using (LoginDBContext db = new LoginDBContext())
            {
                var NewUserData = db.UserAccounts.OrderBy(a => a.UserName).ToList();
                return Json(new { data = NewUserData }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}

    