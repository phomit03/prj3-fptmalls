using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPTMallsProject.Models
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetAllMovies();

        Movie GetMovie(int id);

        Movie AddMovie(Movie newMovie);

        Movie EditMovie(Movie changedMovie);

        Movie DeleteMovie(int id);
    }
}
