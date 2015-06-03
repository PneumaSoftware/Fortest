USE [USCACCT]
GO

/****** Object:  Table [dbo].[ART05]    Script Date: 05/19/2015 15:21:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ART05](
	[CORP_NO] [varchar](10) NOT NULL,
	[ACEP_NO] [varchar](15) NOT NULL,
	[DATA_SEQ] [varchar](4) NOT NULL,
	[RECP_NO] [varchar](15) NULL,
	[DATA_TYPE] [varchar](1) NOT NULL,
	[RECP_SEQ] [varchar](4) NULL,
	[SALE_NO] [varchar](6) NULL,
	[INVO_NO] [varchar](10) NULL,
	[CUST_NO] [varchar](10) NOT NULL,
	[ORDER_NO] [varchar](15) NOT NULL,
	[CURR_CODE] [varchar](3) NULL,
	[CURR_RATE] [money] NULL,
	[CAN_AMT] [money] NULL,
	[CAN_AMT_NT] [money] NULL,
	[REMK] [varchar](60) NULL,
	[ACEP_DATE] [varchar](10) NULL,
	[CERT_NO] [varchar](15) NULL,
	[CUST_RECP_NO] [varchar](15) NULL,
	[ENTRY_ID] [varchar](10) NULL,
	[ENTRY_DATE] [varchar](10) NULL,
	[ENTRY_TIME] [varchar](8) NULL,
	[MODIFY_ID] [varchar](10) NULL,
	[MODIFY_DATE] [varchar](10) NULL,
	[MODIFY_TIME] [varchar](8) NULL,
	[INDE_NO] [varchar](10) NULL,
 CONSTRAINT [PK_ART05] PRIMARY KEY NONCLUSTERED 
(
	[CORP_NO] ASC,
	[ACEP_NO] ASC,
	[DATA_SEQ] ASC,
	[DATA_TYPE] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 80) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

