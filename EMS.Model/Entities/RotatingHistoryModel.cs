using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Entities
{
    public class RotatingHistoryModel
    {
        public int ID { get; set; }
        public int RequestUserId { get; set; }
        public virtual UserModel RequestUser { get; set; }
        public int EquipmentId { get; set; }
        public virtual EquipmentModel Equipment { get; set; }
        public DateTime RotatingDate { get; set; }
        public int FromDepartmentId { get; set; }
        public virtual DepartmentModel FromDepartment { get; set; }

        public int ToDepartmentId { get; set; }
        public virtual DepartmentModel ToDepartment { get; set; }

        public int? ReviewerLv2Id { get; set; }
        public virtual UserModel ReviewerLv2 { get; set; }
        public int? ReviewerLv3Id { get; set; }
        public virtual UserModel ReviewerLv3 { get; set; }
        public int RotatingRequestId { get; set; }
        public virtual RotatingRequestModel RotatingRequest { get; set; }
	}
}
