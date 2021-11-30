using System;
using System.Collections.Generic;

namespace TodoStudy.Models
{
    public class UserListModel
    {
        public IEnumerable<UserModel.Response> List { get; set; }
        public int TotalCount { get; set; }
    }
}
