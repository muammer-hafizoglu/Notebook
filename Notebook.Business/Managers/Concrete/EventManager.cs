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
    public class EventManager : Manager<Event>, IEventManager
    {
        private IEventDal servisDal;
        public EventManager(IEventDal _servisDal) : base(_servisDal)
        {
            servisDal = _servisDal;
        }

        public override void Add(Event model)
        {
            model.CreateDate = DateTime.Now;

            base.Add(model);
        }

        public void Delete(string ID)
        {
            var model = base.getOne(a => a.ID == ID);
            if (model != null)
                base.Delete(model);
            else
                throw new Exception("Event not found");
        }

        public IQueryable<Event> GetEventsForTimeline(string userID, Status status)
        {
            var model = base.getMany(a => a.User.ID == userID && (status != Status.Owner ? a.View == true : true)).OrderByDescending(a => a.CreateDate);

            return model;
        }

        public void Hide(string ID)
        {
            var model = base.getOne(a => a.ID == ID);
            if (model != null)
            {
                model.View = false;
                base.Update(model);
            }
            else
            {
                throw new Exception("Event not found");
            }
               
        }

        public void Show(string ID)
        {
            var model = base.getOne(a => a.ID == ID);
            if (model != null)
            {
                model.View = true;
                base.Update(model);
            }
            else
            {
                throw new Exception("Event not found");
            }
        }
    }
}
