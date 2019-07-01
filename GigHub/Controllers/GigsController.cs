using GigHub.Models;
using GigHub.Repositories;
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
        private readonly GigRepository _gigsRepository;
        private readonly AttendanceRepository _attendanceRepository;
        private readonly GenreRepository _genreRepository;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            _gigsRepository = new GigRepository(_context);
            _attendanceRepository = new AttendanceRepository(_context);
            _genreRepository = new GenreRepository(_context);
        }

        [Authorize]
        public ActionResult Create()
        {
            var ViewModel = new GigFormViewModel
            {
                Genres = _context.Genres.ToList(),
                Heading = "Create Gig"
            };
            return View("GigForm", ViewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var gig = _gigsRepository.GetGig(id);
            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();
            if (gig.DateTime < DateTime.Now)
                return null;

            var ViewModel = new GigFormViewModel
            {
                Venue = gig.Venue,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genres = _genreRepository.GetAllGenre(),
                Genre = gig.GenreId,
                Heading = "Edit Gig",
                Id = gig.Id
            };
            return View("GigForm", ViewModel);
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
                    Genres = _genreRepository.GetAllGenre()
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
                    Genres = _genreRepository.GetAllGenre()
                };
                return View("GigForm", viewModel);
            }

            var gig = _gigsRepository.GigWithAttednace(viewModel.Id);
            if (gig == null)
                return HttpNotFound();
            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            gig.Update();

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
            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = _gigsRepository.GetGigsUserAttending(attendee),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                Attendances = _attendanceRepository.GetFutureAttendances(attendee)
                .ToLookup(a => a.GigId)
            };
            return View("Gigs", viewModel);
        }

        [Authorize]
        public ActionResult Following()
        {
            var follower = User.Identity.GetUserId();
            var artistList = _context.Follows
                .Where(a => a.FollowerId == follower)
                .Select(f => f.Artist.Name).ToList();
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
                .Include(g => g.Genre)
                .Where(g => g.ArtistId == artistId && !g.IsCanceled);
            return View(gigs);
        }

        public ActionResult Search(GigsViewModel vwModel)
        {
            return RedirectToAction("Index", "Home", new { query = vwModel.SearchTerm });
        }

        public ActionResult Details(int id)
        {
            var gig = _gigsRepository.GetGigWithGenreAndArtist(id);
            if (gig == null)
                return HttpNotFound();
            var viewModel = new GigDetailsViewModel
            {
                Gig = gig
            };
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                viewModel.IsAttending = _context.Attendances.Any(a => a.GigId == id && a.AttendeeId == userId);
                viewModel.IsFollowing = _context.Follows.Any(a => a.ArtistId == gig.ArtistId && a.FollowerId == userId);
            }
            return View("Details", viewModel);
        }
    }
}