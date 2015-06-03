using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using VS2008.Module;
using System.Data;
using Newtonsoft.Json;

namespace OrixMvc
{
    /// <summary>
    /// $codebehindclassname$ 的摘要描述
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ComboService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            

            string strValue = "";
            string strSource="";
            string strItem = "";


            if (context.Request.QueryString["SourceTable"] != null)
                strSource = context.Request.QueryString["SourceTable"].ToString();

            if (context.Request.QueryString["Item"] != null)
                strItem = context.Request.QueryString["Item"].ToString();

            if (context.Request.Form["q"] != null)
                strValue = context.Request.Form["q"].Trim();

            string strSQL = "", queryString="",sort="";
            switch (strSource)
            {
                case "OR_CUSTOM":
                    queryString += " select KEY_NO=CUST_NO,CUST_NAME,CUST_SNAME,EN_NAME from OR_CUSTOM";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where CUST_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where CUST_NO like '%" + strValue + "%' or CUST_SNAME like '%" + strValue + "%'  or CUST_NAME like '%" + strValue + "%' ";


                    break;

                case "OR_BLOC":
                    queryString += " select KEY_NO=BLOC_NO,BLOC_NAME,BLOC_SNAME from OR_BLOC";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where BLOC_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where BLOC_NO like '%" + strValue + "%' or BLOC_NAME like '%" + strValue + "%'  or BLOC_SNAME like '%" + strValue + "%' ";

                    break;
                case "OR_FRC":
                    queryString += " select KEY_NO=FRC_CODE,FRC_NAME,FRC_SNAME from OR_FRC";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where FRC_CODE='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where FRC_CODE like '%" + strValue + "%' or FRC_NAME like '%" + strValue + "%'  or FRC_SNAME like '%" + strValue + "%' ";

                    break;

                case "OR_CASE_APLY_BASE":
                    queryString += " SELECT KEY_NO=APLY_NO,APLY_DATE,a.CUST_NO,b.CUST_SNAME FROM OR_CASE_APLY_BASE a left join OR_CUSTOM b on a.CUST_NO=b.CUST_NO";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where APLY_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where APLY_NO like '%" + strValue + "%' or APLY_DATE like '%" + strValue + "%'  or a.CUST_NO like '%" + strValue + "%'  or CUST_SNAME like '%" + strValue + "%' ";

                    break;
                case "OR_RECV_PAPER":
                    queryString += " SELECT distinct KEY_NO=a.APLY_NO,c.APLY_DATE,a.CUST_NO,b.CUST_SNAME FROM OR_RECV_PAPER a left join OR_CUSTOM b on a.CUST_NO=b.CUST_NO left join OR_CASE_APLY_BASE c on a.APLY_NO=c.APLY_NO";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where a.APLY_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where a.APLY_NO like '%" + strValue + "%' or c.APLY_DATE like '%" + strValue + "%'  or a.CUST_NO like '%" + strValue + "%'  or CUST_SNAME like '%" + strValue + "%' ";

                    break;
                case "OR_APLY_WA060":
                    queryString += " select KEY_NO=A.APLY_NO ,CON_SEQ_NO ,APLY_DATE ,CUR_STS from OR_CASE_APLY_BASE A inner join OR_CASE_APLY_EXE_COND B on A.aply_no=B.aply_no";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where APLY_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += " where A.APLY_NO like '%" + strValue + "%' or CON_SEQ_NO like '%" + strValue + "%' order by A.APLY_NO";

                    break;
                case "V_OR_EMP":
                    queryString += " SELECT KEY_NO=CORP_ACCT,EMP_NAME,EMP_ENAME FROM V_OR_EMP";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where CORP_ACCT='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (CORP_ACCT like '%" + strValue + "%' or EMP_NAME like '%" + strValue + "%'  or EMP_ENAME like '%" + strValue + "%') and CORP_ACCT!='' ";

                    break;

                case "OR_EMP":
                    queryString += " SELECT KEY_NO=CORP_ACCT,EMP_NAME,EMP_ENAME FROM V_OR_EMP";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where CORP_ACCT='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (CORP_ACCT like '%" + strValue + "%' or EMP_NAME like '%" + strValue + "%'  or EMP_ENAME like '%" + strValue + "%') and CORP_ACCT!='' ";


                    break;


                case "ACC18":
                    queryString += " SELECT KEY_NO=BANK_NO,BANK_NAME FROM ACC18";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where BANK_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (BANK_NO like '%" + strValue + "%' or BANK_NAME like '%" + strValue + "%'  ) and BANK_NO!='' ";

                    break;

                case "SALES":
                    queryString += " SELECT KEY_NO=SALES,SALES1 FROM [業務員資料表]";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where SALES='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (SALES like '%" + strValue + "%' or SALES1 like '%" + strValue + "%'  ) and SALES!='' ";


                    break;

                case "OR_DEPT":
                    queryString += " SELECT KEY_NO=DEPT_CODE,DEPT_NAME FROM [OR_DEPT]";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where DEPT_CODE='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (DEPT_CODE like '%" + strValue + "%' or DEPT_NAME like '%" + strValue + "%'  ) and DEPT_NAME!='' ";

                    break;

                case "OR_MERG_MAIL":
                    queryString += " SELECT KEY_NO=MMAIL_NO,MMAIL_NAME FROM [OR_MERG_MAIL]";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where MMAIL_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (MMAIL_NO like '%" + strValue + "%' or MMAIL_NAME like '%" + strValue + "%'  ) and MMAIL_NAME!='' ";


                    break;

                /*   申請書編號	"""select A.APLY_NO 申請書編號,CON_SEQ_NO 契約編號,APLY_DATE 申請日期,CUR_STS [*目前狀況] from OR_CASE_APLY_BASE A inner join OR_CASE_APLY_EXE_COND B on A.aply_no=B.aply_no where A.APLY_NO like '%"" _
                 *  /*  & Trim(Screen.ActiveForm.DataField(CAPLY_NO)) & ""%' or CON_SEQ_NO like '%"" & Trim(Screen.ActiveForm.DataField(CAPLY_NO)) & ""%' order by A.APLY_NO"""
                 */
                case "OR_AUD_LVL_NAME":
                    queryString += "select KEY_NO=AUD_LVL_CODE ,AUD_LVL_NAME  from OR_AUD_LVL_NAME ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where AUD_LVL_CODE='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (AUD_LVL_CODE like '%" + strValue + "%' or AUD_LVL_NAME like '%" + strValue + "%'  ) and AUD_LVL_CODE!='' ";

                    break;

                case "OR_CASE_TYPE":
                    queryString += "select KEY_NO=CASE_TYPE_CODE ,CASE_TYPE_NAME,CASE WHEN CASE_STS='O' THEN '營業型' ELSE CASE WHEN CASE_STS='C' THEN '資本型' ELSE CASE WHEN CASE_STS='S'THEN '分期' ELSE '應收受讓' END END END Type  from OR_CASE_TYPE ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where CASE_TYPE_CODE='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (CASE_TYPE_CODE like '%" + strValue + "%' or CASE_TYPE_NAME like '%" + strValue + "%'  ) and CASE_TYPE_CODE!='' ";

                    break;

                case "OR_CASE_CAL":
                    queryString += "select KEY_NO=CAL_NO ,CAL_NAME  from OR_CASE_CAL";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where CAL_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (CAL_NO like '%" + strValue + "%' or CAL_NAME like '%" + strValue + "%'  ) and CAL_NO!='' ";

                    break;

                case "OR_CASE_APLY_BASE_060":
                    queryString += " select KEY_NO=A.APLY_NO ,CON_SEQ_NO ,APLY_DATE ,CUR_STS  from OR_CASE_APLY_BASE A inner join OR_CASE_APLY_EXE_COND B on A.aply_no=B.aply_no ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where A.APLY_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (A.APLY_NO like '%" + strValue + "%' or CON_SEQ_NO like '%" + strValue + "%'  ) and A.APLY_NO!='' ";

                    break;

                case "OR_DEPT_060":
                    queryString += " select DEPT_CODE 部門代碼,DEPT_NAME 部門名稱 from OR_DEPT where ltrim(DEPT_CODE) <>'' and CORP_CODE='OTC' ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where DEPT_CODE='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (DEPT_CODE like '%" + strValue + "%' or DEPT_NAME like '%" + strValue + "%'  ) and DEPT_CODE!='' ";

                    break;


                case "OR_ASUR_TYPE":
                    queryString += " select KEY_NO=ASUR_TYPE_CODE ,ASUR_TYPE_NAME from OR_ASUR_TYPE  ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where ASUR_TYPE_CODE='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (ASUR_TYPE_CODE like '%" + strValue + "%' or ASUR_TYPE_NAME like '%" + strValue + "%'  ) and ASUR_TYPE_CODE!='' ";

                    break;

                case "OR_ATCH_CODE":
                    queryString += " select KEY_NO=ATCH_CODE,ATCH_NAME from OR_ATCH_CODE  ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where ATCH_CODE='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (ATCH_CODE like '%" + strValue + "%' or ATCH_NAME like '%" + strValue + "%'  ) and ATCH_CODE!='' ";

                    break;

                case "OLD_APLY_NO":
                   
                    queryString += "  select KEY_NO=b.APLY_NO,a.OBJ_CODE,OBJ_LOC_ADDR,a.PROD_NAME,a.OTC,a.MAC_NO,a.BRAND,a.FRC_CODE,c.FRC_SNAME";
                    queryString += " from OR_OBJECT  a left join OR_CASE_APLY_OBJ b on a.OBJ_CODE=b.OBJ_CODE ";
                    queryString += "  left join OR_FRC c on a.FRC_CODE=c.FRC_CODE ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where b.APLY_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (b.APLY_NO like '%" + strValue + "%' or a.OBJ_CODE like '%" + strValue + "%'  ) and b.APLY_NO!='' ";

                    break;
                /*

新部門	"select DEPT_CODE 部門代碼,DEPT_NAME 部門名稱 from OR_DEPT where ltrim(DEPT_CODE) <>'' order by DEPT_CODE"
新經辦	"""select EMP_CODE 員工代號,EMP_NAME 姓名 from V_OR_EMP where (EMP_CODE like '%"" _
                   & Trim(Screen.ActiveForm.DataField(CNEW_EMP)) & ""%' or EMP_name like '%"" & Trim(Screen.ActiveForm.DataField(CNEW_EMP)) & ""%') order by EMP_NAME"""
經辦	"""select EMP_CODE 員工代號,EMP_NAME 姓名,corp_acct as 社內帳號 from V_OR_EMP where (EMP_CODE like '%"" _
                   & Trim(Screen.ActiveForm.DataField(CEMP_CODE)) & ""%' or EMP_name like '%"" & Trim(Screen.ActiveForm.DataField(CEMP_CODE)) & ""%') order by EMP_NAME"""
經辦代號	"select EMP_CODE 員工代號,EMP_NAME 姓名,corp_acct as 社內帳號 from V_OR_EMP where dept_code<'5' "
經辦社內帳號	"""select corp_acct as 社內帳號,EMP_CODE [*員工代號],EMP_NAME 姓名 from V_OR_EMP where  dept_code<'5' and corp_acct<>'' and inc_sts='1' and (corp_acct like '%"" _
                   & Trim(Screen.ActiveForm.DataField(CEMP_ACCT)) & ""%' or EMP_name like '%"" & Trim(Screen.ActiveForm.DataField(CEMP_ACCT)) & ""%') order by inc_sts desc,emp_name"""
	

                 * 
保險種類	"select ASUR_TYPE_CODE 保險種類代號,ASUR_TYPE_NAME 保險種類名稱, ASUR_payer 負擔者 from OR_ASUR_TYPE where ltrim(ASUR_TYPE_CODE) <>'' order by ASUR_TYPE_CODE"
                                     * 
案件審查類別	"select CAL_NO 代號,CAL_NAME 審查類別 from OR_CASE_CAL where ltrim(CAL_NO) <>'' order by CAL_NO"
案件類別	"select CASE_TYPE_CODE 案件類別代號,CASE_TYPE_NAME 案件類別名稱,CASE WHEN CASE_STS='O' THEN '營業型' ELSE CASE WHEN CASE_STS='C' THEN '資本型' ELSE CASE WHEN CASE_STS='S'THEN '分期' ELSE '應收受讓' END END END 類型  from OR_CASE_TYPE where ltrim(CASE_TYPE_CODE) <>'' order by CASE_TYPE_CODE"
授權類別	"select AUD_LVL_CODE 審查代號,AUD_LVL_NAME 審查名稱 from OR_AUD_LVL_NAME where ltrim(AUD_LVL_CODE) <>'' order by AUD_LVL_CODE"
標的物代號	"select OBJ_CODE,OBJ_LOC_ADDR from OR_OBJECT where ltrim(OBJ_CODE) <>'' order by OBJ_CODE"

審查級別代號	"select AUD_LVL_CODE 審查級別代號,AUD_LVL_NAME 級別名稱 from OR_AUD_LVL_NAME where ltrim(AUD_LVL_CODE) <>'' order by AUD_LVL_CODE"
保險種類	"select ASUR_TYPE_CODE 保險種類代號,ASUR_TYPE_NAME 保險種類名稱, ASUR_payer 負擔者,asur_factor [*保險因子]  from OR_ASUR_TYPE where ltrim(ASUR_TYPE_CODE) <>'' order by ASUR_TYPE_CODE"

與申戶關係	"select toge_no 與申戶關係,toge_name 名稱 from or_common_code where toge_group='A01' and (end_date is null or end_date='') order by seq"
保險種類	"select ASUR_TYPE_CODE 保險種類代號,ASUR_TYPE_NAME 保險種類名稱, ASUR_payer 負擔者 from OR_ASUR_TYPE where ltrim(ASUR_TYPE_CODE) <>'' order by ASUR_TYPE_CODE"
*/



            }

            sort = "KEY_NO";
            if (queryString == "")
                return;

            int intPage = context.Request.Form["page"].toInt();
            int intRow = context.Request.Form["rows"].toInt();

            int start = ((intPage - 1) * intRow)+1;
            int end = intPage  * intRow;
            strSQL += "  declare @cnt int ";
            strSQL += "select @cnt=COUNT(*) from ";
            strSQL += " ( ";
            strSQL += queryString;
            strSQL += " ) S  ";
            strSQL += " select @CNT cnt,* from ( select ROW_NUMBER() OVER(ORDER BY " + sort + " ) AS Row,* FROM (";
            strSQL += queryString;
            strSQL += " ) Ss ) sss where row between " + start.ToString() + " and " + end.ToString();


            DataGetting dg = new DataGetting("myConnectionString");
            DataTable dt = dg.GetDataTable(strSQL);//select top 100 wfc_mem_no productid ,mem_name productname,out_date unitcost, status,'' listprice, '' attr1, wfc_mem_no itemid from wfc_member_main where wfc_mem_no like '%" + str + "%' ");
            string resultJSON = String.Empty;
            resultJSON = JsonConvert.SerializeObject(dt, Formatting.Indented);

            int intRows = 0;
            if (dt.Rows.Count>0)
                intRows=dt.Rows[0]["cnt"].ToString().toInt();
            context.Response.Clear();
            context.Response.ClearContent();
            resultJSON = "{\"total\":" + intRows + ",\"rows\":" + resultJSON + "}";
            context.Response.Write(resultJSON);



            //{"productid":"FI-SW-01","productname":"Koi","unitcost":10.00,"status":"P","listprice":36.50,"attr1":"Large","itemid":"EST-1"},
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
