using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoStudy.Database;

namespace TodoStudy.Models
{
    public class TodoModel
    {

        public class Create
        {
            public string Contents { get; set; }
            public bool IsChecked { get; set; }
            public DateTimeOffset CreateDate { get; set; }
        }

        public class Response
        {
            public Response()
            {
            }

           public Response(Todo todo)
           {
                UserIndex = todo.UserIndex;
                Index = todo.Index;
                Contents = todo.Contents;
                IsChecked = todo.IsChecked;
                CreateDate = todo.CreateDate;
                DeleteDate = todo.DeleteDate;
            }

            public int UserIndex { get; set; }
            public int Index { get; set; }
            public string Contents { get; set; }
            public bool IsChecked { get; set; }
            public DateTimeOffset CreateDate { get; set; }
            public DateTimeOffset? DeleteDate { get; set; }
        }
    }
}
