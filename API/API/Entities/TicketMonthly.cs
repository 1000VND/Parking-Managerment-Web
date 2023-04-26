using API.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class TicketMonthly
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Status IsDelete { get; set; }

        public string LicensePlate { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerImgage { get; set; }
        public DateTime? Birthday { get; set; }
        public Status? Gender { get; set; }
        public int CustomerPoint { get; set; }
    }
}
