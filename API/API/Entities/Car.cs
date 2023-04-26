using API.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Car
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Status IsDelete { get; set; }

        public string LicensePlateIn { get; set; }
        public string LicensePlateOut { get; set; }
        public DateTime? CarTimeIn { get; set; }
        public DateTime? CarTimeOut { get; set; }
        public string ImgCarIn { get; set; }
        public string ImgCarOut { get; set; }
        public int TypeCard { get; set; }
        public Status? IsCarParking { get; set; }
    }
}
