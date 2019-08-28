using Notebook.Business.Models;
using Notebook.Entities.Entities;
using Notebook.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notebook.Web.Models
{
    public class FolderDetailModel
    {
        public string ID { get; set; }
        public string List { get; set; }

        public ObjectListModel Data { get; set; }
        public FolderInfoModel Folder { get; set; }
        public NavigationModel Navigation { get; set; }
    }
}
