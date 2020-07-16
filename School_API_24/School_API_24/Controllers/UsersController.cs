using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace School_API_24.Controllers
{
    [Route("api/organizations/{organizationId}/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public UsersController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUsersForOrganization(Guid organizationId)
        {
            var organization = _repository.Organization.GetOrganization(organizationId, trackChanges: false);
            if (organization == null)
            {
                _logger.LogInfo($"Organization with id: {organizationId} doesn't exist in the database.");
                return NotFound();
            }

            var usersFromDb = _repository.User.GetUsers(organizationId, trackChanges: false);

            var usersDto = _mapper.Map<IEnumerable<UserDto>>(usersFromDb);

            return Ok(usersDto);
        }

        [HttpGet("{id}", Name = "GetUserForOrganization")]
        public IActionResult GetUserForOrganization(Guid organizationId, Guid id)
        {
            var organization = _repository.Organization.GetOrganization(organizationId, trackChanges: false);
            if(organization == null)
            {
                _logger.LogInfo($"Organization with id: {organizationId} doesn't exist in the database.");
                return NotFound();
            }

            var userDb = _repository.User.GetUser(organizationId, id, trackChanges: false);
            if (userDb == null)
            {
                _logger.LogInfo($"User with id: {id} doesn't exist in the database.");
            }

            var user = _mapper.Map<UserDto>(userDb);
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUserForOrganization(Guid organizationId, [FromBody]
        UserForCreationDto user)
        {
            {
                if(user == null)
                {
                    _logger.LogError("UserForCreationDto object sent from client is null.");
                    return BadRequest("UserForCreationDto object is null");
                }

                if(!ModelState.IsValid)
                {
                    _logger.LogError("Invalid model state for the UserForCreationDto object");
                    return UnprocessableEntity(ModelState);
                }

                var organization = _repository.Organization.GetOrganization(organizationId, trackChanges: false);
                if (organization == null)
                {
                    _logger.LogInfo($"Organization with Id: {organizationId} doesn't exist in the database.");
                    return NotFound();
                }
                var userEntity = _mapper.Map<User>(user);

                _repository.User.CreateUserForOrganization(organizationId, userEntity);
                _repository.Save();

                var userToReturn = _mapper.Map<UserDto>(userEntity);

                return CreatedAtRoute("GetUserForOrganization", new { organizationId, id = userToReturn.Id }, userToReturn);

            } 
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUserForOrganization(Guid organizationId, Guid id, [FromBody]
        UserForUpdateDto user)
        {
            if(user == null)
            {
                _logger.LogError("UserForUpdateDto object sent from client is null.");
                return BadRequest("UserForUpdateDto object is null");
            }

            var organization = _repository.Organization.GetOrganization(organizationId, trackChanges: false);
            if(organization == null)
            {
                _logger.LogInfo($"Organization with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            var userEntity = _repository.User.GetUser(organizationId, id, trackChanges: true);
            if(userEntity == null)
            {
                _logger.LogInfo($"User with id: {id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(user, userEntity);
            _repository.Save();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult PartiallyUpdateUserForOrganization(Guid organizationId, Guid id,
[FromBody] JsonPatchDocument<UserForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var organization = _repository.Organization.GetOrganization(organizationId, trackChanges: false);
            if (organization == null)
            {
                _logger.LogInfo($"Organization with id: {organizationId} doesn't exist in the database.");
            return NotFound();
            }
            var userEntity = _repository.User.GetUser(organizationId, id, trackChanges:
           true);
            if (userEntity == null)
            {
                _logger.LogInfo($"User with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var userToPatch = _mapper.Map<UserForUpdateDto>(userEntity);
            patchDoc.ApplyTo(userToPatch);
            _mapper.Map(userToPatch, userEntity);
            _repository.Save();
            return NoContent();

        }

    }
}
