using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface IMovieManager
    {
        List<Movie> RetrieveMovieByStatus(string status);
        bool EditMovieStatus(int movieID, string oldMovieStatus, string newMovieStatus);
        bool AddNewMovie(Movie movie);
        bool EditMovie(Movie oldMovie, Movie newMovie);
        List<RentedMovie> RetrieveMovieByRented(string movieStatus);
        bool DeactivateMovie(int id);
        bool DeleteMovie(int id);
        List<Movie> RetrieveMovieByID(int id);
    }
}
