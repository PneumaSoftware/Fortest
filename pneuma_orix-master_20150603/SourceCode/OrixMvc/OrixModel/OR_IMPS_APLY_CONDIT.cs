//------------------------------------------------------------------------------
// <auto-generated>
//    這個程式碼是由範本產生。
//
//    對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//    如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace OrixModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class OR_IMPS_APLY_CONDIT
    {
        public OR_IMPS_APLY_CONDIT()
        {
            this.OR_IMPS_APLY_IMPS_AUD = new HashSet<OR_IMPS_APLY_IMPS_AUD>();
        }
    
        public string APLY_NO { get; set; }
        public string FRC_CODE { get; set; }
        public string CUST_NO { get; set; }
        public string TRAN_CASE_TYPE { get; set; }
        public string TRACE_COND { get; set; }
        public string SELL_SUPL_TICKET { get; set; }
        public string IF_PAY_DRAFT { get; set; }
        public string OBL_TRANS_NOTY { get; set; }
        public string ACCT_TRANS_DOC { get; set; }
        public string MAIN_TRAN_PROD { get; set; }
        public decimal MAX_ENDOR_AMT { get; set; }
        public decimal PCT_AVD_PCT { get; set; }
        public decimal MANG_RATE { get; set; }
        public string TRAN_COND { get; set; }
        public string PAY_TYPE_BUYER_ { get; set; }
        public string PAY_TYPE_BUYER__O { get; set; }
        public string OTH_RMK { get; set; }
        public decimal TRAN_REC_APLY { get; set; }
        public decimal TRAN_REC_ACCT { get; set; }
        public decimal TRAN_REC_REN { get; set; }
        public decimal TRAN_REC_VP { get; set; }
        public decimal TRAN_REC_CAR { get; set; }
        public decimal TRAN_REC_REL { get; set; }
        public decimal TRAN_MAX_SUR { get; set; }
        public string TRAN_MAX_SUR_DATE { get; set; }
        public string APRV_DATE { get; set; }
    
        public virtual OR_FRC OR_FRC { get; set; }
        public virtual OR_IMPS_APLY_BASE OR_IMPS_APLY_BASE { get; set; }
        public virtual OR_IMPS_CUST OR_IMPS_CUST { get; set; }
        public virtual ICollection<OR_IMPS_APLY_IMPS_AUD> OR_IMPS_APLY_IMPS_AUD { get; set; }
    }
}
