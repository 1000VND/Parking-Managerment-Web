using API.Data;
using API.Dto;
using API.Dto.Promotion;
using API.Dto.TicketMonthly;
using API.Entities;
using API.Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class PromotionController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public PromotionController
            (
                DataContext dataContext
            )
        {
            _dataContext = dataContext;
        }
        [HttpPost("CreateOrEdit")]
        public async Task<IActionResult> CreateOrEdit(CreateEditPromotionDto input)
        {
            if (input.Id == null) return await Create(input);
            else return await Update(input);
        }

        private async Task<IActionResult> Create(CreateEditPromotionDto input)
        {
            var checkExist = await _dataContext.Promotions.FirstOrDefaultAsync(promotion => promotion.PromotionName == input.PromotionName && promotion.IsDelete == 0);

            if (checkExist != null) return CustomResult("Promotion Name is taken", System.Net.HttpStatusCode.BadRequest);

            var promotion = new Promotion
            {
                PromotionName = input.PromotionName,
                FromDate = input.FromDate,
                ToDate = input.ToDate,
                Discount = input.Discount,
                Point = input.Point,
                IsDelete = Status.No,
                CreationTime = DateTime.Now
            };
            _dataContext.Promotions.Add(promotion);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Add success!");
        }

        private async Task<IActionResult> Update(CreateEditPromotionDto input)
        {
            var dataExit = await _dataContext.Promotions.FindAsync(input.Id);
            dataExit.PromotionName = input.PromotionName;
            dataExit.FromDate = input.FromDate;
            dataExit.ToDate = input.ToDate;
            dataExit.Discount = input.Discount;
            dataExit.Point = input.Point;
            dataExit.LastModificationTime = DateTime.Now;

            _dataContext.Promotions.Update(dataExit);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Update success!");
        }
        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAllPromotion(SearchPromotionDto input)
        {
            try
            {
                var data = await (from p in _dataContext.Promotions
                                  join pd in _dataContext.PromotionDetails on p.Id equals pd.PromotionId into ProDetailJoined
                                  from ljPd in ProDetailJoined.DefaultIfEmpty()
                                  join tm in _dataContext.TicketMonthlys on ljPd.UserId equals tm.Id into tiketJoined
                                  from ljTm in tiketJoined.DefaultIfEmpty()
                                  where ((string.IsNullOrWhiteSpace(input.PromotionName) || input.PromotionName.Contains(p.PromotionName)) &&
                                       (string.IsNullOrWhiteSpace(input.LicensePlate) || input.LicensePlate.Contains(ljTm.LicensePlate)) &&
                                       ((!input.FromDate.HasValue || input.FromDate.Value.Date <= p.FromDate.Value.Date) &&
                                       (!input.ToDate.HasValue || input.ToDate.Value.Date >= p.ToDate.Value.Date)))
                                  && p.IsDelete == 0
                                  orderby p.CreationTime descending
                                  select new GetAllPromotionDto
                                  {
                                      Id = p.Id,
                                      PromotionName = p.PromotionName,
                                      CreationTime = p.CreationTime,
                                      FromDate = p.FromDate.Value,
                                      ToDate = p.ToDate.Value,
                                      Discount = p.Discount,
                                      Point = p.Point,
                                      LastModificationTime = p.LastModificationTime
                                  }).ToListAsync();

                return CustomResult(data);
            }
            catch (Exception ex)
            {
                return CustomResult(ex);
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dataExits = await _dataContext.Promotions.FindAsync(id);

            if (dataExits != null && dataExits.IsDelete == Status.No)
            {
                dataExits.IsDelete = Status.Yes;
                dataExits.LastModificationTime = DateTime.Now;
                _dataContext.Entry(dataExits).State = EntityState.Modified;
                await _dataContext.SaveChangesAsync();
                return CustomResult(dataExits);
            }

            return CustomResult("Promotion not exits", System.Net.HttpStatusCode.NotFound);
        }

        [HttpGet("PromotionDetail")]
        public async Task<IActionResult> GetPromotionDetail(int id)
        {
            var promotion = await (from p in _dataContext.Promotions
                                   join pd in _dataContext.PromotionDetails on p.Id equals pd.PromotionId into pdJoined
                                   from pdj in pdJoined.DefaultIfEmpty()
                                   join tm in _dataContext.TicketMonthlys on pdj.UserId equals tm.Id
                                   where p.Id == id && p.IsDelete == 0
                                   select new 
                                   {
                                       Id = p.Id,
                                       CustomerName = tm.CustomerName,
                                       PhoneNumber = tm.PhoneNumber,
                                       LicensePlate = tm.LicensePlate
                                   }).ToListAsync();
            return CustomResult(promotion);
        }

    }
}
