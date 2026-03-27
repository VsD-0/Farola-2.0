using Farola.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Farola.Domain.Interfaces.Repositories
{
    public interface IRoleRepository
    {
        Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
    }
}
