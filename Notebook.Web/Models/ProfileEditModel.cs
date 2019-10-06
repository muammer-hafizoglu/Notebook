using Notebook.Business.Models;
using Notebook.Entities.Entities;

namespace Notebook.Web.Models
{
    public class ProfileEditModel
    {
        public UserInfoModel UserInfo { get; set; }
        public User User { get; set; }
    }
}
