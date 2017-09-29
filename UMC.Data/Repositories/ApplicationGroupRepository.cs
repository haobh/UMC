using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UMC.Data.Infrastructure;
using UMC.Model.Models;

namespace UMC.Data.Repositories
{
    public interface IApplicationGroupRepository : IRepository<ApplicationGroup>
    {

    }
    public class ApplicationGroupRepository : RepositoryBase<ApplicationGroup>, IApplicationGroupRepository
    {
        public ApplicationGroupRepository(IDbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
