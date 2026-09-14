using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using System.Security.Claims;

namespace ContosoDashboard.Services
{
    /// <summary>
    /// Custom authentication state provider for Blazor Server with cookie authentication
    /// </summary>
    public class CustomAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public CustomAuthenticationStateProvider(
            ILoggerFactory loggerFactory,
            IServiceScopeFactory serviceScopeFactory)
            : base(loggerFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

        protected override async Task<bool> ValidateAuthenticationStateAsync(
            AuthenticationState authenticationState, CancellationToken cancellationToken)
        {
            var userIdValue = authenticationState.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdValue, out var userId))
            {
                return false;
            }

            await using var scope = _serviceScopeFactory.CreateAsyncScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            return await userService.GetUserByIdAsync(userId) is not null;
        }
    }
}
