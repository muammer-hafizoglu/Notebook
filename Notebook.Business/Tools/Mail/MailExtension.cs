using Notebook.Business.Managers.Abstract;
using Notebook.Business.Models;
using Notebook.Business.Tools.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Notebook.Business.Tools.Mail
{
    public class MailExtension : IMailExtension
    {
        private ISettingsManager _settingsManager;
        public MailExtension(ISettingsManager settingsManager)
        {
            _settingsManager = settingsManager;
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }
        public async Task<bool> SendMail(MailInfoModel model)
        {
            var settings = _settingsManager.Table().FirstOrDefault();

            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(settings.Email);

                if (model.MailTo.Contains(","))
                {
                    string[] mailaddress = model.MailTo.Split(",");
                    foreach (var adress in mailaddress)
                    {
                        msg.To.Add(new MailAddress(adress));
                    }
                }
                else
                {
                    msg.To.Add(new MailAddress(model.MailTo));
                }

                if (!string.IsNullOrEmpty(model.File))
                {
                    Attachment attachment = new Attachment(model.File);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.FileName = Path.GetFileName(model.File);

                    msg.Attachments.Add(attachment);
                }

                msg.Subject = model.Subject;
                msg.IsBodyHtml = true;
                msg.Body = model.Message;

                ServicePointManager.ServerCertificateValidationCallback += (s, cert, chain, sslPolicyErrors) => true;

                SmtpClient client = new SmtpClient(settings.Host, Convert.ToInt32(settings.Port));
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential(settings.Username, settings.Password);
                await client.SendMailAsync(msg);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
