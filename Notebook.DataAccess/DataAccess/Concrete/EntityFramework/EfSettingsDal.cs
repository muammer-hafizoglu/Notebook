using Microsoft.EntityFrameworkCore;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Core.EntityRepository.EntityFramework;
using Notebook.Entities.Entities;

namespace Notebook.DataAccess.DataAccess.Concrete.EntityFramework
{
    public class EfSettingsDal : EfEntityRepository<NotebookSettings>, ISettingsDal
    {
        private DbContext dbContext;
        public EfSettingsDal(DbContext _dbContext) : base(_dbContext)
        {
            dbContext = _dbContext;
        }
    }
}
