using GigHub.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace GigHub.Repositories
{
    public class GigRepository
    {
        private readonly ApplicationDbContext _context;

        public GigRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Gig> GetGigsUserAttending(string attendee)
        {
            return _context.Attendances
               .Where(a => a.AttendeeId == attendee)
               .Select(a => a.Gig)
               .Include(g => g.Artist)
               .Include(g => g.Genre).ToList();
        }

        public Gig GigWithAttednace(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee))
                .SingleOrDefault(g => g.Id == gigId);
        }
        public Gig GetGig(int gigId)
        {
            return _context.Gigs.SingleOrDefault(g => g.Id == gigId);
        }

        public Gig GetGigWithGenreAndArtist(int gigId)
        {
            return _context.Gigs
                .Include(g => g.Artist)
                .Include(g => g.Genre)
                .SingleOrDefault(g => g.Id == gigId);
        }
    }
}