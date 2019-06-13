using Newtonsoft.Json;
using Notebook.Business.Managers.Abstract;
using Notebook.Core.CrossCuttingConcerns.Logging;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Tools.Logging
{
    public class DatabaseLogger : ILoggerService
    {
        private readonly ILogManager _logManager;
        public DatabaseLogger(ILogManager logManager)
        {
            _logManager = logManager;
        }
        public void Logging(LogDetail log)
        {
            if (log != null)
            {
                string _detail = JsonConvert.SerializeObject(log);

                Log _log = new Log()
                {
                    ID = _logManager.CreateGuid(),
                    DateTime = DateTime.Now,
                    Detail = _detail
                };

                _logManager.Add(_log);
                _logManager.Save();
            }
        }
    }
}
