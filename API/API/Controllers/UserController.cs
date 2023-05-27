using API.Data;
using API.Dto;
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
    public class UserController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public UserController
            (
                DataContext dataContext
            )
        {
            _dataContext = dataContext;
        }

        [HttpPost("CreateOrEdit")]
        public async Task<IActionResult> CreateOrEdit(RegisterDto input)
        {
            if (input.Id == null) return await Create(input);
            else return await Update(input);
        }

        private async Task<IActionResult> Create(RegisterDto input)
        {
            var checkExist = await _dataContext.Users.FirstOrDefaultAsync(user => user.UserName == input.UserName.ToLower() && user.IsDelete == 0);

            if (checkExist != null) return CustomResult("Username is taken", System.Net.HttpStatusCode.BadRequest);

            var hmac = new HMACSHA512();

            var user = new User
            {
                UserName = input.UserName.ToLower(),
                PassWordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(input.PassWord)),
                PassWordSalt = hmac.Key,
                FullName = input.FullName,
                IsDelete = Status.No,
                Role = input.Role,
                CreationTime = DateTime.Now
            };
            var a = await _dataContext.Users.AddAsync(user);
            //int b = a.Entity.Id;
            await _dataContext.SaveChangesAsync();

            return CustomResult(a.Entity.Id);
        }

        private async Task<IActionResult> Update(RegisterDto input)
        {
            var dataExit = await _dataContext.Users.FindAsync(input.Id);
            dataExit.FullName = input.FullName;
            dataExit.Role = input.Role;
            dataExit.LastModificationTime = DateTime.Now;

            _dataContext.Users.Update(dataExit);
            await _dataContext.SaveChangesAsync();

            return CustomResult("Update success!");
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUser()
        {
            var data = await (from u in _dataContext.Users
                              where u.IsDelete == 0
                              orderby u.CreationTime descending
                              select new UserDto
                              {
                                  Id = u.Id,
                                  UserName = u.UserName,
                                  FullName = u.FullName,
                                  Role = u.Role,
                                  CreationTime = u.CreationTime,
                                  LastModificationTime = u.LastModificationTime
                              }).ToListAsync();

            return CustomResult(data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dataExits = await _dataContext.Users.FindAsync(id);

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
