using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BTCA.DomainLayer.Managers.Interface;
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

        [HttpGet("{companyId}")]
        [ProducesResponseType(typeof(IEnumerable<DailyLogModel>), 200)]        
        public IActionResult GetAllByCompany(int companyId) =>
            Ok(_logMgr.GetDailyLogsForCompany(companyId));         

        [HttpGet("{driverId}", Name = "GetAllByDriver")]
        [ProducesResponseType(typeof(IEnumerable<DailyLogModel>), 200)]
        public IActionResult GetAllByDriver(int driverId) =>
            Ok(_logMgr.GetDailyLogsForDriver(driverId));

        [HttpGet("{driverId}/{logDate}")]
        [ProducesResponseType(typeof(DailyLogModel), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetForDriverAndDate(int driverId, DateTime logDate)
        {
            var dailyLog = _logMgr.GetDailyLog(logDate.Date, driverId);

            if (dailyLog == null)
            {
                return NotFound($"No log with log date {logDate} and driver Id {driverId} found.");
            }

            return Ok(dailyLog);
        } 

        [HttpGet("{driverId}/{beginDate}/endDate")]
        [ProducesResponseType(typeof(IEnumerable<DailyLogModel>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetForDateRange(int driverId, DateTime beginDate, DateTime endDate)
        {
            var dateRange = _logMgr.GetDailyLogs(dl => dl.DriverID == driverId && 
                                                       dl.LogDate.Date >= beginDate.Date && 
                                                       dl.LogDate.Date <= endDate.Date);

            if (dateRange == null)
            {
                return NotFound("No logs found meeting search criteria.");
            }

            return Ok(dateRange);
        }

        [HttpGet("{logId}", Name = "GetByLogId")]
        [ProducesResponseType(typeof(IEnumerable<DailyLogModel>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetByLogId(int logId){
            var dailyLog = _logMgr.GetDailyLog(logId);
            if (dailyLog == null)
            {
                return NotFound($"No daily log with ID: {logId} found.");
            }

            return Ok(dailyLog);
        }

        [HttpPost]
        [ProducesResponseType(typeof(DailyLogModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]        
        public IActionResult Create([FromBody] DailyLogModel dailyLog)
        {
            try {

                if (dailyLog == null || !ModelState.IsValid)
                {
                    _logger.LogInformation("Create daily log failed.");
                    return BadRequest(ModelState);
                }

                _logMgr.Create(dailyLog);
                _logMgr.SaveChanges();
                return CreatedAtRoute("GetByLogId", new {logId = dailyLog.LogID}, dailyLog);

            } catch (Exception ex) when(Log(ex, "HttpPost: create daily log failed"))
            {
                return new StatusCodeResult(500);
            }            
        }

        [HttpPut("{logId}")]
        [ProducesResponseType(204)]       
        [ProducesResponseType(400)]        
        [ProducesResponseType(500)]
        public IActionResult Update(int logId, [FromBody] DailyLogModel dailyLog)
        {
            try {

                if (dailyLog == null || dailyLog.LogID != logId)
                {
                    _logger.LogInformation("Update daily log failed for LogID: {0}", logId);
                    return BadRequest($"Invalid daily logID: {logId}");
                }

                if (!ModelState.IsValid)
                {   
                    _logger.LogInformation("Invalid model state.");
                    return BadRequest(ModelState);
                }

                _logMgr.Update(dailyLog);
                _logMgr.SaveChanges();

                return NoContent();

            } catch (Exception ex) when(Log(ex, "HttpPut: update daily log failed"))
            {
                return new StatusCodeResult(500); 
            }            
        } 

        [HttpDelete("{logId}")]
        [ProducesResponseType(204)]               
        [ProducesResponseType(404)]       
        [ProducesResponseType(500)] 
        public IActionResult Delete(int logId)
        {
            try {
                
                var toBeDeleted = _logMgr.GetDailyLog(logId);

                if (toBeDeleted == null)
                {
                    _logger.LogInformation($"Delete daily log failed. No daily log with Id: {logId} found.");
                    return NotFound($"Delete daily log failed. No daily log with Id: {logId} found.");
                }

                _logMgr.Delete(toBeDeleted);
                _logMgr.SaveChanges();
                return NoContent();

            } catch (Exception ex) when(Log(ex, "HttpDelete: delete daily log failed"))
            {
                return new StatusCodeResult(500); 
            }                        
        }    

        private bool Log(Exception e, string msg)
        {
            _logger.LogError(e, msg);
            return true;
        }                                                       
    }
}
