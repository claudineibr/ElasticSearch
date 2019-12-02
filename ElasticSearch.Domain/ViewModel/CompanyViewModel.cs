using System;

namespace ElasticSearch.Domain.ViewModel
{
    class CompanyViewModel
    {
        public string CountryCode { get; set; }
        public string CurrencyType { get; set; }
        public string Language { get; set; }
        public string CompanyCode { get; set; }
        public string Name { get; set; }
        public string SmallName { get; set; }
        public string LogoUrl { get; set; }
        public bool IsDefault { get; set; }
        public bool IsActive { get; set; }
        public string PrimaryColor { get; set; }
        public string SecondColor { get; set; }
        public string ThirdColor { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }

    }
}
