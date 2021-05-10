using DataObjectsLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class MovieAccessor : IMovieAccessor
    {
        public bool DeactivateMovie(int id)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_deactivate_movie", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@MovieID", SqlDbType.Int);
            cmd.Parameters["@MovieID"].Value = id;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new ApplicationException("Movie could not be deactivated");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result == 1;
        }

        public bool DeleteMovie(int id)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_delete_movie", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@MovieID", SqlDbType.Int);
            cmd.Parameters["@MovieID"].Value = id;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();

                if (result != 1)
                {
                    throw new ApplicationException("Movie could not be deleted");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result == 1;
        }

        public int InsertNewMovie(Movie movie)
        {
            int movieID = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_insert_new_movie", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@MovieTitle", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@MovieDate", SqlDbType.Int);

            cmd.Parameters["@MovieTitle"].Value = movie.MovieTitle;
            cmd.Parameters["@MovieDate"].Value = movie.MovieDate;

            try
            {
                conn.Open();
                movieID = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return movieID;
        }

        public List<Movie> SelectMovieByID(int id)
        {
            List<Movie> movies = new List<Movie>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_retreive_movie_by_id", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@MovieID", SqlDbType.Int);
            cmd.Parameters["@MovieID"].Value = id;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        movies.Add(new Movie()
                        {
                            MovieID = reader.GetInt32(0),
                            MovieTitle = reader.GetString(1),
                            MovieDate = reader.GetInt32(2)
                        });

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return movies;
        }

        public List<RentedMovie> SelectMovieByRented(string movieStatus)
        {
            List<RentedMovie> movies = new List<RentedMovie>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_movie_by_rented", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@MovieStatusID", SqlDbType.NVarChar, 50);
            cmd.Parameters["@MovieStatusID"].Value = movieStatus;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        movies.Add(new RentedMovie()
                        {
                            MovieID = reader.GetInt32(0),
                            UserID = reader.GetInt32(1),
                            MovieTitle = reader.GetString(2),
                            FirstName = reader.GetString(3)
                        }); 

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return movies;
        }

        public List<Movie> SelectMovieByStatus(string status)
        {
            List<Movie> movies = new List<Movie>();

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_select_movie_by_status", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@MovieStatusID", SqlDbType.NVarChar, 50);
            cmd.Parameters["@MovieStatusID"].Value = status;

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        movies.Add(new Movie()
                        {
                            MovieID = reader.GetInt32(0),
                            MovieTitle = reader.GetString(1),
                            MovieDate = reader.GetInt32(2)
                        });

                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return movies;
        }

        public int UpdateMovie(Movie oldMovie, Movie newMovie)
        {
            int result = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_update_movie", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@MovieID", SqlDbType.Int);

            cmd.Parameters.Add("@NewMovieTitle", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@NewMovieDate", SqlDbType.Int);

            cmd.Parameters.Add("@OldMovieTitle", SqlDbType.NVarChar, 256);
            cmd.Parameters.Add("@OldMovieDate", SqlDbType.Int);

            cmd.Parameters["@MovieID"].Value = oldMovie.MovieID;

            cmd.Parameters["@NewMovieTitle"].Value = newMovie.MovieTitle;
            cmd.Parameters["@NewMovieDate"].Value = newMovie.MovieDate;

            cmd.Parameters["@OldMovieTitle"].Value = oldMovie.MovieTitle;
            cmd.Parameters["@OldMovieDate"].Value = oldMovie.MovieDate;

            try
            {
                conn.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return result;
        }

        public int UpdateMovieStatus(int movieID, string oldMovieStatus, string newMovieStatus)
        {
            int rows = 0;

            var conn = DBConnection.GetDBConnection();
            var cmd = new SqlCommand("sp_update_movie_status", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@MovieID", SqlDbType.Int);
            cmd.Parameters["@MovieID"].Value = movieID;
            cmd.Parameters.Add("@OldMovieStatusID", SqlDbType.NVarChar, 50);
            cmd.Parameters["@OldMovieStatusID"].Value = oldMovieStatus;
            cmd.Parameters.Add("@NewMovieStatusID", SqlDbType.NVarChar, 50);
            cmd.Parameters["@NewMovieStatusID"].Value = newMovieStatus;

            try
            {
                conn.Open();
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
            }

            return rows;
        }
    }
}
