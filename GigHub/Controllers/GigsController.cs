using GigHub.Models;
using GigHub.Persistance;
using GigHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GigHub.Controllers
{

    public class GigsController : Controller
    {
        // GET: Gigs
        private ApplicationDbContext _context;
        private readonly UnitOfWork _unitOfWork;

        public GigsController()
        {
            _context = new ApplicationDbContext();
            _unitOfWork = new UnitOfWork(_context);
        }

        [Authorize]
        public ActionResult Create()
        {
            var ViewModel = new GigFormViewModel
            {
                Genres = _unitOfWork.Genres.GetAllGenre(),
                Heading = "Create Gig"
            };
            return View("GigForm", ViewModel);
        }

        [Authorize]
        public ActionResult Edit(int id)
        {
            var gig = _unitOfWork.Gigs.GetGig(id);
            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();
            if (gig.DateTime < DateTime.Now)
                return null;

            var ViewModel = new GigFormViewModel
            {
                Venue = gig.Venue,
                Date = gig.DateTime.ToString("d MMM yyyy"),
                Time = gig.DateTime.ToString("HH:mm"),
                Genres = _unitOfWork.Genres.GetAllGenre(),
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
                    Genres = _unitOfWork.Genres.GetAllGenre()
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
            _unitOfWork.Gigs.Add(gig);
            _unitOfWork.Complete();
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
                    Genres = _unitOfWork.Genres.GetAllGenre()
                };
                return View("GigForm", viewModel);
            }

            var gig = _unitOfWork.Gigs.GigWithAttednace(viewModel.Id);
            if (gig == null)
                return HttpNotFound();
            if (gig.ArtistId != User.Identity.GetUserId())
                return new HttpUnauthorizedResult();

            gig.Update();

            gig.Venue = viewModel.Venue;
            gig.GenreId = viewModel.Genre;
            gig.DateTime = viewModel.GetDateTime();

            _unitOfWork.Complete();
            return RedirectToAction("Mine");
        }

        [Authorize]
        public ActionResult Attending()
        {
            var attendee = User.Identity.GetUserId();
            var viewModel = new GigsViewModel()
            {
                UpcomingGigs = _unitOfWork.Gigs.GetGigsUserAttending(attendee),
                ShowActions = User.Identity.IsAuthenticated,
                Heading = "Gigs I'm Attending",
                Attendances = _unitOfWork.Attendances.GetFutureAttendances(attendee)
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
            var gigs = _unitOfWork.Gigs.GetUpcomingGigsByArtist(artistId);
            return View(gigs);
        }

        public ActionResult Search(GigsViewModel vwModel)
        {
            return RedirectToAction("Index", "Home", new { query = vwModel.SearchTerm });
        }

        public ActionResult Details(int id)
        {
            var gig = _unitOfWork.Gigs.GetGig(id);
            if (gig == null)
                return HttpNotFound();
            var viewModel = new GigDetailsViewModel
            {
                Gig = gig
            };
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                viewModel.IsAttending = _unitOfWork.Attendances.GetAttendance(id, userId) != null;
                viewModel.IsFollowing = _unitOfWork.Followings.GetFollowing(userId, gig.ArtistId) != null;
            }
            return View("Details", viewModel);
        }
    }
}