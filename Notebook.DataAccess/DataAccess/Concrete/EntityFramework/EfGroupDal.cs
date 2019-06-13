using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Core.EntityRepository.EntityFramework;
using Notebook.Entities.Entities;

namespace Notebook.DataAccess.DataAccess.Concrete.EntityFramework
{
    public class EfGroupDal : EfEntityRepository<Group>, IGroupDal
    {
        private DbContext dbContext;
        public EfGroupDal(DbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }
    }
}
