using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Entities
{
    public class PurchasingRequestDetailModel
    {
		public int ID { get; set; }
		public int PurchasingRequestId { get; set; }
		public virtual PurchasingRequestModel PurchasingRequest { get; set; }
		public int EquipmentTypeId { get; set; }
		public virtual EquipmentTypeModel EquipmentType { get; set; }
		public int Quantity { get; set; }
	}
}
