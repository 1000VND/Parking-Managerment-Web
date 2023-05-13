using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.Promotion
{
    public class PromotionDetailInputDto
    {
        public int? Id { get; set; }
        public int PromotionId { get; set; }
        public int UserId { get; set; }
    }
}
