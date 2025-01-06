using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vendor_Portal.LinqToSql;

namespace Vendor_Portal.Controllers
{
    public class WebAPIController : ApiController
    {
        DataClasses1DataContext _db = new DataClasses1DataContext();
        [HttpGet]
        public IHttpActionResult InitData()
        {
            try
            {
                var Itmgrp = _db.OITBs
                  .Select(item => new { item.ItmsGrpCod, item.ItmsGrpNam })
                  .OrderBy(item => item.ItmsGrpCod)
                  .ToList();
                var branch = _db.OBPLs
                .Select(item => new { item.BPLName, item.BPLId })
                .ToList();
                
                var Dep = (from o in _db.OUDPs
                           select new { o.Code, o.Name }).ToList();

                var Result = new
                {
                    Itmgrp,
                    branch,
                    Dep
                };
                return Ok(Result);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        public IHttpActionResult GetVendor()
        {
            try
            {
                var Vendor = (from o in _db.OCRDs
                              where o.CardType == 'S'
                              select new
                              {
                                  CardCode = o.CardCode,
                                  CardName = o.CardName
                              }).ToList();
                var VendorGRPO = (from opor in _db.OPORs
                                  where opor.DocStatus == 'O'
                                  select new { opor.CardName, opor.CardCode })
             .Distinct()
             .ToList();
                var Result = new
                {
                    Vendor,
                    VendorGRPO
                };
                return Ok(Result);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        public IHttpActionResult GetEmp()
        {
            try
            {
                var Emp = (from employee in _db.OHEMs
                            join department in _db.OUDPs
                            on employee.dept equals department.Code
                            select new
                            {
                                EmpName = employee.firstName + " " + (employee.middleName ?? "") + " " + (employee.lastName ?? ""),
                                EmpID=employee.Code,
                                EmpDepCode=employee.dept,
                                EmpDepName=department.Name
                            }).ToList();

                var Result = new
                {
                    Emp
                };
                return Ok(Result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public IHttpActionResult AllVendor()
        {
            try
            {
                var Allvendor1 = (from opor in _db.OPORs
                                  select new
                                  {
                                      opor.DocEntry,
                                      opor.DocNum,
                                      opor.DocStatus,
                                      opor.DocDate,
                                      opor.DocDueDate,
                                      opor.CardCode,
                                      opor.CardName,
                                      opor.Address,
                                      opor.DocCur,
                                      opor.DocRate,
                                      opor.DocTotal,
                                      opor.WddStatus
                                  }).Distinct().ToList();
                var Allvendor = Allvendor1.AsEnumerable()
                                          .Select(x => new
                                          {
                                              x.DocEntry,
                                              x.DocNum,
                                              x.DocStatus,
                                              DocDate = x.DocDate.HasValue ? x.DocDate.Value.ToString("dd/MM/yyyy") : "",
                                              DocDueDate = x.DocDueDate.HasValue ? x.DocDueDate.Value.ToString("dd/MM/yyyy") : "",
                                              x.CardCode,
                                              x.CardName,
                                              x.Address,
                                              x.DocCur,
                                              x.DocRate,
                                              x.DocTotal,
                                              x.WddStatus
                                          }).ToList();
                var Result = new
                {
                    Allvendor
                };
                return Ok(Result);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        public IHttpActionResult VendorRowDtl(int DocNum, string CardCode, char DocStatus)
        {
            try
            {
                var AllVendorRow1 = from T0 in _db.OPORs
                                    join T1 in _db.POR1s on T0.DocEntry equals T1.DocEntry
                                    where T0.DocStatus == DocStatus
                                          && T0.CardCode == CardCode
                                          && T0.DocNum == DocNum
                                          && T1.OpenQty != 0
                                    select new
                                    {
                                        PONumber = T0.DocNum,
                                        DocEntry = T0.DocEntry,
                                        LineNumber = T1.LineNum,
                                        PODate = T0.DocDate,
                                        ItemCode = T1.ItemCode,
                                        ItemName = T1.Dscription,
                                        WarehouseCode = T1.WhsCode,
                                        Quantity = T1.Quantity,
                                        BalanceQty = T1.OpenQty,
                                        U_QCStatus = T1.U_QCStatus
                                    };
                var AllVendorRow = AllVendorRow1.AsEnumerable()
                                          .Select(x => new
                                          {
                                              x.PONumber,
                                              x.DocEntry,
                                              x.LineNumber,
                                              PODate = x.PODate.HasValue ? x.PODate.Value.ToString("dd/MM/yyyy") : "",
                                              x.ItemCode,
                                              x.ItemName,
                                              x.WarehouseCode,
                                              x.Quantity,
                                              x.BalanceQty,
                                              x.U_QCStatus
                                          }).ToList();
                var distinctResult = AllVendorRow.Distinct().ToList();
                var Result = new
                {
                    AllVendorRow
                };
                return Ok(Result);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public IHttpActionResult GetassHed(string UserType)
        {
            try
            {
                var Head = _db._USERCREATE_Hs
                .Where(u => u.U_userType == "Admin" || u.U_userType == "Manager")
                .Select(u => new { u.U_userName, u.U_userID }).ToList();
                
                return Ok(Head);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        public IHttpActionResult AllVendorAging()
        {
            try
            {
                var Agine = _db.Agine().ToList();
                var result = new
                {
                    Agine
                };
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
