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
    
    public partial class OR_RLTY_IMP_DATA
    {
        public string CON_NO { get; set; }
        public decimal SEQ_NO { get; set; }
        public string CREDITOR { get; set; }
        public string DEBTOR { get; set; }
        public string COLL_PROVID_ID { get; set; }
        public string COLL_PROVID_NAME { get; set; }
        public string CITY { get; set; }
        public string PAWN_LAND_GOV { get; set; }
        public string PAWN_LAND_NO { get; set; }
        public string BUILD_NO { get; set; }
        public string PAWN_ROAD_NO { get; set; }
        public string PAWN_SUB_ROAD_NO { get; set; }
        public string SALES_RGT_ADDR { get; set; }
        public decimal SET_ORD { get; set; }
        public decimal SET_AMT { get; set; }
        public decimal PCASE_SET_AMT { get; set; }
        public string IF_USE_PCASE { get; set; }
        public string PCASE_CON_NO { get; set; }
        public string SET_DATE_FR { get; set; }
        public string SET_DATE_TO { get; set; }
        public string EXT_DATE_TO { get; set; }
        public string IMP_CANCEL_DATE { get; set; }
        public string CANCEL_REASON { get; set; }
        public string ANNU_FLAG { get; set; }
        public string REMARK { get; set; }
        public string ADD_USER_ID { get; set; }
        public string ADD_DATE { get; set; }
        public string ADD_TIME { get; set; }
        public string LAST_UPD_USER_ID { get; set; }
        public string LAST_UPD_DATE { get; set; }
        public string LAST_UPD_TIME { get; set; }
    
        public virtual OR_CONTRACT OR_CONTRACT { get; set; }
    }
}