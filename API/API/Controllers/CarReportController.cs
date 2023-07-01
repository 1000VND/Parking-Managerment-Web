using API.Data;
using API.Dto;
using API.Dto.Report.CarReport;
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
    public class CarReportController : BaseApiController
    {

        private readonly DataContext _dataContext;

        public CarReportController
            (
                DataContext dataContext
            )
        {
            _dataContext = dataContext;
        }

        [HttpPost("CreateOrEdit")]
        public async Task<IActionResult> CreateOrEdit(CarReportInputDto input)
        {
            if (input.Id == null) return await Create(input);
            else return await Update(input);
        }

        private async Task<IActionResult> Create(CarReportInputDto input)
        {
            var checkExists = await _dataContext.Cars.FirstOrDefaultAsync(f => f.IsCarParking == 0 && f.IsDelete == 0 && f.LicensePlateIn == input.LicensePlate);
            if (checkExists != null)
            {
                var carReport = new CarReport
                {
                    CustomerName = input.CustomerName,
                    UserId = input.UserId,
                    LicensePlate = input.LicensePlate,
                    Reason = input.Reason,
                    Content = input.Content,
                    CustomerNumber = input.CustomerNumber,
                    CustomerBirthday = input.CustomerBirthday,
                    IdentityCard = input.IdentityCard,
                    IsDelete = Status.No,
                    CreationTime = DateTime.Now,
                };

                await _dataContext.CarReports.AddAsync(carReport);
                await _dataContext.SaveChangesAsync();

                var updateCar = await _dataContext.Cars.FirstOrDefaultAsync(f => f.IsCarParking == 0 && f.IsDelete == 0 && f.LicensePlateIn == carReport.LicensePlate);
                if (updateCar != null)
                {
                    updateCar.LastModificationTime = DateTime.Now;
                    updateCar.IsDelete = Status.Yes;
                    _dataContext.Cars.Update(updateCar);
                }
                return CustomResult("Add success!");

            }
            return CustomResult("The car is not in the parking lot !", System.Net.HttpStatusCode.NotFound);
        }

        private async Task<IActionResult> Update(CarReportInputDto input)
        {
            var dataExit = await _dataContext.CarReports.FindAsync(input.Id);
            dataExit.CustomerName = input.CustomerName;
            dataExit.UserId = input.UserId;
            dataExit.LicensePlate = input.LicensePlate;
            dataExit.Reason = input.Reason;
            dataExit.Content = input.Content;
            dataExit.LastModificationTime = DateTime.Now;

            _dataContext.CarReports.Update(dataExit);

            return CustomResult("Update success!");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dataExist = await _dataContext.CarReports.FindAsync(id);

            if (dataExist != null && dataExist.IsDelete == Status.No)
            {
                dataExist.IsDelete = Status.Yes;
                dataExist.LastModificationTime = DateTime.Now;
                await _dataContext.SaveChangesAsync();
                return CustomResult(dataExist);
            }
            return NotFound();
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllCarReport()
        {

            var data = await (from c in _dataContext.CarReports
                              join u in _dataContext.Users on c.UserId equals u.Id
                              where c.IsDelete == 0
                              orderby c.CreationTime descending
                              select new CarReportGetAllDto
                              {
                                  Id = c.Id,
                                  CustomerName = c.CustomerName,
                                  UserId = c.UserId,
                                  UserName = u.FullName,
                                  LicensePlate = c.LicensePlate,
                                  Reason = c.Reason,
                                  Content = c.Content,
                                  CreationTime = c.CreationTime,
                                  CustomerNumber = c.CustomerNumber,
                                  CustomerBirthday = c.CustomerBirthday,
                                  IdentityCard = c.IdentityCard,
                              }).ToListAsync();

            return CustomResult(data);
        }

        #region -- Get data for Car Report
        [HttpGet("GetDataForByIdCarReport")]
        public async Task<IActionResult> GetDataForByIdCarReport(int id)
        {
            var data = await (from c in _dataContext.CarReports
                              join u in _dataContext.Users on c.UserId equals u.Id
                              where c.IsDelete == 0 && c.Id == id
                              orderby c.CreationTime descending
                              select new CarReportGetAllDto
                              {
                                  Id = c.Id,
                                  CustomerName = c.CustomerName,
                                  UserId = c.UserId,
                                  UserName = u.FullName,
                                  LicensePlate = c.LicensePlate,
                                  Reason = c.Reason,
                                  Content = c.Content,
                                  CreationTime = c.CreationTime,
                                  CustomerNumber = c.CustomerNumber,
                                  CustomerBirthday = c.CustomerBirthday,
                                  IdentityCard = c.IdentityCard,
                              }).ToListAsync();

            return CustomResult(data);
        }
        #endregion
    }
}
