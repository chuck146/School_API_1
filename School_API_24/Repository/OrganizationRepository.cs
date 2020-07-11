﻿using Contracts;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public class OrganizationRepository : RepositoryBase<Organization>, IOrganizationRepository
    {
        public OrganizationRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {
        }
        public IEnumerable<Organization> GetAllOrganizations(bool trackChanges) =>
            FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToList();

        public Organization GetOrganization(Guid organizationId, bool trackChanges) =>
            FindByCondition(c => c.Id.Equals(organizationId), trackChanges)
            .SingleOrDefault();

        public void CreateOrganization(Organization organization) => Create(organization);

        public IEnumerable<Organization> GetByIds(IEnumerable<Guid> ids, bool trackChanges) =>
            FindByCondition(x => ids.Contains(x.Id), trackChanges)
            .ToList();

        public void DeleteOrganization(Organization organization)
        {
            Delete(organization);
        }
    }
}
