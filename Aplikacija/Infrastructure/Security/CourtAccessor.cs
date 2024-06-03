using Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Security
{
    public class CourtAccessor : ICourtAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CourtAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            
        }
        public string GetCourtName()
        {
            return ""; // TODO: Implement this method
        }
    }
}