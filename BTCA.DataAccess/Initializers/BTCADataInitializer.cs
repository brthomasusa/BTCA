using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BTCA.DataAccess.EF;

namespace BTCA.DataAccess.Initializers
{
    public static class SampleData
    {
        public static void InitializeData(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetService<HOSContext>();
            InitializeData(context);
        }

        public static void InitializeData(HOSContext context)
        {
            context.Database.Migrate();
            ClearData(context);
            SeedData(context);
        }

        public static void ClearData(HOSContext context)
        {
            ExecuteDeleteSql(context, "DailyLogDetails");
            ExecuteDeleteSql(context, "DailyLogs");
            ExecuteDeleteSql(context, "Addresses");
            ExecuteDeleteSql(context, "AspNetRoles");
            ExecuteDeleteSql(context, "AspNetUsers");             
            ExecuteDeleteSql(context, "Companies");
            ExecuteDeleteSql(context, "DutyStatuses");
            ExecuteDeleteSql(context, "DutyStatusActivities");
            ExecuteDeleteSql(context, "StateProvinceCodes");           
            ResetIdentity(context);
        }

        public static void ExecuteDeleteSql(HOSContext context, string tableName)
        {
            var sql = $"DELETE FROM dbo.{tableName}";
            context.Database.ExecuteSqlCommand(sql);
        }

        public static void ResetIdentity(HOSContext context)
        {
            var tables = new[] 
            {
                "Addresses", 
                "Companies", 
                "DailyLogs", 
                "DailyLogDetails", 
                "DutyStatuses", 
                "DutyStatusActivities",
                "AspNetRoles", 
                "StateProvinceCodes",
                "AspNetUsers"
            };

            foreach (var table in tables)
            {
                var sql = $"DBCC CHECKIDENT (\"{table}\", RESEED, -1);";
                context.Database.ExecuteSqlCommand(sql);                
            }
        }

        public static void SeedData(HOSContext context)
        {
            context.Database.OpenConnection();

            try {

                if (!context.StateProvinceCodes.Any())
                {     
                    context.StateProvinceCodes.AddRange(BTCASampleData.LoadStateProvinceCodeTable());
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.StateProvinceCodes ON");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.StateProvinceCodes OFF");                                  
                }

                if (!context.DutyStatuses.Any())
                {                    
                    context.DutyStatuses.AddRange(BTCASampleData.LoadDutyStatusTable());
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DutyStatuses ON");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DutyStatuses OFF");                     
                }                

                if (!context.DutyStatusActivities.Any())
                {                    
                    context.DutyStatusActivities.AddRange(BTCASampleData.LoadDutyStatusActivityTable());
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DutyStatusActivities ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DutyStatusActivities OFF;");                     
                }

                if (!context.Companies.Any())
                {                    
                    context.Companies.AddRange(BTCASampleData.LoadCompanyTable());
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Companies ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Companies OFF;");                     
                }                

                if (!context.Users.Any())
                {                    
                    context.Users.AddRange(BTCASampleData.LoadAppUserTable());
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AspNetUsers ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AspNetUsers OFF;");                     
                }

                if (!context.Roles.Any())
                {                    
                    context.Roles.AddRange(BTCASampleData.LoadAppRoleTable());
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AspNetRoles ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AspNetRoles OFF;");                     
                }

                if (!context.Addresses.Any())
                {                    
                    context.Addresses.AddRange(BTCASampleData.LoadCompanyAddresses());
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Addresses ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Addresses OFF;");                     
                }    

                if (!context.DailyLogs.Any())
                {                    
                    context.DailyLogs.AddRange(BTCASampleData.LoadDailyLogTable());
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DailyLogs ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DailyLogs OFF;");                     
                }                                                                      

                if (!context.DailyLogDetails.Any())
                {                    
                    context.DailyLogDetails.AddRange(BTCASampleData.LoadDailyLogDetailTable());
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DailyLogDetails ON;");
                    context.SaveChanges();
                    context.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DailyLogDetails OFF;");                      
                } 

            } catch (Exception ex) {
                Console.WriteLine(ex);
            } finally {
                context.Database.CloseConnection();
            }

        }
    }
}
