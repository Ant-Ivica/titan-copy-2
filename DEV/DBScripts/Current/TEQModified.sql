
/****** Object:  StoredProcedure [dbo].[GetExceptionInfo]    Script Date: 10/22/2020 10:12:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================    
-- Author:  Anand Vidyarthi    
-- Create date: 04/10/2017    
-- Description: Get all Exceptions to display in Tower    
-- ==============================================================================================================================================    
-- Date  User     Description      
-- 05/16/2018  Satha     Changes to pick up missing records which has a file in ObjectPath    
-- 07/10/2018  Satha     Add parameter Exception Type Name    
-- 08/07/2018  Satha     Add Buyer and Transaction Type Name    
--  03/19/2019 Satha     BUG 1772664 - Change date format to datetime for start/end date to pull exceptions based on created date   
-- 02/11/2020  Vuong     Performance Tuning   
-- 06/29/2020  Vuong     US#2105180 - Performance Tuning Restructure 
-- 07/21/2020  Vuong     US#2105180 - Performance Optimization XML Changes and Prevent parameter sniffing 
-- 08/17/2020  Vuong     US#2105180 - Performance Optimization remove XML & #ExceptionList Restructure
-- ==============================================================================================================================================    
    
ALTER PROCEDURE [dbo].[GetExceptionInfo]    
(    
 @startdate DATETIME,    
 @enddate DATETIME,    
 @exceptiongroupid INT,    
 @tenantId INT,    
 @isincluderesolved BIT,    
 @exceptionTypeName VARCHAR(50) = NULL    
)    
AS    
------------ Prevent Parameter sniffing --------------------    
Declare    
 @p_startdate DATETIME,    
 @p_enddate DATETIME,    
 @p_exceptiongroupid INT,    
 @p_tenantId INT,    
 @p_isincluderesolved BIT,    
 @p_exceptionTypeName VARCHAR(50)
    
select @p_startdate = @startdate    
select @p_enddate = @enddate    
select @p_exceptiongroupid = @exceptiongroupid
select @p_tenantId = @tenantId    
select @p_isincluderesolved = @isincluderesolved    
select @p_exceptionTypeName = @exceptionTypeName

DROP TABLE IF EXISTS #ExceptionList  
CREATE TABLE #ExceptionList    
(    
 ExceptionId int,    
    ExceptionTypeName varchar(50),         
    MessageLogId int,    
    ExceptionDesc varchar(max),     
    Comments varchar(max),    
    TypeCodeId int,    
 TypeCodeDesc varchar(200),    
    CreatedDate datetime,    
    CreatedById int,    
    LastModifiedDate datetime,    
    LastModifiedById int,     
 ServiceRequestId int,     
 TenantId int,     
 MessageLogDesc varchar(1000),     
 MessageMapId int,     
 DocumentObjectId bigint    
)    
    
    
BEGIN     
 SET NOCOUNT ON;   
 
 SET FMTONLY OFF;    
        
DROP TABLE IF EXISTS #ExceptionType  
SELECT ext.ExceptionTypeId, ext.ExceptionTypeName 
INTO #ExceptionType
FROM  dbo.ExceptionType ext (NOLOCK)    
WHERE ext.ExceptionGroupId = @p_exceptiongroupid  
AND (@p_exceptionTypeName IS NULL OR ext.ExceptionTypeName = @p_exceptionTypeName)  

DROP TABLE IF EXISTS #TypeCode 
Select tc.TypeCodeID, tc.TypeCodeDesc
INTO #TypeCode
FROM dbo.TypeCode tc (NOLOCK) 
WHERE @p_isincluderesolved = 1 OR (@p_isincluderesolved = 0 AND tc.TypeCodeId <> 204)  

 INSERT INTO #ExceptionList    
 SELECT ex.ExceptionId ,    
           ext.ExceptionTypeName,         
           ex.MessageLogId ,    
           ex.ExceptionDesc ,    
           ex.Comments ,    
           ex.TypeCodeId,    
     tc.TypeCodeDesc,    
           ex.CreatedDate ,    
           ex.CreatedById ,    
           ex.LastModifiedDate ,    
           ex.LastModifiedById,   
     ServiceRequestId = null, TenantId = null, MessageLogDesc = null, MessageMapId = null, ex.DocumentObjectId   
 FROM dbo.Exception ex (NOLOCK)    
JOIN #ExceptionType ext ON ext.ExceptionTypeId = ex.ExceptionTypeId
JOIN #TypeCode tc ON ex.TypeCodeId = tc.TypeCodeId 
 WHERE ex.CreatedDate >= @p_startdate    
 AND ex.CreatedDate <= @p_enddate    
 
 CREATE NONCLUSTERED INDEX ix_tempExceptionList ON #ExceptionList ([MessageLogId]);    
  
 UPDATE ex  
 SET ServiceRequestId = ml.ServiceRequestId, TenantId = ml.TenantId, MessageLogDesc = ml.MessageLogDesc, MessageMapId = ml.MessageMapId  
 , DocumentObjectId = ISNULL(ml.DocumentObjectId, ex.DocumentObjectId)  
 FROM #ExceptionList ex  
 JOIN dbo.MessageLog ml (NOLOCK) ON ex.MessageLogId = ml.MessageLogId 
   
 CREATE NONCLUSTERED INDEX ix_tempExceptionList4 ON #ExceptionList ([DocumentObjectId]);    
 CREATE NONCLUSTERED INDEX ix_tempExceptionList5 ON #ExceptionList ([TenantId]);    

    
 IF(@p_tenantId <> 3)    
 BEGIN    
  DELETE FROM #ExceptionList     
  WHERE TenantId <> @p_tenantId OR TenantId IS NULL    
 END    
 
 DROP TABLE IF EXISTS #temp 
      
 SELECT do.Object, do.DocumentObjectId, t.ExceptionId, do.ObjectPath
 , DocObj1 = patindex('%<DocumentObjectId>%',Object), DocObj2 = patindex('%</DocumentObjectId>%',Object)
 , XRefNo1 = patindex('%<ExternalRefNum%>',Object), XRefNo2 = patindex('%</ExternalRefNum%>',Object)
 INTO #temp
 FROM #ExceptionList t    
 LEFT JOIN TerminalDocument.dbo.DocumentObject do (NOLOCK) ON t.DocumentObjectId = do.DocumentObjectId     
 where t.MessageLogId = 0    

    
	DROP TABLE IF EXISTS #Report

	SELECT t.*, MessageTypeId = cast(null as int), MessageTypeName = cast(null as varchar(50)), ExternalRefNum = cast(null as varchar(50)), ServiceName = cast(null as varchar(50)), TenantName = cast(null as varchar(50)), CreatedBy = cast(null as varchar(128)), LastModifiedBy = cast(null as varchar(128)),       
		  DocObjectPath = cast(NULL AS varchar(250)), dm.BuyerName, dm.TransactionType    
	INTO #Report
	FROM #ExceptionList t     
	LEFT JOIN TerminalDocument.dbo.DocumentObjectMetadata (nolock) dm on t.DocumentObjectId = dm.DocumentObjectId
	WHERE t.MessageLogId > 0 

	Update t
	Set MessageTypeId = mt.MessageTypeId
	, MessageTypeName = mt.MessageTypeName
	, ExternalRefNum = sr.ExternalRefNum 
	, ServiceName = svc.ServiceName 
	FROM #Report t 
	LEFT JOIN dbo.ServiceRequest sr (NOLOCK) ON t.ServiceRequestId = sr.ServiceRequestId    
	LEFT JOIN dbo.[Service] svc ON sr.ServiceId = svc.ServiceId     
	LEFT JOIN dbo.MessageMap mm (NOLOCK) ON t.MessageMapId = mm.MessageMapId    
	LEFT JOIN dbo.MessageType mt (NOLOCK) ON mm.MessageTypeId = mt.MessageTypeId   

	INSERT INTO #Report
	Select t.ExceptionId, ExceptionTypeName, MessageLogId, ExceptionDesc, Comments, TypeCodeId, TypeCodeDesc, CreatedDate
	, CreatedById, LastModifiedDate, LastModifiedById, ServiceRequestId, TenantId, MessageLogDesc, MessageMapId
	, DocumentObjectId = case when DocObj1 > 0 then SUBSTRING(Object, DocObj1 + 18, DocObj2 - DocObj1 - 18) else t.DocumentObjectId end
	, MessageTypeId = null, MessageTypeName = null
	, ExternalRefNum = case when XRefNo1 > 0 then SUBSTRING(Object, XRefNo1 + 16, XRefNo2 - XRefNo1 - 16) else null end
	, ServiceName = cast(null as varchar(50)), TenantName = cast(null as varchar(50)), CreatedBy = cast(null as varchar(128)), LastModifiedBy = cast(null as varchar(128))       
	, do.ObjectPath, null, null       
	  FROM #ExceptionList t    
	  JOIN #temp do ON t.DocumentObjectId = do.DocumentObjectId
	WHERE t.MessageLogId = 0

	Update t
	Set TenantName = ten.TenantName
	, CreatedBy = tu.UserName
	, LastModifiedBy = tu1.UserName
	FROM #Report t 	
	LEFT JOIN dbo.Tenant ten (nolock) on t.TenantId = ten.TenantId    
	LEFT JOIN dbo.[Tower.Users] (nolock) tu on tu.UserId = t.CreatedById    
	LEFT JOIN dbo.[Tower.Users] (nolock) tu1 on tu1.UserId = t.LastModifiedById  

	SELECT ExceptionId, ExceptionTypeName, MessageLogId, ExceptionDesc, DocumentObjectId, Comments, TypeCodeId, ExceptionStatus = TypeCodeDesc, CreatedDate, CreatedById    
	, LastModifiedDate, LastModifiedById, ServiceRequestId, MessageTypeId, MessageTypeName, ExternalRefNum, ServiceName, TenantName, CreatedBy, LastModifiedBy 
	, DocObjectPath, BuyerName, TransactionType  
	From #Report
	ORDER BY CreatedDate desc  
    
END 

