using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoStudy.Models
{
    public class TodoListModel
    {
        public IEnumerable<TodoModel.Response> TodoList { get; set; }

        public int TotalCount { get; set; }
    }
}
