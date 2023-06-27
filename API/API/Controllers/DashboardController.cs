using Abp.Dapper.Repositories;
using API.Data;
using API.Dto.Dashboard;
using API.Dto.TicketMonthly;
using API.Interfaces;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class DashboardController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly IDapperRepository _dapperRepository;

        public DashboardController
            (
                DataContext dataContext,
                IDapperRepository dapperRepository

            )
        {
            _dataContext = dataContext;
            _dapperRepository = dapperRepository;
        }

        [HttpPost("CountParkingAccessCar")]
        public Task<IActionResult> CountParkingAccessCar()
        {
            var checkExits = _dataContext.Cars
                .GroupBy(e => e.LicensePlateIn)
                .Select(grp => grp.Count());

            if (checkExits != null)
            {
                return (Task<IActionResult>)CustomResult(checkExits.Count()) ;
            }

            return (Task<IActionResult>)CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }

        [HttpPost("CountAllParkingCar")]
        public Task<IActionResult> CountAllParkingCar()
        {
            var checkExits = _dataContext.Cars
                .GroupBy(e => e.Id)
                .Select(grp => grp.Count());

            if (checkExits != null)
            {
                return (Task<IActionResult>)CustomResult(checkExits.Count());
            }

            return (Task<IActionResult>)CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }

        [HttpGet("CountAllTicketMonthly")]
        public async Task<IActionResult> GetDataStored()
        {
            var dp = new DynamicParameters();

            var data = await Task.FromResult(_dapperRepository.GetAll<CountAllTicketMonthlyDto>("CountAllTicketMonthly", dp, commandType: System.Data.CommandType.StoredProcedure));

            return CustomResult(data);
        }

        [HttpPost("CountAllCarInventory")]
        public Task<IActionResult> CountAllCarInventory()
        {
            var checkExits = _dataContext.Cars
                .Where(h => ((int?)h.IsCarParking) == 2)
                .GroupBy(e => e.Id)
                .Select(grp => grp.Count());

            if (checkExits != null)
            {
                return (Task<IActionResult>)CustomResult(checkExits.Count());
            }

            return (Task<IActionResult>)CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }

        [HttpPost("CountParkingCarByYear")]
        public Task<IActionResult> CountParkingCarByYear(int year)
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
                return (Task<IActionResult>)CustomResult(checkExits);
            }

            return (Task<IActionResult>)CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }

        [HttpGet("RateMaleFemale")]
        public async Task<IActionResult> PercentMaleFemale()
        {
            int countMale = await _dataContext.TicketMonthlys.AsNoTracking()
                .Where(e => e.Gender == Enum.Status.Yes && e.CreationTime.Year == DateTime.Now.Year)
                .CountAsync();

            int countFemale = await _dataContext.TicketMonthlys.AsNoTracking()
                .Where(e => e.Gender == Enum.Status.No && e.CreationTime.Year == DateTime.Now.Year)
                .CountAsync();

            float total = countMale + countFemale;

            float percentMale = total != 0 ? (countMale / total) * 100 : 0;

            float percentFemale = total != 0 ? (countFemale / total) * 100 : 0;

            percentMale = (float)Math.Round(percentMale, 2);

            percentFemale = (float)Math.Round(percentFemale, 2);

            PercentMaleFemaleDto data = new PercentMaleFemaleDto
            {
                PercentMale = percentMale,
                PercentFemale = percentFemale
            };

            return CustomResult(data);
        }
    }
}
