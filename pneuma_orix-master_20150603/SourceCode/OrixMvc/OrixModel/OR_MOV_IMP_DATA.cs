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
    
    public partial class OR_MOV_IMP_DATA
    {
        public string CON_NO { get; set; }
        public decimal SEQ_NO { get; set; }
        public string OBJ_CTENT { get; set; }
        public string OBJ_OWNER { get; set; }
        public string IMP_TYPE { get; set; }
        public decimal OBJ_TOL_AMT { get; set; }
        public decimal COLL_AMT { get; set; }
        public decimal PERD_AMT { get; set; }
        public string PAPER_DATE_FR { get; set; }
        public string PAPER_DATE_TO { get; set; }
        public string SET_DATE_FR { get; set; }
        public string SET_DATE_TO { get; set; }
        public string EXT_DATE_TO { get; set; }
        public string IMP_CANCEL_DATE { get; set; }
        public string CANCEL_REASON { get; set; }
        public string ANNU_FLAG { get; set; }
        public string IF_USE_PCASE { get; set; }
        public string PCASE_CON_NO { get; set; }
        public Nullable<decimal> PCASE_SET_AMT { get; set; }
        public string ADD_USER_ID { get; set; }
        public string ADD_DATE { get; set; }
        public string ADD_TIME { get; set; }
        public string LAST_UPD_USER_ID { get; set; }
        public string LAST_UPD_DATE { get; set; }
        public string LAST_UPD_TIME { get; set; }
    
        public virtual OR_CONTRACT OR_CONTRACT { get; set; }
    }
}
