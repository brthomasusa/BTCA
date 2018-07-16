using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.Common.BusinessObjects;

namespace BTCA.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]    
    public class CompanyAddressController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;
        private ICompanyAddressManager _addressMgr;

        public CompanyAddressController(ICompanyAddressManager addMgr, ILogger<AdminController> logger)
        {
            _addressMgr = addMgr;
            _logger = logger;
        }

        [HttpGet("{companyId}", Name = "GetByCompanyId")]
        [ProducesResponseType(typeof(IEnumerable<CompanyAddress>), 200)]
        public IActionResult GetAll() =>
            Ok(_addressMgr.GetAll()); 

        [HttpGet("{companyId}", Name = "GetByCompanyId")]
        [ProducesResponseType(typeof(IEnumerable<CompanyAddress>), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetByCompanyId(int companyId)
        {
            var addresses = _addressMgr.GetCompanyAddresses(companyId);
            if (addresses == null)
            {
                return NotFound($"No addresses with companyId: {companyId} found.");
            }
            return Ok(addresses); 
        }
            


        [HttpGet("{addressId}", Name = "GetByAddressId")]
        [ProducesResponseType(typeof(CompanyAddress), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetByAddressId(int addressId)
        {
            var address = _addressMgr.GetCompanyAddress(addressId);
            if (address == null)
            { 
                _logger.LogInformation("Unable to retrieve address with addressId: {0}", addressId);
                return NotFound($"No address with addressId: {addressId} found.");
            }

            return Ok(address);
        }   

        [HttpPost]
        [ProducesResponseType(typeof(CompanyAddress), 201)]
        [ProducesResponseType(400)]        
        public IActionResult Create([FromBody] CompanyAddress address)
        {
            try {

                if (address == null || !ModelState.IsValid)
                {
                    _logger.LogInformation("Create company address failed.");
                    return BadRequest(ModelState);
                }

                _addressMgr.Create(address);
                _addressMgr.SaveChanges();
                return CreatedAtRoute("GetByAddressId", new {id = address.ID}, address);

            } catch (Exception ex) when(Log(ex, "HttpPost: create company address failed"))
            {
                return new StatusCodeResult(500); 
            }            
        }

        [HttpPut("{addressId}")]
        [ProducesResponseType(204)]       
        [ProducesResponseType(400)]        
        public IActionResult Update(int addressId, [FromBody] CompanyAddress address)
        {
            try {

                if (address == null || address.ID != addressId)
                {
                    _logger.LogInformation("Update company address failed for ID: {0}", addressId);
                    return BadRequest($"Invalid address ID: {addressId}");
                }

                if (!ModelState.IsValid)
                {   _logger.LogInformation("Invalid model state.");
                    return BadRequest(ModelState);
                }

                _addressMgr.Update(address);
                _addressMgr.SaveChanges();

                return NoContent();

            } catch (Exception ex) when(Log(ex, "HttpPut: update company address failed"))
            {
                return new StatusCodeResult(500);
            }            
        } 

        [HttpDelete("{addressId}")]
        [ProducesResponseType(204)]       
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]        
        public IActionResult Delete(int addressId)
        {
            try {
                
                var toBeDeleted = _addressMgr.GetCompanyAddress(addressId);

                if (toBeDeleted == null)
                {
                    _logger.LogInformation($"Delete company address failed. No address with Id: {addressId} found.");
                    return NotFound($"Delete company address failed. No address with Id: {addressId} found.");
                }

                _addressMgr.Delete(toBeDeleted);
                _addressMgr.SaveChanges();
                return NoContent();

            } catch (Exception ex) when(Log(ex, "HttpDelete: delete company address failed"))
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
