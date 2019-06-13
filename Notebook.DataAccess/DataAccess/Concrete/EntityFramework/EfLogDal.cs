using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Core.EntityRepository.EntityFramework;
using Notebook.Entities.Entities;

namespace Notebook.DataAccess.DataAccess.Concrete.EntityFramework
{
    public class EfLogDal : EfEntityRepository<Log>, ILogDal
    {
        private DbContext dbContext;
        public EfLogDal(DbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }
    }
}
