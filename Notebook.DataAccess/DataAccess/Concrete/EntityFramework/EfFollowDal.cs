using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Core.EntityRepository.EntityFramework;
using Notebook.Entities.Entities;

namespace Notebook.DataAccess.DataAccess.Concrete.EntityFramework
{
    public class EfFollowDal : EfEntityRepository<Follow>, IFollowDal
    {
        private DbContext dbContext;
        public EfFollowDal(DbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }
    }
}
