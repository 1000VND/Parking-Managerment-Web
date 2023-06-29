using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.Report
{
    public class CarReportDto
    {
        public string userName { get; set; }
        public string customerName { get; set; }
        public string customerBirthday { get; set; }
        public string identityCard { get; set; }
        public string licensePlate { get; set; }
        public string customerNumber { get; set; }
    }
}
