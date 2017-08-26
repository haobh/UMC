using UMC.Data.Infrastructure;
using UMC.Model.Models;

namespace UMC.Data.Repositories
{
    public interface IFooterRepository : IRepository<Footer>
    {
        Footer GetSingleById(string id);
    }

    public class FooterRepository : RepositoryBase<Footer>, IFooterRepository
    {
        public FooterRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
        //Viet phuong thuc ngoai Repository
        public Footer GetSingleById(string id)
        {
            return DbContext.Footers.Find(id);
        }
    }
}