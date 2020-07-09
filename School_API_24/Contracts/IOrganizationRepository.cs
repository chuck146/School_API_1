using Entities.Models;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Contracts
{
    public interface IOrganizationRepository
    {
        IEnumerable<Organization> GetAllOrganizations(bool trackChanges);
        Organization GetOrganization(Guid organizationId, bool trackChanges);
        
    }
}
