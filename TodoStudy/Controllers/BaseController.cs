using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TodoStudy.Database;
using TodoStudy.Models;

namespace TodoStudy.Controllers
{
    public class BaseController: ControllerBase
    {
        public TodoDBContext DatabaseContext { get; }

        protected BaseController(TodoDBContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }

        protected async Task SignInAsync(User user)
        {
            var value = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Index.ToString()),
                        new Claim(ClaimTypes.Name, user.Name),
                    }, CookieAuthenticationDefaults.AuthenticationScheme
                )
            );
            await HttpContext.SignInAsync(value);
        }

        protected User Self 
        {
            get
            {
                var index = User.Identity.IsAuthenticated
                    ? User.Claims.Single(a => a.Type == ClaimTypes.NameIdentifier).Value
                    : string.Empty;

                if (string.IsNullOrEmpty(index))
                {
                    return null;
                }

                var user = DatabaseContext.User.SingleOrDefault(u => u.Index == int.Parse(index));
                return user;
            }
        }
    }
}
