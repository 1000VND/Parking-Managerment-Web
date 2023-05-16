using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto
{
    public class CreateEditPromotionDto
    {
        public int? Id { get; set; }
        public string PromotionName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int Discount { get; set; }
    }
}
