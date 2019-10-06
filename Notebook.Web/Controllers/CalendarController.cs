using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Notebook.Business.Managers.Abstract;
using Notebook.Business.Models;
using Notebook.Entities.Entities;
using Notebook.Web.Filters;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notebook.Web.Controllers
{
    [TypeFilter(typeof(AccountFilterAttribute))]
    public class CalendarController : Controller
    {
        private ICalendarManager _calendarManager;
        public CalendarController(ICalendarManager calendarManager)
        {
            _calendarManager = calendarManager;
        }

        [Route("~/calendar")]
        public IActionResult Calendar()
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            var events = _calendarManager.getMany(a => a.User.ID == _user.ID &&
                        (a.Start.ToString("M-yyyy") == DateTime.Now.ToString("M-yyyy") || a.Finish.ToString("M-yyyy") == DateTime.Now.ToString("M-yyyy"))).ToList();

            var jsonEvent = HelperMethods.ObjectConvertJson(events);

            return View("Calendar", jsonEvent);
        }

        [HttpGet]
        [Route("~/eventDays")]
        public JsonResult EventDays(string date = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            var events = _calendarManager.getMany(a => a.User.ID == _user.ID &&
                        (!string.IsNullOrEmpty(date) ? (a.Start.ToString("yyyy-MM-dd").Contains(date) || a.Finish.ToString("yyyy-MM-dd").Contains(date) || DatetimeBetween(a.Start, a.Finish, date)) :
                        (a.Start.ToString("M-yyyy") == DateTime.Now.ToString("M-yyyy") || a.Finish.ToString("M-yyyy") == DateTime.Now.ToString("M-yyyy"))))
                        .Select(a => a.Start.ToString("yyyy-MM-dd")).ToList();

            var jsonEvent = HelperMethods.ObjectConvertJson(events);

            return Json(events);
        }

        [HttpGet]
        [Route("~/get-events")]
        public IActionResult Eventlist(string date = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");

            var events = _calendarManager.getMany(a => a.User.ID == _user.ID &&
                    (!string.IsNullOrEmpty(date) ? (a.Start.ToString("yyyy-MM-dd").Contains(date) || a.Finish.ToString("yyyy-MM-dd").Contains(date) || DatetimeBetween(a.Start,a.Finish,date)) :
                    (a.Start.ToString("MM-yyyy") == DateTime.Now.ToString("MM-yyyy") || a.Finish.ToString("MM-yyyy") == DateTime.Now.ToString("MM-yyyy")))).ToList();

            return PartialView(events);
        }

        private bool DatetimeBetween(DateTime Start, DateTime Finish, string Date)
        {
            var _date = Convert.ToDateTime(Date);

            if (Start <= _date && _date <= Finish)
                return true;
            else
                return false;

        }



        [HttpGet]
        [Route("~/add-event")]
        [Route("~/{ID?}/edit-event")]
        public IActionResult Form(string ID = "")
        {
            var _user = HttpContext.Session.GetSession<User>("User");
            var _event = _calendarManager.getOne(a => a.ID == ID && a.User.ID == _user.ID);

            return PartialView(_event ?? new Calendar());
        }

        [HttpPost]
        [Route("~/addCalendarEvent")]
        [ValidateAntiForgeryToken]
        public IActionResult AddEvent(Calendar calendar)
        {
            calendar.User = HttpContext.Session.GetSession<User>("User");

            _calendarManager.Add(calendar);

            return Redirect("/calendar");
        }

        [Route("~/remove-event")]
        [HttpGet]
        public JsonResult DeleteEvent(string ID = "")
        {
            _calendarManager.Delete(ID);

            return Json("");
        }
    }
}
