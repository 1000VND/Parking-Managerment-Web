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

            int numOfMonths = (input.LastRegisterDate.Year - DateTime.Now.Year) * 12 + (input.LastRegisterDate.Month - DateTime.Now.Month);

            //lấy dữ liệu cho trường hợp chưa đăng ký lần nào
            var checkCarExist = await _dataContext.TicketMonthlys.AsNoTracking().FirstOrDefaultAsync(e => e.LicensePlate == input.LicensePlate && e.IsDelete == 0);

            //lấy dữ liệu cho trường hợp quay lại đăng ký
            var checkCarExist1 = await _dataContext.TicketMonthlys.AsNoTracking().OrderBy(e => e.Id).FirstOrDefaultAsync(e => e.LicensePlate == input.LicensePlate && e.IsDelete == Status.Yes);

            TicketMonthly ticketMonthly = new TicketMonthly
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
            var userIdnew = await _dataContext.TicketMonthlys.AddAsync(ticketMonthly);
            await _dataContext.SaveChangesAsync();

            if (input.PromotionId != 0)
            {
                if (checkCarExist1 != null)
                {
                    var getPromoByPlate = await (from pd in _dataContext.PromotionDetails.Where(e => e.IsDelete == 0 && e.Status == 0)
                                                 join p in _dataContext.Promotions.Where(e => e.IsDelete == 0 && DateTime.Now.Date >= e.FromDate.Value.Date
                                                 && DateTime.Now.Date <= e.ToDate.Value.Date) on pd.PromotionId equals p.Id into pJoined
                                                 from lj in pJoined
                                                 join t in _dataContext.TicketMonthlys.Where(e => e.IsDelete == Status.Yes) on pd.UserId equals t.Id into tJoined
                                                 from ljt in tJoined
                                                 where pd.UserId == checkCarExist1.Id
                                                 select new
                                                 {
                                                     PromotionDetailId = pd.Id,
                                                     PromotionId = lj.Id,
                                                     Status = pd.Status
                                                 }).ToListAsync();
                    var findPromotion = getPromoByPlate.Find(f => f.PromotionId == input.PromotionId);
                    if (findPromotion.Status == 0)
                    {
                        var promoDetail = await _dataContext.PromotionDetails.FindAsync(findPromotion.PromotionDetailId);
                        promoDetail.Status = 1;
                        promoDetail.LastModificationTime = DateTime.Now;

                        _dataContext.PromotionDetails.Update(promoDetail);
                        await _dataContext.SaveChangesAsync();
                    }
                }
                else
                {
                    if (checkCarExist == null)
                    {
                        var promoDetail = new PromotionDetail
                        {
                            CreationTime = DateTime.Now,
                            IsDelete = Status.No,
                            PromotionId = input.PromotionId,
                            UserId = userIdnew.Entity.Id,
                            Status = 1
                        };

                        await _dataContext.PromotionDetails.AddAsync(promoDetail);
                        await _dataContext.SaveChangesAsync();
                    }
                }
            }


            return CustomResult("Add success!");
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
            var dataExist = await _dataContext.TicketMonthlys.FindAsync(id);

            var dataExistPromotionDetail = await _dataContext.PromotionDetails.Where(e => e.UserId == id).ToListAsync();

            if (dataExist != null && dataExist.IsDelete == Status.No)
            {
                dataExist.IsDelete = Status.Yes;
                dataExist.LastModificationTime = DateTime.Now;
                _dataContext.Entry(dataExist).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();

                if (dataExistPromotionDetail.Count != 0)
                {
                    foreach (var data in dataExistPromotionDetail)
                    {
                        data.IsDelete = Status.Yes;
                        data.LastModificationTime = DateTime.Now;
                        _dataContext.Entry(data).State = EntityState.Modified;
                        await _dataContext.SaveChangesAsync();
                    }
                }
                return CustomResult(dataExist);
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

        
    }
}
