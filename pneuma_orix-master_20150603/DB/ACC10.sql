USE [USCACCT]
GO

/****** Object:  Table [dbo].[ACC10]    Script Date: 04/27/2015 14:47:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ACC10](
	[MANU_NO] [varchar](10) NOT NULL,
	[MANU_NAME] [varchar](80) NULL,
	[MANU_ABBR] [varchar](10) NULL,
	[MANU_NAME_ENG] [varchar](100) NULL,
	[MANU_ABBR_ENG] [varchar](50) NULL,
	[FOR_YN] [varchar](1) NULL,
	[CORP_KIND] [varchar](4) NULL,
	[UNIF_NO] [varchar](10) NULL,
	[TAX_NO] [varchar](10) NULL,
	[CHIEF_NAME] [varchar](20) NULL,
	[CONT_NAME] [varchar](20) NULL,
	[ZIP_CODE] [varchar](6) NULL,
	[ADDR_MANU] [varchar](100) NULL,
	[ADDR_ENG] [varchar](200) NULL,
	[ZIP_CONT] [varchar](6) NULL,
	[ADDR_CONT] [varchar](100) NULL,
	[ADDR_CONT_ENG] [varchar](200) NULL,
	[ADDR_INVO] [varchar](100) NULL,
	[TEL_MANU] [varchar](30) NULL,
	[FAX_MANU] [varchar](30) NULL,
	[EMAIL] [varchar](50) NULL,
	[EMP_NO] [varchar](10) NULL,
	[PURC_NAME] [varchar](20) NULL,
	[ACCT_NO] [varchar](15) NULL,
	[RELA_NAME] [varchar](80) NULL,
	[BANK_NO] [varchar](40) NULL,
	[BEGIN_DATE] [varchar](10) NULL,
	[REMK] [varchar](60) NULL,
	[FIELD_1] [varchar](100) NULL,
	[FIELD_2] [varchar](100) NULL,
	[ENTRY_ID] [varchar](10) NULL,
	[ENTRY_DATE] [varchar](10) NULL,
	[ENTRY_TIME] [varchar](6) NULL,
	[MODIFY_ID] [varchar](10) NULL,
	[MODIFY_DATE] [varchar](10) NULL,
	[MODIFY_TIME] [varchar](6) NULL,
	[FIELD_3] [varchar](100) NULL,
	[FIELD_4] [varchar](100) NULL,
	[FIELD_5] [varchar](100) NULL,
	[FIELD_6] [varchar](100) NULL,
	[FIELD_7] [varchar](100) NULL,
	[FIELD_8] [varchar](100) NULL,
	[FIELD_9] [varchar](100) NULL,
	[FIELD_10] [varchar](100) NULL,
 CONSTRAINT [PK_ACC10] PRIMARY KEY CLUSTERED 
(
	[MANU_NO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

