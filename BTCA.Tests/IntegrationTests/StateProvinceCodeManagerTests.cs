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

namespace BTCA.Tests.IntegrationTests
{
    public class StateProvinceCodeManagerTests
    {
        [Fact]
        [Trait("Category", "Integration.StateProvinceCodeManager")]
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
        [Trait("Category", "Integration.StateProvinceCodeManager")]
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
