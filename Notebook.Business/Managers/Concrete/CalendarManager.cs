using Microsoft.EntityFrameworkCore;
using Notebook.Business.Managers.Abstract;
using Notebook.Business.Models;
using Notebook.Business.Tools.Validation.FluentValidation;
using Notebook.Core.Aspects.SimpleProxy.Validation;
using Notebook.DataAccess.DataAccess.Abstract;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Notebook.Business.Managers.Concrete
{
    public class CalendarManager : Manager<Calendar>, ICalendarManager
    {
        private ICalendarDal servisDal;
        public CalendarManager(ICalendarDal _servisDal) : base(_servisDal)
        {
            servisDal = _servisDal;
        }

        public override void Add(Calendar model)
        {
            var calendar = new Calendar()
            {
                Title = model.Title,
                Content = model.Content,
                Location = model.Location,
                Start = model.Start,
                Finish = model.Finish,
                User = model.User
            };

            base.Add(calendar);
        }

        public void Delete(string ID)
        {
            var model = base.getOne(a => a.ID == ID);
            if (model != null)
                base.Delete(model);
            else
                throw new Exception("Event not found");
        }
    }
}
