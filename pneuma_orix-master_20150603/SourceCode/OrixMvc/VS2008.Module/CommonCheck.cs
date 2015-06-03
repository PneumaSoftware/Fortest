using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Collections;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Security.Cryptography;
using System.Web.Configuration;
using System.Web;

namespace VS2008.Module
{
    #region CommonCheck 驗證函式

    /// <summary>
    /// Common library
    /// Author      ：  Alinta 
    /// Build Date  ：  2008/03/28 
    /// Modify Date ：  2008/03/28 by Alinta 
    /// Purpose     ： １．提供所有函式驗證(包含擴充型別)，不含資料庫連結處理
    ///                ２．提供畫面上欄位的處理：   1.清空資料
    ///                                             2.是否開放編輯
    ///                                             3.欄位不可空白驗證
    ///                                             4.欄位避掉單引號
    /// </summary>         
    public static class CommonCheck
    {

        /// <summary>
        /// 以MD5進行加密
        /// </summary>
        /// <param name="myString">加密來源字串</param>
        /// <returns>加密後字串</returns>
        public static string GetMD5(this string myString)
        {
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(myString));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();

        }
        
        ///<summary>Alinta：驗證身份證號, 回傳true/false</summary>
        ///<remarks></remarks>   
        ///<param name="strIDNo">身份證號</param>           
        ///<value>回傳true/false</value>            
        public static bool IsIDNo(this string strIDNo)
        {
            int intSum = 0;

            if (strIDNo.Length != 10)
                return false;

            string strChkID = "ABCDEFGHJKLMNPQRSTUVXYWZIO";

            if (strChkID.IndexOf(strIDNo.Substring(0, 1)) == -1)
                return false;

            if (!IsNumeric(strIDNo.Substring(1, 9)))
                return false;

            strIDNo = strChkID.IndexOf(strIDNo.Substring(0, 1)) + 10 + strIDNo.Substring(1, 9);

            for (int i = 0; i < 11; i++)
            {
                intSum += Int32.Parse(strIDNo.Substring(i, 1).ToString()) * ((i != 0 && i != 10) ? (10 - i) : 1);
            }

            if (intSum % 10 == 0)
                return true;
            else
                return false;
        }


        /// <summary>
        /// 壓縮資料夾檔案到根目錄下
        /// </summary>
        /// <param name="SourceDir"></param>
        public static void ZipDir(string SourceDir)
        {
            if (SourceDir.Substring(SourceDir.Length - 1, 1) == "/")
                SourceDir = SourceDir.Substring(0, SourceDir.Length - 1);

            string strFile = SourceDir + ".zip";
            //判斷要產生的ZIP檔案是否存在
            if (File.Exists(strFile) == true)
                File.Delete(strFile);
            

            ZipDir(SourceDir, strFile, "", false);
        }

        /// <summary>
        /// 壓縮資料夾檔案
        /// </summary>
        /// <param name="SourceDir"></param>
        /// <param name="TargetFile"></param>
        /// <param name="Password"></param>
        /// <param name="BackupOldFile"></param>
        private static void ZipDir(string SourceDir, string TargetFile, string Password, bool BackupOldFile)
        {
            ICSharpCode.SharpZipLib.Zip.FastZip oZipDir = new ICSharpCode.SharpZipLib.Zip.FastZip();
            try
            {
                if (!Directory.Exists(SourceDir))
                {
                    throw new Exception("資料夾不存在!");
                }

                if (BackupOldFile == true)
                {
                    //判斷要產生的ZIP檔案是否存在
                    if (File.Exists(TargetFile) == true)
                    {
                        //原本的檔案存在，把他ReName
                        File.Copy(TargetFile, TargetFile + "-" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".back");
                        File.Delete(TargetFile);
                    }
                }

                if (Password != "")
                {
                    oZipDir.Password = Password;
                }
                oZipDir.CreateZip(TargetFile, SourceDir, true, "");
            }
            catch
            {
                throw;
            }
        }

        ///<summary>Alinta：驗證統一編號, 回傳true/false</summary>
        ///<remarks></remarks>   
        ///<param name="strGUINo">統一編號</param>           
        ///<value>回傳true/false</value>    
        public static bool IsGUINO(this string strGUINo)
        {
            int[] iX = new int[8];
            int[] iY = new int[8];
            int iMod;
            int iSum;

            if (strGUINo.Length != 8) { return false; }
            try
            {
                /*取得8個X值	第1位數 * 1/第2位數 * 2/第3位數 * 1/第4位數 * 2
                                第5位數 * 1/第6位數 * 2/第7位數 * 4/第8位數 * 1	 */

                int[] iC = new int[8] { 1, 2, 1, 2, 1, 2, 4, 1 };
                
                for (int i = 0; i < 8; i++)
                {
                    iX[i] = int.Parse(strGUINo.Substring(i, 1)) * iC[i];
                }
                 

                iY[0] = iX[1] / 10;		// 第 2位數的乘積可能大於10, 除以10, 取其整數
                iY[1] = iX[1] % 10;     // 第 2位數的乘積可能大於10, 除以10, 取其餘數
                iY[2] = iX[3] / 10;		// 第 4位數的乘積可能大於10, 除以10, 取其整數
                iY[3] = iX[3] % 10; 	// 第 4位數的乘積可能大於10, 除以10, 取其餘數
                iY[4] = iX[5] / 10;		// 第 6位數的乘積可能大於10, 除以10, 取其整數
                iY[5] = iX[5] % 10; 	// 第 6位數的乘積可能大於10, 除以10, 取其整數
                iY[6] = iX[6] / 10;		// 第 7位數的乘積可能大於10, 除以10, 取其整數
                iY[7] = iX[6] % 10; 	// 第 7位數的乘積可能大於10, 除以10, 取其整數

                iSum = iX[0] + iX[2] + iX[4] + iX[7] + iY[0] + iY[1] + iY[2] + iY[3] + iY[4] + iY[5] + iY[6] + iY[7];
                iMod = iSum % 10; ;

                if (iMod == 0) { return true; }

                //令iSum=iSum+1再再除以10取得餘數
                iSum = iSum + 1;
                iMod = iSum % 10; ;

                //判斷 1: 第 7位數是否為 7 ,餘數是否為0
                if (strGUINo.Substring(6, 1) == "7" && iMod == 0) { return true; }
            }
            catch
            {
                return false;
            }
            return false;
            
        }

        /// <summary>
        /// Alinta：網頁的擴充型別，避免User直接用URL登錄
        /// </summary>        
        /// <param name="myPage">網頁本身</param>
        /// <param name="strURL">URL路徑</param>
        public static void openProgram(this System.Web.UI.Page myPage,string strURL)
        {
            myPage.Session["openURL"] = true;
            myPage.Response.Redirect(strURL); 
        }       

        /// <summary>
        /// Alinta：將文字型態轉為數字型態, ","要避掉
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static double toNumber(this string strNumber)
        {
            try
            {
                double intNumber = double.Parse(strNumber.Replace(",", ""));
                return intNumber;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Alinta：將文字型態轉為數字型態, ","要避掉
        /// </summary>
        /// <param name="strNumber"></param>
        /// <returns></returns>
        public static  int  toInt(this string strNumber)
        {
            try
            {
                int intNumber = Int32.Parse(strNumber.Replace(",", ""));
                return intNumber;
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Alinta：將DropDownList裏的Item value轉為sql 字串,以提供給副程式下sql 條件值
        /// </summary>
        /// <param name="dp">DropDownList</param>
        /// <returns></returns>
        public static string toSqlString(this DropDownList dp)
        {
            try
            {
                string strSQL = "";

                for (int i = 0; i < dp.Items.Count; i++)
                {
                    if (dp.Items[i].Value.ToString()!="")
                        strSQL +=(strSQL==""?"":",")+ "'"+ dp.Items[i].Value  +"'";
                }

                if (strSQL!="")
                    return "("+strSQL+")" ;
                else
                    return "('')";
            }
            catch
            {
                return "('')";
            }
        }


        /// <summary>
        /// Alinta：將datatable中的資料匯出至excel
        /// </summary>
        /// <param name="srcDataTable">資料表來源</param>
        public static void ExportToExcel(this System.Data.DataTable dt,string strName)
        {
            
            HttpContext myPage = System.Web.HttpContext.Current;

            StringWriter sw = new StringWriter();

            string strHeader = "";
            string strBody = "";

            for (int i = 0; i < dt.Columns.Count; i++)
            {
                strHeader = strHeader + (i != 0 ? "\t" : "") + dt.Columns[i].ColumnName;
            }

            sw.WriteLine(strHeader);

            foreach (DataRow dr in dt.Rows)
            {
                strBody = "";
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    strBody = strBody + (i != 0 ? "\t" : "") + dr[dt.Columns[i].ColumnName].ToString();
                }

                sw.WriteLine(strBody);
            }

            sw.Close();
            myPage.Response.AddHeader("Content-Disposition", "attachment; filename=" + strName + ".xls");
            myPage.Response.ContentType = "application/ms-excel";
            myPage.Response.ContentEncoding = System.Text.Encoding.Default;
            myPage.Response.Write(sw);
            myPage.Response.End(); 
        }

        /// <summary>
        /// Alinta：刪除DataTable裏的所有筆數
        /// </summary>
        /// <param name="srcDataTable">資料表來源</param>
        public static void DeleteRows(this System.Data.DataTable srcDataTable)
        {

            int intRowCnt = srcDataTable.Rows.Count;

            for (int i = 0; i < intRowCnt; i++)
            {

                srcDataTable.Rows[i].Delete();
            }
        }

        /// <summary>
        /// Alinta：刪除DataTable的筆數：必須符合條件值
        /// </summary>
        /// <param name="srcDataTable">資料表來源</param>
        /// <param name="ConditionKey">條件值</param>
        public static void DeleteRows(this System.Data.DataTable srcDataTable, string ConditionKey)
        {
            DataRow[]  aryDr = srcDataTable.Select(ConditionKey);
            int intRowCnt=aryDr.Length; 

            for (int i = 0; i < intRowCnt ; i++)
            {
                aryDr[i].Delete();
            }
        }

        ///<summary>Alinta：判斷是否為數字型態, 回傳true/false</summary>
        ///<remarks></remarks>   
        ///<param name="strData">字串來源</param>           
        ///<value>回傳true/false</value>                 
        public static bool IsNumeric(this object strData)
        {            
            try
            {                
                double dblString = double.Parse(strData.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        ///<summary>Alinta：判斷是否為英文型態, 回傳true/false</summary>
        ///<remarks></remarks>   
        ///<param name="strData">字串來源</param>           
        ///<value>回傳true/false</value>                 
        public static bool IsLetter(this string strData)
        {
            try
            {                
                string strLetter = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                for (int i = 0; i < strData.Length; i++)
                {
                    if (strLetter.IndexOf(strData.ToUpper().Substring(i, 1)) == -1)
                        return false;
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        ///<summary> Alinta：判斷是否為日期型態, 回傳true/false</summary>
        ///<remarks></remarks>   
        ///<param name="strData">字串來源</param>           
        ///<value>回傳true/false</value>                 
        public static bool IsDate(this string strData)
        {
            if (strData.Length != 10)
                return false;

            try
            {
                DateTime dblString = DateTime.Parse(strData.ToString());
                return true;
            }
            catch
            {
                return false;
            }
        }

        ///<summary>Alinta：傳回國曆日</summary>
        ///<remarks></remarks>   
        ///<param name="strData">日期來源</param>                               
        public static string toChineseDate(this object strData)
        {
            try
            {
                DateTime dt = DateTime.Parse(strData.ToString());
                return (dt.Year - 1911).ToString().PadLeft(3, '0') + "/" + dt.Month.ToString().PadLeft(2, '0') + "/" + dt.Day.ToString().PadLeft(2, '0');
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 傳回日期格式值
        /// </summary>
        /// <param name="strDate"></param>
        public static DateTime toDate(this string strDate)
        {
           
                strDate = strDate.Replace("/", "");

                int y = Int32.Parse(strDate.Substring(0, 4));
                int m = Int32.Parse(strDate.Substring(4, 2));
                int d = Int32.Parse(strDate.Substring(6, 2));

                return DateTime.Parse(y.ToString()+"/"+m.ToString()+"/"+d.ToString());
            
        }

        ///<summary>Alinta：傳回西曆日</summary>
        ///<remarks></remarks>   
        ///<param name="strData">字串來源</param>                   
        public static String toUSADate(this string strData)
        {
            try
            {
                int y = Int32.Parse(strData.Substring(0, 3)) + 1911;
                int m = Int32.Parse(strData.Substring(4, 2));
                int d = Int32.Parse(strData.Substring(7, 2));

                return y.ToString().PadLeft(4, '0') + "/" + m.ToString().PadLeft(2, '0') + "/" + d.ToString().PadLeft(2, '0');
            }
            catch
            {
                return null;
            }
        }



        ///<summary>Alinta：傳回格式為99:99的時間</summary>
        ///<remarks></remarks>   
        ///<param name="strData">日期來源</param>                                
        public static string toTime(this DateTime strData)
        {
            return toTime(strData);
        }

        ///<summary>Alinta：傳回格式為99:99的時間</summary>
        ///<remarks></remarks>   
        ///<param name="strData">字串來源</param>                              
        public static string toTime(this string strData)
        {
            return toTime((object)strData);
        }

        ///<summary>Alinta：傳回格式為99:99的時間</summary>
        ///<remarks></remarks>
        ///<param name="strData">字串來源</param>
        private  static string toTime(object strData)
        {
            try
            {
                DateTime dt = DateTime.Parse(strData.ToString());
                return (dt.Hour).ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2,'0');
            }
            catch
            {
                return "";
            }
        }


        ///<summary>將將Panel容器裏物件lock/unlock</summary>
        ///<remarks></remarks>   
        ///<param name="ctls">Panel容器</param>                               
        ///<param name="bolEditing">是否開放編輯</param>                               
        private static void PanelEditing(object ctl, bool bolEditing)
        {

            System.Web.UI.ControlCollection ctls = ((System.Web.UI.Control)ctl).Controls;

            foreach (object elm in ctls)
            {
                if (((System.Web.UI.Control)elm).Controls.Count > 0)
                    PanelEditing(elm, bolEditing);
                else
                    Editing(elm, bolEditing);                                       

            }
        }


        ///<summary>將Panel容器裏的物件資料清空</summary>
        ///<remarks></remarks>   
        ///<param name="ctls">Panel容器</param>                                  
        private static void PanelClearing(object ctl)
        {
            System.Web.UI.ControlCollection ctls = ((System.Web.UI.Control)ctl).Controls;
            foreach (object elm in ctls)
            {
                if (((System.Web.UI.Control)elm).Controls.Count > 0)
                    PanelClearing(elm);
                else
                    Clearing(elm);
            }
        }

        ///<summary>Alinta：將物件lock/unlock</summary>
        ///<remarks></remarks>   
        ///<param name="ctl">物件來源</param>                               
        ///<param name="bolEditing">是否開放編輯</param>                               
        public static void Editing(this object ctl, bool bolEditing)
        {
            System.Web.UI.ControlCollection ctls = ((System.Web.UI.Control)ctl).Controls; ;
            if (ctls.Count > 0)
            {
                PanelEditing(ctl, bolEditing);
            }

            switch (ctl.GetType().Name)
            {
                case "LinkButton":
                    if (!bolEditing)                    
                        ((LinkButton)ctl).Attributes.Add("disabled", "disabled");                    
                    else
                        ((LinkButton)ctl).Attributes.Remove("disabled");
                    
                    break;

                case "TextBox":
                    TextBox txb = ((TextBox)ctl);

                    txb.Attributes.Remove("readonly");
                    txb.Attributes.Remove("tabindex");

                    if ((txb.CssClass).IndexOf("display") == -1)
                    {
                        if (!bolEditing)
                        {
                            txb.Attributes.Add("readonly", "readonly");
                            txb.Attributes.Add("tabindex", "-1");
                        }
                    }
                    else
                    {
                        txb.Attributes["readonly"] = "readonly";
                        txb.Attributes.Add("tabindex", "-1");
                    }

                    ((TextBox)ctl).CssClass = ((TextBox)ctl).CssClass.Replace("slock", "") + (bolEditing ? "" : " slock");

                    break;

                case "RadioButtonList":
                    RadioButtonList rdo = (RadioButtonList)ctl;

                    if (!bolEditing)
                    {
                        rdo.Attributes.Add("disabled", "disabled");
                        rdo.Attributes.Add("tabindex", "-1");
                    }
                    else
                    {
                        rdo.Attributes.Remove("disabled");
                        rdo.Attributes.Remove("tabindex");
                        rdo.Attributes.Add("onchange", "document_onChange(this.id,this.value)");
                    }

                    break;

                case "DropDownList":
                    
                  //  DropDownList dp = (DropDownList)ctl;

                  //  dp.Enabled = bolEditing;

                    if (!bolEditing)
                        ((DropDownList)ctl).Attributes.Add("disabled", "disabled");
                    else
                        ((DropDownList)ctl).Attributes.Remove("disabled");

                    break;

                case "CheckBox":
                    if (!bolEditing)
                        ((CheckBox)ctl).Attributes.Add("disabled", "disabled");
                    else                    
                        ((CheckBox)ctl).Attributes.Remove("disabled");

                    break;

                case "FileUpload":
                    FileUpload myFile = (FileUpload)ctl;

                    myFile.Enabled = bolEditing;

                    break;

                case "Button":
                    if (!bolEditing)
                        ((Button)ctl).Attributes.Add("disabled", "disabled");
                    else
                        ((Button)ctl).Attributes.Remove("disabled");

                    break;
                case "ImageButton":
                    ((ImageButton)ctl).Enabled = bolEditing;

                    if (!bolEditing)
                        ((ImageButton)ctl).Style.Remove("cursor");
                    else
                        ((ImageButton)ctl).Style.Add("cursor", "pointer");
                    

                    break;
                

                case "HtmlInputCheckBox":
                    ((HtmlInputCheckBox)ctl).Disabled = !bolEditing;
                    break;

            }
        }

        ///<summary>Alinta：將物件lock/unlock</summary>
        ///<remarks></remarks>   
        ///<param name="ctl">物件來源</param>                               
        ///<param name="bolEditing">是否開放編輯</param>                               
        public static void Clearing(this object ctl)
        {
             System.Web.UI.ControlCollection ctls = ((System.Web.UI.Control)ctl).Controls; ;
            if (ctls.Count > 0)
            {
                PanelClearing(ctl);
            }

            switch (ctl.GetType().Name)
            {
                case "TextBox":
                    ((TextBox)ctl).Text = "";
                    break;

                case "DropDownList":
                case "srvDropDownList":
                    ((DropDownList)ctl).SelectedIndex = 0;
                    break;

                case "CheckBox":
                    ((CheckBox)ctl).Checked=false;
                    break;
            }
        }


        /// <summary>
        /// Alinta：取得系統時間,以半點及整點為一個刻度 
        /// </summary>
        /// <returns></returns>
        public static string getHalfTime()
        {
            int intHours = System.DateTime.Now.Hour;
            int intMinutes = System.DateTime.Now.Minute;

            if (intMinutes > 1 && intMinutes <= 30)
                intMinutes = 30;
            else
            {
                intMinutes = 0;
                intHours++;
                if (intHours > 24)
                    intHours = 1;
            }

            return intHours.ToString().PadLeft(2, '0') + ":" + intMinutes.ToString().PadLeft(2, '0');
        }

        /// <summary>
        /// Alinta：取得系統時間
        /// </summary>
        /// <returns></returns>
        public static string getSystemTime()
        {
            return System.DateTime.Now.ToString("HH:mm");
        }

        /// <summary>
        /// Date Type Day/Month/Year
        /// </summary>
        public enum DateType
        {
            Day, Month,  Year
        }
               
        /// <summary>
        /// Alinta：DateDiff 日期相減 
        /// </summary>
        /// <param name="Interval">Date Type</param>
        /// <param name="StartDate">日期起</param>
        /// <param name="EndDate">日期迄</param>
        /// <returns></returns>
        public static int DateDiff(DateType Interval, System.DateTime StartDate, System.DateTime EndDate)
        {
            int lngDateDiffValue = 0;
            System.TimeSpan TS = new System.TimeSpan(EndDate.Ticks - StartDate.Ticks);

            switch (Interval)
            {
                case DateType.Day:
                    lngDateDiffValue = (int)TS.Days;
                    break;

                case DateType.Month:
                    lngDateDiffValue = (int)(TS.Days / 30);
                    break;

                case DateType.Year:
                    lngDateDiffValue = (int)(TS.Days / 365);
                    break;
            }
            return (lngDateDiffValue);
        }

       
        /// <summary>
        ///  Alinta：避掉ListControl的Valu中的單引號，同時加上Trim()功能
        /// </summary>
        /// <param name="strValue">物件來源</param>
        /// <returns>欄位值</returns>
        public static String rpsText(this string strValue)
        {
            if (strValue == null)
                return "";
            else
                return strValue.Trim().Replace("'", "''");
        }


        /// <summary>
        ///  Alinta：避掉HiddenField的Valu中的單引號，同時加上Trim()功能
        /// </summary>
        /// <param name="hiddenObj">物件來源</param>
        /// <returns>欄位值</returns>
        public static String rpsText(this  HiddenField  hiddenObj)
        {
            return hiddenObj.Value.Trim().Replace("'", "''");
        }

        /// <summary>
        ///  Alinta：避掉ListControl的Valu中的單引號，同時加上Trim()功能
        /// </summary>
        /// <param name="txtObj">物件來源</param>
        /// <returns>欄位值</returns>
        public static String rpsText(this ListControl txtObj)
        {
            return txtObj.Text.Trim().Replace("'", "''");
        }
        
        /// <summary>
        ///  Alinta：避掉單引號，同時加上Trim()功能
        /// </summary>
        /// <param name="txtObj">物件來源</param>
        /// <returns>欄位值</returns>
        public static String rpsText(this TextBox  txtObj)
        {
            return txtObj.Text.Trim().Replace("'", "''");
        }

        /// <summary>
        /// Alinta：欄位不可空白的驗證
        /// </summary>
        /// <param name="strField">欄位來源</param>
        /// <param name="strTitle">欄位Title</param>
        /// <returns></returns>
        public static string FieldSpaceCheck(string strField, string strTitle)
        {
            string[] aryField = strField.Split(',');
            string[] aryTitle = strTitle.Split(',');

            string strMessage = "";
            try
            {
                for (int i = 0; i < aryField.Length; i++)
                {
                    if (aryField[i].ToString().Trim() == "" || aryField[i].ToString() == "0")
                        strMessage += (strMessage == "" ? "" : ",") + aryTitle[i].ToString();
                }
            }
            catch (Exception err)
            {
                commonWriteFile.writeErrorLog(err.ToString(), "FieldSpaceCheck Error");
                return "欄位不可空白的驗證有誤，請通知系統相關人員！";
            }
            if (strMessage != "")
                strMessage += "必須輸入！";

            return strMessage;

        }


        /// <summary>
        /// Alinta：欄位不可空白的驗證
        /// </summary>
        /// <param name="aryField">欄位來源</param>       
        /// <returns></returns>
        public static string FieldSpaceCheck(ArrayList aryField)
        {

            string strMessage = "";
            try
            {                
                for (int i = 0; i < aryField.Count; i++)
                {
                    if (((ArrayList)aryField[i])[0].ToString().Trim() == "" || ((ArrayList)aryField[i])[0].ToString() == "0")
                        strMessage += (strMessage == "" ? "" : ",") + ((ArrayList)aryField[i])[1].ToString();
                }
            }
            catch (Exception err)
            {
                commonWriteFile.writeErrorLog(err.ToString(), "FieldSpaceCheck Error");
                return "欄位不可空白的驗證有誤，請通知系統相關人員！";
            }
            if (strMessage != "")
                strMessage += "必須輸入！";

            return strMessage;

        }
               
        
    }
    #endregion 


    #region commonWriteFile 寫檔案

    ///<summary>Alinta：txtFile：將資料寫入記事本檔案</summary>   
    ///<example>String.commonWriteFile()</example>
    public static class commonWriteFile
    {
        ///<summary>將字串寫入txtFile</summary>
        ///<remarks>該Method沒有設定檔案路徑，則直接存入根目錄下</remarks>   
        ///<param name="strFileName">檔案名稱</param> 
        ///<param name="strData">string型態的文字內容</param>           
        ///<value></value>     
        public static void writeFile(string strFileName, string strData)
        {
            string strFilePath = AppDomain.CurrentDomain.BaseDirectory + strFileName;

            FileStream fs = new FileStream(strFilePath, FileMode.Create, FileAccess.Write);

            StreamWriter w = new StreamWriter(fs, System.Text.Encoding.Default);
            w.BaseStream.Seek(0, SeekOrigin.End);
            w.WriteLine("{1}", "", strData);
            w.Flush();
            w.Close();
        }

        ///<summary>Alinta：將字串寫入txtFile</summary>
        ///<remarks></remarks>   
        ///<param name="strFilePath">完整檔案路徑．．路徑+檔案名稱</param> 
        ///<param name="strFileName">檔案名稱</param> 
        ///<param name="strData">string型態的文字內容</param>           
        ///<value></value>     
        public static void writeFile(string strFilePath,string strFileName, string strData)
        {
            FileStream fs = new FileStream(strFilePath+strFileName, FileMode.Create, FileAccess.Write);

            StreamWriter w = new StreamWriter(fs, System.Text.Encoding.Default);
            w.BaseStream.Seek(0, SeekOrigin.End);
            w.WriteLine("{1}", "", strData);
            w.Flush();
            w.Close();
        }


        ///<summary>Alinta：將ArrayList的內容寫入txtFile</summary>
        ///<remarks></remarks>   
        ///<param name="strFilePath">完整檔案路徑．．路徑</param>           
        ///<param name="strFileName">檔案名稱</param>  
        ///<param name="aryData">ArrayList型態的文字內容</param>           
        ///<value></value>     
        public static void writeFile(string strFilePath, string strFileName, ArrayList aryData)
        {
            FileStream fs = new FileStream(strFilePath+strFileName, FileMode.Create, FileAccess.Write);

            StreamWriter w = new StreamWriter(fs, System.Text.Encoding.Default);
            w.BaseStream.Seek(0, SeekOrigin.End);
            for (int i = 0; i < aryData.Count; i++)
            {
                w.WriteLine("{1}","", aryData[i].ToString().Trim());
            }
            w.Flush();
            w.Close();
        }
       

        ///<summary>
        /// Alinta：將程式錯誤訊息寫入errorlog，檔案error.log會存在專案的根目錄下</summary>
        ///<remarks></remarks>   
        ///<param name="strSource">錯誤類別</param>           
        ///<param name="strErrDescription">錯誤訊息</param>           
        ///<value></value>     
        public static void writeErrorLog(string strSource, string strErrDescription)
        {
            
            string strFileName="";

            if (WebConfigurationManager.AppSettings["ErrorLog_Path"] != null)
                strFileName = WebConfigurationManager.AppSettings["ErrorLog_Path"].ToString();
            else
                strFileName = AppDomain.CurrentDomain.BaseDirectory + "\\Error.log";


            FileStream fs = new FileStream(strFileName, FileMode.OpenOrCreate, FileAccess.Write);

            StreamWriter w = new StreamWriter(fs, System.Text.Encoding.Default);
            w.BaseStream.Seek(0, SeekOrigin.End);
            //Write To Log            
            w.Write("\r\nError DateTime : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
            w.WriteLine("Source : {0}", strSource);
            w.WriteLine("Description : {0}", strErrDescription);
            w.WriteLine("------------------------------------------------------------------");
            w.Flush();
            w.Close();
        }
    }
    #endregion

}
