using Notebook.Business.Managers.Abstract;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class SettingsManager : Manager<Settings>, ISettingsManager
    {
        private ISettingsDal servisDal;
        public SettingsManager(ISettingsDal _servisDal) : base(_servisDal)
        {
            servisDal = _servisDal;
        }

        public Settings GetSettings()
        {
            return base.getAll().FirstOrDefault();
        }

        public override void Update(Settings model)
        {
            var _settings = servisDal.getOne(a => a.ID == model.ID);

            if (_settings != null)
            {
                _settings.Gmail = model.Gmail;
                _settings.Host = model.Host;
                _settings.Port = model.Port;
                _settings.Password = model.Password;
                _settings.Username = model.Username;
                _settings.MembershipEmailControl = model.MembershipEmailControl;
                _settings.Metadata = model.Metadata;
                _settings.Introduction = model.Introduction;
                _settings.Footer = model.Footer;
                _settings.Facebook = model.Facebook;
                _settings.Twitter = model.Twitter;
                _settings.WebAddress = model.WebAddress;
                _settings.Title = model.Title;
                _settings.Name = model.Name;
                _settings.Phone = model.Phone;
                _settings.SingleFileSize = model.SingleFileSize;
                _settings.TotalFileSize = model.TotalFileSize;
                _settings.Logo = model.Logo;
                _settings.Icon = model.Icon;
                _settings.Email = model.Email;
                _settings.DefaultLanguage = model.DefaultLanguage;
                _settings.Description = model.Description;
                _settings.Address = model.Address;
                _settings.IsMailActive = model.IsMailActive;
                _settings.Linkedin = model.Linkedin;
                _settings.AcceptedFileTypes = model.AcceptedFileTypes;
            }

            base.Update(_settings);
        }
    }
}
