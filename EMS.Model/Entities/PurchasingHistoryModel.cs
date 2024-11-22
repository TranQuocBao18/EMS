using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.Entities
{
    public class PurchasingHistoryModel
    {
		public int ID { get; set; } // ID của PurchasingHistory
		public int PurchasingRequestID { get; set; } // ID của PurchasingRequest gốc
		public virtual PurchasingRequestModel PurchasingRequest { get; set; }
		public DateTime PurchasedDate { get; set; } // Ngày hoàn thành mua thiết bị
		public string Notes { get; set; } // Ghi chú (nếu cần)
		public int? ReviewerLv2ID { get; set; }  // Người duyệt cấp 2 (Khóa ngoại đến User)
		public virtual UserModel ReviewerLv2 { get; set; }
		public int? ReviewerLv3ID { get; set; }  // Người duyệt cấp 3 (Khóa ngoại đến User)
		public virtual UserModel ReviewerLv3 { get; set; }

		// Danh sách các chi tiết trong lịch sử mua
		public ICollection<PurchasingRequestDetailModel> PurchasingRequestDetails { get; set; }
	}
}
