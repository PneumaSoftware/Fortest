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
    /// Alinta：create delegate for functionbar events
    /// </summary>   
    /// <param name="strStatus">作業狀態</param>    
    public delegate void displayUPDelegate();




    public partial class displayUP : System.Web.UI.MasterPage
    {


        /// <summary>
        /// 將MasterPage的Process event傳回主網頁
        /// </summary>
        public event displayUPDelegate DisplayEvent;


        /// <summary>
        /// 取得detail page
        /// </summary>
        /// <returns></returns>
        public bool  bolPostBack
        {
            set { ViewState["bolPostBack"] = value; }
            get { return (ViewState["bolPostBack"] == null ? false  : (bool)ViewState["bolPostBack"]); }
        }

        /// <summary>
        /// 資料查詢
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Query_Click(object sender, CommandEventArgs e)
        {

            this.Master.nowStatus = e.CommandName;
            if (DisplayEvent != null)
                DisplayEvent();

           
            //if (e.CommandName!="Query")



            //  if (SelectEvent != null)
            //      SelectEvent();


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
