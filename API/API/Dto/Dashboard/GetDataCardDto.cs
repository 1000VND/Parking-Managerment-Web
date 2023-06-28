using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.Dashboard
{
    public class GetDataCardDto
    {
        public int CarNumber { get; set; }
        public int CarToday { get; set; }
        public int MoneyToday { get; set; }
    }
}
