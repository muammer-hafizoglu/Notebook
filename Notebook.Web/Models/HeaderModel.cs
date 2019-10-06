using Notebook.Entities.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class HeaderModel
    {
        public Settings Settings { get; set; }
        public User User { get; set; }
    }
}
