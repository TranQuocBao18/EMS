using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS.Model.DTOs
{
    public class ApproveRequestDto
    {
        public int RequestId { get; set; }
        public bool AcceptanceStatus { get; set; }  // true nếu duyệt, false nếu từ chối
        public string Reason { get; set; }  // Lý do nếu từ chối
        public int ReviewerId { get; set; }  // ID người duyệt
    }
}
