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

        public User GetUser(Guid organizationId, Guid id, bool trackChanges) =>
            FindByCondition(e => e.OrganizationId.Equals(organizationId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefault();

        public void CreateUserForOrganization(Guid organizationId, User user)
        {
            user.OrganizationId = organizationId;
            Create(user);
        }

        public void DeleteUser(User user)
        {
            Delete(user);
        }
    }
}
