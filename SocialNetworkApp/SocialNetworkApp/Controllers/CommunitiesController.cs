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
    public class CommunitiesController : ControllerBase
    {
        private readonly SocialNetworkContext _context;

        public CommunitiesController(SocialNetworkContext context)
        {
            _context = context;
        }

        // GET: api/Communities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Community>>> GetCommunities()
        {
            return await _context.Communities
                .Include(c => c.Posts)
                .Include(c => c.UserCommunities)
                .ToListAsync();
        }

        // GET: api/Communities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Community>> GetCommunity(int id)
        {
            var community = await _context.Communities
                .Include(c => c.Posts)
                .Include(c => c.UserCommunities)
                .FirstOrDefaultAsync(c => c.CommunityId == id);

            if (community == null)
            {
                return NotFound();
            }

            return community;
        }

        // POST: api/Communities
        [HttpPost]
        public async Task<ActionResult<Community>> PostCommunity(Community community)
        {
            _context.Communities.Add(community);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCommunity), new { id = community.CommunityId }, community);
        }

        // PUT: api/Communities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCommunity(int id, Community community)
        {
            if (id != community.CommunityId)
            {
                return BadRequest();
            }

            _context.Entry(community).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommunityExists(id))
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

        // DELETE: api/Communities/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommunity(int id)
        {
            var community = await _context.Communities.FindAsync(id);
            if (community == null)
            {
                return NotFound();
            }

            _context.Communities.Remove(community);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommunityExists(int id)
        {
            return _context.Communities.Any(e => e.CommunityId == id);
        }
    }
}
