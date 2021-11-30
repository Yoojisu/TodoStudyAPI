using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoStudy.Database;
using TodoStudy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using TodoStudy.Exceptions;
using TodoStudy.Exceptions.Enums;
using TodoStudy.Extensions;

namespace TodoStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {

        public UserController(TodoDBContext context): base(context)
        {
        }

        [HttpGet("list")]
        [Authorize]
        [Description("사용자 목록 데이터 호출")]
        public UserListModel List()
        {
            var users = DatabaseContext.User.Where(u => !u.DeleteDate.HasValue);

            return new UserListModel()
            {
                List = users.Select(u => new UserModel.Response(u)).ToList(),
                TotalCount = users.Count()
            };
        }

        [HttpPut]
        [Description("사용자를 생성")]
        public UserModel.Response Create(UserModel.Create model)
        {
         
            if(DatabaseContext.User.Any(u => !u.DeleteDate.HasValue && u.Id == model.Id))
            {
                throw new ConflictException(ExceptionCode.Id, "이미 존재하는 아이디입니다.");
            }

            var user = new User();

            if (user == null)
            {
                throw new NotFoundException("존재하지 않는 사용자 입니다.");
            }

            user = new User(model);


            using(var tran = DatabaseContext.Database.BeginTransaction())
            {
                DatabaseContext.User.Add(user);
                DatabaseContext.SaveChanges();
                tran.Commit();
            }
            return new UserModel.Response(user);
        }


        [HttpDelete("{index}")]
        [Authorize]
        [Description("사용자를 삭제")]
        public UserModel.Response Delete(int index)
        {
            var user = DatabaseContext.User
                .Where(u => !u.DeleteDate.HasValue)
                .SingleOrDefault(u => u.Index == index);

            if (user == null)
            {
                throw new NotFoundException("존재하지 않는 계정입니다.");
            }

            user.DeleteDate = DateTimeOffset.Now;

            using(var tran = DatabaseContext.Database.BeginTransaction())
            {
                DatabaseContext.User.Update(user);
                DatabaseContext.SaveChanges();
                tran.Commit();
            }

            return new UserModel.Response(user); 
        }


    }
}
