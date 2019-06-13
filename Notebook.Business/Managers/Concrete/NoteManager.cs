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
    public class NoteManager : Manager<Note>, INoteManager
    {
        private INoteDal servisDal;
        public NoteManager(INoteDal _servisDal) : base(_servisDal)
        {
            servisDal = _servisDal;
        }


        [Validate(typeof(Note), typeof(NoteFluentValidation))]
        public override void Add(Note model)
        {
            model.CreateDate = DateTime.Now;

            base.Add(model);
        }

        [Validate(typeof(Note), typeof(NoteFluentValidation))]
        public override void Update(Note model)
        {
            var _note = servisDal.getOne(a => a.ID == model.ID);
            if (_note != null)
            {
                // TODO: AutoMapper Uygulanacak
                _note.Title = model.Title;
                _note.Explanation = model.Explanation;
                _note.Visible = model.Visible;
                _note.Tags = model.Tags;
                _note.Content = model.Content;
                _note.UpdateDate = DateTime.Now;

                base.Update(_note);
            }
            else
            {
                throw new NullReferenceException();
            }
        }
    }
}
