

If not exists (select * from information_schema.tables where table_name = 'BEQRegionRetention')
BEGIN
CREATE TABLE [dbo].[BEQRegionRetention](
       [ID] [int] IDENTITY(1,1) NOT NULL,
       [RegionID] [int] NOT NULL,
       [RetentionDays] [int] NOT NULL,
         CONSTRAINT AK_RegionID UNIQUE(RegionID)  
) ON [PRIMARY]
END