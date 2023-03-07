using DestifyMovies.Core.Domain;

namespace DestifyMovies.Core.Features.Movies.Seed
{
    public static class MovieSeedData
    {
        public static IList<Movie> GetSeedMovies()
        {
            return new List<Movie>()
            {
                new Movie
                {
                    Id = 1,
                    Title = "The Matrix",
                    Description =
                        "When a beautiful stranger leads computer hacker Neo to a forbidding underworld, he discovers the shocking truth--the life he knows is the elaborate deception of an evil cyber-intelligence.",
                    Director = "The Wachowskis",
                    ReleasedOn = new(1999, 3, 31),
                    Actors = new List<Actor>
                    {
                        new Actor
                        {
                            FirstName = "Keanu",
                            LastName = "Reeves",
                            CountryOfOrigin = "Canada",
                        },
                        new Actor
                        {
                            FirstName = "Laurence",
                            LastName = "Fishburne",
                            CountryOfOrigin = "USA"
                        },
                        new Actor
                        {
                            FirstName = "Carrie-Anne",
                            LastName = "Moss",
                            CountryOfOrigin = "Canada"
                        }
                    }
                },
                new Movie
                {
                    Id = 2,
                    Title = "Pulp Fiction",
                    Description = "The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.",
                    Director = "Quentin Tarantino",
                    ReleasedOn = new(1994,10,14),
                    Actors = new List<Actor>
                    {
                        new Actor
                        {
                            FirstName = "John",
                            LastName = "Travolta",
                            CountryOfOrigin = "USA"
                        },
                        new Actor
                        {
                            FirstName = "Samuel",
                            LastName = "Jackson",
                            CountryOfOrigin = "USA"
                        },
                        new Actor
                        {
                            FirstName = "Uma",
                            LastName = "Thurman",
                            CountryOfOrigin = "USA"
                        }
                    }
                },
                new Movie
                {
                    Id = 3,
                    Title = "Interstellar",
                    Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.",
                    Director = "Christopher Nolan",
                    ReleasedOn = new(2014,10,14),
                    Actors = new List<Actor>
                    {
                        new Actor
                        {
                            FirstName = "Matthew ",
                            LastName = "McConaughey",
                            CountryOfOrigin = "USA"
                        },
                        new Actor
                        {
                            FirstName = "Anne",
                            LastName = "Hathaway",
                            CountryOfOrigin = "USA"
                        },
                        new Actor
                        {
                            FirstName = "Jessica",
                            LastName = "Chastain",
                            CountryOfOrigin = "USA"
                        }
                    },
                    Ratings = new List<MovieRating>
                    {
                        new MovieRating()
                        {
                            Comments = "Excellent soundtrack by Hans Zimmer!",
                            Rating = 7,
                            RatingSubmittedBy = "moviecritic@moviesapi.com"
                        }
                    }
                },
            };
        }

        public static void AddSeedData(DataContext dataContext)
        {
            dataContext.Set<Movie>()
                .AddRange(GetSeedMovies());

            dataContext.SaveChanges();
        }
    }
}
