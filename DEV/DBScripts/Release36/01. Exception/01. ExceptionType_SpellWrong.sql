BEGIN TRY
		BEGIN TRAN UpdateExceptionType

		--User Story 2022763: [Tower] Dashboard Title: Spellings on few Tiles are incorrect

		If Exists(Select *from ExceptionType where ExceptionTypeName = 'ValuTrust Recieve' and ExceptionGroupId = 2)
		Begin
			Update ExceptionType Set ExceptionTypeName = 'ValuTrust Receive' where ExceptionTypeName = 'ValuTrust Recieve' and ExceptionGroupId = 2
		End

		If Exists(Select *from ExceptionType where ExceptionTypeName = 'LTX Recieve' and ExceptionGroupId = 2)
		Begin
			Update ExceptionType Set ExceptionTypeName = 'LTX Receive' where ExceptionTypeName = 'LTX Recieve' and ExceptionGroupId = 2
		End

		If Exists(Select *from ExceptionType where ExceptionTypeName = 'TitlePort Recieve' and ExceptionGroupId = 2)
		Begin
			Update ExceptionType Set ExceptionTypeName = 'TitlePort Receive' where ExceptionTypeName = 'TitlePort Recieve' and ExceptionGroupId = 2
		End

		If Exists(Select *from ExceptionType where ExceptionTypeName = 'SafeEscrow Recieve' and ExceptionGroupId = 2)
		Begin
			Update ExceptionType Set ExceptionTypeName = 'SafeEscrow Receive', ExceptionTypeDesc = 'SafeEscrow Recieve' where ExceptionTypeName = 'SafeEscrow Recieve' and ExceptionGroupId = 2
		End

		If Exists(Select *from ExceptionType where ExceptionTypeName = 'OpenAPI Recieve' and ExceptionGroupId = 2)
		Begin
			Update ExceptionType Set ExceptionTypeName = 'OpenAPI Receive' where ExceptionTypeName = 'OpenAPI Recieve' and ExceptionGroupId = 2
		End

 COMMIT TRAN UpdateExceptionType
END TRY
BEGIN CATCH
		ROLLBACK TRAN UpdateExceptionType 
		DECLARE @error int, @message varchar(4000)
		SELECT @error = ERROR_NUMBER(), @message = ERROR_MESSAGE()
		RAISERROR ('ERROR OCCURED IN THE TENANT MIGRATION SCRIPT FOR FASTWorkflowMap TABLE CHANGES HAVE BEEN ROLLEDBACK TRANSACTION UpdateExceptionType: %d: %s', 16, 1, @error, @message) ;		
END CATCH