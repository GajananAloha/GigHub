using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.API
{
    public class GigsController : ApiController
    {
        private ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        [Authorize]
        public IHttpActionResult Cancel(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.SingleOrDefault(g => g.Id == id && g.ArtistId == userId);
            if(gig == null)
            {
                return BadRequest();
            }
            gig.IsCanceled = true;
            _context.SaveChanges();
            return Ok();
        }
    }
}
