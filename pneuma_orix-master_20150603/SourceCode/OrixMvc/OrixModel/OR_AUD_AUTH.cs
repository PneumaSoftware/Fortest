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
    
    public partial class OR_AUD_AUTH
    {
        public OR_AUD_AUTH()
        {
            this.OR_IMPS_APLY_IMPS_AUD = new HashSet<OR_IMPS_APLY_IMPS_AUD>();
            this.OR_IMPS_APLY_SELL_AUD = new HashSet<OR_IMPS_APLY_SELL_AUD>();
        }
    
        public string AUD_LVL_CODE { get; set; }
        public string CHECK_PCASE { get; set; }
        public Nullable<decimal> GEN_CASE_AUTH_AMT_LINE { get; set; }
        public Nullable<decimal> GEN_CASE_AUTH_AMT_LMT { get; set; }
        public Nullable<decimal> FANC_CASE_AUTH_AMT_LINE { get; set; }
        public Nullable<decimal> FANC_CASE_AUTH_AMT_LMT { get; set; }
        public Nullable<decimal> VP_CASE_AUTH_AMT_LINE { get; set; }
        public Nullable<decimal> VP_CASE_AUTH_AMT_LMT { get; set; }
        public Nullable<decimal> SPEC_CASE_AUTH_AMT_LINE { get; set; }
        public Nullable<decimal> SPEC_CASE_AUTH_AMT_LMT { get; set; }
        public Nullable<decimal> RECV_ACCT_ENDOR_AMT_LINE { get; set; }
        public Nullable<decimal> RECV_ACCT_ENDOR_AMT_LMT { get; set; }
        public string ADD_USER_ID { get; set; }
        public string ADD_DATE { get; set; }
        public string ADD_TIME { get; set; }
        public string LAST_UPD_USER_ID { get; set; }
        public string LAST_UPD_DATE { get; set; }
        public string LAST_UPD_TIME { get; set; }
    
        public virtual OR_AUD_LVL_NAME OR_AUD_LVL_NAME { get; set; }
        public virtual ICollection<OR_IMPS_APLY_IMPS_AUD> OR_IMPS_APLY_IMPS_AUD { get; set; }
        public virtual ICollection<OR_IMPS_APLY_SELL_AUD> OR_IMPS_APLY_SELL_AUD { get; set; }
    }
}