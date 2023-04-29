using API.Data;
using API.Dto;
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
        public async Task<IActionResult> CreateOrEdit(PromotionDto input)
        {
            if (input.Id == null) return await Create(input);
            else return await Update(input);
        }

        private async Task<IActionResult> Create(PromotionDto input)
        {
            var checkExist = await _dataContext.Promotions.FirstOrDefaultAsync(promotion => promotion.PromotionName == input.PromotionName.ToLower() && promotion.IsDelete == 0);

            if (checkExist != null) return CustomResult("Promotion Name is taken", System.Net.HttpStatusCode.BadRequest);

            var promotion = new Promotion
            {
                PromotionName = input.PromotionName.ToLower(),
                FromDate = input.FromDate,
                ToDate = input.ToDate,
                DisCount = input.DisCount,
                Point = input.Point,
                IsDelete = Status.No,
                CreationTime = DateTime.Now
            };
            _dataContext.Promotions.Add(promotion);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Add success!");
        }

        private async Task<IActionResult> Update(PromotionDto input)
        {
            var dataExit = await _dataContext.Promotions.FindAsync(input.Id);
            dataExit.PromotionName = input.PromotionName;
            dataExit.FromDate = input.FromDate;
            dataExit.ToDate = input.ToDate;
            dataExit.DisCount = input.DisCount;
            dataExit.Point = input.Point;
            dataExit.LastModificationTime = DateTime.Now;

            _dataContext.Promotions.Update(dataExit);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Update success!");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPromotion()
        {
            var data = await (from u in _dataContext.Promotions
                              where u.IsDelete == 0
                              orderby u.CreationTime descending
                              select new PromotionDto
                              {
                                  Id = u.Id,
                                  PromotionName = u.PromotionName,
                                  CreationTime = u.CreationTime,
                                  FromDate =u.FromDate,
                                  ToDate =u.ToDate,
                                  DisCount=u.DisCount,
                                  Point=u.Point,
                                  LastModificationTime = u.LastModificationTime
                              }).ToListAsync();

            return CustomResult(data);
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

            return NotFound();
        }

    }
}
