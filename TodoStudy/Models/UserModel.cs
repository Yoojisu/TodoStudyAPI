using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoStudy.Database;
using System.ComponentModel.DataAnnotations;

namespace TodoStudy.Models
{
    public class UserModel
    {
        public class Login
        {
            [Required]
            public string Id { get; set; }

            [Required]
            public string Password { get; set; }

        }

        public class Create : Login
        {

            public string Name { get; set; }
        }


        public class Response
        {
            public Response()
            {

            }

            public Response(User user)
            {
                Index = user.Index;
                Id = user.Id;
                Password = user.Password;
                Name = user.Name;
                CreateDate = user.CreateDate;
            }

            public int Index { get; set; }
            public string Id { get; set; }
            public string Password { get; set; }
            public string Name { get; set; }
            public DateTimeOffset CreateDate { get; set; }

            public DateTimeOffset? DeleteDate { get; set; }

        }
    }
}
