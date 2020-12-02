using RopeDetection.CommonData.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace RopeDetection.CommonData.ViewModels.UserViewModel
{
    public class UserShortModel : BaseModel
    {
        public string UserFIO { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public bool IsAllowed { get; set; }

        public string Token { get; set; }
    }
}
