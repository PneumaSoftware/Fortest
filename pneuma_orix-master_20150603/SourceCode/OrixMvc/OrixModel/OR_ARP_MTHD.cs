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
    
    public partial class OR_ARP_MTHD
    {
        public string CON_NO { get; set; }
        public decimal SEQ_NO { get; set; }
        public string ARP_MTHD { get; set; }
        public decimal PAY_AMT { get; set; }
        public string TLT_NAME { get; set; }
        public string DATE { get; set; }
        public string PAPER_ACCT_NO { get; set; }
        public string DESC { get; set; }
        public string ANNU_FLAG { get; set; }
        public string CHECK_DATE { get; set; }
        public string BANK_CODE { get; set; }
        public string BANK_BRANCH { get; set; }
    
        public virtual OR_CONTRACT OR_CONTRACT { get; set; }
    }
}