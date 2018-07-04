using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using BTCA.DomainLayer.Managers.Interface;

namespace BTCA.WebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class StatesController : Controller
    {
        private readonly ILogger<StatesController> _logger;
        private IStateProvinceCodeManager _manager;
 
        public StatesController(IStateProvinceCodeManager manager, ILogger<StatesController> logger)
        {
            _manager = manager;
            _logger = logger;
        } 

        [HttpGet]
        public IActionResult Get() => Ok(_manager.GetAll()); 

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var stateCode = _manager.GetStateProvinceCode(code => code.ID == id);
            if (stateCode == null)
            {
                _logger.LogInformation($"StateProvinceCode with ID {id} was not found.");
                return NotFound(new { Id = id, error = "There was no StateProvinceCode with an ID of {id}." });
            }
            return Ok(stateCode);
        }                
    }
}
