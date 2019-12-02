using System;
using System.Collections.Generic;

namespace ElasticSearch.Domain.ViewModel
{
    public class ProductViewModel
    {
        public string ProductCode { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool HasArImage { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public ICollection<ImageViewModel> Images { get; set; }
    }
}
