using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OrixModel;
using System.Web.Mvc;
using PagedList;

namespace OrixMvc.ViewModels
{
    public class WE010VO
    {
        //查詢條件
        public string CUST_BLOC_CODE { get; set; }
        public string CUST_BLOC_SNAME { get; set; }
        public string IS_TRANSACTION { get; set; }
        public string SPEC_COND { get; set; }
        public string CUST_NO { get; set; }
        public string CUST_SNAME { get; set; }
        public string CUST_NAME { get; set; }
        public string CUST_STS { get; set; }

        //查詢結果
        public IPagedList<GridVO> GridData { get; set; }

        //目前頁數
        public int PageIndex { get; set; }

        //每頁筆數
        public int PageSize { get; set; }

        //總筆數
        public int TotalData { get; set; }

        //Hint
        public List<OR3_HINT> Hint { get; set; }

        public class GridVO
        {
            public string CUST_NO { get; set; }
            public string CUST_SNAME { get; set; }
            public string CUST_BLOC_CODE { get; set; }
            public string BLOC_SNAME { get; set; }
            public string SPEC_COND { get; set; }
        }
    }

}