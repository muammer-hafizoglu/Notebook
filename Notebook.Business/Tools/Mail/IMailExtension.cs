using Notebook.Business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Notebook.Business.Tools.Mail
{
    public interface IMailExtension
    {
        Task<bool> SendMail(MailInfoModel model);
    }
}
