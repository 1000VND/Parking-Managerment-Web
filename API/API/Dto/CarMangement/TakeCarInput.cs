using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dto.CarMangement
{
    public class TakeCarInput
    {
        public string LicensePlateIn { get; set; }
        public string ImgCarIn { get; set; }
        public int TypeCard { get; set; }
    }
}
