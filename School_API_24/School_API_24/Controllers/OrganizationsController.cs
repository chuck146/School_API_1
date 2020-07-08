using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities.DataTransferObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace School_API_24.Controllers
{
    [Route("api/organizations")]
    [ApiController]
    public class OrganizationsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;

        public OrganizationsController(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetOrganizations()
        {
            try
            {
                var organizations = _repository.Organization.GetAllOrganizations(trackChanges: false);
                var organizationsDto = organizations.Select(c => new OrganizationDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    FullAddress = string.Join(", ", c.Address, c.Country)
                }).ToList();

                return Ok(organizationsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetOrganizations)} action {ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
