using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace CryptoMonitor.Services
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _js;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

        public CustomAuthStateProvider(IJSRuntime js)
        {
            _js = js;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var email = await _js.InvokeAsync<string>("localStorage.getItem", "authEmail");
                var role = await _js.InvokeAsync<string>("localStorage.getItem", "authRole");
                if (!string.IsNullOrEmpty(email))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, email),
                        new Claim(ClaimTypes.Role, string.IsNullOrEmpty(role) ? "User" : role)
                    };
                    var identity = new ClaimsIdentity(claims, "apiauth_type");
                    return new AuthenticationState(new ClaimsPrincipal(identity));
                }
            }
            catch (InvalidOperationException)
            {
                // JSInterop недоступен (пререндеринг) — возвращаем анонимного пользователя
            }
            return new AuthenticationState(_anonymous);
        }

        public async Task UpdateAuthenticationState(ClaimsPrincipal user)
        {
            if (user?.Identity?.IsAuthenticated == true)
            {
                var email = user.Identity.Name;
                var role = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "User";
                await _js.InvokeVoidAsync("localStorage.setItem", "authEmail", email);
                await _js.InvokeVoidAsync("localStorage.setItem", "authRole", role);
            }
            else
            {
                await _js.InvokeVoidAsync("localStorage.removeItem", "authEmail");
                await _js.InvokeVoidAsync("localStorage.removeItem", "authRole");
            }
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
} 