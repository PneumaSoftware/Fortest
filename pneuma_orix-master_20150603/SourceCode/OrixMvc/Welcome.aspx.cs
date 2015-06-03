using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using VS2008.Module;


namespace OrixMvc
{
	/// <summary>
	/// Program And Name      ：
	/// Author                  ：
	/// Build Date              ：
	/// Modify Date/Description ：
	///
	/// </summary>
	public partial class Welcome : Pattern.PageParent {

		

		#region 引用Module設定
		//*******************begin 勿動***********************
		/// <summary>
		/// dss：將dataset存入sql server
		/// cts：將sql command 存入 sql server
		/// dg ：取得sql server的資料
		/// </summary>
        VS2008.Module.DataSetToSql dss = new VS2008.Module.DataSetToSql();
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

            

        }
		#endregion

		

               
	}
}

