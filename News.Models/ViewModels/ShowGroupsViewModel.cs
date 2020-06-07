using System;
using System.Collections.Generic;
using System.Text;

namespace News.Models.ViewModels
{
    public class ShowGroupsViewModel
    {
        public int GroupId { get; set; }
        public string GroupTitle { get; set; }
        public int NewsCount { get; set; }
    }
}
