using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Models;
using SocialNetworkApp.Models.SocialNetworkApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetworkApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserCommunitiesController : ControllerBase
    {
        private readonly SocialNetworkContext _context;

        public UserCommunitiesController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: api/UserCommunities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCommunity>>> GetUserCommunities()
        {
            return await _context.UserCommunities
                .Include(uc => uc.User)
                .Include(uc => uc.Community)
                .ToListAsync();
        }

        // GET: api/UserCommunities/1/2
        [HttpGet("{userId:int}/{communityId:int}")]
        public async Task<ActionResult<UserCommunity>> GetUserCommunity(int userId, int communityId)
        {
            var userCommunity = await _context.UserCommunities
                .Include(uc => uc.User)
                .Include(uc => uc.Community)
                .FirstOrDefaultAsync(uc => uc.UserId == userId && uc.CommunityId == communityId);

            if (userCommunity == null)
            {
                return NotFound();
            }

            return userCommunity;
        }

        // POST: api/UserCommunities
        [HttpPost]
        public async Task<ActionResult<UserCommunity>> PostUserCommunity(UserCommunity userCommunity)
        {
            var user = await _context.Users.FindAsync(userCommunity.UserId);
            var community = await _context.Communities.FindAsync(userCommunity.CommunityId);

            if (user == null || community == null)
            {
                return BadRequest("Invalid UserId or CommunityId");
            }

            userCommunity.User = user;
            userCommunity.Community = community;

            _context.UserCommunities.Add(userCommunity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserCommunity), new { userId = userCommunity.UserId, communityId = userCommunity.CommunityId }, userCommunity);
        }

        // PUT: api/UserCommunities/1/2
        [HttpPut("{userId:int}/{communityId:int}")]
        public async Task<IActionResult> PutUserCommunity(int userId, int communityId, UserCommunity userCommunity)
        {
            if (userId != userCommunity.UserId || communityId != userCommunity.CommunityId)
            {
                return BadRequest();
            }

            _context.Entry(userCommunity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCommunityExists(userId, communityId))
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

        // DELETE: api/UserCommunities/1/2
        [HttpDelete("{userId:int}/{communityId:int}")]
        public async Task<IActionResult> DeleteUserCommunity(int userId, int communityId)
        {
            var userCommunity = await _context.UserCommunities.FindAsync(userId, communityId);
            if (userCommunity == null)
            {
                return NotFound();
            }

            _context.UserCommunities.Remove(userCommunity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserCommunityExists(int userId, int communityId)
        {
            return _context.UserCommunities.Any(e => e.UserId == userId && e.CommunityId == communityId);
        }
    }
}
