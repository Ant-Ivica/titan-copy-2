USE [Terminal]
GO

/****** Object:  Table [dbo].[FASTOfficeMap]    Script Date: 03/18/2019 1:31:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF Not EXISTS(select column_name from  information_schema.columns where table_name = 'FASTOfficeMap' and column_name = 'EscrowAssistantCode')
Begin 
ALTER TABLE [dbo].[FASTOfficeMap] ADD EscrowAssistantCode VARCHAR(150) NULL;
End

