using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Entities
{
	public class RotatingRequestModel
	{
		public int Id { get; set; }  // Khóa chính

		// Người tạo request
		public int UserId { get; set; }  // Khóa ngoại đến bảng User
		public virtual UserModel User { get; set; }

		// Thông tin thiết bị
		public int EquipmentTypeId { get; set; } // Khóa ngoại đến bảng Equipment
		public virtual EquipmentTypeModel EquipmentType { get; set; }

		// Phòng ban liên quan
		public int DepartmentId { get; set; } // Khóa ngoại đến bảng Department
		public virtual DepartmentModel Department { get; set; }

		// Lý do yêu cầu
		public string RequestReason { get; set; }

		// Trạng thái duyệt cấp 2
		public bool? AcceptanceLv2Status { get; set; }  // Có thể null (chưa duyệt)
		public string ReasonLv2 { get; set; }  // Lý do nếu bị từ chối cấp 2
		public int ReviewerLv2Id { get; set; }  // Người duyệt cấp 2 (Khóa ngoại đến User)
		public virtual UserModel ReviewerLv2User { get; set; }

		// Trạng thái duyệt cấp 3
		public bool? AcceptanceLv3Status { get; set; }  // Có thể null (chưa duyệt)
		public string ReasonLv3 { get; set; }  // Lý do nếu bị từ chối cấp 3
		public int ReviewerLv3Id { get; set; }  // Người duyệt cấp 3 (Khóa ngoại đến User)
		public virtual UserModel ReviewerLv3User { get; set; }

	}
}
