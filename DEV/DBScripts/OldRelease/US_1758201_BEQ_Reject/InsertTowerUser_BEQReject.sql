if not exists(select id from dbo.[Tower.Users] where name='BEQ Reject Job')
Begin
insert into dbo.[Tower.Users](Id,Name,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,EmployeeId,UserName,IsActive,TenantId)
values('6D0ED039-EC9A-43DD-854C-A32299F5F16C','BEQ Reject Job',null,null,null,null,null,null,null,null,null,null,0,'BEQ Reject Job',1,3)
end