using FA.LVIS.Tower.Data.TerminalDBEntities;
using FA.LVIS.Tower.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FA.LVIS.Tower.Data
{
    public class CategoryMappingDataProvider : Core.DataProviderBase, ICategoryMappingDataProvider
    {
       
        public CategoryMapping AddCategory(CategoryMapping GroupProfile, int tenantId, int userId)
        {
            Entities dbContext = new Entities();
            Category AddGroup = new Category();

            AddGroup.CategoryName = GroupProfile.CategoryName;
            //AddGroup.ApplicationId = GroupProfile.Applicationid;
            AddGroup.TenantId = tenantId;
            AddGroup.CreatedDate = DateTime.Now;
            AddGroup.LastModifiedDate = DateTime.Now;
            AddGroup.CreatedById = userId;
            AddGroup.LastModifiedById = userId;
                             
            dbContext.Categories.Add(AddGroup);
            int Success = AuditLogHelper.SaveChanges(dbContext);

            if (Success == 1)
                GroupProfile.CategoryId = AddGroup.CategoryId;
            return GroupProfile;
        }

        public int DeleteCategory(int value)
        {         
            using (var dbContext = new Entities())
            {

               var result=dbContext.GetDependancyRecordOutput(value, "Category").FirstOrDefault();

                if (result != null)
                {
                    return 0;
                }

                else
                {
                    return 1;
                }
            }                
        }

        public int ConfirmDelete(int ID)
        {
            int success = 0;
            using (var dbContext = new Entities())
            {
                var categoryDelete = (from category in dbContext.Categories
                                      where category.CategoryId == ID
                                      select category);

                if (categoryDelete != null)
                {
                    dbContext.Categories.RemoveRange(categoryDelete);
                    success= AuditLogHelper.SaveChanges(dbContext);
                   
                }
            }
            return success;
        }

        public List<CategoryMapping> GetCategoryMappings(int tenantId)
        {
            Entities dbContext = new Entities();

            List<CategoryMapping> GroupMappings = new List<CategoryMapping>();

            if (dbContext.Categories.Count() > 0)
            {
                 GroupMappings = dbContext.Categories
                   .Select(Group => new CategoryMapping
                   {
                       CategoryId = Group.CategoryId,
                       CategoryName = Group.CategoryName,
                       ObjectCD = Group.CategoryName,
                       TenantId = Group.TenantId,
                       Tenant=Group.Tenant.TenantName,
                       Applicationid = dbContext.Subscriptions.Where(se=> se.CategoryId == Group.CategoryId && se.ApplicationId != null).FirstOrDefault()  != null ? dbContext.Subscriptions.Where(se => se.CategoryId == Group.CategoryId && se.ApplicationId != null).FirstOrDefault().Application.ApplicationId:0,
                       ApplicationName = dbContext.Subscriptions.Where(se => se.CategoryId == Group.CategoryId && se.ApplicationId != null).FirstOrDefault() != null ? dbContext.Subscriptions.Where(se => se.CategoryId == Group.CategoryId && se.ApplicationId != null).FirstOrDefault().Application.ApplicationName :"",



                   }).ToList();
            }
            
            if (GroupMappings.Count() > 0 && tenantId != (int)TenantIdEnum.LVIS)
            {
                GroupMappings = GroupMappings
                    .Where(sel => sel.TenantId == tenantId).ToList();
            }

            return GroupMappings;
        }

        public int UpdateCategory(CategoryMapping Group, int userId)
        {
            using (var dbContext = new Entities())
            {
                var GroupTopdate = (from Groups in dbContext.Categories
                                      where Groups.CategoryId == Group.CategoryId
                                      select Groups).FirstOrDefault();
                if (GroupTopdate != null)
                {
                    GroupTopdate.LastModifiedDate = DateTime.Now;
                    GroupTopdate.LastModifiedById = userId;
                    GroupTopdate.CategoryName = Group.CategoryName;
                    //GroupTopdate.ApplicationId = Group.Applicationid;
                    dbContext.Entry(GroupTopdate).State = System.Data.Entity.EntityState.Modified;

                    int Success = AuditLogHelper.SaveChanges(dbContext);

                    if (Success == 1)
                        return 1;
                    else
                        return 0;
                }
            }

            return 0;
        }


    }
}
