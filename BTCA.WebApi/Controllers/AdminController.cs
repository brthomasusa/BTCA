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
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private ICompanyManager _companyMgr;
        private ICompanyAddressManager _addressMgr;

        public AdminController(ICompanyManager coMgr, ICompanyAddressManager addMgr, ILogger<AdminController> logger)
        {
            _addressMgr = addMgr;
            _companyMgr = coMgr;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_companyMgr.GetAll());            
        }

        [HttpGet("{id}", Name = "GetCompanyById")]
        public IActionResult GetById(int id)
        {     
            _logger.LogInformation("Request path: {Path}", Request.Path);
            var company = _companyMgr.GetCompany(c => c.ID == id);

            if (company == null)
            {   
                _logger.LogInformation("Company with id: {ID} not found: {Path}", id, Request.Path);
                return NotFound(new { Id = id, error = $"There was no company found with an id of {id}." });
            }       
            return Json(company);
        }

        [HttpPost]
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

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpPost. Create company failed: {@COMPANY}", company);
                return BadRequest(ex.Message);
            }

            return CreatedAtRoute("GetCompanyById", new { id = company.ID }, company);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Company company)
        {
            try {

                if (company == null || company.ID != id)
                {
                    _logger.LogInformation("Update company received invalid data: {@COMPANY}", company);
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

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpPut - Update company failed: {@Company}", company);
                return BadRequest(ex.Message);
            }
                        
            return StatusCode(204);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try {

                var toBeDeleted = _companyMgr.GetCompany(c => c.ID == id);
                if (toBeDeleted == null)
                {
                    _logger.LogInformation("Company delete failed. No company with Id: {ID} found.", id);
                    return NotFound($"Company delete failed. No company with Id: {id} found.");
                }

                _companyMgr.Delete(toBeDeleted);
                _companyMgr.SaveChanges();

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpDelete: Delete company with company Id {CompanyId} failed", id);
                return BadRequest(ex.Message);
            }
            
            return Ok();
        }

        [HttpGet("{companyId}/addresses", Name = "GetAddressesForCompany")]
        public IEnumerable<CompanyAddress> GetAddressesForCompany(int companyId) =>
            _addressMgr.GetCompanyAddresses(companyId);

        [HttpGet("{companyId}/addresses/{addressId}", Name = "GetCompanyAddress")]
        public IActionResult GetCompanyAddress(int companyId, int addressId)
        {
            var address = _addressMgr.GetCompanyAddress(companyId, addressId);
            if (address == null)
            { 
                _logger.LogInformation("Unable to retrieve address with addressId: {ADDRESSID}", addressId);
                return NotFound($"No address with addressId: {addressId} found.");
            }

            return new ObjectResult(address);
        }

        [HttpPost("{companyId}/addresses")]
        public IActionResult CreateAddress([FromBody] CompanyAddress address)
        {
            try {

                if (address == null || !ModelState.IsValid)
                {
                    _logger.LogInformation("Create company address failed: {@ADDRESS}", address);
                    return BadRequest(ModelState);
                }

                address.CreatedBy = "admin";
                address.CreatedOn = DateTime.Now;
                address.UpdatedBy = "admin";
                address.UpdatedOn = DateTime.Now;

                _addressMgr.Create(address);
                _addressMgr.SaveChanges();

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpPost: create company address failed");
                return BadRequest(ex.Message); 
            }

            return CreatedAtRoute("GetCompanyAddress", new {companyId = address.CompanyId, addressId = address.ID}, address);
        }

        [HttpPut("{companyId}/addresses/{addressId}")]
        public IActionResult UpdateAddress(int addressId, [FromBody] CompanyAddress address)
        {
            try {

                if (address == null || address.ID != addressId)
                {
                    _logger.LogInformation("Update company address failed: {@ADDRESS}", address);
                    return BadRequest("Invalid address ID");
                }

                if (!ModelState.IsValid)
                {   _logger.LogInformation("Invalid model state {@MODELSTATE}.", ModelState);
                    return BadRequest(ModelState);
                }

                address.UpdatedBy = "admin";
                address.UpdatedOn = DateTime.Now;

                _addressMgr.Update(address);
                _addressMgr.SaveChanges();

                return new NoContentResult();

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpPut: update company address failed");
                return BadRequest(ex.Message); 
            }            
        } 

        [HttpDelete("{companyId}/addresses/{addressId}")]
        public IActionResult DeleteAddress(int addressId)
        {
            try {

                var toBeDeleted = _addressMgr.GetCompanyAddress(a => a.ID == addressId);
                if (toBeDeleted == null)
                {
                    _logger.LogInformation($"Delete company address failed. No address with Id: {addressId} found.");
                    return NotFound($"Delete company address failed. No address with Id: {addressId} found.");
                }

                _addressMgr.Delete(toBeDeleted);
                _addressMgr.SaveChanges();

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpDelete: delete company address failed");
                return BadRequest(ex.Message); 
            }
            
            return new NoContentResult();
        }                
    }
}
