using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.Common.Entities;

namespace BTCA.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    //[Authorize]    
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private ICompanyManager _companyMgr;

        public AdminController(ICompanyManager coMgr, ILogger<AdminController> logger)
        {
            _companyMgr = coMgr;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Company>), 200)]
        public IActionResult Get()
        {
            return Ok(_companyMgr.GetAll());            
        }

        [HttpGet("{id}", Name = "GetCompanyById")]
        [ProducesResponseType(typeof(Company), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {     
            var company = _companyMgr.GetCompany(c => c.ID == id);

            if (company == null)
            {   
                _logger.LogInformation("Company with id: {0} not found.", id);
                return NotFound(new { Id = id, error = $"There was no company found with an id of {id}." });
            }       
            return Ok(company);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Company), 201)]
        [ProducesResponseType(400)]
        public IActionResult Create([FromBody] Company company)
        {
            try {

                if (company == null || !ModelState.IsValid)
                {
                    _logger.LogInformation("Create company received invalid model state: {@modelState}", ModelState);
                    return BadRequest(ModelState);
                }

                _companyMgr.Create(company);
                _companyMgr.SaveChanges();
                return CreatedAtRoute("GetCompanyById", new { id = company.ID }, company);

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpPost. Create company failed: {0}", company);
                return BadRequest(ex.Message);
            }            
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]        
        [ProducesResponseType(400)]
        public IActionResult Update(int id, [FromBody] Company company)
        {
            try {

                if (company == null || company.ID != id || !ModelState.IsValid)
                {
                    _logger.LogInformation("Update company received invalid data: {0}", company);
                    return BadRequest("Null company or invalid company.Id");
                }

                var toBeUpdated = _companyMgr.GetCompany(c => c.ID == company.ID);
                if (toBeUpdated == null)
                {
                    _logger.LogInformation("Company update failed. No company with Id: {ID} found.", company.ID);
                    return NotFound($"Company update failed. No company with Id: {company.ID} found.");
                }

                toBeUpdated.CompanyCode = company.CompanyCode;
                toBeUpdated.CompanyName = company.CompanyName;
                toBeUpdated.DOT_Number = company.DOT_Number;
                toBeUpdated.MC_Number = company.MC_Number;
                toBeUpdated.UpdatedBy = "admin";
                toBeUpdated.UpdatedOn = DateTime.Now;

                _companyMgr.Update(toBeUpdated);
                _companyMgr.SaveChanges();
                return NoContent();

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpPut - Update company failed: {@Company}", company);
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]        
        [ProducesResponseType(400)]        
        public IActionResult Delete(int id)
        {
            try {

                var toBeDeleted = _companyMgr.GetCompany(c => c.ID == id);
                if (toBeDeleted == null)
                {
                    _logger.LogInformation("Company delete failed. No company with Id: {0} found.", id);
                    return NotFound($"Company delete failed. No company with Id: {id} found.");
                }

                _companyMgr.Delete(toBeDeleted);
                _companyMgr.SaveChanges();
                return NoContent();

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpDelete: Delete company with company Id {CompanyId} failed", id);
                return BadRequest(ex.Message);
            }                        
        }               
    }
}
