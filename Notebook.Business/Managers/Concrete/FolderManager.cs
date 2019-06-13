using Notebook.Business.Managers.Abstract;
using Notebook.Business.Tools.Validation.FluentValidation;
using Notebook.Core.Aspects.SimpleProxy.Validation;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class FolderManager : Manager<Folder>, IFolderManager
    {
        private IFolderDal servisDal;
        public FolderManager(IFolderDal _servisDal) : base(_servisDal)
        {
            servisDal = _servisDal;
        }

        [Validate(typeof(Folder), typeof(FolderFluentValidation))]
        public override void Add(Folder model)
        {
            model.CreateDate = DateTime.Now;

            base.Add(model);
        }

        [Validate(typeof(Folder), typeof(FolderFluentValidation))]
        public override void Update(Folder model)
        {
            var _folder = servisDal.getOne(a => a.ID == model.ID);
            if (_folder != null)
            {
                // TODO: AutoMapper Uygulanacak
                _folder.Name = model.Name;
                _folder.Explanation = model.Explanation;
                _folder.Visible = model.Visible;

                base.Update(_folder);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
