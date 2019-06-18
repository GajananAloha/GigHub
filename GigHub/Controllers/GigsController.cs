using GigHub.Models;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{

    public class GigsController : Controller
    {
        // GET: Gigs
        private ApplicationDbContext _context;

            public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [Authorize]
        public ActionResult Create()
        {
            var ViewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Heading = "Create Gig"
            };
            return View("GigForm",ViewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == id
            && g.ArtistId == userId
            && g.DateTime > DateTime.Now);
            var ViewModel = new GigFormViewModel
            {
                Venue = gig.Venue,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time=gig.DateTime.ToString("HH:mm"),
                Genres = _context.Genres.ToList(),
                Genre = gig.GenreId,
                Heading="Edit Gig",
                Id = gig.Id
            };
            return View("GigForm",ViewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = new GigFormViewModel
                {
                    Genres = _context.Genres.ToList()
                };
                return View("GigForm", viewModel);
            }
            var gig = new Gig()
            {
                ArtistId = User.Identity.GetUserId(),
                DateTime = viewModel.GetDateTime(),
                GenreId = viewModel.Genre,
                Venue = viewModel.Venue
            };
            _context.Gigs.Add(gig);
            _context.SaveChanges();
            return RedirectToAction("Mine");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(GigFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel = new GigFormViewModel
                {
                    Genres = _context.Genres.ToList()
                };
                return View("GigForm", viewModel);
            }
            var userId = User.Identity.GetUserId();
            var gig = _context.Gigs.Single(g => g.Id == viewModel.Id && g.ArtistId == userId);
            gig.Venue = viewModel.Venue;
            gig.GenreId = viewModel.Genre;
            gig.DateTime = viewModel.GetDateTime();
            _context.SaveChanges();
            return RedirectToAction("Mine");
        }

        [Authorize]
        public ActionResult Attending()
        {
            var attendee = User.Identity.GetUserId();
            var gigs = _context.Attendances
                .Where(a => a.AttendeeId == attendee)
                .Select(a => a.Gig)
                .Include(g=>g.Artist)
                .Include(g => g.Genre);
            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = gigs,
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending"
            };
            return View("Gigs",viewModel);
        }

        [Authorize]
        public ActionResult Following()
        {
            var follower = User.Identity.GetUserId();
            var artistList = _context.Follows
                .Where(a => a.FollowerId ==follower )
                .Select(f=>f.Artist.Name).ToList();
            var viewModel = new FolloweeViewModel()
            {
                Artists = artistList
            };
            return View(viewModel);
        }

        [Authorize]
        public ActionResult Mine()
        {
            var artistId = User.Identity.GetUserId();
            var gigs = _context.Gigs
                .Include(g=>g.Genre)
                .Where(g => g.ArtistId == artistId && !g.IsCanceled);
            return View(gigs);
        }
    }
}