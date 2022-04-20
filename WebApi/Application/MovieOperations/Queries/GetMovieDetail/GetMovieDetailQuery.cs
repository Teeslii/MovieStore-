using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebApi.DBOperations;

namespace WebApi.Application.MovieOperations.Queries.GetMovieDetail
{
    public class GetMovieDetailQuery
    {
        public int MovieId { get; set; }
        private readonly IMovieStoreDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetMovieDetailQuery(IMovieStoreDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

           public MovieDetailViewModel Handle(){
            var movie = _dbContext.Movies.Include(m=>m.Genre).Include(m=>m.Director).Include(m=>m.MovieOfActors).ThenInclude(ma=>ma.Actor).SingleOrDefault(x=>x.Id==MovieId);
            if (movie is null)
                throw new InvalidOperationException("The movie you were looking for was not found.");
                
            MovieDetailViewModel returnObj = _mapper.Map<MovieDetailViewModel>(movie);
            return returnObj;
        }

    }

     public class MovieDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public decimal Price { get; set; }  
        public List<MovieOfActorsDetailQueryViewModel> Actors { get; set; }

        public struct MovieOfActorsDetailQueryViewModel
        {
            public int Id { get; set; }
            public string NameSurname { get; set; }
        }

    }    
}