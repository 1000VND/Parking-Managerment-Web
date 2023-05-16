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

        private async Task<IActionResult> Create(TicketMonthlyInput input)
        {
            var checkExits = await _dataContext.TicketMonthlys.FirstOrDefaultAsync(ticket => ticket.LicensePlate == input.LicensePlate);

            var checkExits1 = await _dataContext.TicketMonthlys.AsNoTracking()
                                     .Where(ticket => ticket.LicensePlate == input.LicensePlate && (int)ticket.IsDelete == 1)
                                     .OrderByDescending(e => e.LastRegisterDate).Select(t => t).FirstOrDefaultAsync();

            int numOfMonths = (input.LastRegisterDate.Year - DateTime.Now.Year) * 12 + (input.LastRegisterDate.Month - DateTime.Now.Month);

            var promotion = await _dataContext.Promotions.FirstOrDefaultAsync(e => e.Id == input.PromotionId);

            if (checkExits == null)
            {
                //Khách hàng chưa đăng ký vé tháng lần nào
                var ticketMonthly = new TicketMonthly
                {
                    LicensePlate = input.LicensePlate,
                    PhoneNumber = input.PhoneNumber,
                    CustomerName = input.CustomerName,
                    CustomerAddress = input.CustomerAddress,
                    CustomerImgage = input.CustomerImage,
                    Birthday = input.Birthday,
                    Gender = input.Gender,
                    LastRegisterDate = input.LastRegisterDate,
                    CreationTime = DateTime.Now,
                    Cost = input.Cost
                };

                _dataContext.TicketMonthlys.Add(ticketMonthly);
                await _dataContext.SaveChangesAsync();
                return CustomResult("Add success!");
            }
            else
            {
                //Khách hàng đã đăng ký vé tháng ít nhất 1 lần
                var ticketMonthly = new TicketMonthly
                {
                    LicensePlate = input.LicensePlate,
                    PhoneNumber = input.PhoneNumber,
                    CustomerName = input.CustomerName,
                    CustomerAddress = input.CustomerAddress,
                    CustomerImgage = input.CustomerImage,
                    Birthday = input.Birthday,
                    Gender = input.Gender,
                    LastRegisterDate = input.LastRegisterDate,
                    CreationTime = DateTime.Now,
                    Cost = input.Cost * numOfMonths
                };
                _dataContext.TicketMonthlys.Add(ticketMonthly);
                await _dataContext.SaveChangesAsync();
                return CustomResult("Add success!");
            }
            return CustomResult("Add fail!");
        }
        private async Task<IActionResult> Update(TicketMonthlyInput input)
        {
            var dataExit = await _dataContext.TicketMonthlys.FindAsync(input.Id);
            dataExit.PhoneNumber = input.PhoneNumber;
            dataExit.CustomerAddress = input.CustomerAddress;
            dataExit.CustomerImgage = input.CustomerImage;
            dataExit.LastModificationTime = DateTime.Now;
            dataExit.LastRegisterDate = input.LastRegisterDate;

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
                                  LastRegisterDate = (DateTime)t.LastRegisterDate,
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

        #region -- Kiểm tra khách hàng đã đăng ký bao giờ chưa
        [HttpGet("CheckRegister")]
        public async Task<IActionResult> CheckRegisterTicket(string plate)
        {
            var checkExits = await _dataContext.TicketMonthlys.FirstOrDefaultAsync(ticket => ticket.LicensePlate == plate);
            var checkExits1 = await _dataContext.TicketMonthlys.AsNoTracking()
                                     .Where(ticket => ticket.LicensePlate == plate && (int)ticket.IsDelete == 1)
                                     .OrderByDescending(e => e.LastRegisterDate).Select(t => t).FirstOrDefaultAsync();
            var checkExits2 = await _dataContext.TicketMonthlys.FirstOrDefaultAsync(e => e.LicensePlate == plate && e.IsDelete == 0);
            if (checkExits != null)
            {
                if (checkExits2 != null)
                {
                    return CustomResult("Not yet expired, the monthly ticket cannot be registered", System.Net.HttpStatusCode.BadRequest);
                }
                return CustomResult(checkExits1);
            }
            else
            {
                return CustomResult("New customer");
            }

        }
        #endregion

        #region -- lấy biển số xe
        [HttpGet("GetCarExits")]
        public async Task<IActionResult> GetCarExits()
        {
            var checkExits = await (from t in _dataContext.TicketMonthlys.AsNoTracking().Where(ticket => (int)ticket.IsDelete == 1)
                                    group t.LicensePlate by t.LicensePlate into g
                                    select new
                                    {
                                        LicensePlate = g.Key
                                    }).ToListAsync();
            return CustomResult(checkExits);
        }
        #endregion
    }
}
