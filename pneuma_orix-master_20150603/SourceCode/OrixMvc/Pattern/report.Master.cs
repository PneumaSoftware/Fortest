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
    /// Alinta：create delegate for Print events
    /// </summary>
     
    public delegate void PrintDelegate();




    public partial class report : System.Web.UI.MasterPage
    {


        /// <summary>
        /// 將MasterPage的Process event傳回主網頁
        /// </summary>
        public event PrintDelegate PrintEvent;

              


        /// <summary>
        /// 儲存鍵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Print_Click(object sender, EventArgs e)
        {
            if (PrintEvent != null)
                PrintEvent();

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
