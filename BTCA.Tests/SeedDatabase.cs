using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using BTCA.DataAccess.EF;
using BTCA.DataAccess.Initializers;

namespace BTCA.Tests
{
    public static class SeedDatabase
    {
        public static void CleanDatabase()
        {
            var tables = new[] 
            {
                "Addresses",                   
                "DailyLogDetails",
                "DailyLogs",
                "AspNetRoles", 
                "AspNetUsers",
                "Companies",
            };

            using (var ctx = new HOSContext())
            {
                try {

                    ctx.Database.ExecuteSqlCommand("DELETE FROM dbo.Addresses");
                    ctx.Database.ExecuteSqlCommand("DELETE FROM dbo.DailyLogDetails");
                    ctx.Database.ExecuteSqlCommand("DELETE FROM dbo.DailyLogs");
                    ctx.Database.ExecuteSqlCommand("DELETE FROM dbo.AspNetRoles");
                    ctx.Database.ExecuteSqlCommand("DELETE FROM dbo.AspNetUsers");
                    ctx.Database.ExecuteSqlCommand("DELETE FROM dbo.Companies");                    

                    foreach (var table in tables)
                    {   
                        var sql = $"DBCC CHECKIDENT (\"{table}\", RESEED, -1);";
                        ctx.Database.ExecuteSqlCommand(sql);
                    }

                    SeedData(ctx);

                } catch (Exception ex) {
                    Console.WriteLine(ex);
                }
            }           
        }

        private static void SeedData(HOSContext ctx)
        {
            ctx.Database.OpenConnection();

            try {

                if (!ctx.Companies.Any())
                {                    
                    ctx.Companies.AddRange(BTCASampleData.LoadCompanyTable());
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Companies ON;");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Companies OFF;");                     
                }                

                if (!ctx.Users.Any())
                {                    
                    ctx.Users.AddRange(BTCASampleData.LoadAppUserTable());
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AspNetUsers ON;");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AspNetUsers OFF;");                     
                }

                if (!ctx.Roles.Any())
                {                    
                    ctx.Roles.AddRange(BTCASampleData.LoadAppRoleTable());
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AspNetRoles ON;");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.AspNetRoles OFF;");                     
                }

                if (!ctx.Addresses.Any())
                {                    
                    ctx.Addresses.AddRange(BTCASampleData.LoadCompanyAddresses());
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Addresses ON;");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.Addresses OFF;");                     
                }    

                if (!ctx.DailyLogs.Any())
                {                    
                    ctx.DailyLogs.AddRange(BTCASampleData.LoadDailyLogTable());
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DailyLogs ON;");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DailyLogs OFF;");                     
                }                                                                      

                if (!ctx.DailyLogDetails.Any())
                {                    
                    ctx.DailyLogDetails.AddRange(BTCASampleData.LoadDailyLogDetailTable());
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DailyLogDetails ON;");
                    ctx.SaveChanges();
                    ctx.Database.ExecuteSqlCommand("SET IDENTITY_INSERT dbo.DailyLogDetails OFF;");                      
                } 

            } catch (Exception ex) {;
                Console.WriteLine(ex);
            } finally {
                ctx.Database.CloseConnection();
            } 
        }
    }
}
