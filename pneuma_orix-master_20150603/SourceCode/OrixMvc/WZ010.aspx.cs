using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using OrixMvc.Pattern;
using OrixMvc.ocxControl;
using VS2008.Module;
using AjaxControlToolkit;

namespace OrixMvc
{
    public partial class WZ010 : OrixMvc.Pattern.PageParent
    {
        #region 引用Module設定
        //*******************begin 勿動***********************  
        /// <summary>
        /// dss：將dataset存入sql server
        /// cts：將sql command 存入 sql server
        /// dg ：取得sql server的資料
        /// </summary>
        VS2008.Module.DataSetToSql dts = new VS2008.Module.DataSetToSql();
        VS2008.Module.CommandToSql cts = new VS2008.Module.CommandToSql();
        VS2008.Module.DataGetting dg = new VS2008.Module.DataGetting();

        //**********************end 勿動***********************   
        #endregion




        #region Page_Load 網頁初始設定：宣告MasterPage所有Event，設定公用參數值
        /// <summary>
        /// 網頁初始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //*************************begin 勿動**************************   

            this.Master.ProcessEvent += new ProcessDelegate(Save_Click);


            if (!IsPostBack)
                this.setParms();
            //***************************end 勿動****************************
        }
        #endregion



        #region setParms：設定公共參數
        /// <summary>
        /// 設定公共參數-->順序不得互換
        /// 1.編輯區域Title
        /// 2.程式編輯功能(新增,修改,刪除)
        /// 3.雖有編輯功能, 但不顯示修改的欄位
        /// </summary>
        private void setParms()
        {
           
            /*
            //1.
            this.Master.DataAreaName = "";

            //2.程式編輯功能
            this.Master.setEditingFunction(false, true, false);

            this.Master.showSystemButton(SystemButton.btnSave.ToString(), false);
            this.Master.showSystemButton(SystemButton.btnCancel.ToString(), false);
            this.Master.showPanel(Area.DataArea.ToString(), false);

            //3.雖有編輯功能, 但不顯示修改的欄位
            this.Master.bolUpd_Show = false;*/


        }
        #endregion



        #region setDefaultValue：欄位預設值
        /// <summary>
        /// 設定欄位預設值
        /// 本頁作業：無作用
        /// </summary>
        private void setDefaultValue()
        {

        }
        #endregion



        #region Save_Click：儲存鍵觸發
        /// <summary>
        /// 儲存鍵觸發Event,
        /// 本頁作業：無  
        /// </summary>
        private void Save_Click(string strType)
        {
            DataTable dt = dts.GetTable("OR3_MENU_SET","1=1");
            DataRow[] adr;
            DataRow dr;
            string strFuncID="";
            string strParentID = "";
            string strAtchSeq = "";
            string strFuncType = "";

            for (int i = 0; i < this.rptFunction.Items.Count; i++)
            {
                strFuncType = ((HiddenField)this.rptFunction.Items[i].FindControl("FUNC_TYPE")).Value;
                strFuncID=((HiddenField)this.rptFunction.Items[i].FindControl("FUNC_ID")).Value;
                strParentID = ((DropDownList)this.rptFunction.Items[i].FindControl("PARENT_ID")).SelectedValue;
                strAtchSeq = ((ocxNumber)this.rptFunction.Items[i].FindControl("ATCH_SEQ")).Text;

                adr = dt.Select("Func_ID='" + strFuncID + "'");
                if (adr.Length > 0)
                {
                    dr = adr[0];

                    if ((dr["PARENT_ID"].ToString().Trim() != strParentID && strFuncType != "S") || dr["ATCH_SEQ"].ToString().Trim() != strAtchSeq)
                    {
                        if (strFuncType!="S")
                            dr["PARENT_ID"] = strParentID;

                        dr["ATCH_SEQ"] = strAtchSeq;
                    }
                }
            }

            if (dts.Save())
            {
                this.setProcessMessage("設定處理成功!!", false);
                this.rptFunction.DataBind();
                //此程式特殊, 需重新載入parent
               // this.setScript("window.parent.self.location.reload();");
            }
            else
                this.setProcessMessage("設定處理失敗!!", true); 

        }
        #endregion



    }
}
