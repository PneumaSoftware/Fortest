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
using VS2008.Module;
using System.IO;

namespace OrixMvc.Pattern
{

   

    /// <summary>
    /// Alinta：create delegate for Save events
    /// </summary>
    /// <param name="strStatus">作業狀態</param>       
    public delegate void ProcessDelegate(string strStatus);

 
        

    public partial class empty : System.Web.UI.MasterPage
    {


        public bool bolSave
        {
            set { ViewState["bolSave"] = value; }
            get { return (ViewState["bolSave"] == null ? true : (bool)ViewState["bolSave"]); }
        }

        public bool editSave
        {
            set { ViewState["editSave"] = value; }
            get { return (ViewState["editSave"] == null ? true : (bool)ViewState["editSave"]); }
        }
        /// <summary>
        /// 將MasterPage的Process event傳回主網頁
        /// </summary>
        public event ProcessDelegate ProcessEvent;


        protected void Page_Load(object sender, EventArgs e)
        {
            this.btnEdit.Editing(editSave);
            this.btnClear.Editing(editSave);

        }

        /// <summary>
        /// 儲存鍵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Save_Click(object sender, CommandEventArgs e)
        {
            if (ProcessEvent != null)
                ProcessEvent(e.CommandName);

        }

        /// <summary>
        /// 取得master page上任何在頁面的物件
        /// </summary>
        /// <param name="strID">物件ID</param>
        /// <returns>物件本身</returns>
        public object masterFindControl(string strID)
        {
            return this.masterFindControl(strID);
        }

    }
}
