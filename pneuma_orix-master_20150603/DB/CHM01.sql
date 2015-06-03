USE [USCACCT]
GO

/****** Object:  Table [dbo].[CHM01]    Script Date: 05/29/2015 16:22:23 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[CHM01](
	[CORP_NO] [varchar](10) NOT NULL,
	[CHEQ_NO] [varchar](15) NOT NULL,
	[DUE_DATE] [varchar](10) NOT NULL,
	[CHEQ_AMT] [money] NULL,
	[CUST_NO] [varchar](10) NULL,
	[BANK_NO] [varchar](10) NULL,
	[ACCT_NO] [varchar](15) NULL,
	[CHEQ_KIND] [varchar](1) NULL,
	[CUST_YN] [varchar](1) NULL,
	[RECE_DATE] [varchar](10) NULL,
	[EMP_NO] [varchar](10) NULL,
	[REMK] [varchar](60) NULL,
	[VOCH_DATE] [varchar](10) NULL,
	[VOCH_NO] [varchar](10) NULL,
	[ENTRY_ID] [varchar](10) NULL,
	[ENTRY_DATE] [varchar](10) NULL,
	[ENTRY_TIME] [varchar](8) NULL,
	[MODIFY_ID] [varchar](10) NULL,
	[MODIFY_DATE] [varchar](10) NULL,
	[MODIFY_TIME] [varchar](8) NULL,
	[REDE_DATE] [varchar](10) NULL,
	[REDE_BANK_NO] [varchar](10) NULL,
	[REDE_ACCT_NO] [varchar](15) NULL,
	[REDE_VOCH_DATE] [varchar](10) NULL,
	[REDE_VOCH_NO] [varchar](10) NULL,
	[REDE_MODIFY_ID] [varchar](10) NULL,
	[REDE_MODIFY_DATE] [varchar](10) NULL,
	[REDE_MODIFY_TIME] [varchar](8) NULL,
	[ENTR_DATE] [varchar](10) NULL,
	[ENTR_BANK_NO] [varchar](10) NULL,
	[ENTR_ACCT_NO] [varchar](15) NULL,
	[ENTR_VOCH_DATE] [varchar](10) NULL,
	[ENTR_VOCH_NO] [varchar](10) NULL,
	[ENTR_MODIFY_ID] [varchar](10) NULL,
	[ENTR_MODIFY_DATE] [varchar](10) NULL,
	[ENTR_MODIFY_TIME] [varchar](8) NULL,
	[BACK_DATE] [varchar](10) NULL,
	[BACK_REASON] [varchar](1) NULL,
	[BACK_VOCH_DATE] [varchar](10) NULL,
	[BACK_VOCH_NO] [varchar](10) NULL,
	[BACK_MODIFY_ID] [varchar](10) NULL,
	[BACK_MODIFY_DATE] [varchar](10) NULL,
	[BACK_MODIFY_TIME] [varchar](8) NULL,
	[DISC_DATE] [varchar](10) NULL,
	[DISC_BANK_NO] [varchar](10) NULL,
	[DISC_ACCT_NO] [varchar](15) NULL,
	[DISC_AMT] [money] NULL,
	[DISC_INTE] [money] NULL,
	[DISC_VOCH_DATE] [varchar](10) NULL,
	[DISC_VOCH_NO] [varchar](10) NULL,
	[DISC_MODIFY_ID] [varchar](10) NULL,
	[DISC_MODIFY_DATE] [varchar](10) NULL,
	[DISC_MODIFY_TIME] [varchar](8) NULL,
	[TRANS_CUST_DATE] [varchar](10) NULL,
	[MANU_NO] [varchar](10) NULL,
	[MANU_NAME] [varchar](50) NULL,
	[ORDER_NO] [nvarchar](25) NULL,
	[DEPT_NO] [nvarchar](10) NULL,
	[INDE_NO] [nvarchar](10) NULL,
	[REDE_ACCT_CODE] [nvarchar](8) NULL,
	[UN_REDE_INTE] [money] NULL,
 CONSTRAINT [PK_CHM01] PRIMARY KEY NONCLUSTERED 
(
	[CHEQ_NO] ASC,
	[DUE_DATE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

