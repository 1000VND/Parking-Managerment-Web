using API.Data;
using API.Dto.TicketMonthly;
using API.Entities;
using API.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class TicketMonthlyController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public TicketMonthlyController
            (
                DataContext dataContext
            )
        {
            _dataContext = dataContext;
        }

        [HttpPost("CreateOrEdit")]
        public async Task<IActionResult> CreateOrEdit(TicketMonthlyInput input)
        {
            if (input.Id == null) return await Create(input);
            else return await Update(input);
        }

        private async Task<IActionResult> Create( TicketMonthlyInput input)
        {
            var ticketMonthly = new TicketMonthly
            {
                LicensePlate = input.LicensePlate,
                PhoneNumber = input.PhoneNumber,
                CustomerName = input.CustomerName,
                CustomerAddress = input.CustomerAddress,
                CustomerImgage = input.CustomerImgage,
                Birthday = input.Birthday,
                Gender = input.Gender,
                CustomerPoint = input.CustomerPoint,
                LastRegisterDate = input.LastRegisterDate,
                CreationTime = DateTime.Now
            };
            _dataContext.TicketMonthlys.Add(ticketMonthly);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Add success!");
        }
        private async Task<IActionResult> Update( TicketMonthlyInput input)
        {
            var dataExit = await _dataContext.TicketMonthlys.FindAsync(input.Id);
            dataExit.PhoneNumber = input.PhoneNumber;
            dataExit.CustomerAddress = input.CustomerAddress;
            dataExit.CustomerImgage = input.CustomerImgage;
            dataExit.LastModificationTime = DateTime.Now;

            _dataContext.TicketMonthlys.Update(dataExit);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Update success!");
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllTicketMonthly()
        {
            var data = await (from t in _dataContext.TicketMonthlys
                              where t.IsDelete == 0
                              orderby t.CreationTime descending
                              select new TicketMonthlyDto
                              {
                                  Id = t.Id,
                                  LicensePlate = t.LicensePlate,
                                  PhoneNumber = t.PhoneNumber,
                                  CustomerName = t.CustomerName,
                                  CustomerAddress = t.CustomerAddress,
                                  CustomerImgage = t.CustomerImgage,
                                  Birthday = t.Birthday,
                                  Gender = t.Gender,
                                  CustomerPoint = t.CustomerPoint,
                                  LastRegisterDate = t.LastRegisterDate,
                                  CreationTime = t.CreationTime
                              }).ToListAsync();

            return CustomResult(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dataExits = await _dataContext.TicketMonthlys.FindAsync(id);

            if (dataExits != null && dataExits.IsDelete == Status.No)
            {
                dataExits.IsDelete = Status.Yes;
                dataExits.LastModificationTime = DateTime.Now;
                _dataContext.Entry(dataExits).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                return CustomResult(dataExits);
            }

            return NotFound();
        }
    }
}
