using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TodoStudy.Models;
using TodoStudy.Extensions;

namespace TodoStudy.Database
{
    public partial class User
    {
        public User()
        {
            //Todo = new HashSet<Todo>();
        }

        public User(UserModel.Create model)
        {
            Id = model.Id;
            Password = model.Password.ToPasswordHash();
            Name = model.Name;
            CreateDate = DateTimeOffset.Now;
        }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }
       
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("createDate")]
        public DateTimeOffset CreateDate { get; set; }

        [JsonProperty("deleteDate")]
        public DateTimeOffset? DeleteDate { get; set; }

        //[JsonIgnore]

        //public virtual ICollection<Todo> Todo { get; set; }
    }
}
