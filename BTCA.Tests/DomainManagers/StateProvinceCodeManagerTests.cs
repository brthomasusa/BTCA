using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Xunit;
using BTCA.Common.Entities;
using BTCA.DataAccess.Core;
using BTCA.DataAccess.EF;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.DomainLayer.Managers.Implementation;

namespace BTCA.Tests.DomainManagers
{
    public class StateProvinceCodeManagerTests
    {
        [Fact]
        public void Test_StateProvinceCodeMgr_StateCodeInsert()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    IStateProvinceCodeManager stateCodeMgr = new StateProvinceCodeManager(new Repository(context));

                    var stateCode = new StateProvinceCode
                    {
                        ID = 1,
                        StateCode = "AK", 
                        StateName = "Alaska",
                        CountryCode = "USA",
                        CreatedBy = "admin",
                        CreatedOn = DateTime.Now,
                        UpdatedBy = "admin",
                        UpdatedOn = DateTime.Now                        
                    };

                    stateCodeMgr.Create(stateCode);
                    stateCodeMgr.SaveChanges();

                    var result = stateCodeMgr.GetStateProvinceCode(state => state.StateCode == "AK");
                    Assert.NotNull(result);
                    Assert.Equal(stateCode.StateCode, result.StateCode);
                }

            } finally {
                connection.Close();
            }            
        }

        [Fact]
        public void Test_StateProvinceCodeMgr_StateCodeUpdate()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadStateProvinceCodeTable(context);
                    HOSTestData.CreateViews(context);                                      
                }

                using (var context = new HOSContext(options))
                {
                    IStateProvinceCodeManager stateCodeMgr = new StateProvinceCodeManager(new Repository(context));
                    var stateCode = stateCodeMgr.GetStateProvinceCode(state => state.StateCode == "AK");
                    var currentTimestamp = DateTime.Now;
                    stateCode.UpdatedBy = "admin";
                    stateCode.UpdatedOn = currentTimestamp;

                    stateCodeMgr.Update(stateCode);
                    stateCodeMgr.SaveChanges();

                    var test = stateCodeMgr.GetStateProvinceCode(state => state.StateCode == "AK");
                    Assert.NotNull(test);
                    Assert.Equal(currentTimestamp, test.UpdatedOn);
                }

            } finally {
                connection.Close();
            } 
        }

        [Fact]
        public void Test_StateProvinceCodeMgr_StateCodeDelete()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadStateProvinceCodeTable(context);                                      
                }

                using (var context = new HOSContext(options))
                {
                    IStateProvinceCodeManager stateCodeMgr = new StateProvinceCodeManager(new Repository(context));

                    var stateCode = stateCodeMgr.GetStateProvinceCode(state => state.StateCode == "AK");

                    stateCodeMgr.Delete(stateCode);
                    stateCodeMgr.SaveChanges();

                    var test = stateCodeMgr.GetStateProvinceCode(state => state.StateCode == "AK");
                    Assert.Null(test);
                }

            } finally {
                connection.Close();
            } 
        }

        [Fact]
        public void Test_StateProvinceCodeMgr_StateCodeSelectAll()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadStateProvinceCodeTable(context);                                      
                }

                using (var context = new HOSContext(options))
                {
                    IStateProvinceCodeManager stateCodeMgr = new StateProvinceCodeManager(new Repository(context));
                    
                    var stateCodes = stateCodeMgr.GetAll().ToList();

                    Assert.NotNull(stateCodes);
                    Assert.Equal(65, stateCodes.Count());
                }

            } finally {
                connection.Close();
            } 
        } 

        [Fact]
        public void Test_StateProvinceCodeMgr_StateCodeSelectUSACodes()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try {

                var options = new DbContextOptionsBuilder<HOSContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new HOSContext(options))
                {
                    context.Database.EnsureCreated();
                }

                using (var context = new HOSContext(options))
                {
                    HOSTestData.LoadStateProvinceCodeTable(context);                                      
                }

                using (var context = new HOSContext(options))
                {
                    IStateProvinceCodeManager stateCodeMgr = new StateProvinceCodeManager(new Repository(context));
                    
                    var usaCodes = stateCodeMgr.GetStateProvinceCodes(code => code.CountryCode == "USA").ToList();

                    Assert.NotNull(usaCodes);
                    // Puerto Rico and Wash D.C.
                    Assert.Equal(52, usaCodes.Count());
                }

            } finally {
                connection.Close();
            } 
        }                                 
    }
}
