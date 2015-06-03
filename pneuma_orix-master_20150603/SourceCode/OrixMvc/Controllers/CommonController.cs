using OrixModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Data;

namespace OrixMvc.Controllers
{
    public class CommonController : Controller
    {
        public CommonController() 
        {

            //if (!checkLogin())
            //{
            //    Redirext();
            //}
            //else
            //{
            //    //ORIXContext _db = new ORIXContext();
            //    // //判斷有無程式權限
            //    // var query = from uf in _db.OR3_USER_FUNC
            //    //             where uf.USER_ID == ""
            //    //             select new string (FUNC_ID = "");


            //    // Redirect("~/Login.aspx");
            //}
        }

        public RedirectResult Redirext(string url) 
        {
            return Redirect(url);
        }

        public string checkLogin(string strFunc_Id) 
        {
            string url;
            if (
                System.Web.HttpContext.Current.Session == null || System.Web.HttpContext.Current.Session["CorpAcct"] == null ||
                System.Web.HttpContext.Current.Session["EmployeeId"] == null || System.Web.HttpContext.Current.Session["EmployeeName"] == null ||
                System.Web.HttpContext.Current.Session["ProgramId"] == null || System.Web.HttpContext.Current.Session["ProgramName"] == null) 
            {
                url = "~/Login.aspx";
                return url;
            }
            else
            {
                using (ORIXContext _db = new ORIXContext())
                {
                    string UserId = System.Web.HttpContext.Current.Session["UserId"].ToString();
                    //判斷有無程式權限
                    var query = ((from uf in _db.OR3_USER_FUNC where uf.USER_ID == UserId && uf.FUNC_ID == strFunc_Id select uf.FUNC_ID)
                                .Union(from uf in _db.OR3_USER_FUNC
                                       where (from Gid in _db.OR3_USERS where Gid.USER_ID == UserId select Gid.GROUP_ID).Contains(uf.USER_ID) && uf.FUNC_ID == strFunc_Id
                                       select uf.FUNC_ID));

                    if (query == null || query.Count() <= 0)
                    {
                        url = "~/NoAuthority.aspx";
                        return url;
                    }
                }
            }
            return "";
        }

        #region DropdownList
        //訪視類別
        [HttpPost]
        public ActionResult getTopic(string INTERVIEW_TOPIC)
        {
            try
            {
                IEnumerable<SelectListItem> Or3Topic;
                using (ORIXContext _db = new ORIXContext())
                {
                    Or3Topic = _db.OR3_COND_DEF.AsEnumerable()
                        .Where(x => x.TypeField == "INTERVIEW_TOPIC")
                        .Where(x => x.end_date == null)
                        .OrderBy(x => x.TypeCode)
                        .Select(x => new SelectListItem()
                        {
                            Value = x.TypeCode,
                            Text = x.TypeCode + ' ' + x.TypeDesc
                        });
                    if (INTERVIEW_TOPIC != "" && Or3Topic.Where(x => x.Value == INTERVIEW_TOPIC).Count() == 0)
                    {
                        Or3Topic = _db.OR3_COND_DEF.AsEnumerable()
                            .Where(x => x.TypeField == "INTERVIEW_TOPIC")
                            .Where(x => x.TypeCode == INTERVIEW_TOPIC)
                            .Select(x => new SelectListItem()
                            {
                                Value = x.TypeCode,
                                Text = x.TypeCode + ' ' + x.TypeDesc
                            })
                            .Union(Or3Topic);
                    }
                    Or3Topic = Or3Topic.OrderBy(x => x.Value).ToList();
                }
                var result = new SelectList(Or3Topic, "Value", "Text");
                return Json(new { Success = "Success", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = "Error:" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //租賃公會行業別
        public ActionResult getCustUnion()
        {
            try
            {
                IEnumerable<SelectListItem> Or3Zip;
                using (ORIXContext _db = new ORIXContext())
                {
                    Or3Zip = _db.OR_CUST_KIND.Where(x => x.CUST_FRC_TYPE_CODE.Substring(0, 1) != "0")
                             .Select(x => new SelectListItem()
                             {
                                 Value = x.CUST_FRC_TYPE_CODE,
                                 Text = x.CUST_FRC_TYPE_NAME
                             }).ToList();
                }
                var result = new SelectList(Or3Zip, "Value", "Text");
                return Json(new { Success = "Success", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = "Error:" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //行業別
        public ActionResult getCustTypeCode()
        {
            try
            {
                IEnumerable<SelectListItem> Or3Zip;
                using (ORIXContext _db = new ORIXContext())
                {
                    Or3Zip = _db.OR_CUST_KIND.Where(x => x.CUST_FRC_TYPE_CODE.Substring(0, 1) == "0")
                             .Select(x => new SelectListItem()
                             {
                                 Value = x.CUST_FRC_TYPE_CODE,
                                 Text = x.CUST_FRC_TYPE_NAME
                             }).ToList(); ;
                }
                var result = new SelectList(Or3Zip, "Value", "Text");
                return Json(new { Success = "Success", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = "Error:" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //縣市
        public ActionResult getCityCode()
        {
            try
            {
                IEnumerable<SelectListItem> Or3Zip;
                using (ORIXContext _db = new ORIXContext())
                {
                    Or3Zip = _db.OR3_Zip
                            .Where(x => x.City_Code != "")
                            .GroupBy(x => new { x.City_Code, x.City_Name },
                                            (key, group) => new { key1 = key.City_Code, key2 = key.City_Name })
                            .Select(x => new SelectListItem() { Value = x.key1, Text = x.key2 }).ToList();
                }
                var result = new SelectList(Or3Zip, "Value", "Text");
                return Json(new { Success = "Success", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = "Error:" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //國別
        public ActionResult getNationality()
        {
            try
            {
                IEnumerable<SelectListItem> Or3Zip;
                using (ORIXContext _db = new ORIXContext())
                {
                    Or3Zip = _db.OR_CUST_NAT
                             .Select(x => new SelectListItem()
                             {
                                 Value = x.NATIONALITY.Trim(),
                                 Text = x.NAT_NAME
                             }).ToList();
                }
                var result = new SelectList(Or3Zip, "Value", "Text");
                return Json(new { Success = "Success", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = "Error:" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //組織型態
        public ActionResult getOrgType()
        {
            try
            {
                IEnumerable<SelectListItem> Or3Zip;
                using (ORIXContext _db = new ORIXContext())
                {
                    Or3Zip = _db.s_ConditionItems("ORG_TYPE", "", "", "")
                             .Select(x => new SelectListItem()
                             {
                                 Value = x.TypeCode,
                                 Text = x.TypeCode + ' ' + x.TypeDesc
                             }).ToList();
                }
                var result = new SelectList(Or3Zip, "Value", "Text");
                return Json(new { Success = "Success", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = "Error:" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //發票開立方式
        public ActionResult getInvoice()
        {
            try
            {
                IEnumerable<SelectListItem> Or3Zip;
                using (ORIXContext _db = new ORIXContext())
                {
                    Or3Zip = _db.s_ConditionItems("INVOICE", "", "", "")
                             .Select(x => new SelectListItem()
                             {
                                 Value = x.TypeCode,
                                 Text = x.TypeDesc
                             }).ToList();
                }
                var result = new SelectList(Or3Zip, "Value", "Text");
                return Json(new { Success = "Success", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = "Error:" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //特殊客戶條件
        public ActionResult getSpecCond()
        {
            try
            {
                IEnumerable<SelectListItem> Or3Zip;
                using (ORIXContext _db = new ORIXContext())
                {
                    Or3Zip = _db.s_ConditionItems("SPEC_COND", "", "", "")
                             .Select(x => new SelectListItem()
                             {
                                 Value = x.TypeCode,
                                 Text = x.TypeCode + ' ' + x.TypeDesc
                             }).ToList();
                }
                var result = new SelectList(Or3Zip, "Value", "Text");
                return Json(new { Success = "Success", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = "Error:" + ex }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        //區域
        public ActionResult getZipCode(string CityCode)
        {
            try
            {
                IEnumerable<SelectListItem> Or3Zip;
                using (ORIXContext _db = new ORIXContext())
                {
                    Or3Zip = _db.OR3_Zip.AsEnumerable()
                        .Where(x => string.IsNullOrEmpty(CityCode) || x.City_Code == CityCode)
                        .Where(x => x.Zip_Code != null)
                        .OrderBy(x => x.Zip_Code)
                        .Select(x => new SelectListItem()
                        {
                            Value = x.Zip_Code,
                            Text = x.Zone_Name + ' ' + x.Zip_Code
                        }).ToList();
                }
                var result = new SelectList(Or3Zip, "Value", "Text");
                return Json(new { Success = "Success", Data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Success = "Error:" + ex }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion
    }
}
