using API.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class DashboardController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public DashboardController
            (
                DataContext dataContext
            )
        {
            _dataContext = dataContext;
        }

        [HttpPost("CountParkingAccessCar")]
        public async Task<IActionResult> CountParkingAccessCar()
        {
            var checkExits = _dataContext.Cars
                .GroupBy(e => e.LicensePlateIn)
                .Select(grp => grp.Count());

            if (checkExits != null)
            {
                return CustomResult(checkExits.Count()) ;
            }

            return CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }

        [HttpPost("CountAllParkingCar")]
        public async Task<IActionResult> CountAllParkingCar()
        {
            var checkExits = _dataContext.Cars
                .GroupBy(e => e.Id)
                .Select(grp => grp.Count());

            if (checkExits != null)
            {
                return CustomResult(checkExits.Count());
            }

            return CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }

        [HttpPost("CountAllTicketMonthly")]
        public async Task<IActionResult> CountAllTicketMonthly()
        {
            var checkExits = _dataContext.TicketMonthlys
                .GroupBy(e => e.Id)
                .Select(grp => grp.Count());

            if (checkExits != null)
            {
                return CustomResult(checkExits.Count());
            }

            return CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }

        [HttpPost("CountAllCarInventory")]
        public async Task<IActionResult> CountAllCarInventory()
        {
            var checkExits = _dataContext.Cars
                .Where(h => ((int?)h.IsCarParking) == 2)
                .GroupBy(e => e.Id)
                .Select(grp => grp.Count());

            if (checkExits != null)
            {
                return CustomResult(checkExits.Count());
            }

            return CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }

        [HttpPost("CountParkingCarByYear")]
        public async Task<IActionResult> CountParkingCarByYear(int year)
        {
            var checkExits = _dataContext.Cars
                .Where(h => h.CreationTime.Year == year)
                .GroupBy(e => e.CreationTime.Month)
                .Select(g => new
                {
                    month = g.Key,
                    count = g.Select(e => e.Id).Count()
                }).ToList();

            if (checkExits != null)
            {
                return CustomResult(checkExits);
            }

            return CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }
    }
}
