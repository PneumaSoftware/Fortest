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
    
    public partial class OR_SALES_AUTH
    {
        public OR_SALES_AUTH()
        {
            this.OR_SALES_AUTH_DTL = new HashSet<OR_SALES_AUTH_DTL>();
            this.OR_SALES_AUTH_HIST = new HashSet<OR_SALES_AUTH_HIST>();
        }
    
        public decimal AD_YEAR { get; set; }
        public decimal AUTH_SART { get; set; }
        public decimal AUTH_END { get; set; }
        public decimal FUND_C { get; set; }
    
        public virtual ICollection<OR_SALES_AUTH_DTL> OR_SALES_AUTH_DTL { get; set; }
        public virtual ICollection<OR_SALES_AUTH_HIST> OR_SALES_AUTH_HIST { get; set; }
    }
}