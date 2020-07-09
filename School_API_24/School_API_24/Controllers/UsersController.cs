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
    }
}
