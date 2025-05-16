using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Models;
using SocialNetworkApp.Models.SocialNetworkApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocialNetworkApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SocialNetworkContext _context;

        public UsersController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.UserCommunities)
                .Include(u => u.Friendships)
                .ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Posts)
                .Include(u => u.UserCommunities)
                .Include(u => u.Friendships)
                .FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            var phoneRegex = new Regex(@"^380\d{9}$");
            if (!phoneRegex.IsMatch(user.UserPhone))
            {
                return BadRequest(new { message = "Phone number must be in the format 380XXXXXXXXX (12 digits)." });
            }

            bool phoneExists = await _context.Users.AnyAsync(u => u.UserPhone == user.UserPhone);
            if (phoneExists)
            {
                return Conflict(new { message = "A user with this phone number already exists." });
            }
            if (user.UserBirth > DateTime.Today.AddYears(-13))
            {
                return BadRequest(new { message = "User must be at least 13 years old." });
            }

            if (user.UserBirth < new DateTime(1900, 1, 1))
            {
                return BadRequest(new { message = "Birth date must be after 01.01.1900." });
            }


            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;
            var phoneRegex = new Regex(@"^380\d{9}$");
            if (!phoneRegex.IsMatch(user.UserPhone))
            {
                return BadRequest(new { message = "Phone number must be in the format 380XXXXXXXXX (12 digits)." });
            }

            if (user.UserBirth > DateTime.Today.AddYears(-13))
            {
                return BadRequest(new { message = "User must be at least 13 years old." });
            }

            if (user.UserBirth < new DateTime(1900, 1, 1))
            {
                return BadRequest(new { message = "Birth date must be after 01.01.1900." });
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
