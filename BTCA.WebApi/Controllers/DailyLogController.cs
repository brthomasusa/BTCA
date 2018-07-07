using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;

namespace BTCA.WebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]        
    public class DailyLogController : Controller
    {
        private readonly ILogger<DailyLogController> _logger;
        private IDailyLogManager _logMgr;

        public DailyLogController(IDailyLogManager logMgr, ILogger<DailyLogController> logger)
        {
            _logMgr = logMgr;
            _logger = logger;
        }        
    }
}
