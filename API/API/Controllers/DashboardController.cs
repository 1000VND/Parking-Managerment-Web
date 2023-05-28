using Abp.Dapper.Repositories;
using API.Data;
using API.Dto.TicketMonthly;
using API.Interfaces;
using Dapper;
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

        [HttpGet("CountAllTicketMonthly")]
        public async Task<IActionResult> GetDataStored(int year)
        {
            var dp = new DynamicParameters();

            dp.Add("@year", year, System.Data.DbType.Int32);
            var data = await Task.FromResult(_dapperRepository.GetAll<CountAllTicketMonthlyDto>("CountAllTicketMonthly", dp, commandType: System.Data.CommandType.StoredProcedure));

            return CustomResult(data);
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
