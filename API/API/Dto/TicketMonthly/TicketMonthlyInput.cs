using API.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.TicketMonthly
{
    public class TicketMonthlyInput
    {
        public int? Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string LicensePlate { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerImage { get; set; }
        public DateTime? Birthday { get; set; }
        public Status? Gender { get; set; }
        public DateTime LastRegisterDate { get; set; }
        [Column(TypeName = "decimal(15, 0)")]
        public decimal Cost { get; set; }
        public int PromotionId { get; set; }

    }
}
