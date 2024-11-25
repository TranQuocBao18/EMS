using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Entities
{
	public class MaintenanceHistoryModel
	{
		public int ID { get; set; }
		public int EquipmentId { get; set; } // Khóa ngoại đến bảng Equipment
		public virtual EquipmentModel Equipment { get; set; }

		public int? ReviewerLv2ID { get; set; }  // Người duyệt cấp 2 (Khóa ngoại đến User)
		public virtual UserModel ReviewerLv2 { get; set; }
		public int? ReviewerLv3ID { get; set; }  // Người duyệt cấp 3 (Khóa ngoại đến User)
		public virtual UserModel ReviewerLv3 { get; set; }

		public DateTime? MaintenanceStartDate { get; set; }
		public DateTime? MaintenanceEndDate { get; set; }
		public int MaintenanceRequestId { get; set; }
		public virtual MaintenanceRequestModel MaintenanceRequest { get; set; }
		public string Notes { get; set; }
	}
}
