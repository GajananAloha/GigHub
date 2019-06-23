using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    public class FollowersController : ApiController
    {
        private ApplicationDbContext _context;

        public FollowersController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult Follow(FollowDto dto)
        {
            var followeeId = User.Identity.GetUserId();
            var follow = new Follow
            {
                ArtistId = dto.ArtistId,
                FollowerId = followeeId
            };
            if(_context.Follows.Any(f=>f.ArtistId == dto.ArtistId && f.FollowerId == followeeId))
                return BadRequest();
            _context.Follows.Add(follow);
            _context.SaveChanges();
            return Ok();
        }
    }
}
