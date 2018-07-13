using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.Common.Entities;
using BTCA.Common.BusinessObjects;

namespace BTCA.WebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]        
    public class DailyLogController : ControllerBase
    {
        private readonly ILogger<DailyLogController> _logger;
        private IDailyLogManager _logMgr;

        public DailyLogController(IDailyLogManager logMgr, ILogger<DailyLogController> logger)
        {
            _logMgr = logMgr;
            _logger = logger;
        }

        [HttpGet("{id}", Name = "GetAll")]
        [ProducesResponseType(typeof(IEnumerable<DailyLogModel>), 200)]
        public IActionResult GetAll(int id) =>
            Ok(_logMgr.GetDailyLogs(id));         

    }
}
