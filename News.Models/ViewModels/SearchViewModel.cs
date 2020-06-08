using System;
using System.Collections.Generic;
using System.Text;

namespace News.Models.ViewModels
{
    public class SearchViewModel
    {
        public string Query { get; set; }

        public IEnumerable<DomainModels.News> Result { get; set; }
    }
}
