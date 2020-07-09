using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers(Guid organizationId, bool trackChanges);
    }
}
