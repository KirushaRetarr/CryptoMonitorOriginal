using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using CryptoMonitor.Models;
using CryptoMonitor.Data;
using Microsoft.EntityFrameworkCore;

namespace CryptoMonitor.Services
{
    public class AuthService
    {
        private readonly CryptoMonitorDbContext _context;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthService(CryptoMonitorDbContext context, AuthenticationStateProvider authStateProvider)
        {
            _context = context;
            _authStateProvider = authStateProvider;
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            if (await _context.Users.AnyAsync(u => u.Email == model.Email))
            {
                return false;
            }

            var user = new User
            {
                Email = model.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.Password),
                RoleId = 2, // Default role: User
                RegisteredAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> LoginAsync(LoginModel model)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role.RoleName)
            };

            var identity = new ClaimsIdentity(claims, "apiauth_type");
            var userPrincipal = new ClaimsPrincipal(identity);

            if (_authStateProvider is CustomAuthStateProvider customAuthStateProvider)
            {
                await customAuthStateProvider.UpdateAuthenticationState(userPrincipal);
            }

            return true;
        }

        public async Task LogoutAsync()
        {
            if (_authStateProvider is CustomAuthStateProvider customAuthStateProvider)
            {
                await customAuthStateProvider.UpdateAuthenticationState(null);
            }
        }

        public async Task<bool> ChangePasswordAsync(string email, ChangePasswordModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(model.CurrentPassword, user.PasswordHash))
            {
                return false;
            }

            // Update password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.NewPassword);
            await _context.SaveChangesAsync();

            return true;
        }
    }
} 