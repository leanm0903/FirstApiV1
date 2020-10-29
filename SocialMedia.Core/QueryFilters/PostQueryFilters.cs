using System;
using System.Collections.Generic;
using System.Text;

namespace SocialMedia.Core.QueryFilters
{
    public class PostQueryFilters
    {
        public int? UserId { get; set; }
        public DateTime ? Date { get; set; }
        public string Description { get; set; }
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
    }
}
