using System;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using BTCA.Common.BusinessObjects;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.DomainLayer.Managers.Implementation;
using BTCA.DataAccess.Initializers;

namespace BTCA.Tests
{
    public static class SeedDatabase
    {
        public static void CleanDatabase()
        {
            var tables = new[] 
            { 
                "DailyLogDetails",
                "DailyLogs"                
            };

            try {

                using (var ctx = new HOSContext())
                {
                    ctx.Database.ExecuteSqlCommand("DELETE FROM dbo.DailyLogDetails");
                    ctx.Database.ExecuteSqlCommand("DELETE FROM dbo.DailyLogs");

                    foreach (var table in tables)
                    {   
                        var sql = $"DBCC CHECKIDENT (\"{table}\", RESEED, -1);";
                        ctx.Database.ExecuteSqlCommand(sql);
                    }                    
                }

                SeedData();

            } catch (Exception ex) {
                Console.WriteLine(ex);
            }            
        }

        private static void SeedData()
        {
            try {
                
                using (var ctx = new HOSContext())
                {
                    ctx.Database.OpenConnection();

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

                    ctx.Database.CloseConnection();
                }


            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
    }
}
