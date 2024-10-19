using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Models.Others
{
    public class LoginResponseModel
    {
        public string Token { get; set; }
        public long TokenExpired { get; set; }
        public string RefreshToken { get; set; }
    }
}
