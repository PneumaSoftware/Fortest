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
    
    public partial class GuestBook
    {
        public int 留言編號 { get; set; }
        public string 姓名 { get; set; }
        public string 性別 { get; set; }
        public string Email { get; set; }
        public string 留言主題 { get; set; }
        public string 留言內容 { get; set; }
        public string 表情 { get; set; }
        public Nullable<System.DateTime> 留言日期 { get; set; }
        public Nullable<int> 回覆篇數 { get; set; }
        public Nullable<int> 點閱數 { get; set; }
        public string IP { get; set; }
    }
}
