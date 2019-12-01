using System;

namespace ElasticSearch.Domain.ViewModel
{
    public class JwtTokenViewModel
    {
        public string TokeType { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiresIn { get; set; }
    }
}
