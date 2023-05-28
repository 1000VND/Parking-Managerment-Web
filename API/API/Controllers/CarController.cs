using API.Data;
using API.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using API.Dto.CarMangement;
using API.Entities;
using API.Dto;

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
        public async Task<IActionResult> TakeCar(TakeCarInput input)
        {
            var car = new Car
            {
                CreationTime = DateTime.Now,
                IsDelete = Status.No,
                LicensePlateIn = input.LicensePlateIn,
                CarTimeIn = DateTime.Now,
                ImgCarIn = input.ImgCarIn,
                TypeCard = input.TypeCard, // 1: Vé tháng, 0: Vé ngày
                IsCarParking = (int)CarStatus.CarIn
            };

            _dataContext.Cars.Add(car);
            await _dataContext.SaveChangesAsync();

            return CustomResult(car);
        }


        [HttpPost("CheckTypeCustomer")]
        public async Task<IActionResult> CheckCar(string plate)
        {
            plate = plate?.Trim();
            if (string.IsNullOrEmpty(plate))
            {
                return CustomResult("No license plate entered!");
            }

            var checkTicket = await _dataContext.TicketMonthlys.FirstOrDefaultAsync(pl => pl.LicensePlate == plate && pl.IsDelete == 0);
            return CustomResult(checkTicket != null ? "Monthly customer" : "Current customers");
        }

        [HttpPost("CarOut")]
        public async Task<IActionResult> GetCar(CarDto input)
        {
            var carExist = await _dataContext.Cars.FirstOrDefaultAsync(car => car.Id == input.Id && car.IsDelete == 0);
            if (carExist != null)
            {


                carExist.LastModificationTime = DateTime.Now;
                carExist.LicensePlateOut = input.LicensePlateOut;
                carExist.CarTimeOut = DateTime.Now;
                carExist.IsCarParking = Status.Yes;
                carExist.ImgCarOut = input.ImgCarOut;
                for (int i = 0; i <= 13; i++)
                {

                    DateTime nextDayIn1 = carExist.CarTimeIn.Value.AddDays(i);
                    DateTime nextDayAtMidnightIn1 = nextDayIn1.Date; //
                    DateTime nextDayOut1 = carExist.CarTimeOut.Value.AddDays(0);
                    DateTime nextDayAtMidnightOut1 = nextDayOut1.Date; //

                    DateTime startDate = nextDayAtMidnightIn1;
                    DateTime endDate = nextDayAtMidnightOut1;
                    TimeSpan duration = endDate.Subtract(startDate);

                    int numOfDays = duration.Days;

                    if (numOfDays == 0)
                    {
                        carExist.Cost = 20000 + 20000 * i;
                    }
                }
                _dataContext.Update(carExist);
                await _dataContext.SaveChangesAsync();

            }



            return CustomResult(carExist);
        }

        [HttpGet("CheckLPCarOut")]
        public async Task<IActionResult> CheckLicensePlate(string plate)
        {
            var carsLists = _dataContext.Cars.OrderByDescending(x => x.CarTimeIn);
            var carExits = await carsLists.FirstOrDefaultAsync(e => e.LicensePlateIn == plate && e.IsCarParking == 0);
            return CustomResult(carExits);
        }
    }

}
