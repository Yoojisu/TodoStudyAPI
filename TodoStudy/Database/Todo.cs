using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TodoStudy.Models;
using TodoStudy.Extensions;

namespace TodoStudy.Database
{
    public partial class Todo
    {
        public Todo()
        {
        }

        public Todo(TodoModel.Create model, User user)
        {
            UserIndex = user.Index;
            IsChecked = false;
            Contents = model.Contents;
            CreateDate = DateTimeOffset.Now;
        }

        [JsonProperty("userIndex")]
        public int UserIndex { get; set; }

        [JsonProperty("index")]
        public int Index { get; set; }

        [JsonProperty("contents")]
        public string Contents { get; set; }

        [JsonProperty("isChecked")]
        public bool IsChecked { get; set; }
       
        [JsonProperty("createDate")]
        public DateTimeOffset CreateDate { get; set; }

        [JsonProperty("deleteDate")]
        public DateTimeOffset? DeleteDate { get; set; }

        [JsonIgnore]
        public virtual User User { get; set; }
    }
}
