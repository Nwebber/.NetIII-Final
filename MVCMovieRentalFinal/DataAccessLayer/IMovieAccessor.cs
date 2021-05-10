using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public interface IMovieAccessor
    {
        List<Movie> SelectMovieByStatus(string status);
        int UpdateMovieStatus(int movieID, string oldMovieStatus, string newMovieStatus);
        int InsertNewMovie(Movie movie);
        int UpdateMovie(Movie oldMovie, Movie newMovie);
        List<RentedMovie> SelectMovieByRented(string movieStatus);
        bool DeactivateMovie(int id);
        bool DeleteMovie(int id);
        List<Movie> SelectMovieByID(int id);
    }
}
