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
    
    public partial class ORIXSend
    {
        public string TYPE { get; set; }
        public int SEQ { get; set; }
        public string RECVER { get; set; }
        public string ADDR { get; set; }
        public string VOCH_DATA { get; set; }
        public string StampedYN { get; set; }
        public string CheckYN { get; set; }
        public Nullable<double> AMT { get; set; }
        public string Entry_ID { get; set; }
        public string Entry_Data { get; set; }
        public string Entry_Time { get; set; }
        public string Proc_Date { get; set; }
        public string Proc_Time { get; set; }
        public string Doc_Status { get; set; }
        public string Register_Num { get; set; }
        public string Void_Name { get; set; }
        public Nullable<System.DateTime> Void_Date { get; set; }
        public string Proof_Num { get; set; }
        public string Modify_ID { get; set; }
        public string Modify_Date { get; set; }
        public string Modify_Time { get; set; }
        public string Cust_Name { get; set; }
        public string Proced_File { get; set; }
        public string Aply_no { get; set; }
        public string Area { get; set; }
        public string XEROX { get; set; }
    }
}
