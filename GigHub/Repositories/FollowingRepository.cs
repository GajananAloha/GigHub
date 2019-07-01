using GigHub.Models;
using System.Linq;

namespace GigHub.Repositories
{
    public class FollowingRepository
    {
        private readonly ApplicationDbContext _context;

        public FollowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public Follow GetFollowing(string userId, string artistId)
        {
            return _context.Follows.SingleOrDefault(a => a.ArtistId ==artistId && a.FollowerId == userId);
        }
    }
}