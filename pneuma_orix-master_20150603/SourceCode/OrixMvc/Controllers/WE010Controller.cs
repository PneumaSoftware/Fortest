using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OrixModel;
using OrixMvc.Service;
using OrixMvc.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
using System.Web.Script.Serialization;

namespace OrixMvc.Controllers
{
    public class WE010Controller : CommonController
    {
        private string strFunc_Id = "WE010";
        private static log4net.ILog _logger = log4net.LogManager.GetLogger(typeof(WE010Controller));

        #region 查詢
        //
        // GET: /WE010/
        public ActionResult Index()
        {
            TimeSpan t1 = new TimeSpan(DateTime.Now.Ticks);
            string url = checkLogin(strFunc_Id);
            if (url != "")
            {
                return Redirect(url);
            }

            string strPRG_ID = "WE010";
            var model = new WE010VO();

            using (ORIXContext _db = new ORIXContext())
            {
                model = new WE010VO
                {
                    Hint = _db.OR3_HINT.Where(x => x.FUNC_ID == strFunc_Id && x.PRG_ID == strPRG_ID).ToList<OR3_HINT>()
                };
            }

            TimeSpan t2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan timeDiff = t1.Subtract(t2).Duration();

            _logger.DebugFormat("Function:{0}, 分:{1}, 豪秒:{2}, Para:{3}", "查詢畫面", timeDiff.Minutes.ToString(), timeDiff.Milliseconds.ToString(), "");

            return View();
        }

        [HttpPost]
        public JsonResult Index(WE010VO model)
        {
            TimeSpan t1 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan t2 = new TimeSpan();
            #region 查詢
            using (ORIXContext _db = new ORIXContext())
            {
                var query = from custom in _db.OR_CUSTOM
                            join bloc in _db.OR_BLOC on custom.CUST_BLOC_CODE equals bloc.BLOC_NO into ps
                            from bloc in ps.DefaultIfEmpty()
                            where (string.IsNullOrEmpty(model.CUST_BLOC_CODE)) || (custom.CUST_BLOC_CODE == model.CUST_BLOC_CODE)
                            where (string.IsNullOrEmpty(model.CUST_BLOC_SNAME)) || (bloc.BLOC_SNAME.Contains(model.CUST_BLOC_SNAME))
                            where (string.IsNullOrEmpty(model.CUST_NO)) || (custom.CUST_NO == model.CUST_NO)
                            where (string.IsNullOrEmpty(model.CUST_SNAME)) || (custom.CUST_SNAME.Contains(model.CUST_SNAME))
                            where (string.IsNullOrEmpty(model.CUST_NAME)) || (custom.CUST_NAME.Contains(model.CUST_NAME))
                            where (string.IsNullOrEmpty(model.CUST_STS)) || (custom.Cust_STS == model.CUST_STS)
                            where (string.IsNullOrEmpty(model.IS_TRANSACTION)) || (custom.Is_Transaction == model.IS_TRANSACTION)
                            where (string.IsNullOrEmpty(model.SPEC_COND)) || (((model.SPEC_COND == "Y") && (custom.Spec_cond != ""))
                                                                               || ((model.SPEC_COND == "N") && (custom.Spec_cond == "" || custom.Spec_cond == null)))
                            orderby custom.CUST_NO
                            select new WE010VO.GridVO
                            {
                                CUST_NO = custom.CUST_NO,
                                CUST_SNAME = custom.CUST_SNAME,
                                CUST_BLOC_CODE = custom.CUST_BLOC_CODE ?? "",
                                BLOC_SNAME = bloc.BLOC_SNAME ?? "",
                                SPEC_COND = custom.Spec_cond ?? ""
                            };


                t2 = new TimeSpan(DateTime.Now.Ticks);
                if (query.Count() > 0)
                {
                    var pageIndex = (model.PageIndex == 0) ? 1 : model.PageIndex;
                    model.PageIndex = pageIndex;
                    model.TotalData = query.Count();
                    model.GridData = query.ToPagedList(model.PageIndex, model.PageSize);
                }
            }
            #endregion

            TimeSpan t3 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan timeDiff = t1.Subtract(t3).Duration();

            _logger.DebugFormat("Function:{0}, 分:{1}, 豪秒:{2}, Strat:{3}, Query:{4}, End:{5}, 集團代號:{6}, 集團簡稱: {7}, 是否成交: {8}, 是否特殊: {9}, 客戶代號: {10}, 客戶簡稱: {11}, 客戶名稱: {12}, 潛在客戶: {13}",
                                "查詢結果", timeDiff.Minutes.ToString(), timeDiff.Milliseconds.ToString(), t1.Minutes + "." + t1.Milliseconds, t2.Minutes + "." + t2.Milliseconds, t3.Minutes + "." + t3.Milliseconds,
                                model.CUST_BLOC_CODE, model.CUST_BLOC_SNAME, model.CUST_NO, model.CUST_SNAME,
                                model.CUST_NAME, model.CUST_STS, model.IS_TRANSACTION, model.SPEC_COND);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        #region 下載
        /**/
        /// <summary>
        /// Generates Excel document using headers grabbed from property names
        /// </summary>
        public ActionResult GenerateExcel(WE010VO model)
        {
            var exportSpource = this.GetExportData(model);
            var dt = JsonConvert.DeserializeObject<DataTable>(exportSpource.ToString());

            var exportFileName = string.Concat("客戶資料維護_", DateTime.Now.ToString("yyyyMMdd"), ".xlsx");

            return new ExportExcelResult
            {
                SheetName = "客戶資料維護",
                FileName = exportFileName,
                ExportData = dt
            };
        }

        private JArray GetExportData(WE010VO model)
        {
            JArray jObjects = new JArray();

            using (ORIXContext _db = new ORIXContext())
            {
                var query = from custom in _db.OR_CUSTOM
                            join bloc in _db.OR_BLOC on custom.CUST_BLOC_CODE equals bloc.BLOC_NO into ps
                            from bloc in ps.DefaultIfEmpty()
                            where (string.IsNullOrEmpty(model.CUST_BLOC_CODE)) || (custom.CUST_BLOC_CODE == model.CUST_BLOC_CODE)
                            where (string.IsNullOrEmpty(model.CUST_BLOC_SNAME)) || (bloc.BLOC_SNAME.Contains(model.CUST_BLOC_SNAME))
                            where (string.IsNullOrEmpty(model.CUST_NO)) || (custom.CUST_NO == model.CUST_NO)
                            where (string.IsNullOrEmpty(model.CUST_SNAME)) || (custom.CUST_SNAME.Contains(model.CUST_SNAME))
                            where (string.IsNullOrEmpty(model.CUST_STS)) || (custom.Cust_STS == model.CUST_STS)
                            where (string.IsNullOrEmpty(model.IS_TRANSACTION)) || (custom.Is_Transaction == model.IS_TRANSACTION)
                            where (string.IsNullOrEmpty(model.SPEC_COND)) || (((model.SPEC_COND == "Y") && (custom.Spec_cond != ""))
                                                                               || ((model.SPEC_COND == "N") && (custom.Spec_cond == "" || custom.Spec_cond == null)))
                            orderby custom.CUST_NO
                            select new WE010VO.GridVO
                            {
                                CUST_NO = custom.CUST_NO,
                                CUST_SNAME = custom.CUST_SNAME,
                                CUST_BLOC_CODE = custom.CUST_BLOC_CODE,
                                BLOC_SNAME = bloc.BLOC_SNAME,
                                SPEC_COND = custom.Spec_cond
                            };

                foreach (var item in query)
                {
                    var jo = new JObject();
                    jo.Add("客戶代號", item.CUST_NO);
                    jo.Add("客戶簡稱", item.CUST_SNAME);
                    jo.Add("集團代號", item.CUST_BLOC_CODE);
                    jo.Add("集團簡稱", item.BLOC_SNAME);
                    jo.Add("特殊客戶", item.SPEC_COND);
                    jObjects.Add(jo);
                }
            }
            return jObjects;
        }
        #endregion
        #endregion

        #region 明細
        public ActionResult Detail(string id)
        {
            TimeSpan t1 = new TimeSpan(DateTime.Now.Ticks);
            string strPRG_ID = "WE0101";
            var model = new WE0101VO();
            using (ORIXContext _db = new ORIXContext())
            {
                #region 查詢
                //var qCTCquery = _db.OR_CUST_KIND.Where(x => x.CUST_FRC_TYPE_CODE.Substring(0, 1) == "0")
                //              .Select(x => new { Value = x.CUST_FRC_TYPE_CODE, Text = x.CUST_FRC_TYPE_NAME }).ToList();

                //var qCUquery = _db.OR_CUST_KIND.Where(x => x.CUST_FRC_TYPE_CODE.Substring(0, 1) != "0")
                //              .Select(x => new { Value = x.CUST_FRC_TYPE_CODE, Text = x.CUST_FRC_TYPE_NAME }).ToList();

                //var qNquery = _db.OR_CUST_NAT
                //              .Select(x => new { Value = x.NATIONALITY.Trim(), Text = x.NAT_NAME }).ToList();

                //var qSCquery = _db.s_ConditionItems("SPEC_COND", "", "", "")
                //              .Select(x => new { Value = x.TypeCode, Text = x.TypeCode + ' ' + x.TypeDesc }).ToList();

                //var qOTquery = _db.s_ConditionItems("ORG_TYPE", "", "", "")
                //              .Select(x => new { Value = x.TypeCode, Text = x.TypeCode + ' ' + x.TypeDesc }).ToList();

                //var qIquery = _db.s_ConditionItems("INVOICE", "", "", "")
                //              .Select(x => new { Value = x.TypeCode, Text = x.TypeDesc }).ToList();

                //var qCDquery = _db.OR3_Zip
                //              .Where(x => x.City_Code != "")
                //              .GroupBy(x => new { x.City_Code, x.City_Name },
                //                                (key, group) => new { key1 = key.City_Code, key2 = key.City_Name })
                //              .Select(x => new { Value = x.key1, Text = x.key2 }).ToList();

                //修改模式
                WE0101VO.DetailData data = new WE0101VO.DetailData();
                if (id != null)
                {
                    var Detail = from custom in _db.OR_CUSTOM
                                 join bloc in _db.OR_BLOC on custom.CUST_BLOC_CODE equals bloc.BLOC_NO into cb
                                 from bloc in cb.DefaultIfEmpty()
                                 join zip in _db.OR3_Zip on custom.Zip_Code equals zip.Zip_Code into cc
                                 from zip in cc.DefaultIfEmpty()
                                 join zip1 in _db.OR3_Zip on custom.RGT_ZIP_CODE equals zip1.Zip_Code into cz
                                 from zip1 in cz.DefaultIfEmpty()
                                 where custom.CUST_NO == id
                                 select new WE0101VO.DetailData
                                 {
                                     CUST_NO = custom.CUST_NO,
                                     CUST_SNAME = custom.CUST_SNAME,
                                     CUST_BLOC_CODE = custom.CUST_BLOC_CODE,
                                     BLOC_SNAME = bloc.BLOC_SNAME,
                                     SPEC_COND = custom.Spec_cond,
                                     CUST_TYPE_CODE = custom.CUST_TYPE_CODE,
                                     CUST_UNION = custom.CUST_UNION,
                                     NATIONALITY = custom.NATIONALITY.Trim(),
                                     CUST_NAME = custom.CUST_NAME,
                                     UNIF_NO = custom.UNIF_NO,
                                     EN_NAME = custom.EN_NAME,
                                     EN_SNAME = custom.EN_SNAME,
                                     CONTACT = custom.CONTACT,
                                     CTAC_TITLE = custom.CTAC_TITLE,
                                     CTAC_EXT = custom.CTAC_EXT,
                                     STOCKCODE = custom.StockCode,
                                     CONTACT2 = custom.CONTACT2,
                                     CTAC_TITLE2 = custom.CTAC_TITLE2,
                                     CTAC_EXT2 = custom.CTAC_EXT2,
                                     TAKER = custom.TAKER,
                                     TAKER_ID = custom.TAKER_ID,
                                     FLOT_DATE = string.IsNullOrEmpty(custom.FLOT_DATE.Trim()) ? "" : custom.FLOT_DATE.Substring(4, 2) + "/" + custom.FLOT_DATE.Substring(6, 2) + "/" + custom.FLOT_DATE.Substring(0, 4),
                                     BUILD_DATE = string.IsNullOrEmpty(custom.BUILD_DATE.Trim()) ? "" : custom.BUILD_DATE.Substring(4, 2) + "/" + custom.BUILD_DATE.Substring(6, 2) + "/" + custom.BUILD_DATE.Substring(0, 4),
                                     PHONE1 = custom.PHONE1,
                                     PHONE2 = custom.PHONE2,
                                     FACSIMILE = custom.FACSIMILE,
                                     EMP_PSNS = SqlFunctions.StringConvert(custom.EMP_PSNS).Trim(),
                                     SALES_RGT_ADDR = custom.SALES_RGT_ADDR,
                                     RGT_CAPT_AMT = SqlFunctions.StringConvert(custom.RGT_CAPT_AMT, 15, 1).Trim(),
                                     CTAC_ADDR = custom.CTAC_ADDR,
                                     REAL_CAPT_AMT = SqlFunctions.StringConvert(custom.REAL_CAPT_AMT, 15, 1).Trim(),
                                     IS_COND_AUTH = custom.Is_Cond_Auth,
                                     IS_BIZ_CUST = custom.Is_Biz_Cust,
                                     IS_HONEST_AGREEMENT = custom.Honest_Agreement,
                                     IS_SECRET_PROMISE = custom.Secret_Promise,
                                     IS_TRANSACTION = custom.Is_Transaction,
                                     ORG_TYPE = custom.ORG_TYPE,
                                     INVOICE = custom.INVOICE,
                                     MAIN_BUS_ITEM = custom.MAIN_BUS_ITEM,
                                     PARENT_COMP_NAME = custom.Parent_Comp_Name,
                                     PARENT_COMP_STOCK_CODE = custom.Parent_Comp_Stock_Code,
                                     GEN_CURR_QUOTA = SqlFunctions.StringConvert(custom.GEN_CURR_QUOTA).Trim() ?? "0",
                                     VP_CURR_QUOTA = SqlFunctions.StringConvert(custom.VP_CURR_QUOTA).Trim() ?? "0",
                                     AR_CURR_QUOTA = SqlFunctions.StringConvert(custom.AR_CURR_QUOTA).Trim() ?? "0",
                                     ZIP_CODE = custom.Zip_Code,
                                     CITY_CODE = zip.City_Code,
                                     CUST_STS = custom.Cust_STS,
                                     RGT_ZIP_CODE = custom.RGT_ZIP_CODE,
                                     RGT_CITY_CODE = zip1.City_Code,
                                     BACKGROUND = custom.BACKGROUND,
                                     MANAGER = custom.Manager,
                                     MANAGER_ID = custom.Manager_ID,
                                     INDUSTRY_NO = custom.INDUSTRY_NO,
                                     IS_CAR_LICENSE = custom.Is_Car_License
                                 };
                    data = Detail.First();
                }
                #endregion

                model = new WE0101VO
                {
                    //CUST_TYPE_CODE = new SelectList(qCTCquery, "Value", "Text"),
                    //CUST_UNION = new SelectList(qCUquery, "Value", "Text"),
                    //NATIONALITY = new SelectList(qNquery, "Value", "Text"),
                    //SPEC_COND = new SelectList(qSCquery, "Value", "Text"),
                    //ORG_TYPE = new SelectList(qOTquery, "Value", "Text"),
                    //INVOICE = new SelectList(qIquery, "Value", "Text"),
                    //CITY_CODE = new SelectList(qCDquery, "Value", "Text"),
                    //Hint = _db.OR3_HINT.Where(x => x.FUNC_ID == strFunc_Id && x.PRG_ID == strPRG_ID).ToList<OR3_HINT>(),
                    Detail = data
                };
            }

            TimeSpan t2 = new TimeSpan(DateTime.Now.Ticks);
            TimeSpan timeDiff = t1.Subtract(t2).Duration();

            _logger.DebugFormat("Function:{0}, 分:{1}, 豪秒:{2}, ID:{2}", "明細畫面", timeDiff.Minutes.ToString(), timeDiff.Milliseconds.ToString(), id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Save(WE0101VO.DetailData model)
        {
            TimeSpan t1 = new TimeSpan(DateTime.Now.Ticks);
            string strMessage = "";
            try
            {
                var Mode = model.hMode;
                string strLog = "";
                OR_CUSTOM custom = new OR_CUSTOM();
                OR_CUST_DATA_CHG_REC oREC = new OR_CUST_DATA_CHG_REC();

                using (ORIXContext _db = new ORIXContext())
                {
                    #region 1.設定資料
                    var sIs_Cond_Auth = model.IS_COND_AUTH == "on" ? "Y" : "N";
                    var sIS_COND_AUTH = model.IS_COND_AUTH == "on" ? "Y" : "N";
                    var sIS_BIZ_CUST = model.IS_BIZ_CUST == "on" ? "Y" : "N";
                    var sHONEST_AGREEMENT = model.IS_HONEST_AGREEMENT == "on" ? "Y" : "N";
                    var sSECRET_PROMISE = model.IS_SECRET_PROMISE == "on" ? "Y" : "N";
                    var sCUST_STS = model.CUST_STS == "on" ? "Y" : "N";
                    var sIs_Car_License = model.IS_CAR_LICENSE == "on" ? "Y" : "N";
                    var sFLOT_DATE = model.FLOT_DATE != null ? DateFormat(model.FLOT_DATE) : "";
                    var sBUILD_DATE = model.BUILD_DATE != null ? DateFormat(model.BUILD_DATE) : "";
                    #endregion

                    #region 2.寫入OR_CUSTOM、OR_CUST_DATA_CHG_REC
                    if (Mode == "ADD")
                    {
                        #region 新增
                        if (model.CUST_NO == null || model.CUST_NO.Trim() == "")
                        {
                            if (model.UNIF_NO != null && model.UNIF_NO.Trim() != "")
                            {
                                custom.CUST_NO = model.UNIF_NO;
                            }
                            else
                            {
                                var dNumber = _db.s_GetNumber("S", "C", DateTime.Now.Year.ToString(), DateTime.Now.Month.ToString()).ToList();
                                custom.CUST_NO = dNumber[0].Number;
                            }
                            model.CUST_NO = custom.CUST_NO;
                        }
                        else
                        {
                            custom.CUST_NO = model.CUST_NO;
                        }
                        custom.ADD_USER_ID = model.USER_ID;
                        custom.ADD_DATE = System.DateTime.Now.ToString("yyyyMMdd");
                        custom.ADD_TIME = System.DateTime.Now.ToString("HH:mm:ss");
                        custom.CAPT_STR = "5";
                        custom.BUILD_STS = "1";
                        #endregion
                    }
                    else
                    {
                        #region 修改
                        custom = _db.OR_CUSTOM.Where(x => x.CUST_NO == model.CUST_NO).First();

                        #region OR_CUST_DATA_CHG_REC
                        #region 檢核修改的欄位
                        strLog = diffData(custom.CUST_SNAME, model.CUST_SNAME, "客戶簡稱");
                        strLog += diffData(custom.CUST_BLOC_CODE, model.CUST_BLOC_CODE, "集團代號");
                        strLog += diffData(custom.CUST_TYPE_CODE, model.CUST_TYPE_CODE, "行業別");
                        strLog += diffData(custom.CUST_UNION, model.CUST_UNION, "租賃公會行業別");
                        strLog += diffData(custom.NATIONALITY, model.NATIONALITY, "國別");
                        strLog += diffData(custom.CUST_NAME, model.CUST_NAME, "客戶名稱");
                        strLog += diffData(custom.UNIF_NO, model.UNIF_NO, "統一編號");
                        strLog += diffData(custom.EN_NAME, model.EN_NAME, "英文名稱");
                        strLog += diffData(custom.EN_SNAME, model.EN_SNAME, "英文簡稱");
                        strLog += diffData(custom.CONTACT, model.CONTACT, "聯絡人");
                        strLog += diffData(custom.CTAC_TITLE, model.CTAC_TITLE, "聯絡人職稱");
                        strLog += diffData(custom.CTAC_EXT, model.CTAC_EXT, "聯絡人電話");
                        strLog += diffData(custom.StockCode, model.STOCKCODE, "股票代號");
                        strLog += diffData(custom.CONTACT2, model.CONTACT2, "聯絡人2");
                        strLog += diffData(custom.CTAC_TITLE2, model.CTAC_TITLE2, "聯絡人職稱2");
                        strLog += diffData(custom.CTAC_EXT2, model.CTAC_EXT2, "聯絡人電話2");
                        strLog += diffData(custom.TAKER, model.TAKER, "負責人");
                        strLog += diffData(custom.TAKER_ID, model.TAKER_ID, "負責人ID");
                        strLog += diffData(custom.FLOT_DATE, model.FLOT_DATE == null ? "" : sFLOT_DATE, "創業日期");
                        strLog += diffData(custom.BUILD_DATE, model.BUILD_DATE == null ? "" : sBUILD_DATE, "設立日期");
                        strLog += diffData(custom.PHONE1, model.PHONE1, "電話一");
                        strLog += diffData(custom.PHONE2, model.PHONE2, "電話二");
                        strLog += diffData(custom.FACSIMILE, model.FACSIMILE, "傳真");
                        strLog += diffNumber(custom.EMP_PSNS ?? 0, Convert.ToDecimal(model.EMP_PSNS), "員工人數");
                        strLog += diffData(custom.SALES_RGT_ADDR, model.SALES_RGT_ADDR, "營業地址");
                        strLog += diffNumber(custom.RGT_CAPT_AMT ?? 0, Convert.ToDecimal(model.RGT_CAPT_AMT), "登記資本額");
                        strLog += diffData(custom.Zip_Code ?? "", model.ZIP_CODE, "營業區碼");
                        strLog += diffData(custom.RGT_ZIP_CODE ?? "", model.RGT_ZIP_CODE, "聯絡區碼");
                        strLog += diffData(custom.CTAC_ADDR, model.CTAC_ADDR, "連絡地址");
                        strLog += diffNumber(custom.REAL_CAPT_AMT ?? 0, Convert.ToDecimal(model.REAL_CAPT_AMT), "實收資本額");
                        strLog += diffData(custom.Spec_cond, model.SPEC_COND, "特殊客戶條件");
                        strLog += diffData(custom.Is_Cond_Auth, sIS_COND_AUTH, "有條件授權");
                        strLog += diffData(custom.Is_Biz_Cust, sIS_BIZ_CUST, "營業用設備客戶");
                        strLog += diffData(custom.Honest_Agreement, sHONEST_AGREEMENT, "廉潔協定");
                        strLog += diffData(custom.Secret_Promise, sSECRET_PROMISE, "保密協定");
                        strLog += diffData(custom.Cust_STS, sCUST_STS, "存為潛在客戶");
                        //strLog += diffData(custom.IS_TRANSACTION, model.IS_TRANSACTION, "是否成交客戶");
                        strLog += diffData(custom.ORG_TYPE, model.ORG_TYPE, "組織型態");
                        strLog += diffData(custom.INVOICE, model.INVOICE, "發票開立方式");
                        strLog += diffData(custom.MAIN_BUS_ITEM, model.MAIN_BUS_ITEM, "主要營業項目");
                        strLog += diffData(custom.Parent_Comp_Name, model.PARENT_COMP_NAME, "母公司名");
                        strLog += diffData(custom.Parent_Comp_Stock_Code, model.PARENT_COMP_STOCK_CODE, "母公司股票代號");
                        strLog += diffData(custom.BACKGROUND, model.BACKGROUND, "客戶背景資料");
                        strLog += diffData(custom.Manager, model.MANAGER, "實際行主");
                        strLog += diffData(custom.Manager_ID, model.MANAGER_ID, "行主ID");
                        strLog += diffData(custom.Is_Car_License, sIs_Car_License, "車行");
                        if (strLog.Length > 0)
                        {
                            strLog = strLog.TrimEnd(',');
                        }
                        #endregion

                        oREC.CUST_NO = model.CUST_NO;
                        oREC.CHG_DATE = System.DateTime.Now.ToString("yyyyMMdd");
                        oREC.CHG_TIME = System.DateTime.Now.ToString("HH:mm:ss");
                        oREC.CHG_USER_CODE = model.USER_ID;
                        oREC.CHG_DTL = strLog;
                        #endregion
                        #endregion
                    }

                    custom.CUST_SNAME = model.CUST_SNAME;
                    custom.CUST_BLOC_CODE = model.CUST_BLOC_CODE;
                    custom.CUST_TYPE_CODE = model.CUST_TYPE_CODE;
                    custom.CUST_UNION = model.CUST_UNION;
                    custom.NATIONALITY = model.NATIONALITY;
                    custom.CUST_NAME = model.CUST_NAME;
                    custom.UNIF_NO = model.UNIF_NO;
                    custom.EN_NAME = model.EN_NAME;
                    custom.EN_SNAME = model.EN_SNAME;
                    custom.CONTACT = model.CONTACT;
                    custom.CTAC_TITLE = model.CTAC_TITLE;
                    custom.CTAC_EXT = model.CTAC_EXT;
                    custom.StockCode = model.STOCKCODE;
                    custom.CONTACT2 = model.CONTACT2;
                    custom.CTAC_TITLE2 = model.CTAC_TITLE2;
                    custom.CTAC_EXT2 = model.CTAC_EXT2;
                    custom.TAKER = model.TAKER;
                    custom.TAKER_ID = model.TAKER_ID;
                    custom.FLOT_DATE = sFLOT_DATE;
                    custom.BUILD_DATE = sBUILD_DATE;
                    custom.PHONE1 = model.PHONE1;
                    custom.PHONE2 = model.PHONE2;
                    custom.FACSIMILE = model.FACSIMILE;
                    custom.EMP_PSNS = model.EMP_PSNS != null ? Convert.ToDecimal(model.EMP_PSNS) : 0;
                    custom.SALES_RGT_ADDR = model.SALES_RGT_ADDR;
                    custom.RGT_CAPT_AMT = model.RGT_CAPT_AMT != null ? Convert.ToDecimal(model.RGT_CAPT_AMT) : 0;
                    custom.Zip_Code = model.ZIP_CODE;
                    custom.RGT_ZIP_CODE = model.RGT_ZIP_CODE;
                    custom.CTAC_ADDR = model.CTAC_ADDR;
                    custom.REAL_CAPT_AMT = model.REAL_CAPT_AMT != null ? Convert.ToDecimal(model.REAL_CAPT_AMT) : 0;
                    custom.Spec_cond = model.SPEC_COND;
                    custom.Is_Cond_Auth = sIs_Cond_Auth;
                    custom.Is_Biz_Cust = sIS_BIZ_CUST;
                    custom.Honest_Agreement = sHONEST_AGREEMENT;
                    custom.Secret_Promise = sSECRET_PROMISE;
                    //custom.Is_Transaction = model.IS_TRANSACTION == "on" ? "Y" : "N";
                    custom.Cust_STS = sCUST_STS;
                    custom.ORG_TYPE = model.ORG_TYPE;
                    custom.INVOICE = model.INVOICE;
                    custom.MAIN_BUS_ITEM = model.MAIN_BUS_ITEM;
                    custom.Parent_Comp_Name = model.PARENT_COMP_NAME;
                    custom.Parent_Comp_Stock_Code = model.PARENT_COMP_STOCK_CODE;
                    custom.BACKGROUND = model.BACKGROUND;
                    custom.Manager = model.MANAGER;
                    custom.Manager_ID = model.MANAGER_ID;
                    custom.Is_Car_License = sIs_Car_License;
                    //strLog += diffData(custom.INDUSTRY_NO, model.INDUSTRY_NO, "");
                    //custom.INDUSTRY_NO = model.INDUSTRY_NO;
                    custom.LAST_UPD_USER_ID = model.USER_ID;
                    custom.LAST_UPD_DATE = System.DateTime.Now.ToString("yyyyMMdd");
                    custom.LAST_UPD_TIME = System.DateTime.Now.ToString("HH:mm:ss");
                    #endregion

                    #region 寫入ACC11
                    ACC11 acc11 = _db.ACC11.Where(x => x.CUST_NO == model.CUST_NO).FirstOrDefault();
                    if (acc11 == null)
                    {
                        acc11 = new ACC11();
                        acc11.CUST_NO = model.CUST_NO;
                        acc11.CUST_NAME = model.CUST_NAME;
                        acc11.CUST_ABBR = model.CUST_SNAME;
                        acc11.UNIF_NO = model.UNIF_NO;
                        acc11.CHIEF_NAME = model.TAKER;
                        acc11.TEL_CUST = model.PHONE1;
                        acc11.FAX_CUST = model.FACSIMILE;
                        acc11.ADDR_CUST = model.CTAC_ADDR;
                        acc11.ADDR_CONT = model.CTAC_ADDR;
                        acc11.CONT_NAME = model.CONTACT;
                        acc11.ADDR_INVO = model.SALES_RGT_ADDR;
                        acc11.INVO_TITLE = model.CUST_NAME.Length > 50 ? model.CUST_NAME.Substring(0, 50) : model.CUST_NAME;
                        acc11.ADDR_ACCT = model.CTAC_ADDR;
                        acc11.ACCT_NAME = model.CONTACT;
                        //acc11.EMAIL;
                        acc11.RECE_WAY = "0";
                        acc11.CONT_NUM = 0;
                        acc11.PAY_DAY = 0;
                        //acc11.CRED_GRADE
                        acc11.DISC_RATE = 0;
                        acc11.SALE_CUST_PROD = "N";
                        //acc11.CRED_AMT
                        acc11.BAL_OR_SA = "SA";
                        acc11.ENTRY_ID = model.USER_ID;
                        acc11.ENTRY_DATE = System.DateTime.Now.ToString("yyyy/MM/dd");
                        acc11.ENTRY_TIME = System.DateTime.Now.ToString("HHmmss");
                        acc11.MODIFY_ID = model.USER_ID;
                        acc11.MODIFY_DATE = System.DateTime.Now.ToString("yyyy/MM/dd");
                        acc11.MODIFY_TIME = System.DateTime.Now.ToString("HHmmss");
                        _db.ACC11.Add(acc11);
                    }
                    else
                    {
                        acc11.CUST_NAME = model.CUST_NAME;
                        acc11.CUST_ABBR = model.CUST_SNAME;
                        acc11.UNIF_NO = model.UNIF_NO;
                        acc11.CHIEF_NAME = model.TAKER;
                        acc11.TEL_CUST = model.PHONE1;
                        acc11.FAX_CUST = model.FACSIMILE;
                        acc11.ADDR_CUST = model.CTAC_ADDR;
                        acc11.ADDR_CONT = model.CTAC_ADDR;
                        acc11.CONT_NAME = model.CONTACT;
                        acc11.ADDR_INVO = model.SALES_RGT_ADDR;
                        acc11.INVO_TITLE = model.CUST_NAME.Length > 50 ? model.CUST_NAME.Substring(0, 50) : model.CUST_NAME;
                        acc11.ADDR_ACCT = model.CTAC_ADDR;
                        acc11.ACCT_NAME = model.CONTACT;
                        //acc11.EMAIL;
                        //acc11.CRED_AMT
                        acc11.MODIFY_ID = model.USER_ID;
                        acc11.MODIFY_DATE = System.DateTime.Now.ToString("yyyy/MM/dd");
                        acc11.MODIFY_TIME = System.DateTime.Now.ToString("HHmmss");
                    }
                    #endregion

                    if (Mode == "ADD")
                    {
                        //2.儲存OR_CUSTOM
                        _db.OR_CUSTOM.Add(custom);
                    }
                    else
                    {
                        //3.寫入OR_CUST_DATA_CHG_REC
                        _db.OR_CUST_DATA_CHG_REC.Add(oREC);
                    }

                    if (Mode == "EDIT")
                    {
                        strMessage = "修改";
                    }
                    else
                    {
                        strMessage = "新增";
                    }

                    _db.SaveChanges();
                }
                TimeSpan t2 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan timeDiff = t1.Subtract(t2).Duration();

                _logger.DebugFormat("Function:{0}, 分:{1}, 豪秒:{2}, Para:{2}", "儲存    ", timeDiff.Minutes.ToString(), timeDiff.Milliseconds.ToString(), "DetailData");
                return Json(new { Error = "Success", Message = strMessage + "處理成功!", CUST_NO = model.CUST_NO, CUST_NAME = model.CUST_NAME }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("{0}處理失敗!, Message:{1}", strMessage, ex.ToString());
                return Json(new { Error = "Error", Message = strMessage + "處理失敗!" }, JsonRequestBehavior.AllowGet);
            }
        }

        //取得ComboGrid
        public string getOrBloc(string page, string rows, string BLOC_NO)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            //頁數
            int PageNum = Convert.ToInt16(page);
            //每頁幾筆
            int Record = Convert.ToInt16(rows);
            //第幾筆
            int Strat = ((PageNum - 1) * Record) + 1;
            string strJson = "";

            using (ORIXContext _db = new ORIXContext())
            {
                var Count = _db.OR_BLOC
                    .Where(x => string.IsNullOrEmpty(BLOC_NO) || x.BLOC_NO.Contains(BLOC_NO)).Count();

                var formateData = _db.OR_BLOC
                    .Where(x => string.IsNullOrEmpty(BLOC_NO) || (x.BLOC_NO.Contains(BLOC_NO) || x.BLOC_NAME.Contains(BLOC_NO) || x.BLOC_SNAME.Contains(BLOC_NO)))
                    .Select(x => new WE0101VO.BLOC
                    {
                        KEY_NO = x.BLOC_NO,
                        BLOC_NAME = x.BLOC_NAME,
                        BLOC_SNAME = x.BLOC_SNAME
                    }).OrderBy(x => x.KEY_NO).Skip(Record * (PageNum - 1)).Take(Record);
                string strData = ConvertToJson(formateData, Count, Strat);
                strJson = "{\"total\":" + Count + ",\"rows\":" + strData + "}";
            }
            return strJson;
        }

        [HttpPost]
        //資料檢核
        public ActionResult checkDate(string Mode, string UnifNo, string CustNo, string CustName)
        {
            string strMessage = "";
            if (UnifNo != "")
            {
                int strUnifNo = 0;
                using (ORIXContext _db = new ORIXContext())
                {
                    strUnifNo = _db.OR_CUSTOM.Where(x => x.UNIF_NO == UnifNo)
                                             .Where(x => x.CUST_NO != CustNo)
                                             .Count();
                }
                if (strUnifNo > 0)
                {
                    strMessage = "統編重複，請查詢修改資料或更正統一編號！";
                    return Json(new { Status = "Error", Massage = strMessage }, JsonRequestBehavior.AllowGet);
                }
            }

            if (Mode == "ADD")//TODO: || this.bolWF0101 == true)
            {
                if (CustNo != "")
                {
                    int strCustNo = 0;
                    using (ORIXContext _db = new ORIXContext())
                    {
                        strCustNo = _db.OR_CUSTOM.Where(x => x.CUST_NO == CustNo)
                                                  .Count();
                    }
                    if (strCustNo > 0)
                    {
                        strMessage = "客戶代號已存在,請確認!";
                        return Json(new { Status = "Error", Massage = strMessage }, JsonRequestBehavior.AllowGet);
                    }
                }

                if (CustName != "")
                {
                    int strCustName = 0;
                    using (ORIXContext _db = new ORIXContext())
                    {
                        strCustName = _db.OR_CUSTOM.Where(x => x.CUST_NO != CustNo)
                                                   .Where(x => x.CUST_NAME == CustName)
                                                   .Count();
                    }
                    if (strCustName > 0)
                    {
                        strMessage = "此客戶名稱已存在，是否確定儲存";
                        return Json(new { Status = "confirm", Massage = strMessage }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);
        }

        //檢核有異動的資料
        public string diffData(string strBefore, string strAfter, string strName)
        {
            string log = "";
            if (strBefore == null)
            {
                strBefore = "";
            }

            if (strAfter != null && strBefore.Trim() != strAfter.Trim())
            {
                log = strName + ":" + strBefore + "→" + strAfter + ",";
            }
            return log;
        }

        //檢核有異動的數值
        public string diffNumber(decimal strBefore, decimal strAfter, string strName)
        {
            string log = "";

            if (strBefore != strAfter)
            {
                log = strName + ":" + strBefore + "→" + strAfter + ",";
            }
            return log;
        }

        //日期Format
        public string DateFormat(string strDate)
        {
            string[] YMD = strDate.Split('/');
            return YMD[0] + YMD[1].PadLeft(2, '0') + YMD[2].PadLeft(2, '0');
        }

        //轉成Json
        public string ConvertToJson(IQueryable<WE0101VO.BLOC> lData, int Count, int Strat)
        {
            string strValue = "[\r\n  ";
            foreach (var d in lData)
            {
                strValue += "{\r\n    \"cnt\":" + Count + ",\r\n    \"Row\":" + Strat + ",\r\n    ";
                strValue += "\"KEY_NO\": \"" + d.KEY_NO + "\",\r\n";
                strValue += "\"BLOC_NAME\": \"" + d.BLOC_NAME + "\",\r\n";
                strValue += "\"BLOC_SNAME\": \"" + d.BLOC_SNAME + "\"\r\n  },";
                Strat++;
            }
            strValue = strValue.TrimEnd(',');
            strValue += "]";
            return strValue;
        }
        #endregion
    }
}
