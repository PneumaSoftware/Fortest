using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Security.Permissions;
using System.Web.Services;
using System.Web.UI.WebControls;
using System.Configuration;
using System.IO;

namespace VS2008.Module
{

    

    #region 從Sql server 取得資料
    ///<summary>Alinta：取得Sql Server 資料 </summary>
    public class DataGetting 
    {
        ///<summary>Alinta：記錄目前的Connection</summary>     
        protected SqlConnection nowConnection;

        ///<summary>存取Sql Server 資料 </summary>
        public DataGetting()
        {
            string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
            nowConnection = new SqlConnection(myConnectionString);
        }

        ///<summary>存取Sql Server 資料 </summary>
        public DataGetting(string strConnection)
        {
            string myConnectionString = ConfigurationManager.ConnectionStrings[strConnection].ToString();
            nowConnection = new SqlConnection(myConnectionString);
        }

        /// <summary>
        /// Alinta：將一些List 像dropdownlist ,checkboxlist, radiobuttonlist
        /// </summary>
        ///<param name="webCtl">要Binding的 List Control</param>   
        ///<param name="strSQL">sql 語法，欄位1是value, 欄位2是text, 要Binding的資料來源</param>         
        public void ListBinding(WebControl webCtl, string strSQL)
        {
            this.ListBinding(webCtl, strSQL, "");
        }

        /// <summary>
        /// Alinta：將一些List 像dropdownlist ,checkboxlist, radiobuttonlist
        /// </summary>
        ///<param name="webCtl">要Binding的 List Control</param>   
        ///<param name="strSQL">sql 語法，欄位1是value, 欄位2是text, 要Binding的資料來源</param> 
        ///<param name="strValue">預設值</param> 
        public void ListBinding(WebControl webCtl, string strSQL, string strValue)
        {
            SqlConnection myConnection = this.nowConnection;
            SqlCommand myCommand = new SqlCommand(strSQL, myConnection);
            myConnection.Open();

            string ItemText = "";
            string ItemValue = "";

            try
            {
                SqlDataReader myReader = myCommand.ExecuteReader();
                while (myReader.Read())
                {
                    if (myReader.FieldCount == 3 && webCtl.GetType().Name == "CheckBoxList")
                    {
                        ItemValue = myReader.GetValue(0).ToString();
                        ItemText = myReader.GetValue(1).ToString();
                        strValue = myReader.GetValue(2).ToString();
                    }
                    else if (myReader.FieldCount >= 2 )
                    {
                        ItemValue = myReader.GetValue(0).ToString();
                        ItemText = myReader.GetValue(1).ToString();
                    }
                    else if (myReader.FieldCount == 1)
                    {
                        ItemValue = myReader.GetValue(0).ToString();
                        ItemText = myReader.GetValue(0).ToString();
                    }
                     

                    ListItem li = new ListItem(ItemText, ItemValue);
                    if (strValue != "")
                    {
                        if (webCtl.GetType().Name != "CheckBoxList")
                        {
                            if (strValue == ItemValue) //預設已選取
                                li.Selected = true;
                        }
                        else
                        {
                            if (myReader.FieldCount != 3 && strValue != "")
                            {
                                if (strValue.IndexOf(ItemValue + ",") != -1 || strValue.IndexOf("," + ItemValue) != -1) //預設已選取
                                    li.Selected = true;
                            }
                            else
                            {
                                if (strValue == "1")
                                    li.Selected = true;
                            }
                        }
                    }

                    switch (webCtl.GetType().Name)
                    {

                        case "DropDownList":
                        case "srvDropDownList":
                            ((DropDownList)webCtl).Items.Add(li);
                            break;

                        case "CheckBoxList":
                            ((CheckBoxList)webCtl).Items.Add(li);
                            break;

                        case "RadioButtonList":
                            ((RadioButtonList)webCtl).Items.Add(li);
                            break;
                    }
                }
                myConnection.Close();

            }
            catch (Exception e)
            {
                try { myConnection.Close(); }
                catch { }
                commonWriteFile.writeErrorLog(e.ToString() + " " + strSQL, "Binding " + webCtl.GetType().Name + " Error");
            }
            finally
            {
                try { myConnection.Close(); }
                catch { }
            }
        }

        ///<summary>Alinta：依據sql 指令, 取得一筆DataRow, 只有一筆時</summary>
        ///<param name="strSQL">sql 指令</param> 
        public DataRow GetDataRow(string strSQL, bool IfTimeOut)
        {
            return this.rGetDataRow(strSQL, false);
        }

        ///<summary>Alinta：依據sql 指令, 取得一筆DataRow, 只有一筆時</summary>
        ///<param name="strSQL">sql 指令</param> 
        public DataRow GetDataRow(string strSQL)
        {
            return this.rGetDataRow(strSQL, true);
        }

        private DataRow  rGetDataRow(string strSQL, bool IfTimeOut)
        {
            DataTable dt = new DataTable();
            DataRow[] arydr = new DataRow[0];
            DataRow dr;

            SqlConnection myConnection = this.nowConnection;

            SqlCommand myCommand = new SqlCommand(strSQL, myConnection);
            if (!IfTimeOut)
                myCommand.CommandTimeout = 0;

            myConnection.Open();

            try
            {
                SqlDataReader myReader = myCommand.ExecuteReader();


                //取得欄位抬頭
                for (int j = 0; j < myReader.FieldCount; j++)
                {
                    dt.Columns.Add(myReader.GetName(j), myReader.GetFieldType(j));
                    switch (myReader.GetFieldType(j).ToString())
                    {

                        case "System.String":
                        case "System.Char":
                            dt.Columns[j].DefaultValue = "";
                            break;

                        case "System.Int16":
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Decimal":
                        case "System.Double":
                            dt.Columns[j].DefaultValue = 0;
                            break;
                        case "System.DateTime":
                            dt.Columns[j].DefaultValue = null;
                            break;

                        case "System.Boolean":
                            dt.Columns[j].DefaultValue = true;
                            break;
                    }
                }

                dr = dt.NewRow();
                if (myReader.Read())
                {
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        if (myReader.GetValue(i) != System.DBNull.Value)
                        {
                            dr[i] = myReader.GetValue(i).ToString();
                        }
                    }
                }
                dt.Rows.Add(dr);
                myConnection.Close();

                return dr;
            }
            catch (Exception e)
            {
                try { myConnection.Close(); }
                catch { }

                commonWriteFile.writeErrorLog(strSQL + " " + e.ToString(), "GetDataRow Error");

            }
            finally {
                try { myConnection.Close(); }
                catch { }
            }
            return null;
        }



        ///<summary>Alinta：依據sql 指令,以SqlDataReader 取得DataTable, 速度快但只能向下讀取</summary>
        ///<param name="strSQL">sql 指令</param> 
        public SqlDataReader openSqlReader(string strSQL)
        {

            SqlConnection myConnection = this.nowConnection;
            SqlCommand myCommand = new SqlCommand(strSQL, myConnection);
            myConnection.Open();

            try
            {
                SqlDataReader myReader = myCommand.ExecuteReader();

                return myReader;
            }
            catch (Exception e)
            {
                try { myConnection.Close(); }
                catch { }

                commonWriteFile.writeErrorLog(strSQL + " " + e.ToString(), "openSqlReader Error");
            }
            return null;
        }

        

        /// <summary>
        /// Alinta：關閉 SqlDataReader
        /// </summary>
        /// <param name="myReader">正在運行的sql DataReader</param>
        public void closeSqlReader(SqlDataReader myReader)
        {
            try
            {
                myReader.Close();
                this.nowConnection.Close();
            }
            catch (Exception e)
            {
                commonWriteFile.writeErrorLog(e.ToString(), "closeSqlReader Error");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strTableName"></param>
        /// <param name="strFilter"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string strTableName, string strKey)
        {
            if (strKey == "")
                strKey = "1=2";
           
            string strSQL = "select * from " + strTableName + " where " + strKey;
            return GetDataTable(strSQL);
            
        }

        ///<summary>Alinta：依據sql 指令,以SqlDataAdapter 取得DataTable, 速度慢但可以上下筆讀取</summary>
        ///<param name="strSQL">sql 指令</param> 
        public DataTable GetDataTable(string strSQL)
        {
            DataSet ds = new DataSet();
            SqlConnection myConnection = this.nowConnection;
            SqlDataAdapter Adapter;

            myConnection.Open();

            try
            {
                
                Adapter = new SqlDataAdapter(strSQL, myConnection);
                Adapter.SelectCommand.CommandTimeout = 50;
                Adapter.Fill(ds, "GridTable");
                
                myConnection.Close();
            }
            catch (Exception e)
            {
                try { myConnection.Close(); }
                catch { }

                commonWriteFile.writeErrorLog(strSQL+" "+e.ToString(), "GetDataTable Error");
            }
            finally
            {
                try { myConnection.Close(); }
                catch { }
            }
            return ds.Tables["GridTable"];
        }

    }
    #endregion


    #region save DataSet to sql server
    /// <summary>
    /// Alinta：Data Access to Sql in DataSet Method
    /// </summary>
    public class DataSetToSql 
    {
        ///<summary>Alinta：記錄目前的Connection</summary>     
        SqlConnection nowConnection;
        DataSet eDs;
        ArrayList alAdapter;
        ArrayList alDataTable;

        /// <summary>
        /// Alinta：Data Access to Sql in DataSet Method : new DataSet, new SqlDataAdapter, new DataTable
        /// </summary>     
        public DataSetToSql()
        {
             eDs = new DataSet();
            alAdapter = new ArrayList();
            alDataTable = new ArrayList();
            string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
            nowConnection = new SqlConnection(myConnectionString);
        }
        
       /// <summary>
        /// Alinta：存取Sql Server 資料 Setting Connection String 
       /// </summary>
       /// <param name="strConnection">Connection String</param>
        public DataSetToSql(string strConnection)
        {
             eDs = new DataSet();
            alAdapter = new ArrayList();
            alDataTable = new ArrayList();

            string myConnectionString = ConfigurationManager.ConnectionStrings[strConnection].ToString();
            nowConnection = new SqlConnection(myConnectionString);

        }

        /// <summary>
        /// Alinta：When Create Row, Get DataTable Form Sql Server 
        /// </summary>
        /// <param name="strTableName">Sql's Table Name</param>
        /// <returns>DataTable</returns>
        public DataTable GetTable(string strTableName)
        {
            return GetTable(strTableName, "1=2");
        }

        /// <summary>
        /// Alinta：When Update or Delete Row, Get DataTable From SQL Server 
        /// </summary>
        /// <param name="strTableName">Sql's Table Name</param>
        /// <param name="strKey">sql condition</param>
        /// <returns>DataTable</returns>
        public DataTable GetTable(string strTableName, string strKey)
        {
            if (strKey == "")
                strKey = "1=2";


            SqlConnection myConnection = this.nowConnection;
            myConnection.Open();

            SqlDataAdapter Adapter;

            try
            {
                string strSQL = "select * from " + strTableName + " where " + strKey;
                SqlCommand Command = new SqlCommand(strSQL, myConnection);

                DataSet ds = new DataSet();
                Adapter = new SqlDataAdapter(Command);
                SqlCommandBuilder myBudlier = new SqlCommandBuilder(Adapter);
                Adapter.FillSchema(eDs, SchemaType.Mapped);
                Adapter.Fill(eDs, strTableName);
                alAdapter.Add(Adapter);
                alDataTable.Add(strTableName);

                myConnection.Close();

            }
            catch (SqlException e)
            {
                try
                {
                    myConnection.Close();
                }
                catch { }

                commonWriteFile.writeErrorLog(e.Message.ToString(), "GetTable update Error");
                throw;
            }
            finally
            {
                try { myConnection.Close(); }
                catch { }
            }

            DataTable tb = eDs.Tables[strTableName];


            return eDs.Tables[strTableName];
        }

        /// <summary>
        /// Alinta：Save change from DataTable to Sql Server
        /// </summary>
        /// <returns>true/false</returns>
        public bool Save()
        {
            SqlConnection myConnection = this.nowConnection; ;           
 
            bool bolCheck = true;
            
            myConnection.Open();
            string strTable = "";
            SqlTransaction myTransaction = myConnection.BeginTransaction();
            try
            {
                for (int i = 0; i < alAdapter.Count; i++)
                {
                    SqlDataAdapter myDataAdapter = ((SqlDataAdapter)alAdapter[i]);
                    myDataAdapter.SelectCommand.Transaction = myTransaction;
                   

                     string strDB = "";
                    if (alDataTable[i].ToString().IndexOf("..") != -1)
                    {
                        strDB = myConnection.Database;
                        myConnection.ChangeDatabase(alDataTable[i].ToString().Substring(0, alDataTable[i].ToString().IndexOf(".."))); 
                    }
                    strTable = alDataTable[i].ToString();
                    myDataAdapter.Update(eDs,alDataTable[i].ToString());
                    
                    if (strDB != "")
                        myConnection.ChangeDatabase(strDB);
                }

                myTransaction.Commit();
                bolCheck = true;
            }
            catch (SqlException e)
            {
                myTransaction.Rollback();
                
                
                commonWriteFile.writeErrorLog("Table："+ strTable +"　"+e.Message.ToString(), "DataSet update Error");

                HttpContext myPage = System.Web.HttpContext.Current;
                myPage.Session["errorMessage"] = e.Message.ToString();

                bolCheck = false;
                
                //throw;
            }
            finally
            {
                myConnection.Close();
            }

            return bolCheck;
        }
    }
    #endregion


    #region 將資料儲存至 sql server
    ///<summary>Alinta：下sql Command 更新 sql server 資料  </summary>
    public class CommandToSql 
    {
        ///<summary>Alinta：記錄目前的Connection</summary>     
        SqlConnection nowConnection;

        ///<summary>存取Sql Server 資料 </summary>
        public CommandToSql()
        {
            string myConnectionString = ConfigurationManager.ConnectionStrings["myConnectionString"].ToString();
            nowConnection = new SqlConnection(myConnectionString);
        }

        ///<summary>存取Sql Server 資料 </summary>
        public CommandToSql(string strConnection)
        {
            string myConnectionString = ConfigurationManager.ConnectionStrings[strConnection].ToString();
            nowConnection = new SqlConnection(myConnectionString);
        }

        ///<summary>Alinta：執行Sql指令,一筆指令 </summary>
        ///<param name="al">一筆以上的Sql指令要執行</param> 
        public bool Execute(ArrayList al)
        {
            return this.Execute(al, true);
        }

        ///<summary>Alinta：執行Sql指令,一筆指令 </summary>
        ///<param name="strSQL">只有一筆Sql指令要執行</param> 
        public bool Execute(string strSQL)
        {
            return this.Execute(strSQL, true);
        }

        ///<summary>Alinta：執行Sql指令,一筆指令 </summary>
        ///<param name="strSQL">只有一筆Sql指令要執行</param> 
        /// <param name="IfTimeOut"> true/false 是否timeout為30秒,預設true  </param>
        public bool Execute(string strSQL, bool IfTimeOut)
        {
            SqlConnection myConnection = this.nowConnection;
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand();

            myCommand.Connection = myConnection;
            
            if (!IfTimeOut)
                myCommand.CommandTimeout = 0;

            bool bolReturn = true;
            try
            {
                myCommand.CommandText = strSQL.ToString().Trim();
                myCommand.ExecuteNonQuery();

                bolReturn = true;
                myConnection.Close();
            }
            catch (SqlException e)
            {
                
                try { myConnection.Close(); }
                catch { }

                commonWriteFile.writeErrorLog(myCommand.CommandText + " " + e.Message.ToString(), "execute single sql Error");
                bolReturn = false;
            }
            finally
            {
                try { myConnection.Close(); }
                catch { }
            }

            return bolReturn;
        }


                
        ///<summary>Alinta：執行Sql指令,多筆,有begin transaction </summary>
        ///<param name="al">一筆以上的Sql指令要執行</param> 
       /// <param name="IfTimeOut"> true/false 是否timeout為30秒,預設true  </param>
       /// <returns></returns>
        public bool Execute(ArrayList al, bool IfTimeOut)
        {
            SqlConnection myConnection = this.nowConnection;
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand();
            SqlTransaction myTrans = myConnection.BeginTransaction();

            myCommand.Connection = myConnection;
            myCommand.Transaction = myTrans;
            
            if (!IfTimeOut)
                myCommand.CommandTimeout = 0;

            string exec_sql = "";
            string total_sql="";
            bool bolReturn = true;
            try
            {
                for (int i = 0; i < al.Count; i++)
                {
                    exec_sql = al[i].ToString();
                    total_sql += exec_sql + "\r\n";
                    myCommand.CommandText = exec_sql;
                    myCommand.ExecuteNonQuery();
                }
                myTrans.Commit();
                bolReturn = true;
                myConnection.Close();
            }
            catch (SqlException e)
            {
                try { myTrans.Rollback(); }
                catch { }

                try { myConnection.Close(); }
                catch { }

                commonWriteFile.writeErrorLog(exec_sql + " " + e.Message.ToString(), "execute multiple sql Error" +"\r\n 批次sql 語法如下"+total_sql );
                bolReturn = false;
            }
            finally
            {
                try { myConnection.Close(); }
                catch { }
            }

            return bolReturn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strSQL">只有一筆Sql指令要執行</param>
        /// <param name="oData">帶入Paramber資料</param>
        /// <param name="IfTimeOut">true/false 是否timeout為30秒,預設true</param>
        /// <returns>回傳資料</returns>
        public string ExecuteGetDataForParam(string strSQL, Object[][] oData, bool IfTimeOut)
        {
            SqlConnection myConnection = this.nowConnection;
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand();

            myCommand.Connection = myConnection;

            if (!IfTimeOut)
                myCommand.CommandTimeout = 0;

            string strReturn = "";
            try
            {
                myCommand.CommandText = strSQL.ToString().Trim();

                for (int i = 0; i < oData[0].Length; i++)
                {
                    myCommand.Parameters.AddWithValue(oData[0][i].ToString(), oData[1][i]);
                }

                strReturn = myCommand.ExecuteScalar().ToString();

                myConnection.Close();
            }
            catch (SqlException e)
            {

                try { myConnection.Close(); }
                catch { }

                commonWriteFile.writeErrorLog(myCommand.CommandText + " " + e.Message.ToString(), "execute single sql Error");
                strReturn = "false";
            }
            finally
            {
                try { myConnection.Close(); }
                catch { }
            }

            return strReturn;
        }

        ///<summary>Alinta：執行Sql指令,一筆指令 </summary>
        ///<param name="strSQL">只有一筆Sql指令要執行</param> 
        /// <param name="oData">帶入Paramber資料</param>
        /// <param name="IfTimeOut"> true/false 是否timeout為30秒,預設true  </param>
        public bool ExecuteForParam(string strSQL, Object[][] oData, bool IfTimeOut)
        {
            SqlConnection myConnection = this.nowConnection;
            myConnection.Open();

            SqlCommand myCommand = new SqlCommand();

            myCommand.Connection = myConnection;

            if (!IfTimeOut)
                myCommand.CommandTimeout = 0;

            bool bolReturn = true;
            try
            {
                myCommand.CommandText = strSQL.ToString().Trim();
                for (int i = 0; i < oData[0].Length; i++)
                {
                    myCommand.Parameters.AddWithValue(oData[0][i].ToString(), oData[1][i]);
                }

                myCommand.ExecuteNonQuery();

                bolReturn = true;
                myConnection.Close();
            }
            catch (SqlException e)
            {

                try { myConnection.Close(); }
                catch { }

                commonWriteFile.writeErrorLog(myCommand.CommandText + " " + e.Message.ToString(), "execute single sql Error");
                bolReturn = false;
            }
            finally
            {
                try { myConnection.Close(); }
                catch { }
            }

            return bolReturn;
        }

    }
    #endregion
}