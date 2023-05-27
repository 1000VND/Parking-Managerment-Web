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
                                  where ((string.IsNullOrWhiteSpace(input.PromotionName) || input.PromotionName == p.PromotionName) &&
                                       ((!input.FromDate.HasValue || input.FromDate.Value.Date <= p.FromDate.Value.Date) &&
                                       (!input.ToDate.HasValue || input.ToDate.Value.Date >= p.ToDate.Value.Date)))
                                  && p.IsDelete == 0
                                  orderby p.ToDate descending, p.FromDate descending
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
        public async Task<IActionResult> FindPromotionByDate(string plate)
        {
            var getPromoNow = await _dataContext.Promotions.AsNoTracking()
                .Where(e => e.IsDelete == 0 &&  
                DateTime.Now.Date >= e.FromDate.Value.Date && DateTime.Now.Date <= e.ToDate.Value.Date).ToListAsync();

            var checkCarExist = await _dataContext.TicketMonthlys.AsNoTracking().FirstOrDefaultAsync(e => e.LicensePlate == plate);
            
            if (checkCarExist != null)
            {
                var getPromoByPlate = await (from pd in _dataContext.PromotionDetails.Where(e => e.IsDelete == 0 && e.Status == 0)
                                             join p in _dataContext.Promotions.Where(e => e.IsDelete == 0) on pd.PromotionId equals p.Id into pJoined
                                             from lj in pJoined
                                             join t in _dataContext.TicketMonthlys.Where(e => e.IsDelete == 0) on pd.UserId equals t.Id into tJoined
                                             from ljt in tJoined
                                             where (checkCarExist == null || (pd.UserId == checkCarExist.Id))
                                             select new
                                             {
                                                 Id = lj.Id,
                                                 PromotionName = lj.PromotionName
                                             }).ToListAsync();
                if (getPromoByPlate.Count != 0)
                {
                    return CustomResult(getPromoByPlate);
                }
                else
                {
                    if (getPromoNow != null)
                    {
                        return CustomResult(getPromoNow);
                    }
                }

            }
            else
            {
                return CustomResult(checkCarExist);
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
            var checkPlate = await _dataContext.TicketMonthlys.FirstOrDefaultAsync(e => e.LicensePlate == input.Plate);
            var promotionDetail = new PromotionDetail
            {
                PromotionId = input.PromotionId,
                UserId = checkPlate.Id,
                IsDelete = Status.No,
                CreationTime = DateTime.Now,
                Status = 0
            };
            await _dataContext.PromotionDetails.AddAsync(promotionDetail);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Add success!");
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
