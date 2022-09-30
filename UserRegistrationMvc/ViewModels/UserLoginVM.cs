using System.ComponentModel.DataAnnotations;

namespace UserRegistrationMvc.ViewModels
{
    public class UserLoginVM
    {
        public string Username { get; set; } = null!;
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
