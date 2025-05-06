using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SocialNetworkApp.Models;
using SocialNetworkApp.Models.SocialNetworkApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipsController : ControllerBase
    {
        private readonly SocialNetworkContext _context;

        public FriendshipsController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: api/Friendships
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Friendship>>> GetFriendships()
        {
            return await _context.Friendships
                .Include(f => f.User1)
                .Include(f => f.User2)
                .ToListAsync();
        }

        // GET: api/Friendships/1/2
        [HttpGet("{userId1:int}/{userId2:int}")]
        public async Task<ActionResult<Friendship>> GetFriendship(int userId1, int userId2)
        {
            var friendship = await _context.Friendships
                .Include(f => f.User1)
                .Include(f => f.User2)
                .FirstOrDefaultAsync(f => f.UserId1 == userId1 && f.UserId2 == userId2);

            if (friendship == null)
            {
                return NotFound();
            }

            return friendship;
        }

        // POST: api/Friendships
        [HttpPost]
        public async Task<ActionResult<Friendship>> PostFriendship(Friendship friendship)
        {
            var user1 = await _context.Users.FindAsync(friendship.UserId1);
            var user2 = await _context.Users.FindAsync(friendship.UserId2);

            if (user1 == null || user2 == null)
            {
                return BadRequest("Invalid UserId");
            }

            if(user1 == user2)
            {
                return BadRequest("Friendship is not a reflexive relation!");
            }

            friendship.User1 = user1;
            friendship.User2 = user2;

            _context.Friendships.Add(friendship);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFriendship), new { userId1 = friendship.UserId1, userId2 = friendship.UserId2 }, friendship);
        }

        // PUT: api/Friendships/1/2
        [HttpPut("{userId1:int}/{userId2:int}")]
        public async Task<IActionResult> PutFriendship(int userId1, int userId2, Friendship friendship)
        {
            if (userId1 != friendship.UserId1 || userId2 != friendship.UserId2)
            {
                return BadRequest();
            }

            _context.Entry(friendship).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FriendshipExists(userId1, userId2))
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

        // DELETE: api/Friendships/1/2
        [HttpDelete("{userId1:int}/{userId2:int}")]
        public async Task<IActionResult> DeleteFriendship(int userId1, int userId2)
        {
            var friendship = await _context.Friendships.FindAsync(userId1, userId2);
            if (friendship == null)
            {
                return NotFound();
            }

            _context.Friendships.Remove(friendship);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FriendshipExists(int userId1, int userId2)
        {
            return _context.Friendships.Any(e => e.UserId1 == userId1 && e.UserId2 == userId2);
        }
    }
}
