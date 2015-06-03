using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrixModel;
using System.Web.Mvc;

namespace OrixMvc.ViewModels
{
    public class WE0101VO
    {
        //行業別
        public SelectList CUST_TYPE_CODE { get; set; }

        //租賃公會行業別
        public SelectList CUST_UNION { get; set; }

        //國別
        public SelectList NATIONALITY { get; set; }

        //特殊客戶條件
        public SelectList SPEC_COND { get; set; }

        //組織型態
        public SelectList ORG_TYPE { get; set; }

        //發票開立方式
        public SelectList INVOICE { get; set; }

        //CITY_CODE
        public SelectList CITY_CODE { get; set; }

        //ZIP_CODE
        public SelectList ZIP_CODE { get; set; }

        //Hint
        public List<OR3_HINT> Hint { get; set; }

        //明細
        public DetailData Detail { get; set; }

        public class DetailData
        {
            public string hMode { get; set; }
            public string CUST_NO { get; set; }
            public string CUST_SNAME { get; set; }
            public string CUST_BLOC_CODE { get; set; }
            public string BLOC_SNAME { get; set; }
            public string SPEC_COND { get; set; }
            public string CUST_TYPE_CODE { get; set; }
            public string CUST_UNION { get; set; }
            public string NATIONALITY { get; set; }
            public string CUST_NAME { get; set; }
            public string UNIF_NO { get; set; }
            public string EN_NAME { get; set; }
            public string EN_SNAME { get; set; }
            public string CONTACT { get; set; }
            public string CTAC_TITLE { get; set; }
            public string CTAC_EXT { get; set; }
            public string STOCKCODE { get; set; }
            public string CONTACT2 { get; set; }
            public string CTAC_TITLE2 { get; set; }
            public string CTAC_EXT2 { get; set; }
            public string TAKER { get; set; }
            public string TAKER_ID { get; set; }
            public string FLOT_DATE { get; set; }
            public string BUILD_DATE { get; set; }
            public string PHONE1 { get; set; }
            public string PHONE2 { get; set; }
            public string FACSIMILE { get; set; }
            public string EMP_PSNS { get; set; }
            public string SALES_RGT_ADDR { get; set; }
            public string RGT_CAPT_AMT { get; set; }
            public string CTAC_ADDR { get; set; }
            public string REAL_CAPT_AMT { get; set; }
            public string IS_COND_AUTH { get; set; }
            public string IS_BIZ_CUST { get; set; }
            public string IS_HONEST_AGREEMENT { get; set; }
            public string IS_SECRET_PROMISE { get; set; }
            public string IS_TRANSACTION { get; set; }
            public string ORG_TYPE { get; set; }
            public string INVOICE { get; set; }
            public string MAIN_BUS_ITEM { get; set; }
            public string PARENT_COMP_NAME { get; set; }
            public string PARENT_COMP_STOCK_CODE { get; set; }
            public string GEN_CURR_QUOTA { get; set; }
            public string VP_CURR_QUOTA { get; set; }
            public string AR_CURR_QUOTA { get; set; }
            public string ZIP_CODE { get; set; }
            public string CITY_CODE { get; set; }
            public string CUST_STS { get; set; }
            public string RGT_ZIP_CODE { get; set; }
            public string RGT_CITY_CODE { get; set; }
            public string BACKGROUND { get; set; }
            public string IS_CAR_LICENSE { get; set; }
            public string MANAGER { get; set; }
            public string MANAGER_ID { get; set; }
            public string INDUSTRY_NO { get; set; }
            public string USER_ID { get; set; }
        }

        public class BLOC {
            public string KEY_NO { get; set; }
            public string BLOC_NAME { get; set; }
            public string BLOC_SNAME { get; set; }
        }
    }

}