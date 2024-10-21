using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Entities
{
	public enum EquipmentStatus
	{
		DangHoatDong = 1,
		DangBaoTri = 2,
		DangSuaChua = 3
	}
	public class EquipmentModel
	{
		public int ID { get; set; }
		public string EquipmentName { get; set; }
		public string EquipmentModelCode { get; set; }
		public string EquipmentSerial { get; set; }
		public string EquipmentTypeDes { get; set; }
		public double Price { get; set; }
		public DateOnly PurchaseDay { get; set; }
		public EquipmentStatus Status { get; set; }
		public DateOnly ExpireDay { get; set; }
		// Khóa ngoại để liên kết với Department
		public int DepartmentId { get; set; }
		public virtual DepartmentModel Department { get; set; }  // Thuộc tính điều hướng
		// Khóa ngoại để liên kết với EquipmentType
		public int EquipmentTypeId { get; set; }
		public virtual EquipmentTypeModel EquipmentType { get; set; }  // Thuộc tính điều hướng
	}
}
