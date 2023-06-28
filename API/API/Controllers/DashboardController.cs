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

        [HttpGet("CountAllTicketMonthly")]
        public async Task<IActionResult> CountAllTicketMonthly()
        {
            var dp = new DynamicParameters();

            var data = await Task.FromResult(_dapperRepository.GetAll<CountAllTicketMonthlyDto>("CountAllTicketMonthly", dp, commandType: System.Data.CommandType.StoredProcedure));

            return CustomResult(data);
        }

        [HttpGet("RateMaleFemale")]
        public async Task<IActionResult> RateMaleFemale()
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

        [HttpGet("GetDataCard")]
        public async Task<IActionResult> GetDataCard()
        {
            var dp = new DynamicParameters();

            var data = await Task.FromResult(_dapperRepository.GetAll<GetDataCardDto>("GetDataCard", dp, commandType: System.Data.CommandType.StoredProcedure));

            return CustomResult(data);
        }
    }
}
