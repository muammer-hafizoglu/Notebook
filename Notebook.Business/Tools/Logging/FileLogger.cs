using Microsoft.Extensions.Hosting;
using Notebook.Core.CrossCuttingConcerns.Logging;
using Microsoft.Extensions.Configuration;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Notebook.Business.Tools.Logging
{
    public class FileLogger : ILoggerService
    {
        private IHostingEnvironment _hostingEnvironment;
        private IConfiguration _configuration;
        public FileLogger(IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }
        public void Logging(LogDetail log)
        {

            if (log != null)
            {
                string _log = HelperMethods.JsonConvertString(log);

                string root = string.Format("{0}{1}", _hostingEnvironment.ContentRootPath, _configuration.GetValue<string>("Logging:Path"));

                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }

                string logFileName = string.Format("{0}/{1}-{2}-{3}.json", root, log.DateTime.Year, log.DateTime.Month, log.DateTime.Day);

                if (!File.Exists(logFileName))
                {
                    var stream = File.Open(logFileName, FileMode.Create);
                    stream.Close();
                }

                using (var stream = File.AppendText(logFileName))
                {
                    stream.WriteLine(_log);
                    stream.Close();
                }
            }
        }
    }
}
