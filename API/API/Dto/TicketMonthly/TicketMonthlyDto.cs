using API.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.TicketMonthly
{
    public class TicketMonthlyDto
    {
        public int? Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public string LicensePlate { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerImgage { get; set; }
        public DateTime? Birthday { get; set; }
        public Status? Gender { get; set; }
        public int CustomerPoint { get; set; }
        public DateTime LastRegisterDate { get; set; }


    }
}
