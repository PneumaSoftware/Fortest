USE [USCACCT]
GO

/****** Object:  Table [dbo].[ARM01]    Script Date: 05/19/2015 15:21:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ARM01](
	[CORP_NO] [varchar](10) NOT NULL,
	[RECP_NO] [varchar](15) NOT NULL,
	[RECP_KIND] [varchar](4) NULL,
	[CUST_NO] [varchar](10) NOT NULL,
	[CUST_NO_SUB] [varchar](10) NULL,
	[RECP_DATE] [varchar](10) NULL,
	[UNIF_NO] [varchar](10) NULL,
	[REMK] [varchar](50) NULL,
	[CURR_CODE] [varchar](3) NULL,
	[CURR_RATE] [money] NULL,
	[EMP_NO] [varchar](10) NULL,
	[DEPT_NO] [varchar](10) NULL,
	[FINAL_CODE] [varchar](1) NULL,
	[CONF_CODE] [varchar](1) NULL,
	[INVO_NO] [varchar](10) NULL,
	[INVO_DATE] [varchar](10) NULL,
	[TAX_CODE] [varchar](1) NULL,
	[COPY_NUM] [varchar](2) NULL,
	[PROD_AMT] [money] NULL,
	[PROD_TAX] [money] NULL,
	[INVO_AMT] [money] NULL,
	[NET_AMT] [money] NULL,
	[NET_AMT_NT] [money] NULL,
	[CAN_AMT] [money] NULL,
	[CAN_AMT_NT] [money] NULL,
	[ORDER_NO] [varchar](15) NOT NULL,
	[INVO_REMK] [varchar](50) NULL,
	[PRED_DATE] [varchar](10) NULL,
	[VOCH_NO] [varchar](10) NULL,
	[VOCH_DATE] [varchar](10) NULL,
	[ENTRY_ID] [varchar](10) NULL,
	[ENTRY_DATE] [varchar](10) NULL,
	[ENTRY_TIME] [varchar](8) NULL,
	[MODIFY_ID] [varchar](10) NULL,
	[MODIFY_DATE] [varchar](10) NULL,
	[MODIFY_TIME] [varchar](8) NULL,
	[CUST_RECP_NO] [varchar](15) NULL,
 CONSTRAINT [PK_ARM01] PRIMARY KEY CLUSTERED 
(
	[CORP_NO] ASC,
	[RECP_NO] ASC,
	[CUST_NO] ASC,
	[ORDER_NO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

