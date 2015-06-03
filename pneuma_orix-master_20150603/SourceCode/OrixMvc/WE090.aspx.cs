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
    public partial class WE090 : OrixMvc.Pattern.PageParent
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

            if (Session["OBJ_CODE"] == null)
            {
                this.Response.Write("登錄錯誤, 請由WE020登錄!");
                this.Response.End();
                return;
            }

            this.OBJ_CODE.Text = (String)Session["OBJ_CODE"];
            /*
            //1.
            this.Master.DataAreaName = "";

            //2.程式編輯功能
            this.Master.setEditingFunction(false, true, false);

            this.Master.showSystemButton(SystemButton.btnSave.ToString(), false);
            this.Master.showSystemButton(SystemButton.btnCancel.ToString(), false);
            this.Master.showPanel(Area.DataArea.ToString(), false);*/

            this.Master.bolSave = false;

            /*/3.雖有編輯功能, 但不顯示修改的欄位
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
            /*START 勿動 更新Grid*/
           

            if (dts.Save())
            {
                this.setProcessMessage("資料處理成功!!", false);
                //this.rptEdit.DataBind();
                //此程式特殊, 需重新載入parent
               // this.setScript("window.parent.self.location.reload();");
            }
            else
                this.setProcessMessage("資料處理失敗!!", true); 

        }
        #endregion


        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Status_Click(object sender, CommandEventArgs e)
        {

            
            switch (e.CommandName)
            {
                case "Query":
                    break;

                case "Query_Apply":
                    
                    break;

            }

            //if (e.CommandName!="Query")



            //  if (SelectEvent != null)
            //      SelectEvent();


        }

    }
}
