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

        #region -- Thêm sửa chương trình khuyến mãi
        [HttpPost("CreateOrEdit")]
        public async Task<IActionResult> CreateOrEdit(CreateEditPromotionDto input)
        {
            if (input.Id == null) return await Create(input);
            else return await Update(input);
        }

        private async Task<IActionResult> Create(CreateEditPromotionDto input)
        {
            var checkExist = await _dataContext.Promotions.FirstOrDefaultAsync(promotion => promotion.PromotionName == input.PromotionName.Trim() && promotion.IsDelete == 0);

            if (checkExist != null) return CustomResult("Promotion name is taken", System.Net.HttpStatusCode.BadRequest);

            var promotion = new Promotion
            {
                PromotionName = input.PromotionName.Trim(),
                FromDate = input.FromDate,
                ToDate = input.ToDate,
                Discount = input.Discount,
                IsDelete = Status.No,
                CreationTime = DateTime.Now,
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
            dataExit.LastModificationTime = DateTime.Now;

            _dataContext.Promotions.Update(dataExit);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Update success!");
        }
        #endregion

        #region -- Tìm kiếm chương trình khuyến mãi
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
                                  where ((string.IsNullOrWhiteSpace(input.PromotionName) || input.PromotionName == p.PromotionName) &&
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
                                      LastModificationTime = p.LastModificationTime
                                  }).ToListAsync();

                return CustomResult(data);
            }
            catch (Exception ex)
            {
                return CustomResult(ex);
            }
        }
        #endregion

        #region -- Lấy dữ liệu chương trình khuyến mãi dựa ngày hiện tại
        [HttpGet("GetPromotionByNow")]
        public async Task<IActionResult> FindPromotionByDate()
        {
            var checkExits = await _dataContext.Promotions.AsNoTracking()
                .Where(e => e.IsDelete == 0 && 
                DateTime.Now.Date >= e.FromDate.Value.Date && DateTime.Now.Date <= e.ToDate.Value.Date).ToListAsync();

            if (checkExits != null)
            {
                return CustomResult(checkExits);
            }
            return CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }
        #endregion

        #region -- Lấy dữ liệu chương trình khuyến mãi dựa vào Id
        [HttpGet("GetDataById")]
        public async Task<IActionResult> FindPromotion(int id)
        {
            var checkExits = await _dataContext.Promotions.FirstOrDefaultAsync(e => e.Id == id);

            if (checkExits != null)
            {
                return CustomResult(checkExits);
            }
            return CustomResult("Not Found", System.Net.HttpStatusCode.NotFound);
        }
        #endregion

        #region -- Xóa chương trình khuyến mãi
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
        #endregion

        #region -- Lấy dữ liệu chi tiết của chương trình khuyến mãi
        [HttpGet("PromotionDetail")]
        public async Task<IActionResult> GetPromotionDetail(int id)
        {
            var promotion = await (from p in _dataContext.Promotions
                                   join pd in _dataContext.PromotionDetails on p.Id equals pd.PromotionId into pdJoined
                                   from pdj in pdJoined.DefaultIfEmpty()
                                   join tm in _dataContext.TicketMonthlys on pdj.UserId equals tm.Id
                                   where p.Id == id && pdj.IsDelete == 0
                                   select new 
                                   {
                                       Id = pdj.Id,
                                       CustomerName = tm.CustomerName,
                                       PhoneNumber = tm.PhoneNumber,
                                       LicensePlate = tm.LicensePlate,
                                       Status = pdj.Status
                                   }).ToListAsync();
            return CustomResult(promotion);
        }
        #endregion

        #region -- Thêm sửa thông tin chi tiết của chương trình khuyến mãi
        [HttpPost("CreateEditPromoDetail")]
        public async Task<IActionResult> CreateEditDetail(PromotionDetailInputDto input)
        {
            if (input.Id == null) return await CreateDetail(input);
            else return await UpdateDetail(input);
        }

        private async Task<IActionResult> CreateDetail(PromotionDetailInputDto input)
        {
            var promotionDetail = new PromotionDetail
            {
                PromotionId = input.PromotionId,
                UserId = input.UserId,
                IsDelete = Status.No,
                CreationTime = DateTime.Now
            };
            _dataContext.PromotionDetails.Add(promotionDetail);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Add success!");
        }

        private async Task<IActionResult> UpdateDetail(PromotionDetailInputDto input)
        {
            var dataExit = await _dataContext.PromotionDetails.FindAsync(input.Id);
            dataExit.PromotionId = input.PromotionId;
            dataExit.UserId = input.UserId;
            dataExit.Status = input.Status;
            dataExit.LastModificationTime = DateTime.Now;

            _dataContext.PromotionDetails.Update(dataExit);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Update success!");
        }
        #endregion

        #region -- Xóa thông tin chi tiết chương trình khuyến mãi
        [HttpDelete("DeletePromotionDetail")]
        public async Task<IActionResult> DeletePromotionDetail(int id)
        {
            var dataExits = await _dataContext.PromotionDetails.FindAsync(id);

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
        #endregion
    }
}
