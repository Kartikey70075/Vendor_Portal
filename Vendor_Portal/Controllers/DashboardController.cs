using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vendor_Portal.LinqToSql;
using Vendor_Portal.Models;
using System.Collections;
namespace Vendor_Portal.Controllers
{
    public class DashboardController : Controller
    {
        DataClasses1DataContext _db = new DataClasses1DataContext();
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult MainDash()
        {
            if (Session.Count != 0)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpGet]
        public ActionResult GetOPO()
        {
            if (Session.Count != 0)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        [HttpGet]
        public JsonResult GetOPOData(string v)
        {
            try
            {
                var query = from t0 in _db.OPORs
                            join t1 in _db.POR1s on t0.DocEntry equals t1.DocEntry
                            where t0.DocStatus.Equals("O") && t0.CardCode.Equals(Session["UserId"].ToString())
                            select new
                            {
                                DocNum = t0.DocNum,
                                DocEntry = t0.DocEntry,
                                LineNum = t1.LineNum,
                                DocDate = t0.DocDate,
                                ItemCode = t1.ItemCode,
                                Dscription = t1.Dscription,
                                WhsCode = t1.WhsCode,
                                Quantity = t1.Quantity,
                                OpenQty = t1.OpenQty
                            };

                var formattedQuery = query.AsEnumerable()
                                          .Select(x => new
                                          {
                                              x.DocNum,
                                              x.DocEntry,
                                              x.LineNum,
                                              DocDate = x.DocDate.HasValue ? x.DocDate.Value.ToString("dd/MM/yyyy") : "",
                                              x.ItemCode,
                                              x.Dscription,
                                              x.WhsCode,
                                              x.Quantity,
                                              x.OpenQty
                                          }).ToList();


                var distinctQuery = formattedQuery.Distinct().ToList();
                return Json(distinctQuery, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        public ActionResult UserCreate()
        {
            if (Session.Count != 0)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult ListOfUser()
        {
            if (Session.Count != 0)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
        public ActionResult AgingData()
        {
            if (Session.Count != 0)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpPost]
        public string UserAdd(User obj)
        {
            try
            {
                int a = 1;
                int docEntries = _db._USERCREATE_Hs
                    .OrderByDescending(u => u.DocEntry)
                    .Select(u => u.DocEntry)
                    .FirstOrDefault();

                if (obj.U_userType != "Vendor" && obj.U_userType != "Employee")
                {
                    obj.U_userID = CreateUserId(docEntries, obj.U_userType);
                }

                int lDoc = docEntries == 0 ? 1 : docEntries + 1;
                int hour = DateTime.Now.Hour;
                int minute = DateTime.Now.Minute;
                int timeInMinutes = hour * 100 + minute;
                short? timeToStore = (short?)timeInMinutes;
                string encryptedPassword = EncryptionHelper.Encrypt(obj.U_password);
                var userEntity_H = new _USERCREATE_H
                {
                    DocEntry = lDoc,
                    DocNum = lDoc,
                    U_userType = obj.U_userType,
                    U_userName = obj.U_userName,
                    U_userID = obj.U_userID,
                    U_password = encryptedPassword,
                    U_BranchID = obj.U_BranchID,
                    U_department = obj.U_department,
                    U_isActive = obj.U_isActive,
                    U_assignHead = obj.U_assignHead,
                    U_assignHeadName = obj.U_assignHeadName,
                    U_CreatorRemarks = obj.U_CreatorRemarks,
                    U_assignHead2 = obj.U_assignHead2,
                    U_AssignHeadName2 = obj.U_AssignHeadName2,
                    U_departmentID = obj.U_departmentID,
                    U_BranchName = obj.U_BranchName,
                    CreateDate = DateTime.Now,
                    CreateTime = timeToStore
                };
                _db._USERCREATE_Hs.InsertOnSubmit(userEntity_H);
                if (obj.User_R != null)
                {
                    foreach (var i in obj.User_R)
                    {
                        var userEntity_R = new _USERCREATE_R
                        {
                            DocEntry = lDoc,
                            LineId = a++,
                            U_ItmsGrpCod = i.ItmsGrpCod,
                            U_ItmsGrpNam = i.ItmsGrpNam
                        };
                        _db._USERCREATE_Rs.InsertOnSubmit(userEntity_R);
                    }
                }
                _db.SubmitChanges();
                return "Ok";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string CreateUserId(int docEntriesm,string Usertype)
        {
            string u = string.Empty;  
            string formattedDocEntriesm = docEntriesm.ToString("D4");

            if (Usertype== "Admin")
            {

                u = "ADM" + formattedDocEntriesm;
                return u;
            }
            else if (Usertype == "Manager")
            {
                u = "MNG" + formattedDocEntriesm;
                return u;
            }
            else if (Usertype == "Employee")
            {
                u = "EMP" + formattedDocEntriesm;
                return u;
            }
            else
            {
                return u;    
            }
        }
        [HttpPost]
        public string UserUpdate(User obj)
        {
            try
            {
                var userEntity_H = _db._USERCREATE_Hs
                    .FirstOrDefault(u => u.DocEntry == obj.DocEntry);

                if (userEntity_H != null)
                {
                    if (obj.U_userType=="Vendor")
                    {
                        userEntity_H.U_userID = obj.U_userID;
                    }
                    string encryptedPassword = EncryptionHelper.Encrypt(obj.U_password);
                    userEntity_H.U_userType = obj.U_userType;
                    userEntity_H.U_userName = obj.U_userName;
                    userEntity_H.U_password = encryptedPassword;
                    userEntity_H.U_BranchID = obj.U_BranchID;
                    userEntity_H.U_department = obj.U_department;
                    userEntity_H.U_isActive = obj.U_isActive;
                    userEntity_H.U_assignHead = obj.U_assignHead;
                    userEntity_H.U_assignHeadName = obj.U_assignHeadName;
                    userEntity_H.U_CreatorRemarks = obj.U_CreatorRemarks;
                    userEntity_H.U_AssignHeadName2 = obj.U_AssignHeadName2;
                    userEntity_H.U_assignHead2 = obj.U_assignHead2;
                    userEntity_H.U_Wrong_Attempts = obj.U_Wrong_Attempts;
                    userEntity_H.U_departmentID = obj.U_departmentID;
                }

                if (obj.User_R != null)
                {
                    var existingUsers = _db._USERCREATE_Rs
                        .Where(u => u.DocEntry == obj.DocEntry).ToList();

                    if (existingUsers.Any())
                    {
                        _db._USERCREATE_Rs.DeleteAllOnSubmit(existingUsers);
                    }

                    int lineId = 1; 
                    foreach (var i in obj.User_R)
                    {
                        var userEntity_R = new _USERCREATE_R
                        {
                            DocEntry = obj.DocEntry,
                            LineId = lineId++,
                            U_ItmsGrpCod = i.ItmsGrpCod,
                            U_ItmsGrpNam = i.ItmsGrpNam
                        };
                        _db._USERCREATE_Rs.InsertOnSubmit(userEntity_R);
                    }
                }

                _db.SubmitChanges();
                return "Update";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        [HttpGet]
        public JsonResult Find()
        {
            try
            {
                var result = (from u in _db._USERCREATE_Hs
                              orderby u.DocEntry descending
                              select new
                              {
                                  u.DocEntry,
                                  u.U_userType,
                                  u.U_userID,
                                  u.U_userName,
                                  u.U_password,
                                  u.U_BranchID,
                                  u.U_department,
                                  u.U_isActive,
                                  u.U_assignHead,
                                  u.U_CreatorRemarks,
                                  u.U_assignHeadName,
                                  u.U_assignHead2,
                                  u.U_AssignHeadName2,
                                  u.U_Wrong_Attempts,
                                  u.U_departmentID,
                                  u.U_BranchName
                              }).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult FindRow(int DocEntry, string U_password)
        {
            try
            {
                string Decpass = EncryptionHelper.Decrypt(U_password);
                var result = _db._USERCREATE_Rs
                    .Where(u => u.DocEntry == DocEntry)
                    .ToList();
                var allResult = new
                {
                    result,
                    Decpass
                };
                return Json(allResult, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Message = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public string CheckVendor(string UserId)
        {
            try
            {
                var result = (from user in _db._USERCREATE_Hs
                             where user.U_userID == UserId
                              select user).ToList();
                if (result.Count!=0)
                {
                    return "NotOk";
                }
                else
                {
                    return "Ok";
                }
                
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public ActionResult  Vendor_Details()
        {
            if (Session.Count != 0)
            {

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Login");
            }
        }
    }
}