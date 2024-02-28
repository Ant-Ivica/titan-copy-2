if not exists (select * from typecode where typecodeid = 809)
begin 
	insert into typecode values(809,'Create Credential',800,GetDate())
end
if not exists (select * from typecode where typecodeid = 841)
begin 
	insert into typecode values(841,'UserId',809,GetDate())
end 
if not exists (select * from typecode where typecodeid = 842)
begin 
	insert into typecode values(842,'Password',809,GetDate())
end 
