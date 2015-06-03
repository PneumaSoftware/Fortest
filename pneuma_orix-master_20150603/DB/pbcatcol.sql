USE [USCACCT]
GO

/****** Object:  Table [dbo].[pbcatcol]    Script Date: 05/07/2015 15:44:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[pbcatcol](
	[pbc_tnam] [varchar](30) NOT NULL,
	[pbc_tid] [int] NULL,
	[pbc_ownr] [varchar](30) NULL,
	[pbc_cnam] [varchar](30) NOT NULL,
	[pbc_cid] [smallint] NULL,
	[pbc_labl] [varchar](254) NULL,
	[pbc_lpos] [smallint] NULL,
	[pbc_hdr] [varchar](254) NULL,
	[pbc_hpos] [smallint] NULL,
	[pbc_jtfy] [smallint] NULL,
	[pbc_mask] [varchar](31) NULL,
	[pbc_case] [smallint] NULL,
	[pbc_hght] [smallint] NULL,
	[pbc_wdth] [smallint] NULL,
	[pbc_ptrn] [varchar](31) NULL,
	[pbc_bmap] [char](1) NULL,
	[pbc_init] [varchar](254) NULL,
	[pbc_cmnt] [varchar](254) NULL,
	[pbc_edit] [varchar](31) NULL,
	[pbc_tag] [varchar](254) NULL,
	[rowguid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_pbcatcol] PRIMARY KEY CLUSTERED 
(
	[pbc_tnam] ASC,
	[pbc_cnam] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[pbcatcol] ADD  CONSTRAINT [DF_pbcatcol_rowguid]  DEFAULT (newid()) FOR [rowguid]
GO

