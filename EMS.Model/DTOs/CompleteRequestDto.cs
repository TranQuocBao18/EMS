using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.DTOs
{
    public class CompleteRequestDto
    {
        public int RequestId { get; set; }
        public int EquipmentId { get; set; }
        public int OldDepartmentId { get; set; }
        public int NewDepartmentId { get; set; }
    }
}
