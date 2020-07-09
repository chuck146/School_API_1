using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }
        public IEnumerable<User> GetUsers(Guid organizationId, bool trackChanges) =>
            FindByCondition(e => e.OrganizationId.Equals(organizationId), trackChanges)
            .OrderBy(e => e.Name);
        

        
    }
}
