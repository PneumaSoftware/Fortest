USE [USCACCT]
GO

/****** Object:  Table [dbo].[ACC11]    Script Date: 04/27/2015 14:47:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ACC11](
	[CUST_NO] [nvarchar](10) NOT NULL,
	[CUST_NAME] [nvarchar](80) NULL,
	[CUST_ABBR] [nvarchar](10) NULL,
	[CUST_NAME_ENG] [nvarchar](100) NULL,
	[CUST_ABBR_ENG] [nvarchar](50) NULL,
	[FOR_YN] [nvarchar](1) NULL,
	[EDI_NO] [nvarchar](20) NULL,
	[CORP_KIND] [nvarchar](4) NULL,
	[UNIF_NO] [nvarchar](10) NULL,
	[TAX_NO] [nvarchar](10) NULL,
	[CHIEF_NAME] [nvarchar](20) NULL,
	[TEL_CUST] [nvarchar](30) NULL,
	[FAX_CUST] [nvarchar](30) NULL,
	[ZIP_CODE] [nvarchar](6) NULL,
	[ADDR_CUST] [nvarchar](100) NULL,
	[ADDR_ENG] [nvarchar](200) NULL,
	[ZIP_CONT] [nvarchar](6) NULL,
	[ADDR_CONT] [nvarchar](100) NULL,
	[CONT_NAME] [nvarchar](20) NULL,
	[ADDR_CONT_ENG] [nvarchar](200) NULL,
	[ADDR_INVO] [nvarchar](100) NULL,
	[INVO_TITLE] [nvarchar](50) NULL,
	[ZIP_ACCT] [nvarchar](6) NULL,
	[ADDR_ACCT] [nvarchar](100) NULL,
	[ACCT_NAME] [nvarchar](20) NULL,
	[TEL_ACCT] [nvarchar](30) NULL,
	[ZIP_CORP] [nvarchar](6) NULL,
	[ADDR_CORP] [nvarchar](100) NULL,
	[CORP_NAME] [nvarchar](20) NULL,
	[TEL_CORP] [nvarchar](30) NULL,
	[EMAIL] [nvarchar](50) NULL,
	[EMP_NO] [nvarchar](10) NULL,
	[RECE_WAY] [nvarchar](1) NULL,
	[CONT_NUM] [smallint] NULL,
	[MM_DAY] [smallint] NULL,
	[TRADE_COND] [nvarchar](4) NULL,
	[PAY_WAY] [nvarchar](4) NULL,
	[PAY_DAY] [smallint] NULL,
	[ACCT_NO] [nvarchar](15) NULL,
	[BANK_NO] [nvarchar](40) NULL,
	[CRED_GRADE] [nvarchar](2) NULL,
	[DISC_RATE] [float] NULL,
	[ST_DATE] [nvarchar](10) NULL,
	[END_DATE] [nvarchar](10) NULL,
	[SALE_CUST_PROD] [nvarchar](1) NULL,
	[CRED_AMT] [money] NULL,
	[BAL_OR_SA] [nvarchar](2) NULL,
	[ORDER_MUST] [nvarchar](2) NULL,
	[AREA_CODE] [nvarchar](10) NULL,
	[SALE_TYPE] [nvarchar](10) NULL,
	[BEGIN_DATE] [nvarchar](10) NULL,
	[SALE_NO] [nvarchar](10) NULL,
	[SCORP_NO] [nvarchar](10) NULL,
	[SDEPT_NO] [nvarchar](10) NULL,
	[PAY_RANGE] [nvarchar](20) NULL,
	[REMK] [nvarchar](60) NULL,
	[FIELD_1] [nvarchar](100) NULL,
	[FIELD_2] [nvarchar](100) NULL,
	[ENTRY_ID] [nvarchar](10) NULL,
	[ENTRY_DATE] [nvarchar](10) NULL,
	[ENTRY_TIME] [nvarchar](6) NULL,
	[MODIFY_ID] [nvarchar](10) NULL,
	[MODIFY_DATE] [nvarchar](10) NULL,
	[MODIFY_TIME] [nvarchar](6) NULL,
	[XTYPE] [nvarchar](50) NULL,
	[COM] [nvarchar](50) NULL,
	[BELONG_TO] [nvarchar](20) NULL,
	[JOIN_SELF_TYPE] [nvarchar](1) NULL,
	[TAX_INNER_TYPE] [nvarchar](1) NULL,
	[FIELD_3] [nvarchar](100) NULL,
	[FIELD_4] [nvarchar](100) NULL,
	[FIELD_5] [nvarchar](100) NULL,
	[FIELD_6] [nvarchar](100) NULL,
	[FIELD_7] [nvarchar](100) NULL,
	[FIELD_8] [nvarchar](100) NULL,
	[FIELD_9] [nvarchar](100) NULL,
	[FIELD_10] [nvarchar](100) NULL,
	[ATTACH_PAPER] [nvarchar](250) NULL,
	[INVO_XTYPE] [nvarchar](1) NULL,
 CONSTRAINT [PK_ACC11] PRIMARY KEY NONCLUSTERED 
(
	[CUST_NO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

