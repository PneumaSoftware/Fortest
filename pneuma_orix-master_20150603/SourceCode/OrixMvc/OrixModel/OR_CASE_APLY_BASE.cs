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
    
    public partial class OR_CASE_APLY_BASE
    {
        public OR_CASE_APLY_BASE()
        {
            this.OR_CASE_APLY_IMAGE = new HashSet<OR_CASE_APLY_IMAGE>();
        }
    
        public string APLY_NO { get; set; }
        public string CUST_NO { get; set; }
        public string EMP_CODE { get; set; }
        public string DEPT_CODE { get; set; }
        public string CON_SEQ_NO { get; set; }
        public string OLD_CON_NO { get; set; }
        public string APLY_DATE { get; set; }
        public string REQ_PAY_ADDR { get; set; }
        public string REQU_ZIP { get; set; }
        public string CONTACT { get; set; }
        public string RECVER { get; set; }
        public string RECVER_DEPT { get; set; }
        public string CTAC_TEL { get; set; }
        public string FAX { get; set; }
        public decimal PAY_COND_DAY { get; set; }
        public decimal PAPER_DURN_D { get; set; }
        public Nullable<decimal> PAY_DAY { get; set; }
        public string PRJ_PAY_DATE { get; set; }
        public decimal THIS_MTH_RATE { get; set; }
        public decimal REL_SUM_CON_AMT { get; set; }
        public decimal REL_SUM_CON_SUR { get; set; }
        public decimal SUM_CON_AMT { get; set; }
        public decimal SUM_CON_SUR { get; set; }
        public decimal CUR_CON_AMT { get; set; }
        public decimal CON_TOL_SUR { get; set; }
        public decimal TRAN_MAX_SUR { get; set; }
        public string TRAN_MAX_SUR_DATE { get; set; }
        public string CUR_STS { get; set; }
        public string APLY_APRV_DATE { get; set; }
        public Nullable<decimal> DELAY_DIVD_RATE { get; set; }
        public string ANNU_ACC { get; set; }
        public string ANNU_FIN { get; set; }
        public string ANNU_CRD { get; set; }
        public string ADDCON { get; set; }
        public string PAPER { get; set; }
        public string DIVIDE { get; set; }
        public Nullable<decimal> PAY_DAY1 { get; set; }
        public Nullable<decimal> PAPER_DURN_D1 { get; set; }
        public Nullable<decimal> PAY_COND_DAY1 { get; set; }
        public Nullable<decimal> DELAY_DIVD_RATE1 { get; set; }
        public string N_EMP_CODE { get; set; }
        public string N_DEPT_CODE { get; set; }
        public string KEYIN_USER { get; set; }
        public string KEYIN_DATE { get; set; }
        public string KEYIN_TIME { get; set; }
        public string UPD_USER { get; set; }
        public string UPD_DATE { get; set; }
        public string UPD_TIME { get; set; }
        public string ASUR_DATE { get; set; }
        public string M_Mail { get; set; }
        public string MMail_NO { get; set; }
        public string Other_Condition { get; set; }
        public string Financial_Purpose { get; set; }
        public string Expect_Ar_Date { get; set; }
        public string Auth_Cond_Remark { get; set; }
        public string InitContactDate { get; set; }
        public string SupplierBackground { get; set; }
        public string MainCondition { get; set; }
        public string PAY_CTAC { get; set; }
        public string PAY_TEL { get; set; }
        public string CEN_CHK { get; set; }
        public Nullable<System.DateTime> CEN_DATE { get; set; }
        public string CEN_ID { get; set; }
        public string credit_yn { get; set; }
        public string Case_NO { get; set; }
        public string FAST_STS { get; set; }
        public string ORG_QUOTA_APLY_NO { get; set; }
        public string CUR_QUOTA_APLY_NO { get; set; }
        public string Mobile { get; set; }
        public string PROCESS_EMP { get; set; }
        public string MAST_CON_NO { get; set; }
        public string OVER_CHK { get; set; }
        public Nullable<System.DateTime> OVER_DATE { get; set; }
        public string OVER_ID { get; set; }
    
        public virtual OR_CASE_APLY_CREDIT OR_CASE_APLY_CREDIT { get; set; }
        public virtual ICollection<OR_CASE_APLY_IMAGE> OR_CASE_APLY_IMAGE { get; set; }
        public virtual OR_FAULT OR_FAULT { get; set; }
    }
}
