using Microsoft.AspNetCore.Http;
using NascorpLib;

namespace ElasticSearch.WebApi.Utilities
{
    public static class IHttpContextAccessorExtension
    {
        public static string CurrentCompanyCode(this IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor?.HttpContext?.User?.FindFirst(PersonalRegisteredClaimNames.CompanyCode)?.Value;
        }
    }
}
