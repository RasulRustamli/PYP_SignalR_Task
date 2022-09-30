using Microsoft.EntityFrameworkCore;
using UserRegistrationMvc.DataContext;
using UserRegistrationMvc.Models;
using UserRegistrationMvc.ViewModels;

namespace UserRegistrationMvc.Services
{
    public class AuthService : IAuthService
    {
        private readonly Context _context;

        public AuthService(Context context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> Login(UserLoginVM loginVM)
        {
            var exist = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginVM.Username);
            if (exist != null && BCrypt.Net.BCrypt.Verify(loginVM.Password, exist.Password))
            {
                return true;
            }
            return false;
        }

        public async Task<string> Register(UserRegisterVM registerVM)
        {
            var exist = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerVM.Username);
            if (exist != null) return "Istifadeci sistemde movcuddur";
            User user = new User()
            {
                Email = registerVM.Email,
                Username = registerVM.Username,
            };
            var result = CheckPassword(registerVM.Password);
            if (result)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(registerVM.Password);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                var userExist = await _context.Users.FirstOrDefaultAsync(u => u.Username == registerVM.Username);
                await _context.RoleUsers.AddAsync(new RoleUser { UserId = user.Id, RoleId = 3 });
                await _context.SaveChangesAsync();
                return "success";
            }
            return "Sifre minimum 8 uzunluqlu olmalidir. " +
                "Minimum 1 boyuk herf olmalidir. " +
                "Minimum 1 kicik herf olmalidir. " +
                "Minimum 1 reqem olmalidir.";
        }

        private bool CheckPassword(string password)
        {
            int digit = 0;
            int upper = 0;
            int lower = 0;
            if (password.Length >= 8)
            {
                foreach (var c in password)
                {
                    if (char.IsDigit(c)) digit++;
                    if (char.IsUpper(c)) upper++;
                    if (char.IsLower(c)) lower++;
                    if (digit > 0 && lower > 0 && upper > 0) return true;
                }
            }
            return false;
        }
    }
}
