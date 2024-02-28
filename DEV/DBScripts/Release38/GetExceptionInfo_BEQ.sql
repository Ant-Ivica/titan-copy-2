IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetExceptionInfo_BEQ')
  DROP PROCEDURE GetExceptionInfo_BEQ
  GO

/****** Object:  StoredProcedure [dbo].[GetExceptionInfo_BEQ]    Script Date: 8/5/2020 2:32:55 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create PROCEDURE [dbo].[GetExceptionInfo_BEQ]    
(    
 @startdate DATETIME,    
 @enddate DATETIME,    
 @exceptiongroupid INT,    
 @tenantId INT,    
 @isincluderesolved BIT,    
 @exceptionTypeName VARCHAR(50) = NULL    
)    
AS    
BEGIN  
   
 SET NOCOUNT ON;    
SET FMTONLY OFF; 


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
 JOIN dbo.ExceptionType (NOLOCK) ext ON ext.ExceptionGroupId = @exceptiongroupid AND ex.ExceptionTypeId = ext.ExceptionTypeId    
 JOIN dbo.TypeCode tc (NOLOCK) ON ex.TypeCodeId = tc.TypeCodeId    
 
 WHERE ex.CreatedDate >= @startdate    
 AND ex.CreatedDate <= @enddate    
 AND ex.MessageLogId > 0 
 AND (@exceptionTypeName IS NULL OR ext.ExceptionTypeName = @exceptionTypeName)    
 AND (@isincluderesolved = 1 OR (@isincluderesolved = 0 AND ex.TypeCodeId <> 204))    
 
 CREATE NONCLUSTERED INDEX ix_tempExceptionList ON #ExceptionList ([MessageLogId]);    
  
 UPDATE ex  
 SET ServiceRequestId = ml.ServiceRequestId, TenantId = ml.TenantId, MessageLogDesc = ml.MessageLogDesc, MessageMapId = ml.MessageMapId  
 , DocumentObjectId = ISNULL(ml.DocumentObjectId, ex.DocumentObjectId)  
 FROM #ExceptionList ex  
 JOIN dbo.MessageLog ml (NOLOCK) ON ex.MessageLogId = ml.MessageLogId   
    
 CREATE NONCLUSTERED INDEX ix_tempExceptionList4 ON #ExceptionList ([DocumentObjectId]);     
    
 IF(@tenantId <> 3)    
 BEGIN    
  DELETE FROM #ExceptionList     
  WHERE TenantId <> @tenantId OR TenantId IS NULL    
 END    
 
 SELECT ExceptionId, ExceptionTypeName, MessageLogId, ExceptionDesc, DocumentObjectId, Comments, TypeCodeId, ExceptionStatus, CreatedDate, CreatedById,     
      LastModifiedDate, LastModifiedById, ServiceRequestId, MessageTypeId, MessageTypeName, ExternalRefNum, ServiceName, TenantName, CreatedBy, LastModifiedBy,    
      ObjectPath as DocObjectPath, BuyerName, TransactionType   
    FROM    
 (  
  SELECT t.ExceptionId, t.ExceptionTypeName, t.MessageLogId, t.ExceptionDesc, t.DocumentObjectId, t.Comments, t.TypeCodeId, t.TypeCodeDesc ExceptionStatus, t.CreatedDate, t.CreatedById,     
      t.LastModifiedDate, t.LastModifiedById, t.ServiceRequestId, mt.MessageTypeId, mt.MessageTypeName, sr.ExternalRefNum, svc.ServiceName, ten.TenantName, tu.UserName CreatedBy, tu1.UserName LastModifiedBy,     
      NULL AS ObjectPath, dm.BuyerName, dm.TransactionType    
  FROM #ExceptionList t    
  left JOIN dbo.ServiceRequest sr (NOLOCK) ON t.ServiceRequestId = sr.ServiceRequestId    
  left JOIN dbo.[Service] svc ON sr.ServiceId = svc.ServiceId     
  left JOIN dbo.MessageMap mm (NOLOCK) ON t.MessageMapId = mm.MessageMapId    
  left JOIN dbo.MessageType mt (NOLOCK) ON mm.MessageTypeId = mt.MessageTypeId    
  left JOIN dbo.Tenant ten (nolock) on t.TenantId = ten.TenantId    
  left JOIN dbo.[Tower.Users] (nolock) tu on tu.UserId = t.CreatedById    
  left JOIN dbo.[Tower.Users] (nolock) tu1 on tu1.UserId = t.LastModifiedById    
  left JOIN TerminalDocument.dbo.DocumentObjectMetadata (nolock) dm on t.DocumentObjectId = dm.DocumentObjectId    
 ) x     
 ORDER BY CreatedDate desc    
    
  
    
END    

GRANT EXECUTE ON [dbo].[GetExceptionInfo_BEQ]    
    TO terminaluser 
GO
