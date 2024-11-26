using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Entities
{
	public enum EquipmentStatus
	{
		DangSuDung = 1,
		KhongSuDung = 2,
		DangBaoTri = 3
	}
	public class EquipmentModel
	{
		public int ID { get; set; }
		public string EquipmentName { get; set; }
		public string EquipmentModelCode { get; set; }
		public string EquipmentSerial { get; set; }

		// Khóa ngoại để liên kết với EquipmentType
		public int EquipmentTypeId { get; set; }
		public virtual EquipmentTypeModel EquipmentType { get; set; }  // Thuộc tính điều hướng

		public double Price { get; set; }
		public DateOnly PurchaseDay { get; set; }
		public EquipmentStatus Status { get; set; }
		public DateOnly ExpireDay { get; set; }

		// Khóa ngoại để liên kết với Department
		public int DepartmentId { get; set; } = 18;
		public virtual DepartmentModel Department { get; set; }  // Thuộc tính điều hướng
		
		// Khóa ngoại để liên kết với User
		public int? UserId { get; set; }
		public virtual UserModel User { get; set; } // Thuộc tính điều hướng
	}
}
