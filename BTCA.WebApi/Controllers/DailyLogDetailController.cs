using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.Common.BusinessObjects;

namespace BTCA.WebApi.Controllers
{
    public class DailyLogDetailController : ControllerBase
    {
        private readonly ILogger<DailyLogController> _logger;
        private IDailyLogManager _logMgr;

        public DailyLogDetailController(IDailyLogManager logMgr, ILogger<DailyLogController> logger)
        {
            _logMgr = logMgr;
            _logger = logger;
        }

        [HttpGet("{logId}")]
        [ProducesResponseType(typeof(IEnumerable<DailyLogDetailModel>), 200)]        
        public IActionResult GetDailyLogDetails(int logId) =>
            Ok(_logMgr.GetDailyLogDetails(logId));   


        [HttpGet("{logDetailId}", Name = "GetByLogDetailId")]
        [ProducesResponseType(typeof(DailyLogDetailModel), 200)] 
        [ProducesResponseType(404)]       
        public IActionResult GetDailyLogDetail(int logDetailId)
        {
            var logDetail = _logMgr.GetDailyLogDetail(logDetailId);

            if (logDetail == null)
            {
                return NotFound($"No log detail with logDetailID {logDetailId} found.");
            }
            return Ok(logDetail);   
        }

        [HttpPost]
        [ProducesResponseType(typeof(DailyLogDetailModel), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]        
        public IActionResult Create([FromBody] DailyLogDetailModel dailyLogDetail)
        {
            try {

                if (dailyLogDetail == null || !ModelState.IsValid)
                {
                    _logger.LogInformation("Create daily log detail failed.");
                    return BadRequest(ModelState);
                }

                _logMgr.CreateLogDetail(dailyLogDetail);
                _logMgr.SaveChanges();
                return CreatedAtRoute("GetByLogDetailId", new {logDetailId = dailyLogDetail.LogDetailID}, dailyLogDetail);

            } catch (Exception ex) when(Log(ex, "HttpPost: create daily log detail failed"))
            {
                return new StatusCodeResult(500);
            }            
        }

        [HttpPut("{logDetailId}")]
        [ProducesResponseType(204)]       
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]        
        public IActionResult Update(int logDetailId, [FromBody] DailyLogDetailModel dailyLogDetail)
        {
            try {

                if (dailyLogDetail == null || dailyLogDetail.LogDetailID != logDetailId)
                {
                    _logger.LogInformation("Update daily log detail failed for LogDetailID: {0}", logDetailId);
                    return BadRequest($"Invalid daily logDetailID: {logDetailId}");
                }

                if (!ModelState.IsValid)
                {   
                    _logger.LogInformation("Invalid model state.");
                    return BadRequest(ModelState);
                }

                _logMgr.UpdateLogDetail(dailyLogDetail);
                _logMgr.SaveChanges();

                return NoContent();

            } catch (Exception ex) when(Log(ex, "HttpPut: update daily log detail failed"))
            {
                return new StatusCodeResult(500); 
            }            
        } 

        [HttpDelete("{logDetailId}")]
        [ProducesResponseType(204)]               
        [ProducesResponseType(404)]       
        [ProducesResponseType(500)] 
        public IActionResult Delete(int logDetailId)
        {
            try {
                
                var toBeDeleted = _logMgr.GetDailyLogDetail(logDetailId);

                if (toBeDeleted == null)
                {
                    _logger.LogInformation($"Delete daily log detail failed. No daily log detail with Id: {logDetailId} found.");
                    return NotFound($"Delete daily log failed. No daily log detail with Id: {logDetailId} found.");
                }

                _logMgr.DeleteLogDetail(toBeDeleted);
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
