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
    
    public partial class OR_BLOC
    {
        public OR_BLOC()
        {
            this.OR_BLOC11 = new HashSet<OR_BLOC>();
        }
    
        public string BLOC_NO { get; set; }
        public string PARENT_BLOC_NO { get; set; }
        public string BLOC_NAME { get; set; }
        public string BLOC_SNAME { get; set; }
        public string BLOC_DESC { get; set; }
        public decimal CREDIT_BLOC { get; set; }
    
        public virtual OR_BLOC OR_BLOC1 { get; set; }
        public virtual OR_BLOC OR_BLOC2 { get; set; }
        public virtual ICollection<OR_BLOC> OR_BLOC11 { get; set; }
        public virtual OR_BLOC OR_BLOC3 { get; set; }
    }
}
