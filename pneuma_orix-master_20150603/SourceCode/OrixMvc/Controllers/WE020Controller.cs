using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrixModel;
using OrixMvc.Service;
using OrixMvc.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace OrixMvc.Controllers
{
    public class WE020Controller : Controller
    {
        private string strFunc_Id = "WE020";
        private string pageDetail = "WE020";
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(WE010Controller));

        public ActionResult Index(bool? isFromReturn)
        {
            TimeSpan t1 = new TimeSpan(DateTime.Now.Ticks);
            var PCUR_STS = new List<SelectListItem>();
            var PCASE_SOUR = new List<SelectListItem>() { new SelectListItem() { Text = "－請選擇－",Value = "" } };

            string oldCUR_STSSelected = "";
            string ordCASE_SOURSelected = "";

            //If return
            if (isFromReturn.HasValue && Session["conditions"] != null)
            {
                if (isFromReturn.Value) 
                {
                    oldCUR_STSSelected = (((WE020VO)Session["conditions"]).PCUR_STS != null) ? ((WE020VO)Session["conditions"]).PCUR_STS.Trim() : "";
                    ordCASE_SOURSelected = (((WE020VO)Session["conditions"]).PCASE_SOUR != null) ? ((WE020VO)Session["conditions"]).PCASE_SOUR.Trim() : "";
                }
 
            }

            if (!isFromReturn.HasValue)
            { 
                Session["results"] = null;
                Session["conditions"] = null;
            }

            using (ORIXContext _db = new ORIXContext())
            {
                foreach (var status in _db.s_ConditionItems("APLYSTS", "", "", ""))
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = (string.IsNullOrWhiteSpace(status.TypeCode)) ? status.TypeCode.Trim() + status.TypeDesc.Trim() : status.TypeCode.Trim() + " - " + status.TypeDesc.Trim();
                    item.Value = status.TypeCode.Trim();
                    if (item.Value == oldCUR_STSSelected)
                        item.Selected = true;

                    PCUR_STS.Add(item);
                }

                foreach (var case_src in _db.OR3_COND_DEF.Where(c => c.TypeField.Trim() == "CASE_SOUR").ToList())
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = case_src.TypeCode.Trim() + " - " + case_src.TypeDesc.Trim();
                    item.Value = case_src.TypeCode.Trim();
                    if (item.Value == ordCASE_SOURSelected)
                        item.Selected = true;

                    PCASE_SOUR.Add(item);
                }
            }

            ViewBag.PCUR_STS = PCUR_STS;
            ViewBag.PCASE_SOUR = PCASE_SOUR;

            if (isFromReturn.HasValue  && Session["conditions"] != null)
                if (isFromReturn.Value)
                    return View((WE020VO)Session["conditions"]);

            TimeSpan t2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan timeDiff = t1.Subtract(t2).Duration();

            _logger.DebugFormat("Function:{0}, 分:{1}, 秒:{2}, 豪秒: {3}, Para:{4}", "查詢畫面", timeDiff.Minutes.ToString(), timeDiff.Seconds.ToString(), timeDiff.Milliseconds.ToString(), "");

            return View();
        }

        [HttpPost]
        public ActionResult SearchContent(WE020VO conditions)
        {
            TimeSpan t1 = new TimeSpan(DateTime.Now.Ticks);

            if (!checkSearchConditions(conditions))
                return GetResponse(false, "請至少輸入一個條件！", null);

            using (ORIXContext _db = new ORIXContext())
            {
                var query_results = _db.s_WE020_Grid(
                                                    conditions.APLY_NO,
                                                    conditions.PCONTACT,
                                                    conditions.PCUST_NO,
                                                    conditions.PIS_SEARCH.ToString(),
                                                    conditions.PCUST_NAME,
                                                    conditions.FRC_CODE,
                                                    conditions.FRC_NAME,
                                                    conditions.PCTAC_TEL,
                                                    conditions.PRECV_NAME,
                                                    conditions.PSEND_ADDR,
                                                    conditions.PADDR,
                                                    conditions.PPROD_NAME,
                                                    conditions.PMAC_NO,
                                                    conditions.PAPLY_DATE_S,
                                                    conditions.PAPLY_DATE_E,
                                                    conditions.PEMP_CODE,
                                                    conditions.PCUR_STS,
                                                    conditions.PDEPT_CODE,
                                                    conditions.PCASE_SOUR,
                                                    conditions.PIS_DEL.ToString(),
                                                    conditions.PINV_NO,
                                                    conditions.PCAR_NO).ToList();
                Session["results"] = query_results;
                Session["conditions"] = conditions;
                RefreshList((List<s_WE020_Grid_Result>)Session["results"]);
                if (query_results.Count == 0)
                    return GetResponse(false, "無符合條件資料！", null);
            }
            TimeSpan t2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan timeDiff = t1.Subtract(t2).Duration();

            _logger.DebugFormat("Function:{0}, 分:{1}, 秒:{2}, 豪秒: {3}, Para:{4}", "查詢資料", timeDiff.Minutes.ToString(), timeDiff.Seconds.ToString(), timeDiff.Milliseconds.ToString(), "");

            return GetResponse(true,"Success",null);
        }

        private bool checkSearchConditions(WE020VO conditions)
        {
            var properties = new List<string>() {
                "APLY_NO",
                "PCONTACT",
                "PCUST_NO",
                //"PIS_SEARCH",//has default
                "PCUST_NAME",
                "FRC_CODE",
                "FRC_NAME",
                "PCTAC_TEL",
                "PRECV_NAME",
                "PSEND_ADDR",
                "PADDR",
                "PPROD_NAME",
                "PMAC_NO",
                "PAPLY_DATE_S",
                "PAPLY_DATE_E",
                "PEMP_CODE",
                "PCUR_STS",//has default
                "PDEPT_CODE",
                "PCASE_SOUR",//has default
                //"PIS_DEL",//has default
                "PINV_NO",
                "PCAR_NO",
                "MCAR_NO",
                "MPROD_NAME"
            };
            foreach (var propertyName in properties)
            {
                if (conditions.GetType().GetProperty(propertyName).GetValue(conditions,null) != null)
                    return true;
            }
            return false;
        }
        [HttpGet]
        public ActionResult QueryResult()
        {
            return View("QueryResult", (List<s_WE020_Grid_Result>)Session["results"]);
        }
        public ActionResult FunctionClick(WE020VO.FunctionClick functionClick)
        {
            string pageDetail = "";
            string strMessage = "";

            functionClick.rowAPLY = (string.IsNullOrWhiteSpace(functionClick.rowAPLY)) ? string.Empty : functionClick.rowAPLY.Trim();
            functionClick.rowOBJECT = (string.IsNullOrWhiteSpace(functionClick.rowOBJECT)) ? string.Empty : functionClick.rowOBJECT.Trim();
            functionClick.functionIndex = (string.IsNullOrWhiteSpace(functionClick.functionIndex)) ? string.Empty : functionClick.functionIndex.Trim();

            if (functionClick.rowAPLY.Trim() == "")
            {
                strMessage = " 請選擇編輯項目！";
                return GetResponse(false, strMessage, null);
            }


            if (functionClick.rowOBJECT.Trim() == "" && functionClick.functionIndex == "Function5")
            {
                strMessage = " 請於標的物資料選擇！";
                return GetResponse(false, strMessage, null);
            }
            string CUR_STS = "";
            using (ORIXContext _db = new ORIXContext())
            {
                var query = (
                               from APLY_BASE in _db.OR_CASE_APLY_BASE
                               join ORCUSTOM in _db.OR_CUSTOM on APLY_BASE.CUST_NO equals ORCUSTOM.CUST_NO into AC
                               from ORCUSTOM in AC.DefaultIfEmpty()
                               join ORBLOC in _db.OR_BLOC on ORCUSTOM.CUST_NO equals ORBLOC.BLOC_NO into CB
                               from ORBLOC in CB.DefaultIfEmpty()
                               where APLY_BASE.APLY_NO == functionClick.rowAPLY.Trim()
                               select new
                               {
                                   CUST_NO = APLY_BASE.CUST_NO,
                                   CUST_NAME = ORCUSTOM.CUST_NAME,
                                   CUR_STS_CODE = APLY_BASE.CUR_STS,
                                   CUST_BLOC_CODE = ORCUSTOM.CUST_BLOC_CODE,
                                   BLOC_sNAME = ORBLOC.BLOC_SNAME,
                                   CUST_SNAME = ORCUSTOM.CUST_SNAME,
                                   EMP_CODE = APLY_BASE.EMP_CODE,
                                   DEPT_CODE = APLY_BASE.DEPT_CODE
                               }
                            ).FirstOrDefault();
                /*
                 * Entity Framework 不支援Scalar-value function,重寫成LINQ
                 */
                if (query != null)
                    CUR_STS = f_ConditionGetDesc("APLYSTS", query.CUR_STS_CODE, "N");

                Session["APLY_NO"] = functionClick.rowAPLY.Trim();

                switch (functionClick.functionIndex)
                {

                    case "Function1": //客戶服務資料異動        ok
                        pageDetail = "WE030";
                        break;

                    case "Function2"://票據明細維護  
                        Session["CUST_NO"] = query.CUST_NO;
                        Session["CUST_NAME"] = query.CUST_NAME;
                        pageDetail = "WE050";
                        break;

                    case "Function3"://電話紀錄維護     ok
                        Session["CUST_NO"] = query.CUST_NO;
                        Session["CUST_NAME"] = query.CUST_NAME;

                        pageDetail = "WE040";
                        break;

                    case "Function4"://解約金試算  hold
                        Session["CUST_NO"] = query.CUST_NO;
                        Session["CUST_NAME"] = query.CUST_NAME;

                        pageDetail = "WE110";
                        break;

                    case "Function5"://標的物查詢  
                        pageDetail = "WE090";
                        Session["OBJ_CODE"] = functionClick.rowOBJECT;

                        break;

                    case "Function6"://客戶歷史交易查詢  
                        Session["CUST_NO"] = query.CUST_NO;
                        Session["CUST_NAME"] = query.CUST_NAME;
                        pageDetail = "WE100";
                        break;

                    case "Function7"://客戶付款記錄查詢  
                        Session["CUST_NO"] = query.CUST_NO;
                        Session["CUST_NAME"] = query.CUST_NAME;
                        Session["CUST_BLOC_CODE"] = query.CUST_BLOC_CODE;
                        Session["BLOC_NAME"] = query.BLOC_sNAME;
                        pageDetail = "WE070";
                        break;

                    case "Function8"://案件生命週期查詢  
                        break;

                    case "Function9"://催收記錄查詢  
                    http://orixtw /Admin/KenLogin.asp


                        // this.setScript("window.open('http://orixtw/admin/Press/PressView.asp?idx=" + this.rowAPLY.Text.Trim() + "&idxno=" + dr[0].ToString() + "&page=','_blank')");
                        //this.setScript("window.open('http://orixtw/Admin/KenLogin.asp?txtId=" + this.Master.Master.CorpAcct + "&KenProgram=Press&idx=" + this.rowAPLY.Text.Trim() + "&idxno=" + dr[0].ToString() + "&page=','_blank')");
                        Redirect(string.Format("http://orixtw/Admin/KenLogin.asp?txtId=" + ((String)Session["CorpAcct"]).Trim() + "&KenProgram=Press&idx=" + functionClick.rowAPLY + "&idxno=" + query.CUST_NO + "&page=','_blank')"));
                        //    Response.End();
                        break;

                    case "Function10"://計張資訊查詢  
                        pageDetail = "WE080";
                        break;

                    case "Function11"://客戶應收帳款查詢  
                        break;

                    case "Function12"://申請書資料查詢  
                        strMessage = " 權限不足！ ";
                        bool permission = false;
                        var queryPermission = _db.s_Emp_Class(Session["UserId"].ToString(), functionClick.rowAPLY, null, null).FirstOrDefault();
                        bool.TryParse(queryPermission.prem, out permission);

                        if (!permission)
                            return GetResponse(false, strMessage, null);


                        //增加查詢條件-動產擔保（品名、車號）OR_MOBJECT.prod_name 、OR_MOBJECT.CAR_NO，帶出有符合條件的申請書資料
                        Session["MCAR_NO"] = ((WE020VO)Session["conditions"]).MCAR_NO;//動產擔保 品名
                        Session["MPROD_NAME"] = ((WE020VO)Session["conditions"]).MPROD_NAME;//動產擔保 車號
                        Session["CUR_STS"] = query.CUR_STS_CODE + "," + CUR_STS;
                        Session["bolWE020"] = true;
                        pageDetail = "WA0601";

                        

                        break;
                }
            }
            return GetResponse(true, "Success", new { pageDetailStr = pageDetail + ".aspx?ndt="+ DateTime.Now.ToString("HHmmss") });
        }

        /// <summary>
        /// Get Depart info
        /// </summary>
        /// <param name="DEPT_CODE_section"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDEPT_SourceList(string DEPT_CODE_section,string page,string rows) {
            try
            { 
                /* 
                 * 根據StoreProcedure : s_WE020_Grid 所查詢的資料表取得部門資料
                 */
                using (ORIXContext _db = new ORIXContext())
                {
                    var result = _db.v_OR_DEPT.Where(d => (d.DEPT_CODE.Contains(DEPT_CODE_section)
                                                          || d.DEPT_NAME.Contains(DEPT_CODE_section)
                                                          ))
                                            .Select(d => new { DEPT_CODE = d.DEPT_CODE.Trim(), DEPT_NAME = d.DEPT_NAME.Trim() }).ToList();
                    var rowData = result.OrderBy(d => d.DEPT_CODE)
                                            .Skip((int.Parse(page) - 1) * int.Parse(rows))
                                            .Take(int.Parse(rows)).ToList();
                    var data = new { total = result.Count, rows = rowData };
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                
                return GetResponse(false, "Fail", new { ErrorMessage = e.InnerException.Message });
            }
        }

        /// <summary>
        /// Get sales data
        /// </summary>
        /// <param name="Sales_NO_section"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSalesList(string Sales_NO_section,string page,string rows)
        {
            try
            {
                /* 
                 * 根據StoreProcedure : s_WE020_Grid 所查詢的資料表取得業務員資料
                 */
                using (ORIXContext _db = new ORIXContext())
                {
                    var result = _db.v_OR_EMP.Where(s => (s.CORP_ACCT.Contains(Sales_NO_section)
                                                         || s.EMP_NAME.Contains(Sales_NO_section)
                                                         || s.EMP_ENAME.Contains(Sales_NO_section)))
                                                    .Select(s => new { CORP_ACCT = s.CORP_ACCT.Trim(), Sales_Name = s.EMP_NAME.Trim(), Sales_EName = s.EMP_ENAME.Trim()}).ToList();
                    var rowData = result.OrderBy(d => d.CORP_ACCT)
                                        .Skip((int.Parse(page) - 1) * int.Parse(rows))
                                        .Take(int.Parse(rows)).ToList();
                    var data = new { total = result.Count, rows = rowData };

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return GetResponse(false, "Fail", new { ErrorMessage = e.InnerException.Message });
            }
        }

        private ActionResult GetResponse(bool resultValue, string Message, object data)
        {
            return Json(new { Result = resultValue, Message = Message, Data = data },JsonRequestBehavior.AllowGet);
        }

        #region Excel
        /// <summary>
        /// Generates Excel document using headers grabbed from property names
        /// </summary>
        public ActionResult GenerateExcel(string type)
        {
            //var exportSpource = this.GetExportData(model);
            ExportExcelResult exportExcelRst = new ExportExcelResult() {
                SheetName = "客戶綜合服務",
                FileName =string.Concat("客戶綜合服務_", DateTime.Now.ToString("yyyyMMdd"), ".xlsx")
            };
            if (Session["results"] != null)
            {
                var resource = JsonConvert.SerializeObject(GetExportData(type));
                var dt = JsonConvert.DeserializeObject<DataTable>(resource.ToString());
                exportExcelRst.ExportData = dt;
            }
            else {
                var dt = JsonConvert.DeserializeObject<DataTable>((new JArray()).ToString());
                exportExcelRst.ExportData = dt;
            }
            return exportExcelRst;
            
        }

        /// <summary>
        /// 組合Excel資料，根據參數分辨標的物/申請書
        /// 輸出結果：畫面資料
        /// </summary>
        /// <param name="type">Obj:標的物,Aply:申請書</param>
        /// <returns></returns>
        private JArray GetExportData(string type)
        {
            JArray jObjects = new JArray();
            var results = (List<s_WE020_Grid_Result>)Session["results"];
            foreach (var item in results.Where(r => r.Type_Code == type).ToList())
            {
                var jo = new JObject();
                jo.Add("申請書編號", item.APLY_NO);
                jo.Add("契約起日", item.con_date_fr);
                jo.Add("契約迄日", item.con_date_to);
                jo.Add("月數", string.Format("{0:###,###,###,##0}", item.APRV_DURN_M.Value));
                jo.Add("期數", string.Format("{0:###,###,###,##0}", item.APRV_PERD.Value));
                jo.Add("期租金", string.Format("{0:###,###,###,##0}", item.APRV_HIRE.Value));
                jo.Add("狀況", item.cursts);
                jo.Add("客戶代號", item.Cust_No);
                jo.Add("客戶簡稱", item.Cust_Sname);
                jo.Add("連絡人", item.Contact);
                #region Aply
                if (type == "Aply")
                {
                    jo.Add("品名", item.PROD_NAME);
                    jo.Add("機號", item.MAC_NO);
                    jo.Add("車號", item.CAR_NO);
                    jo.Add("經銷商簡稱", item.FRC_SNAME);
                    jo.Add("請款地址", item.REQ_PAY_ADDR);
                    jo.Add("計張", item.PAPER);
                    jo.Add("分開發票", item.DIVIDE);
                    jo.Add("合併開立", item.MERGE_NO);
                    jo.Add("統一郵寄", item.MMail_NO);
                    jo.Add("契約總額", string.Format("{0:###,###,###,##0}",item.CUR_CON_AMT.Value));
                }
                #endregion
                #region Obj
                if (type == "Obj")
                {
                    jo.Add("序號", item.Row);
                    jo.Add("品名", item.PROD_NAME);
                    jo.Add("機號", item.MAC_NO);
                    jo.Add("車號", item.CAR_NO);
                    jo.Add("經銷商簡稱", item.FRC_SNAME);
                    jo.Add("規格", item.SPEC);
                    jo.Add("市價", string.Format("{0:###,###,###,##0}", item.Market_price.Value));
                    jo.Add("殘值", string.Format("{0:###,###,###,##0}", item.RV_AMT.Value));
                    jo.Add("標的物狀態", item.OBJ_STS);
                    jo.Add("標的物代號", item.OBJ_CODE);
                    jo.Add("所在地址", item.OBJ_LOC_ADDR);
                    jo.Add("所在電話", item.OBJ_LOC_TEL);
                    jo.Add("計張", item.PAPER);
                    jo.Add("分開發票", item.DIVIDE);
                    jo.Add("MERGE_NO", item.MERGE_NO);
                }
                #endregion
                jObjects.Add(jo);
            }

            return jObjects;
        }
        #endregion
        /// <summary>
        /// Replace null data to empty string
        /// </summary>
        private static void RefreshList(List<s_WE020_Grid_Result> results)
        {
            foreach (var result in results)
            {
                result.Type_Code = (!string.IsNullOrWhiteSpace(result.Type_Code)) ? result.Type_Code.Trim() : "";
                result.APLY_NO = (!string.IsNullOrWhiteSpace(result.APLY_NO)) ? result.APLY_NO.Trim() : "";
                result.con_date_fr = (!string.IsNullOrWhiteSpace(result.con_date_fr)) ? result.con_date_fr.Trim() : "";
                result.con_date_to = (!string.IsNullOrWhiteSpace(result.con_date_to)) ? result.con_date_to.Trim() : "";
                result.cursts = (!string.IsNullOrWhiteSpace(result.cursts)) ? result.cursts.Trim() : "";
                result.Contact = (!string.IsNullOrWhiteSpace(result.Contact)) ? result.Contact.Trim() : "";
                result.Cust_Sname = (!string.IsNullOrWhiteSpace(result.Cust_Sname)) ? result.Cust_Sname.Trim() : "";
                result.Cust_No = (!string.IsNullOrWhiteSpace(result.Cust_No)) ? result.Cust_No.Trim() : "";
                result.PROD_NAME = (!string.IsNullOrWhiteSpace(result.PROD_NAME)) ? result.PROD_NAME.Trim() : "";
                result.MAC_NO = (!string.IsNullOrWhiteSpace(result.MAC_NO)) ? result.MAC_NO.Trim() : "";
                result.FRC_SNAME = (!string.IsNullOrWhiteSpace(result.FRC_SNAME)) ? result.FRC_SNAME.Trim() : "";
                result.REQ_PAY_ADDR = (!string.IsNullOrWhiteSpace(result.REQ_PAY_ADDR)) ? result.REQ_PAY_ADDR.Trim() : "";
                result.PAPER = (!string.IsNullOrWhiteSpace(result.PAPER)) ? result.PAPER.Trim() : "";
                result.DIVIDE = (!string.IsNullOrWhiteSpace(result.DIVIDE)) ? result.DIVIDE.Trim() : "";
                result.MERGE_NO = (!string.IsNullOrWhiteSpace(result.MERGE_NO)) ? result.MERGE_NO.Trim() : "";
                result.MMail_NO = (!string.IsNullOrWhiteSpace(result.MMail_NO)) ? result.MMail_NO.Trim() : "";
                result.SPEC = (!string.IsNullOrWhiteSpace(result.SPEC)) ? result.SPEC.Trim() : "";
                result.OBJ_STS = (!string.IsNullOrWhiteSpace(result.OBJ_STS)) ? result.OBJ_STS.Trim() : "";
                result.OBJ_CODE = (!string.IsNullOrWhiteSpace(result.OBJ_CODE)) ? result.OBJ_CODE.Trim() : "";
                result.OBJ_LOC_ADDR = (!string.IsNullOrWhiteSpace(result.OBJ_LOC_ADDR)) ? result.OBJ_LOC_ADDR.Trim() : "";
                result.OBJ_LOC_TEL = (!string.IsNullOrWhiteSpace(result.OBJ_LOC_TEL)) ? result.OBJ_LOC_TEL.Trim() : "";

                result.Row = (result.Row.HasValue) ? result.Row : 0;
                result.APRV_DURN_M = (result.APRV_DURN_M.HasValue) ? result.APRV_DURN_M : decimal.Zero;
                result.APRV_PERD = (result.APRV_PERD.HasValue) ? result.APRV_PERD : decimal.Zero;
                result.APRV_HIRE = (result.APRV_HIRE.HasValue) ? result.APRV_HIRE : decimal.Zero;
                result.CUR_CON_AMT = (result.CUR_CON_AMT.HasValue) ? result.CUR_CON_AMT : decimal.Zero;
                result.Market_price = (result.Market_price.HasValue) ? result.Market_price : decimal.Zero;
                result.RV_AMT = (result.RV_AMT.HasValue) ? result.RV_AMT : decimal.Zero;
            }
        }

        /// <summary>
        /// Replace scalar-value function f_ConditionGetDesc
        /// </summary>
        /// <param name="Field"></param>
        /// <param name="Code"></param>
        /// <param name="PShowCode"></param>
        /// <returns></returns>
        private string f_ConditionGetDesc(string Field, string Code, string PShowCode)
        {
            string strReturn = "";
            using (ORIXContext _db = new ORIXContext())
            {
                strReturn = (from COND_DEF in _db.OR3_COND_DEF
                 where COND_DEF.TypeField == Field && COND_DEF.TypeCode == Code
                 select (
                        (PShowCode == "Y") ? COND_DEF.TypeCode + " " :
                          "") + COND_DEF.TypeDesc
                                  ).FirstOrDefault();
            }

            return strReturn;
        }
    }
}
