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
    
    public partial class OR_INV_DATA
    {
        public string CON_NO { get; set; }
        public string INV_NO { get; set; }
        public string C_CORP_NO_IVM02_ { get; set; }
        public string INV_TITLE { get; set; }
        public string TAX_TYPE { get; set; }
        public decimal INV_PAY { get; set; }
        public decimal INV_TAX { get; set; }
        public decimal INV_AMT { get; set; }
    
        public virtual OR_CONTRACT OR_CONTRACT { get; set; }
    }
}
