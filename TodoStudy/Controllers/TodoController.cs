using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TodoStudy.Database;
using TodoStudy.Models;
using TodoStudy.Exceptions;
using TodoStudy.Extensions;
using System.ComponentModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using TodoStudy.Models.Filters;
using Microsoft.EntityFrameworkCore;


namespace TodoStudy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : BaseController
    {
        public TodoController(TodoDBContext context) : base(context)
        {
        }

        // 생성, 수정(체크, 글 수정), 삭제, 리스트(검색, 페이지네이션)
        [HttpGet]
        [Authorize]
        [Description("목록 호출")]
        public TodoListModel List([FromQuery] TodoFilter filter, DateTime? date = null)
        {
            if (string.IsNullOrEmpty(Self.Id))
            {
                throw new NotFoundException("로그인 필요");
            }

            // todo 리스트 조회 
            var todos = DatabaseContext.Todo
                .Include(t => t.User)
                .Where(t => t.UserIndex == Self.Index)
                .Where(t => !t.DeleteDate.HasValue);

            // 날짜조회
            if (date.HasValue)
            {
                todos = todos.Where(t => t.CreateDate.Date.ToShortDateString() == date.Value.ToShortDateString());
            }

            // 키워드 검색
            if (!string.IsNullOrEmpty(filter.Search))
            {
                filter.Search = filter.Search.ToLower();

                todos = todos.Where(t => t.Contents.ToLower().Contains(filter.Search));
            }

            return new TodoListModel()
            {
                TodoList = todos
                .Skip(filter.Skip)
                .Take(filter.Take)
                .Select(t => new TodoModel.Response(t))
                .ToList()
                .OrderByDescending(t => t.CreateDate),
                TotalCount = todos.Count()
            };
        }


        [HttpPut]
        [Authorize]
        [Description("할 일을 생성")]
        public TodoModel.Response Create(TodoModel.Create model)
        {
            if (Self == null)
            {
                throw new BadRequestException("로그인이 필요합니다.");

            }

            var todo = new Todo(model, Self);

            if (todo.Contents == null)
            {
                throw new BadRequestException("할 일을 입력해 주세요.");
            }

            using (var tran = DatabaseContext.Database.BeginTransaction())
            {
                DatabaseContext.Todo.Add(todo);
                DatabaseContext.SaveChanges();
                tran.Commit();
            }

            return new TodoModel.Response(todo);
        }

        [HttpPost("{index}")]
        [Authorize]
        [Description("할 일을 수정")]
        public TodoModel.Response Update(int index, TodoModel.Create model)
        {
            var todo = DatabaseContext.Todo
                .Where(t => !t.DeleteDate.HasValue)
                .SingleOrDefault(t => t.Index == index);

            if (todo == null)
            {
                throw new NotFoundException("Todo가 존재하지 않습니다.");
            }

            if (!string.IsNullOrEmpty(model.Contents) && model.Contents != todo.Contents)
            {
                todo.Contents = model.Contents;
            }

            if (model.IsChecked != todo.IsChecked)
            {
                todo.IsChecked = model.IsChecked;
            }

            return null;

        }

        [HttpDelete("{index}")]
        [Authorize]
        [Description("할 일을 삭제")]
        public TodoModel.Response Delete(int index)
        {

            var todo = DatabaseContext.Todo
                .Where(t => !t.DeleteDate.HasValue)
                .SingleOrDefault(t => t.Index == index);

            if (todo == null)
            {
                throw new NotFoundException("할 일이 존재하지 않습니다.");
            }

            todo.DeleteDate = DateTime.Now;

            using (var tran = DatabaseContext.Database.BeginTransaction())
            {
                DatabaseContext.Todo.Update(todo);
                DatabaseContext.SaveChanges();
                tran.Commit();
            }

            return new TodoModel.Response(todo);

        }
    }
}
