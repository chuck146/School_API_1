using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using School_API_24.ModelBinders;

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

        [HttpGet("collection/({ids})", Name = "OrganizationCollection")]
        public IActionResult GetOrganizationCollection([ModelBinder(BinderType =
            typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var organizationEntities = _repository.Organization.GetByIds(ids, trackChanges: false);

            if(ids.Count() != organizationEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var organizationsToReturn = _mapper.Map<IEnumerable<OrganizationDto>>(organizationEntities);
            return Ok(organizationsToReturn);
        }



        [HttpGet("{id}", Name = "OrganizationById")]
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

        [HttpPost]
        public IActionResult CreateOrganization([FromBody]OrganizationForCreationDto organization)
        {
            if(organization == null)
            {
                _logger.LogError("OrganizationForCreationDto object sent from client is null");
                return BadRequest("OrganizationForCreationDto object is null");
            }

            var organizationEntity = _mapper.Map<Organization>(organization);

            _repository.Organization.CreateOrganization(organizationEntity);
            _repository.Save();

            var organizationToReturn = _mapper.Map<OrganizationDto>(organizationEntity);

            return CreatedAtRoute("OrganizationByID", new { id = organizationToReturn.Id }, organizationToReturn);
        }

        [HttpPost("collection")]
        public IActionResult CreateOrganizationCollection([FromBody]
            IEnumerable<OrganizationForCreationDto> organizationCollection)
        {
            if(organizationCollection == null)
            {
                _logger.LogError("Organization collection sent from client is null.");
                return BadRequest("Organization collection is null");
            }

            var organizationEntities = _mapper.Map<IEnumerable<Organization>>(organizationCollection);
            foreach (var organization in organizationEntities)
            { _repository.Organization.CreateOrganization(organization);
            }

            _repository.Save();

            var organizationCollectionToReturn =
                _mapper.Map<IEnumerable<OrganizationDto>>(organizationEntities);
            var ids = string.Join(", ", organizationCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("OrganizationCollection", new { ids },
                organizationCollectionToReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUserForOrganization(Guid organizationId, Guid id)
        {
            var organization = _repository.Organization.GetOrganization(organizationId, trackChanges: false);
            if(organization == null)
            {
                _logger.LogInfo($"Organization with id: {organizationId} doesn't exist in the database.");
                return NotFound();
            }

            var userForOrganization = _repository.User.GetUser(organizationId, id, trackChanges: false);
            if(userForOrganization == null)
            {
                _logger.LogInfo($"User with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.User.DeleteUser(userForOrganization);

            _repository.Save();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteOrganization(Guid id)
        {
            var organization = _repository.Organization.GetOrganization(id, trackChanges: false);
            if(organization == null)
            {
                _logger.LogInfo($"Organization with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _repository.Organization.DeleteOrganization(organization);
            _repository.Save();

            return NoContent();
        }
    }
}
