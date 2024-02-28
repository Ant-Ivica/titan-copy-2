if not exists (select * from information_schema.columns where table_name =  'Credential' and COLUMN_NAME = 'CustomerId')
	Begin
		ALTER TABLE Credential
		ADD customerid INTEGER,
		CONSTRAINT FK_Credential_Customer FOREIGN KEY(customerid) REFERENCES Customer(customerid)
	End