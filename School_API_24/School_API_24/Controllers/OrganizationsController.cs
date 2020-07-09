using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public OrganizationsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetOrganizations()
        {
            var organizations = _repository.Organization.GetAllOrganizations(trackChanges: false);
            var organizationsDto = _mapper.Map<IEnumerable<OrganizationDto>>(organizations);
            return Ok(organizationsDto);
            throw new Exception("Exception");
        }

        [HttpGet("{id}")]
        public IActionResult GetOrganization(Guid id)
        {
            var organization = _repository.Organization.GetOrganization(id, trackChanges: false);
            if (organization == null)
            {
                _logger.LogInfo($"Organization with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var organizationDto = _mapper.Map<OrganizationDto>(organization);
                return Ok(organizationDto);
            }
        }
    }
}
