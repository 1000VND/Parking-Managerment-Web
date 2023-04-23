using API.Data;
using API.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class CarController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public CarController(
            DataContext dataContext
            )
        {
            _dataContext = dataContext;
        }

        [HttpPost("CarIn")]
        public async Task<IActionResult> TakeCar([FromBody]CarDto input)
        {
            //var carExistParking = await _dataContext.Cars.FirstOrDefaultAsync(c=>c.)
            return null;
        }

        [HttpGet("CarOut")]
        public async Task<IActionResult> GetCar(string lp)
        {
            var carExist = await _dataContext.Cars.FirstOrDefaultAsync(car => car.LicensePlateIn == lp);
            return null;
        }
    }
}
