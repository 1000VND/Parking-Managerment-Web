using API.Data;
using API.Dto;
using API.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

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

        /*[HttpPost("CarIn")]
        public async Task<IActionResult> TakeCar([FromBody]CarDto input)
        {
            //check vé xe có đăng ký vé tháng không
            var checkTicket = await _dataContext.TicketMonthlys.FirstOrDefaultAsync(pl => pl.LicensePlate == input.LicensePlateIn);
            var car = new CarDto
            {
                CreationTime = DateTime.Now,
                IsDelete = Status.No,
                LicensePlateIn = input.LicensePlateIn,
                CarTimeIn = DateTime.Now,
                ImgCarIn = input.ImgCarIn,
                TypeCard = checkTicket != null ? (int)Status.No : (int)Status.Yes, // Yes: Vé tháng, No: Vé ngày
                IsCarParking = (int)CarStatus.CarIn
            };
            return null;
        }

        [HttpGet("CarOut")]
        public async Task<IActionResult> GetCar(string lp)
        {
            var carExist = await _dataContext.Cars.FirstOrDefaultAsync(car => car.LicensePlateIn == lp);
            return null;
        }*/

        [HttpPost("ShortenImage")]
        public async Task<IActionResult> ShortenImage(string input)
        {
            byte[] imageBytes;

            try
            {
                imageBytes = Convert.FromBase64String(input);
            }
            catch (FormatException)
            {
                return BadRequest("Invalid base64 string input");
            }

            // Perform image processing logic on imageBytes here

            return CustomResult(imageBytes); // or return processed image data
        }

    }
}
