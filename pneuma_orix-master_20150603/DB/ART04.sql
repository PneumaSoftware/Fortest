USE [USCACCT]
GO

/****** Object:  Table [dbo].[ART04]    Script Date: 05/19/2015 15:21:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ART04](
	[CORP_NO] [varchar](10) NOT NULL,
	[ACEP_NO] [varchar](15) NOT NULL,
	[ACEP_DATE] [varchar](10) NULL,
	[DATA_SEQ] [varchar](4) NOT NULL,
	[ACCA_NO] [varchar](4) NULL,
	[CHEQ_NO] [varchar](15) NULL,
	[DUE_DATE] [varchar](10) NULL,
	[CHEQ_AMT] [money] NULL,
	[CURR_CODE] [varchar](3) NULL,
	[ACEP_AMT] [money] NULL,
	[ACEP_AMT_NT] [money] NULL,
	[BANK_NO] [varchar](10) NULL,
	[ACCT_NO] [varchar](15) NULL,
	[CHEQ_KIND] [varchar](1) NULL,
	[CUST_YN] [varchar](1) NULL,
	[REMK] [varchar](50) NULL,
	[CHEQ_MAN] [varchar](50) NULL,
	[CUST_NO] [varchar](10) NULL,
	[ENTRY_ID] [varchar](10) NULL,
	[ENTRY_DATE] [varchar](10) NULL,
	[ENTRY_TIME] [varchar](8) NULL,
	[MODIFY_ID] [varchar](10) NULL,
	[MODIFY_DATE] [varchar](10) NULL,
	[MODIFY_TIME] [varchar](8) NULL,
 CONSTRAINT [PK_ART04] PRIMARY KEY CLUSTERED 
(
	[CORP_NO] ASC,
	[ACEP_NO] ASC,
	[DATA_SEQ] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

