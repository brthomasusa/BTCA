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
    public class CompanyUserController : Controller
    {
        private readonly ILogger<CompanyUserController> _logger;
        private ICompanyUserManager _compUserMgr;

        public CompanyUserController(ICompanyUserManager coUserMgr, ILogger<CompanyUserController> logger)
        {
            _compUserMgr = coUserMgr;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _compUserMgr.GetAll();
            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound("No users found.");
            }            
        }

        [HttpGet("{companyId}")]
        public IActionResult GetUsersByCompanyId(int companyId)
        {
            var users = _compUserMgr.GetCompanyUsers(companyId);
            if (users != null)
            {
                return Ok(users);
            }
            else
            {
                return NotFound(new {Id = companyId, Message = $"No users found for company with Id: {companyId}"});
            }
        }

        [HttpGet("{companyId}/users/{userId}")]
        public IActionResult GetUserByCompanyId(int companyId, int userId)
        {
            var user = _compUserMgr.GetCompanyUser(companyId, userId);
            if (user != null)
            {
                return Ok(user);
            }
            else
            {
                return NotFound(new {Message = $"No user found for company: {companyId} with user Id: {userId}"});
            }
        }                
    }
}
