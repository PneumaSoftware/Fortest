using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrixModel;
using System.Web.Mvc;
using PagedList;

namespace OrixMvc.ViewModels
{
    public class WE020VO
    {
        /// <summary>
        /// 申請書編號
        /// </summary>
        public string APLY_NO { get; set; }

        /// <summary>
        /// 契約起始日期
        /// </summary>
        public string con_date_fr { get; set; }

        /// <summary>
        /// 契約終止日期
        /// </summary>
        public string con_date_to { get; set; }


        /// <summary>
        /// 狀況
        /// </summary>
        public string cursts { get; set; }

        /// <summary>
        /// 客戶代號
        /// </summary>
        public string Cust_No { get; set; }

        /// <summary>
        /// 客戶簡稱
        /// </summary>
        public string Cust_Sname { get; set; }

        /// <summary>
        /// 連絡人
        /// </summary>
        public string Contact { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string PROD_NAME { get; set; }

        /// <summary>
        /// 機號
        /// </summary>
        public string MAC_NO { get; set; }

        /// <summary>
        /// 經銷商簡稱
        /// </summary>
        public string FRC_SNAME { get; set; }

        /// <summary>
        /// 計張
        /// </summary>
        public string PAPER { get; set; }

        /// <summary>
        /// 分開發票
        /// </summary>
        public string DIVIDE { get; set; }

        /// <summary>
        /// MERGE_NO
        /// </summary>
        public string MERGE_NO { get; set; }

        /// <summary>
        /// 月數
        /// </summary>
        public int APRV_DURN_M { get; set; }

        /// <summary>
        /// 期數
        /// </summary>
        public int APRV_PERD { get; set; }

        /// <summary>
        /// 期租金
        /// </summary>
        public int APRV_HIRE { get; set; }

        /// <summary>
        /// 動產擔保 品名
        /// </summary>
        public string MPROD_NAME { get; set; }

        /// <summary>
        /// 動產擔保 車號
        /// </summary>
        public string MCAR_NO { get; set; }

        #region 搜尋條件

        /// <summary>
        /// 連絡人
        /// </summary>
        public string PCONTACT { get; set; }

        /// <summary>
        /// 客戶代號
        /// </summary>
        public string PCUST_NO { get; set; }

        /// <summary>
        /// 搜尋共同承租人
        /// </summary>
        public string PIS_SEARCH { get; set; }

        /// <summary>
        /// 客戶名稱
        /// </summary>
        public string PCUST_NAME { get; set; }

        /// <summary>
        /// 經銷商代碼
        /// </summary>
        public string FRC_CODE { get; set; }

        /// <summary>
        /// 經銷商名稱
        /// </summary>
        public string FRC_NAME { get; set; }

        /// <summary>
        /// 連絡電話
        /// </summary>
        public string PCTAC_TEL { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string PRECV_NAME { get; set; }

        /// <summary>
        /// 存放地址
        /// </summary>
        public string PSEND_ADDR { get; set; }

        /// <summary>
        /// 請款地址
        /// </summary>
        public string PADDR { get; set; }

        /// <summary>
        /// 品名
        /// </summary>
        public string PPROD_NAME { get; set; }

        /// <summary>
        /// 機號
        /// </summary>
        public string PMAC_NO { get; set; }

        /// <summary>
        /// 申請日期 , Start
        /// </summary>
        public string PAPLY_DATE_S { get; set; }

        /// <summary>
        /// 申請日期 , End
        /// </summary>
        public string PAPLY_DATE_E { get; set; }

        /// <summary>
        /// 目前狀況
        /// </summary>
        public string PCUR_STS { get; set; }

        /// <summary>
        /// 業務員
        /// </summary>
        public string PEMP_CODE { get; set; }

        /// <summary>
        /// 案件來源
        /// </summary>
        public string PCASE_SOUR { get; set; }

        /// <summary>
        /// 含作廢駁回
        /// </summary>
        public string PIS_DEL { get; set; }

        /// <summary>
        /// 部門代號
        /// </summary>
        public string PDEPT_CODE { get; set; }

        /// <summary>
        /// 發票號碼
        /// </summary>
        public string PINV_NO { get; set; }

        /// <summary>
        /// 車號
        /// </summary>
        public string PCAR_NO { get; set; }

        #endregion 
        #region 申請書

        /// <summary>
        /// 請款地址
        /// </summary>
        public string REQ_PAY_ADDR { get; set; }



        /// <summary>
        /// 統一郵寄
        /// </summary>
        public string MMail_NO { get; set; }

        /// <summary>
        /// 契約總額
        /// </summary>
        public int CUR_CON_AMT { get; set; }
        #endregion
        #region 標的物
        /// <summary>
        /// 序號
        /// </summary>
        public string Row { get; set; }



        /// <summary>
        /// 規格
        /// </summary>
        public string SPEC { get; set; }

        /// <summary>
        /// 市價
        /// </summary>
        public int Market_price { get; set; }

        /// <summary>
        /// 殘值
        /// </summary>
        public int RV_AMT { get; set; }

        /// <summary>
        /// 標的物狀態
        /// </summary>
        public string OBJ_STS { get; set; }

        /// <summary>
        /// 標的物代號
        /// </summary>
        public string OBJ_CODE { get; set; }

        /// <summary>
        /// 所在地址
        /// </summary>
        public string OBJ_LOC_ADDR { get; set; }

        /// <summary>
        /// 所在電話
        /// </summary>
        public string OBJ_LOC_TEL { get; set; }

        #endregion
        #region FunctionClick
        public class FunctionClick
        {
            public string rowAPLY { get; set; }
            public string rowOBJECT { get; set; }
            public string functionIndex { get; set; }
        }
        #endregion

    }
}