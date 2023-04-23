using API.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class CarDto
    {
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Status IsDelete { get; set; }

        public string ImageCarIn { get; set; }
        public string ImageCarOut { get; set; }
        public string LicensePlateIn { get; set; }
        public string LicensePlateOut { get; set; }
        public DateTime? CarInTime { get; set; }
        public DateTime? CarOutTime { get; set; }
        public string TypeCustomer { get; set; }
        public int CardId { get; set; }
        public Status? IsCarParking { get; set; }
    }
}
