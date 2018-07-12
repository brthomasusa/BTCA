using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Moq;
using BTCA.Common.BusinessObjects;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.WebApi.Controllers;

namespace BTCA.Tests.UnitTests
{
    public class DailyLogControllerTests : BaseTestClass
    {
        private readonly ILogger<DailyLogController> _logger;
        private Mock<IDailyLogManager> _mockDailyLogMgr; 

        public DailyLogControllerTests()
        {
            var serviceProvider = new ServiceCollection().AddLogging().BuildServiceProvider();
            var factory = serviceProvider.GetService<ILoggerFactory>();
            _logger = factory.CreateLogger<DailyLogController>();                        
            _mockDailyLogMgr = new Mock<IDailyLogManager>();             
        }



    }
}
