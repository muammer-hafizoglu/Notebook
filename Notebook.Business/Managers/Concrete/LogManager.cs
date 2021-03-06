﻿using Notebook.Business.Managers.Abstract;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class LogManager : Manager<Log>, ILogManager
    {
        private ILogDal servisDal;
        public LogManager(ILogDal _servisDal) : base(_servisDal)
        {
            servisDal = _servisDal;
        }
    }
}
