using Notebook.Business.Models;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class GroupDetailModel
    {
        public string ID { get; set; }
        public string List { get; set; }

        public ObjectListModel Data { get; set; }
        public GroupInfoModel Group { get; set; }
        public NavigationModel Navigation { get; set; }
    }
}
