using System.Net.Http.Headers;

namespace RestaurantBookingMVC.Handlers
{
    public class JwtTokenHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public JwtTokenHandler(IHttpContextAccessor httpContextAccessor)
        {
            _contextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _contextAccessor.HttpContext.Request.Cookies["jwtToken"];
            if(!string.IsNullOrEmpty(token) )
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
