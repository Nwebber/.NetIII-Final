using DataAccessLayer;
using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class MovieManager : IMovieManager
    {
        private IMovieAccessor _movieAccessor = null;

        public MovieManager()
        {
            _movieAccessor = new MovieAccessor();
        }

        public MovieManager(IMovieAccessor movieAccessor)
        {
            _movieAccessor = movieAccessor;
        }

        public bool AddNewMovie(Movie movie)
        {
            bool result = false;
            int newMovieID = 0;

            try
            {
                newMovieID = _movieAccessor.InsertNewMovie(movie);

                if (newMovieID == 0)
                {
                    throw new ApplicationException("New movie was not added");
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Add movie failed", ex);
            }

            return result;
        }

        public bool DeactivateMovie(int id)
        {
            bool result = false;

            try
            {
                result = _movieAccessor.DeactivateMovie(id);
                if (result == false)
                {
                    throw new ApplicationException("Movie could not be deactivated");
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Movie could not be deactivated");
            }

            return result;
        }

        public bool DeleteMovie(int id)
        {
            bool result = false;

            try
            {
                result = _movieAccessor.DeleteMovie(id);
                if (result == false)
                {
                    throw new ApplicationException("Movie could not be deleted");
                }
                result = true;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Movie could not be deleted");
            }

            return result;
        }

        public bool EditMovie(Movie oldMovie, Movie newMovie)
        {
            bool result = false;

            try
            {
                result = (1 == _movieAccessor.UpdateMovie(oldMovie, newMovie));

                if (result == false)
                {
                    throw new ApplicationException("Movie data not Updated");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Update failed", ex);
            }

            return result;
        }

        public bool EditMovieStatus(int movieID, string oldMovieStatus, string newMovieStatus)
        {
            try
            {
                return (1 == _movieAccessor.UpdateMovieStatus(movieID, oldMovieStatus, newMovieStatus));
            }
            catch (Exception ex)
            {

                throw new ApplicationException("Change status to '" + newMovieStatus + "' failed.", ex);
            }
        }

        public List<Movie> RetrieveMovieByID(int id)
        {
            try
            {
                return _movieAccessor.SelectMovieByID(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data unavailable", ex);
            }
        }

        public List<RentedMovie> RetrieveMovieByRented(string movieStatus)
        {
            try
            {
                return _movieAccessor.SelectMovieByRented(movieStatus);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data unavailable", ex);
            }
        }

        public List<Movie> RetrieveMovieByStatus(string status)
        {
            try
            {
                return _movieAccessor.SelectMovieByStatus(status);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Data unavailable", ex);
            }
        }
    }
}
