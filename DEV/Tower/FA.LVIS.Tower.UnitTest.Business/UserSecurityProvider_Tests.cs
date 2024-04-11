using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FA.LVIS.Tower.Data.TerminalDBEntities;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using FA.LVIS.Tower.Data;

namespace FA.LVIS.Tower.UnitTests.Business
{
    //[TestClass]
    //public class UserSecurityProvider_Tests
    //{
    //    [TestMethod]
    //    public void UnitTest_GetApplicationUsers_Tenant_Success()
    //    {

    //        var mockEntities = new Mock<Entities>();

    //        var user = new Data.TerminalDBEntities.Tower_Users { UserId = 1, UserName = "test", TenantId = 1, Email = "test@test.com", EmployeeId = 11677, Id = "1", IsActive = true ,Name ="jahnavi"};
    //        user.Tower_Roles.Add(new Tower_Roles { Id = 1, Name = "super" });
    //        user.Tower_UserClaims.Add(new Tower_UserClaims {Id=1,UserId ="1", ClaimType ="test",ClaimValue ="1231" });
    //        user.Tenant = new Tenant {TenantId =1,TenantName= "test" };

    //        var user1 = new Data.TerminalDBEntities.Tower_Users { UserId = 2, UserName = "test1", TenantId = 1, Email = "test@test.com", EmployeeId = 11677, Id = "1", IsActive = true, Name="test" };
    //        user1.Tower_Roles.Add(new Tower_Roles { Id = 1, Name = "super" });
    //        user1.Tower_UserClaims.Add(new Tower_UserClaims { Id = 2, UserId = "2", ClaimType = "test", ClaimValue = "1231" });
    //        user1.Tenant = new Tenant { TenantId = 1, TenantName = "test" };


    //        IQueryable<Data.TerminalDBEntities.Tower_Users> Users = new List<Data.TerminalDBEntities.Tower_Users>
    //        {
    //            user,
    //            user1 
    //         }.AsQueryable();


    //        var mockSet = new Mock<DbSet<Data.TerminalDBEntities.Tower_Users>>();
    //        mockSet.As<IQueryable<Data.TerminalDBEntities.Tower_Users>>().Setup(m => m.Provider).Returns(Users.Provider);
    //        mockSet.As<IQueryable< Data.TerminalDBEntities.Tower_Users>> ().Setup(m => m.Expression).Returns(Users.Expression);
    //        mockSet.As<IQueryable< Data.TerminalDBEntities.Tower_Users>> ().Setup(m => m.ElementType).Returns(Users.ElementType);
    //        mockSet.As<IQueryable< Data.TerminalDBEntities.Tower_Users>> ().Setup(m => m.GetEnumerator()).Returns(Users.GetEnumerator());

    //        mockEntities.Setup(c => c.Tower_Users).Returns(mockSet.Object);

    //        int expected = Users.Count();

    //        var service = new UserSecurityDataProvider(mockEntities.Object);
    //        int actual = service.GetApplicationUsers(1).Count();

    //        Assert.AreEqual(expected, actual);
            

    //    }

    //    [TestMethod]
    //    public void UnitTest_GetApplicationUsers_Tenant_EmptyUsers()
    //    {

    //        var mockEntities = new Mock<Entities>();
    //        IQueryable<Data.TerminalDBEntities.Tower_Users> Users = new List<Data.TerminalDBEntities.Tower_Users>
    //        {
    //         }.AsQueryable();

    //        var mockSet = new Mock<DbSet<Data.TerminalDBEntities.Tower_Users>>();
    //        mockSet.As<IQueryable<Data.TerminalDBEntities.Tower_Users>>().Setup(m => m.Provider).Returns(Users.Provider);
    //        mockSet.As<IQueryable<Data.TerminalDBEntities.Tower_Users>>().Setup(m => m.Expression).Returns(Users.Expression);
    //        mockSet.As<IQueryable<Data.TerminalDBEntities.Tower_Users>>().Setup(m => m.ElementType).Returns(Users.ElementType);
    //        mockSet.As<IQueryable<Data.TerminalDBEntities.Tower_Users>>().Setup(m => m.GetEnumerator()).Returns(Users.GetEnumerator());

    //        mockEntities.Setup(c => c.Tower_Users).Returns(mockSet.Object);

    //        int expected = 0;

    //        var service = new UserSecurityDataProvider(mockEntities.Object);
    //        int actual = service.GetApplicationUsers(1).Count();

    //        Assert.AreEqual(expected, actual);


    //    }




    //}
}
