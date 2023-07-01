using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.Report.CarReport
{
    public class CarReportInputDto
    {
        public int? Id { get; set; }
        public string CustomerName { get; set; }
        public int UserId { get; set; }
        public string LicensePlate { get; set; }
        public string Reason { get; set; }
        public string Content { get; set; }
        public string CustomerNumber { get; set; }
        public DateTime? CustomerBirthday { get; set; }
        public string IdentityCard { get; set; }
    }
}
