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

namespace OrixMvc
{
    #region ControlValue 驗證函式

    /// <summary>
    /// Common library
    /// Author      ：  Alinta 
    /// Build Date  ：  2013/06/05 
    /// Modify Date ：  2013/06/05 by Alinta 
    /// Purpose     ： １．提供所有函式驗證(包含擴充型別)，不含資料庫連結處理
    ///                ２．提供畫面上欄位的處理：   1.清空資料
    ///                                             2.是否開放編輯
    ///                                             3.欄位不可空白驗證
    ///                                             4.欄位避掉單引號
    /// </summary>         
    public static class ControlValue
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public static string value(this object ctl)
        {
            string strValue = "";
            System.Web.UI.ControlCollection ctls = ((System.Web.UI.Control)ctl).Controls; ;


            switch (ctl.GetType().Name)
            {

                case "HiddenField":
                    HiddenField hid = ((HiddenField)ctl);

                    strValue = hid.Value.Trim(); 
                    break;

                case "TextBox":
                    TextBox txb = ((TextBox)ctl);

                    strValue = txb.Text.Trim();

                    break;

                case "RadioButtonList":
                    RadioButtonList rdo = (RadioButtonList)ctl;

                    strValue = rdo.SelectedValue.Trim();

                    break;

                case "DropDownList":
                    DropDownList dp = (DropDownList)ctl;

                    strValue = dp.SelectedValue.Trim();

                    break;

                case "CheckBox":
                    strValue = (((CheckBox)ctl).Checked ? "Y" : "N");


                    break;

                case "ocxcontrol_ocxdate_ascx":
                    strValue = ((ocxControl.ocxDate)ctl).Text.Trim();
                    break;

                case "ocxcontrol_ocxupload_ascx":
                    strValue = ((ocxControl.ocxUpload)ctl).Seq.Trim();

                    break;

                case "ocxcontrol_ocxnumber_ascx":
                    strValue = ((ocxControl.ocxNumber)ctl).Text.Trim();
                    break;

                case "ocxcontrol_ocxyear_ascx":
                    strValue = ((ocxControl.ocxYear)ctl).Text.Trim();
                    break;

                case "ocxcontrol_ocxdialog_ascx":
                    strValue = ((ocxControl.ocxDialog)ctl).Text.Trim();
                    break;

                case "ComboBox":
                    strValue = (((CheckBox)ctl).Checked ? "Y" : "N");
                    break;
            

            }

            return strValue;
        }
        
               
        
    }
    #endregion 



}
