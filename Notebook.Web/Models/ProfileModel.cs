using Notebook.Business.Models;

namespace Notebook.Web.Models
{
    public class ProfileModel
    {
        public string ID { get; set; }
        public string List { get; set; }

        public ObjectListModel Data { get; set; }
        public UserInfoModel User { get; set; }
        public NavigationModel Navigation { get; set; }
    }
}
