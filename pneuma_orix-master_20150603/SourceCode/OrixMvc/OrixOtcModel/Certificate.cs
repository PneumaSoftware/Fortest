//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrixOtcModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Certificate
    {
        public int SEQ { get; set; }
        public string Doc_STATUS { get; set; }
        public string IN_STOCK_Num { get; set; }
        public Nullable<System.DateTime> SETING_DATE { get; set; }
        public string FUND_NUM { get; set; }
        public string BANK_ACCT_NO { get; set; }
        public string BANK_ACCT_NAME { get; set; }
        public Nullable<double> ACEP_AMT { get; set; }
        public Nullable<System.DateTime> STAR_DATE_DUE { get; set; }
        public Nullable<System.DateTime> DUE_DATE { get; set; }
        public string BANK_NAME { get; set; }
        public string MEMO { get; set; }
        public Nullable<int> QTY { get; set; }
        public string Crd_File { get; set; }
        public Nullable<System.DateTime> IN_STOCK_DATE { get; set; }
        public string CHECK_USER { get; set; }
        public Nullable<System.DateTime> CHECK_DATE { get; set; }
        public string entry_id { get; set; }
        public Nullable<System.DateTime> entry_date { get; set; }
        public string Crd_Del { get; set; }
        public Nullable<System.DateTime> Crd_DelDate { get; set; }
        public string modify_id { get; set; }
        public Nullable<System.DateTime> modify_date { get; set; }
        public Nullable<int> TOL_MTH { get; set; }
    }
}