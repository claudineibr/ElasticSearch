using System;

namespace ElasticSearch.Domain.ViewModel
{
    public class BuildTokenViewModel
    {
        public string PersonId { get; set; }
        public string PersonName { get; set; }
        public DateTime? ExpireApiKey { get; set; }
    }
}
