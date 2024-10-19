using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Models.Others
{
    public class BaseResponseModel
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public object Data { get; set; }
    }
}
