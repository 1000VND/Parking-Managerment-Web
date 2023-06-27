using API.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    [Table("CarReports")]
    public class CarReport
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Status IsDelete { get; set; }

        public string CustomerName { get; set; }
        public int UserId { get; set; }
        public string LicensePlate { get; set; }
        public string Reason { get; set; }
        public string Content { get; set; }

    }
}
