using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BTCA.DomainLayer.Managers.Interface;
using BTCA.Common.Entities;
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

        [HttpGet("{id}", Name = "GetAll")]
        [ProducesResponseType(typeof(IEnumerable<CompanyAddress>), 200)]
        public IActionResult GetAll(int id) =>
            Ok(_addressMgr.GetCompanyAddresses(id)); 


        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(typeof(CompanyAddress), 200)]
        [ProducesResponseType(404)]
        public IActionResult GetById(int id)
        {
            var address = _addressMgr.GetCompanyAddress(id);
            if (address == null)
            { 
                _logger.LogInformation("Unable to retrieve address with addressId: {0}", id);
                return NotFound($"No address with addressId: {id} found.");
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
                return CreatedAtRoute("GetById", new {id = address.ID}, address);

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpPost: create company address failed");
                return BadRequest(ex.Message); 
            }            
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]       
        [ProducesResponseType(400)]        
        public IActionResult Update(int id, [FromBody] CompanyAddress address)
        {
            try {

                if (address == null || address.ID != id)
                {
                    _logger.LogInformation("Update company address failed for ID: {0}", id);
                    return BadRequest($"Invalid address ID: {id}");
                }

                if (!ModelState.IsValid)
                {   _logger.LogInformation("Invalid model state.");
                    return BadRequest(ModelState);
                }

                _addressMgr.Update(address);
                _addressMgr.SaveChanges();

                return NoContent();

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpPut: update company address failed");
                return BadRequest(ex.Message); 
            }            
        } 

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]       
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]        
        public IActionResult Delete(int id)
        {
            try {
                
                /// TODO Fix _addressMgr.GetCompanyAddress(add => add.ID == id)
                //var toBeDeleted = _addressMgr.GetCompanyAddress(add => add.ID == id);
                
                var toBeDeleted = _addressMgr.GetCompanyAddress(id);

                if (toBeDeleted == null)
                {
                    _logger.LogInformation($"Delete company address failed. No address with Id: {id} found.");
                    return NotFound($"Delete company address failed. No address with Id: {id} found.");
                }

                _addressMgr.Delete(toBeDeleted);
                _addressMgr.SaveChanges();
                return NoContent();

            } catch (Exception ex) {
                _logger.LogError(ex, "HttpDelete: delete company address failed");
                return BadRequest(ex.Message); 
            }                        
        }                                         
    }
}
