using API.Data;
using API.Dto;
using API.Dto.Login;
using API.Entities;
using API.Enum;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class LoginController : BaseApiController
    {
        private readonly DataContext _dataContext;
        private readonly ITokenService _tokenService;

        public LoginController
            (
            DataContext dataContext,
            ITokenService tokenService

            )
        {
            _dataContext = dataContext;
            _tokenService = tokenService;
        }

        [HttpGet("login")]
        public async Task<IActionResult> Login([FromQuery] LoginDto input)
        {
            var userExist = await _dataContext.Users.SingleOrDefaultAsync(e => e.UserName == input.UserName && e.IsDelete == 0);
            
            if (userExist == null) return CustomResult("User Name don't exsits", System.Net.HttpStatusCode.NotFound);

            var hmac = new HMACSHA512(userExist.PassWordSalt);

            var checkPass = hmac.ComputeHash(Encoding.UTF8.GetBytes(input.PassWord));

            for (int i = 0; i < checkPass.Length; i++)
            {
                if (checkPass[i] != userExist.PassWordHash[i]) return CustomResult("Invalid password", System.Net.HttpStatusCode.NotFound);
            }

            return CustomResult(userExist);
        }

    }
}
