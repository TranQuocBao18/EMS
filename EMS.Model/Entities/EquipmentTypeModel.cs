using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Entities
{
	public class EquipmentTypeModel
	{
		public int ID { get; set; }
		public string EquipmentTypeName { get; set; }

		// Thuộc tính điều hướng tới danh sách thiết bị
		public virtual ICollection<EquipmentModel> Equipments { get; set; } = new List<EquipmentModel>();
	}
}
