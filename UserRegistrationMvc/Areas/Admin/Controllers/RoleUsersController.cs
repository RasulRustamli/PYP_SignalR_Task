using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserRegistrationMvc.CustomActionFilters;
using UserRegistrationMvc.DataContext;
using UserRegistrationMvc.Models;

namespace UserRegistrationMvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleUsersController : Controller
    {
        private readonly Context _context;
        private const string LOGIN_SESSION_KEY = "login";

        public RoleUsersController(Context context)
        {
            _context = context;
        }


        // GET: Admin/RoleUsers
        public async Task<IActionResult> Index()
        {
            var username = HttpContext.Session.GetString(LOGIN_SESSION_KEY);
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Auth", new { Area = "" });
            var context = _context.RoleUsers.Include(r => r.Role).Include(r => r.User);
            return View(await context.ToListAsync());
        }

        // GET: Admin/RoleUsers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var username = HttpContext.Session.GetString(LOGIN_SESSION_KEY);
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Auth", new { Area = "" });
            if (id == null || _context.RoleUsers == null)
            {
                return NotFound();
            }

            var roleUser = await _context.RoleUsers
                .Include(r => r.Role)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roleUser == null)
            {
                return NotFound();
            }

            return View(roleUser);
        }

        // GET: Admin/RoleUsers/Create
        public IActionResult Create()
        {
            var username = HttpContext.Session.GetString(LOGIN_SESSION_KEY);
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Auth", new { Area = "" });
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username");
            return View();
        }

        // POST: Admin/RoleUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,RoleId")] RoleUser roleUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(roleUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", roleUser.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", roleUser.UserId);
            return View(roleUser);
        }

        // GET: Admin/RoleUsers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var username = HttpContext.Session.GetString(LOGIN_SESSION_KEY);
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Auth", new { Area = "" });
            if (id == null || _context.RoleUsers == null)
            {
                return NotFound();
            }

            var roleUser = await _context.RoleUsers.FindAsync(id);
            if (roleUser == null)
            {
                return NotFound();
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", roleUser.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Username", roleUser.UserId);
            return View(roleUser);
        }

        // POST: Admin/RoleUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,RoleId")] RoleUser roleUser)
        {
            if (id != roleUser.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(roleUser);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleUserExists(roleUser.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", roleUser.RoleId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", roleUser.UserId);
            return View(roleUser);
        }

        // GET: Admin/RoleUsers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var username = HttpContext.Session.GetString(LOGIN_SESSION_KEY);
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login", "Auth", new { Area = "" });
            if (id == null || _context.RoleUsers == null)
            {
                return NotFound();
            }

            var roleUser = await _context.RoleUsers
                .Include(r => r.Role)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (roleUser == null)
            {
                return NotFound();
            }

            return View(roleUser);
        }

        // POST: Admin/RoleUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RoleUsers == null)
            {
                return Problem("Entity set 'Context.RoleUsers'  is null.");
            }
            var roleUser = await _context.RoleUsers.FindAsync(id);
            if (roleUser != null)
            {
                _context.RoleUsers.Remove(roleUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RoleUserExists(int id)
        {
            return _context.RoleUsers.Any(e => e.Id == id);
        }
    }
}
