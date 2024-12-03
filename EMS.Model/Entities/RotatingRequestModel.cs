using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Entities
{
	public enum RequestStatus
	{
		Pending = 1,
		InProgress = 2,
		Completed = 3
	}
	public class RotatingRequestModel
	{
		public int ID { get; set; }  // Khóa chính

		// Người tạo request
		public int UserId { get; set; }  // Khóa ngoại đến bảng User
		public virtual UserModel User { get; set; }

		// Thông tin thiết bị
		public int EquipmentId { get; set; } // Khóa ngoại đến bảng Equipment
		public virtual EquipmentModel Equipment { get; set; }

		public int FromDepartmentId { get; set; } 
		public virtual DepartmentModel FromDepartment { get; set; }
		public int ToDepartmentId { get; set; } 
		public virtual DepartmentModel ToDepartment { get; set; }

		// Lý do yêu cầu
		public string RequestReason { get; set; }

		// Trạng thái duyệt cấp 2
		public bool? AcceptanceLv2Status { get; set; }  // Có thể null (chưa duyệt)
		public string ReasonLv2 { get; set; }  // Lý do nếu bị từ chối cấp 2
		public int? ReviewerLv2ID { get; set; }  // Người duyệt cấp 2 (Khóa ngoại đến User)
		public virtual UserModel ReviewerLv2 { get; set; }

		// Trạng thái duyệt cấp 3
		public bool? AcceptanceLv3Status { get; set; }  // Có thể null (chưa duyệt)
		public string ReasonLv3 { get; set; }  // Lý do nếu bị từ chối cấp 3
		public int? ReviewerLv3ID { get; set; }  // Người duyệt cấp 3 (Khóa ngoại đến User)
		public virtual UserModel ReviewerLv3 { get; set; }
		public RequestStatus RequestStatus { get; set; } // Pending, InProgress, Completed

	}
}
