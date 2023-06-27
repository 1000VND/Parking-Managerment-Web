using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.Report.CarReport
{
    public class CarReportGetAllDto
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }

        public string CustomerName { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string LicensePlate { get; set; }
        public string Reason { get; set; }
        public string Content { get; set; }
    }
}
