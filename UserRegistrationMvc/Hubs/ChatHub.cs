using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using UserRegistrationMvc.DataContext;

namespace UserRegistrationMvc.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly Context _context;

        public ChatHub(IHttpContextAccessor contextAccessor, Context context)
        {
            _contextAccessor = contextAccessor;
            _context = context;
        }

        public async Task SendMessageAsync(string message)
        {
            var username = _contextAccessor.HttpContext.Session.GetString("login");
            await Clients.All.SendAsync("ReceiveMessage", username, message);
        }
    }
}
