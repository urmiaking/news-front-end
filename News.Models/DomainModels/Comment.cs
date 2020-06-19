using System;
using System.Collections.Generic;
using System.Text;

namespace News.Models.DomainModels
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public bool IsConfirmed { get; set; }

        public int NewsId { get; set; }

        public News News { get; set; }

        public int UserId { get; set; }

        public DateTime DateTime { get; set; }
    }
}
