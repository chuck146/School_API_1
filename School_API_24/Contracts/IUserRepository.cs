using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers(Guid organizationId, bool trackChanges);
        User GetUser(Guid userId, Guid Id, bool trackChanges);
        void CreateUserForOrganization(Guid organizationId, User user);
        void DeleteUser(User user);
    }
}
