using Notebook.Business.Managers.Abstract;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class UserFolderManager : Manager<UserFolder>, IUserFolderManager
    {
        private IUserFolderDal servisDal;
        public UserFolderManager(IUserFolderDal _servisDal) : base(_servisDal)
        {
            servisDal = _servisDal;
        }
    }
}
