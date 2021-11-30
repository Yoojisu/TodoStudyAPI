using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using TodoStudy.Database;
using TodoStudy.Models;
using TodoStudy.Extensions;
using TodoStudy.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;

namespace TodoStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
       public AuthController(TodoDBContext context): base(context)
        {
        }

        [HttpPost("login")]
        [Description("사용자로 로그인 합니다.")]
        public async Task<UserModel.Response> Login(UserModel.Login model)
        {                      
            var loginSuccess = true;
            var errorMessage = "";

            var user = DatabaseContext.User
                .SingleOrDefault(u => u.Id == model.Id && u.Password == model.Password.ToPasswordHash());

            if (user == null)
            {
                loginSuccess = false;
                errorMessage = "사용자를 찾을 수 없습니다.";
            }
            else if (user.DeleteDate.HasValue)
            {
                loginSuccess = false;
                errorMessage = "탈퇴한 계정입니다.";

            }

            if (loginSuccess)
            {
                await SignInAsync(user);
            }
            else
            {
                throw new BadRequestException(errorMessage);
            }

            return new UserModel.Response(user);
        }

        [Authorize]
        [HttpGet("logout")]
        [Description("사용자를 로그아웃 합니다. [로그인 시]")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok();

        }

        [Authorize]
        [HttpGet("self")]
        [Description("사용자의 정보를 호출합니다. [로그인 시]")]
        public UserModel.Response UserSelf()
        {
            return new UserModel.Response(Self);
        }
    }
}
