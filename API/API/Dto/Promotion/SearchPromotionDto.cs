using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.Promotion
{
    public class SearchPromotionDto
    {
        public string PromotionName { get; set; }
        public string LicensePlate { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
