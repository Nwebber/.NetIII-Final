using LogicLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataObjectsLayer;

namespace MVCMovieRentalFinal.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        private MovieManager _movieManager = new MovieManager();

        //---------------------------------- Rental Section ----------------------------------

        public ActionResult Available()
        {
            return View(_movieManager.RetrieveMovieByStatus("Available"));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie m = _movieManager.RetrieveMovieByStatus("Available").Find(mov => mov.MovieID == id);
            if (m == null)
            {
                return HttpNotFound();
            }
            return View(m);
                    
        }

        public ActionResult Rent(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie m = _movieManager.RetrieveMovieByStatus("Available").Find(mov => mov.MovieID == id);
            if (m == null)
            {
                return HttpNotFound();
            }
            _movieManager.EditMovieStatus((int)id, "Available", "Rented");

            return View();
        }

        //---------------------------------- Return a Rental Section ---------------------------

        public ActionResult Rented()
        {
            return View(_movieManager.RetrieveMovieByStatus("Rented").OrderBy(m => m.MovieTitle));
        }

        public ActionResult Return(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            try
            {
                _movieManager.EditMovieStatus((int)id, "Rented", "Available");
            }
            catch (Exception)
            {
                return HttpNotFound();
            }

            return View();
        }

        //---------------------------------- Edit A Movie Section ---------------------------

        [Authorize(Roles ="Admin, IT")]
        public ActionResult Edit()
        {
            return View(_movieManager.RetrieveMovieByStatus("Available"));
        }

        [Authorize(Roles ="Admin, IT")]
        public ActionResult EditMovie(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            List<Movie> movies = _movieManager.RetrieveMovieByStatus("Available");
            var oldMovie = movies.Where(om => om.MovieID == id).FirstOrDefault();

            return View(oldMovie);
        }

        [Authorize(Roles ="Admin, IT")]
        [HttpPost]
        public ActionResult EditMovie(Movie newMovie)
        {
            List<Movie> movies = _movieManager.RetrieveMovieByStatus("Available");
            var oldMovie = movies.Where(om => om.MovieID == newMovie.MovieID).FirstOrDefault();
            movies.Remove(oldMovie);
            movies.Add(newMovie);
            _movieManager.EditMovie(oldMovie, newMovie);

            return RedirectToAction("Edit");
        }

        //---------------------------------- Delete A Movie Section ---------------------------

        [Authorize(Roles ="Admin, IT")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Movie dm = _movieManager.RetrieveMovieByStatus("Available").Find(m => m.MovieID == id);
            if (dm == null)
            {
                return HttpNotFound();
            }
            return View(dm);
        }

        [Authorize(Roles ="Admin, IT")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            _movieManager.DeleteMovie(id);

            return RedirectToAction("Edit");
        }

        //---------------------------------- Create a Movie Section ----------------------------

        [Authorize(Roles ="Admin, IT")]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize(Roles ="Admin, IT")]
        [HttpPost]
        public ActionResult Create(Movie newMovie)
        {
            if (ModelState.IsValid)
            {
                _movieManager.AddNewMovie(newMovie);
                return RedirectToAction("Available");
            }
            else
            {
                return View(newMovie);
            }
        }

        //---------------------------------- Disposal Section ----------------------------------

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _movieManager = null;
            }
            base.Dispose(disposing);
        }
    }
}