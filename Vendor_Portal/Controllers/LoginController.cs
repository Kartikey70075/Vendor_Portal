using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vendor_Portal.LinqToSql;

namespace Vendor_Portal.Controllers
{
    public class LoginController : Controller
    {
        public static class glbvar
        {
            public static string UserId { get; set; }
        }
        // GET: Login
        DataClasses1DataContext _db = new DataClasses1DataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(_USERCREATE_H obj)
        {
            try
            {
                var user = _db._USERCREATE_Hs.FirstOrDefault(a => a.U_userID.Equals(obj.U_userID));
                if (user != null)
                {
                    if (user.U_Wrong_Attempts != 3)
                    {
                        if (user.U_isActive != "No")
                        {
                            if (user.U_password != null)
                            {
                                if (obj.U_password != null)
                                {
                                    string decryptedPassword = EncryptionHelper.Decrypt(user.U_password);

                                    if (decryptedPassword.Equals(obj.U_password))
                                    {
                                        Session["UserName"] = user.U_userName.ToString();
                                        Session["UserId"] = user.U_userID.ToString();
                                        glbvar.UserId = user.U_userID.ToString();
                                        Session["Permit"] = "Employee";
                                        Session["UserTyp"] = user.U_userType.ToString();

                                        // Reset the wrong attempts on successful login
                                        var res = _db._USERCREATE_Hs.FirstOrDefault(u => u.DocEntry == user.DocEntry);
                                        if (res != null)
                                        {
                                            res.U_Wrong_Attempts = null;
                                            _db.SubmitChanges();
                                        }

                                        return RedirectToAction("MainDash", "Dashboard");
                                    }
                                    else
                                    {

                                        var res = _db._USERCREATE_Hs.FirstOrDefault(u => u.DocEntry == user.DocEntry);
                                        if (res != null)
                                        {
                                            res.U_Wrong_Attempts = res.U_Wrong_Attempts.HasValue ? res.U_Wrong_Attempts + 1 : 1;
                                            _db.SubmitChanges();
                                        }

                                        TempData["Vendor-Msg"] = "Password not correct! Please try again!!";
                                        return View();
                                    }
                                }
                                else
                                {
                                    TempData["Vendor-Msg"] = "Please Enter Password!!";
                                    return View();
                                }
                            }
                            else
                            {
                                TempData["Vendor-Msg"] = "Please Create Your Password from SAP!!";
                                return View();
                            }
                        }
                        else
                        {
                            TempData["Vendor-Msg"] = "User is Not Active !!";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Vendor-Msg"] = "The user has been blocked. Please contact the admin!!";
                        return View();
                    }
                }
                else
                {
                    TempData["Vendor-Msg"] = "User Not Found !!";
                    return View();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult AdminLogin(_USERCREATE_H obj)
        {
            try
            {
                var user = _db._USERCREATE_Hs.FirstOrDefault(a => a.U_userID.Equals(obj.U_userID));
                if (user != null)
                {
                    if (user.U_password != null)
                    {
                        if (obj.U_password != null)
                        {
                            if (user != null && user.U_password.Equals(obj.U_password))
                            {
                                Session["UserName"] = user.U_userName.ToString();
                                Session["UserId"] = user.U_userID.ToString();
                                Session["Permit"] = "Employee";
                                Session["UserTyp"] = user.U_userType.ToString();
                                return RedirectToAction("MainDash", "Dashboard");
                            }
                            else
                            {
                                TempData["Admin-msg"] = "Password not correct! Please try again!!";
                                return RedirectToAction("Login", "Login");
                            }
                        }
                        else
                        {
                            TempData["Admin-msg"] = "Please Enter Password!!";
                            return RedirectToAction("Login", "Login");
                        }
                    }
                    else
                    {
                        TempData["Admin-msg"] = "Please Create Your Password from SAP!!";
                        return RedirectToAction("Login", "Login");
                    }
                }
                else
                {
                    TempData["Admin-msg"] = "Vendor Not Found !!";
                    return RedirectToAction("Login", "Login");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            //FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");


        }

    }
}