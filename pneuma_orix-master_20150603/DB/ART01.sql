USE [USCACCT]
GO

/****** Object:  Table [dbo].[ART01]    Script Date: 05/29/2015 16:21:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ART01](
	[CORP_NO] [varchar](10) NOT NULL,
	[RECP_NO] [varchar](15) NOT NULL,
	[RECP_DATE] [varchar](10) NULL,
	[DATA_TYPE] [varchar](1) NOT NULL,
	[DATA_SEQ] [varchar](4) NOT NULL,
	[SALE_NO] [varchar](6) NULL,
	[CASE_NO] [varchar](8) NULL,
	[CERT_NO] [varchar](15) NULL,
	[DATA_DATE] [varchar](10) NULL,
	[PROD_NO] [varchar](20) NULL,
	[SIZE] [varchar](10) NULL,
	[COLOR] [varchar](10) NULL,
	[PROD_UNIT] [varchar](10) NULL,
	[UNIT_PRICE] [money] NULL,
	[UNIT_PRICE_NT] [money] NULL,
	[QTY] [money] NULL,
	[CURR_CODE] [varchar](3) NOT NULL,
	[CURR_RATE] [money] NULL,
	[PROD_AMT] [money] NULL,
	[PROD_AMT_NT] [money] NULL,
	[RECE_AMT] [money] NULL,
	[RECE_AMT_NT] [money] NULL,
	[ORDER_NO] [varchar](15) NOT NULL,
	[INVO_NO] [varchar](10) NULL,
	[INVO_DATE] [varchar](10) NULL,
	[CUST_RECP_NO] [varchar](15) NULL,
	[CUST_RECP_DATE] [varchar](10) NULL,
	[PRED_AP_DATE] [varchar](10) NULL,
	[CHEQ_NO] [varchar](15) NULL,
	[CHEQ_AMT] [money] NULL,
	[DUE_DATE] [varchar](10) NULL,
	[ACCT_NO] [varchar](15) NULL,
	[BANK_NO] [varchar](10) NULL,
	[CHEQ_KIND] [varchar](1) NULL,
	[CUST_YN] [varchar](1) NULL,
	[CUST_NO] [varchar](10) NOT NULL,
	[REMK] [varchar](60) NULL,
	[ENTRY_ID] [varchar](10) NULL,
	[ENTRY_DATE] [varchar](10) NULL,
	[ENTRY_TIME] [varchar](8) NULL,
	[MODIFY_ID] [varchar](10) NULL,
	[MODIFY_DATE] [varchar](10) NULL,
	[MODIFY_TIME] [varchar](8) NULL,
	[INDE_NO] [varchar](10) NULL,
 CONSTRAINT [PK_ART01] PRIMARY KEY CLUSTERED 
(
	[CORP_NO] ASC,
	[RECP_NO] ASC,
	[DATA_TYPE] ASC,
	[DATA_SEQ] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

