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
    public class DialogService : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            

            string strValue = "";
            string strSource="";
            string strItem = "";
            bool bolPage = true;
            bool bolReturn = false;
            if (context.Request.QueryString["SourceTable"] != null)
                strSource = context.Request.QueryString["SourceTable"].ToString();

            if (context.Request.QueryString["Item"] != null)
                strItem = context.Request.QueryString["Item"].ToString();

            if (context.Request.Form["q"] != null)
                strValue = context.Request.Form["q"].Trim();

            string strSQL = "", queryString="",sort="";
            switch (strSource)
            {
                case "OR3_FRC_SALES":
                    queryString += " select MOBILE as KEY_NO from OR3_FRC_SALES where  FRC_SALES_NAME='" + strItem.Split(',')[1].ToString() + "'";
                    bolPage = false;

                    break;

                case "otc_proced_content":
                    queryString += " select main as KEY_NO from orix_otc..otc_proced_content where PROCED_APLY_NO='" + strItem + "' and APLY_COM='OTC'";
                    bolPage = false;

                    break;


                case "OR3_HINT":
                    //strItem = URLDecoder.decode(strItem, "utf-8");
                    queryString += strItem;
                    bolPage = false;
                    bolReturn = true;

                    break;

                case "OR_CUSTOM":
                    queryString += " select KEY_NO=CUST_NO,CUST_NAME,CUST_SNAME,EN_NAME,REAL_CAPT_AMT=replace(convert(varchar,cast(REAL_CAPT_AMT as money),1), '.00','') from OR_CUSTOM";
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

                case "OR_CASE_APLY_BASE_WD010":
                    queryString += " SELECT KEY_NO=APLY_NO,CUR_STS=dbo.f_ConditionGetDesc('AplySts',CUR_STS,'N'),APLY_DATE,a.CUST_NO,b.CUST_SNAME FROM OR_CASE_APLY_BASE a left join OR_CUSTOM b on a.CUST_NO=b.CUST_NO where PAPER='Y' ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " and  APLY_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  and APLY_NO like '%" + strValue + "%'  ";

                    break;

                case "OR_CASE_APLY_BASE_WD040":
                    queryString += " select distinct KEY_NO=a.aply_no,CUR_STS=dbo.f_ConditionGetDesc('AplySts',CUR_STS,'N'),b.APLY_DATE,b.CUST_NO,c.CUST_SNAME  from OR3_PAPER_USE_DTL_TMP a left join OR_CASE_APLY_BASE b on a.APLY_NO=b.APLY_NO  left join OR_CUSTOM c on b.CUST_NO=c.CUST_NO";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " and  a.APLY_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  and a.APLY_NO like '%" + strValue + "%'  ";

                    break;

                case "OR_CASE_APLY_BASE":
                    queryString += " SELECT KEY_NO=APLY_NO,CUR_STS=dbo.f_ConditionGetDesc('AplySts',CUR_STS,'N'),APLY_DATE,a.CUST_NO,b.CUST_SNAME FROM OR_CASE_APLY_BASE a left join OR_CUSTOM b on a.CUST_NO=b.CUST_NO";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where APLY_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where APLY_NO like '%" + strValue + "%'  ";

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
                    string strDEPT = "";
                    string[] aryEmp = strValue.Split(',');
                    if (strValue.IndexOf(",") != -1)
                    {
                        strDEPT = aryEmp[0].ToString().Trim();
                        strValue = aryEmp[1].ToString().Trim();
                    }
                    queryString += " SELECT KEY_NO=CORP_ACCT,EMP_NAME,EMP_ENAME FROM V_OR_EMP";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where CORP_ACCT='" + strItem + "'";
                    else if (strValue != "" && aryEmp.Length == 1)
                        queryString += "  where (CORP_ACCT like '%" + strValue + "%' or EMP_NAME like '%" + strValue + "%'  or EMP_ENAME like '%" + strValue + "%') and CORP_ACCT!='' ";

                    if (strDEPT != "")
                        queryString += (queryString.IndexOf("where") != -1 ? " and" : " where") + " DEPT_CODE='" + strDEPT + "'";
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
                    queryString += " SELECT KEY_NO=DEPT_CODE,DEPT_NAME FROM [v_OR_DEPT]";
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
                    queryString += " from OR_OBJECT  a inner join OR_CASE_APLY_OBJ b on a.OBJ_CODE=b.OBJ_CODE ";
                    queryString += "  left join OR_FRC c on a.FRC_CODE=c.FRC_CODE ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where b.APLY_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (b.APLY_NO like '%" + strValue + "%' or a.OBJ_CODE like '%" + strValue + "%'  ) and b.APLY_NO!='' ";

                    break;

                case "OR_BANK_AMT":
                    queryString += "select SeqNo,KEY_NO=a.BANK_NO,BANK_TYPE=(case a.BANK_TYPE when '1' then '本國銀行' when '2' then '票券金融' when '3' then '外商銀行' when '4' then '其他' end),b.BANK_NAME,CRD_AMT,USED_CREDIT,REST_CREDIT=CRD_AMT-USED_CREDIT,CRD_DATE_TO=dbo.f_DateAddSlash(CRD_DATE_TO),AMT_LONG_SHORT_LOAN=(case Long_Short_LOAN WHEN 1 then '綜合' WHEN 2 THEN '長借' ELSE '短借' END ) ";
                    queryString += " from OR_BANK_AMT a left join ACC18 b on a.BANK_NO=b.BANK_NO ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where a.BANK_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where (a.BANK_NO like '%" + strValue + "%' or b.BANK_NAME like '%" + strValue + "%'  ) and a.BANK_NO!='' ";

                    break;

                case "OR3_MASTER_CONTRACT":
                    string strDEPT1 = "";
                    string strCUST1 = "";
                    if (strValue.IndexOf(",") != -1)
                    {
                        string[] aryValue = strValue.Split(',');
                        strDEPT1 = aryValue[0].ToString();
                        strCUST1 = aryValue[1].ToString();
                        strValue = aryValue[2].ToString();
                    }
                    queryString += " select KEY_NO=MAST_CON_NO from OR3_MASTER_CONTRACT ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " where MAST_CON_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  where MAST_CON_NO like '%" + strValue + "%' and MAST_CON_NO!='' ";

                    if (strDEPT1 != "" )
                        queryString += (queryString.IndexOf("where") != -1 ? " and" : " where") + " DEPT_CODE='" + strDEPT1.Trim() + "'";

                    if (strCUST1 != "")
                        queryString += (queryString.IndexOf("where") != -1 ? " and" : " where") + " CUST_NO='" + strCUST1.Trim() + "'";
                    break;

                case "OR3_QUOTA_APLY_BASE":

                    string strCUST = "";
                    if (strValue.IndexOf(",") != -1)
                    {
                        string[] aryValue = strValue.Split(',');
                        strCUST = aryValue[0].ToString();
                        strValue = aryValue[1].ToString();
                    }
                    queryString += " select KEY_NO=QUOTA_APLY_NO from OR3_QUOTA_APLY_BASE where 1=1 ";
                    if (context.Request.Form["q"] == null && strItem != "")
                        queryString += " and QUOTA_APLY_NO='" + strItem + "'";
                    else if (strValue != "")
                        queryString += "  and QUOTA_APLY_NO like '%" + strValue + "%' and QUOTA_APLY_NO!='' ";

                    if (strCUST != "")
                        queryString += " AND CUST_NO='" + strCUST + "'";
                    break;

                case "OR_APLY_WH020":
                    queryString = "  select distinct CAB.aply_no as KEY_NO ";
                    queryString += " FROM OR3_FUNDS,  ";
                    queryString += " OR_CASE_APLY_BASE as CAB  ";
                    queryString += " INNER JOIN OR_CASE_APLY_EXE_COND as CAEC ON CAB.APLY_NO = CAEC.APLY_NO  ";
                    queryString += " INNER JOIN OR_EMP ON CAB.EMP_CODE = OR_EMP.EMP_CODE  ";
                    queryString += " INNER JOIN 業務員資料表 as SALES ON OR_EMP.EMP_NAME = SALES.SALES1  ";
                    queryString += " WHERE OR3_FUNDS.START<=CAEC.[APRV_DURN_M] ";
                    queryString += " AND OR3_FUNDS.[END]>=CAEC.[APRV_DURN_M]  ";
                    queryString += " AND CAB.CUR_STS='a'  ";
                    queryString += " AND SALES.SP_ENABLE=1";
                    break;





            }

            sort = "KEY_NO";
            if (queryString == "")
                return;

            if (bolPage)
            {
                int intPage = context.Request.Form["page"].toInt();
                int intRow = context.Request.Form["rows"].toInt();

                int start = ((intPage - 1) * intRow) + 1;
                int end = intPage * intRow;
                strSQL += "  declare @cnt int ";
                strSQL += "select @cnt=COUNT(*) from ";
                strSQL += " ( ";
                strSQL += queryString;
                strSQL += " ) S  ";
                strSQL += " select @CNT cnt,* from ( select ROW_NUMBER() OVER(ORDER BY " + sort + " ) AS Row,* FROM (";
                strSQL += queryString;
                strSQL += " ) Ss ) sss where row between " + start.ToString() + " and " + end.ToString();
            }
            else
                strSQL = queryString;

            DataGetting dg = new DataGetting("myConnectionString");
            DataTable dt = dg.GetDataTable(strSQL);//select top 100 wfc_mem_no productid ,mem_name productname,out_date unitcost, status,'' listprice, '' attr1, wfc_mem_no itemid from wfc_member_main where wfc_mem_no like '%" + str + "%' ");
            if (bolReturn)
                return;

            string resultJSON = String.Empty;
            resultJSON = JsonConvert.SerializeObject(dt, Formatting.Indented);

            int intRows = 0;
           
            if (dt.Rows.Count > 0 && bolPage)
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
