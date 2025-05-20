using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using SearchingService_POC_c_.Controllers;
using SearchingService_POC_c_.DTO;
using SearchingService_POC_c_.Interfaces;
using SearchingService_POC_c_.Models;

namespace SearchServiceTestingProject
{
    public class MovieControllerTests
    {
        private readonly Mock<IMovieService> _mockMovieService;
        private readonly MoviesController _controller;

        public MovieControllerTests()
        {
            _mockMovieService = new Mock<IMovieService>();
            _controller = new MoviesController(_mockMovieService.Object);
        }

        // ✅ GetTrendingMovies - Success
        [Fact]
        public async Task GetTrendingMovies_ReturnsOk_WithMovies()
        {
            var movies = new List<MovieDto> { new() { Title = "Movie A" } };
            _mockMovieService.Setup(s => s.GetTrendingMovies(1)).ReturnsAsync(movies);

            var result = await _controller.GetTrendingMovies(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Single(returned);
        }

        // ✅ GetTrendingMovies - Empty
        [Fact]
        public async Task GetTrendingMovies_ReturnsOk_WithEmptyList()
        {
            _mockMovieService.Setup(s => s.GetTrendingMovies(1)).ReturnsAsync(new List<MovieDto>());

            var result = await _controller.GetTrendingMovies(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Empty(returned);
        }

 

        // ✅ GetMovieDetailsById - NotFound
        [Fact]
        public async Task GetMovieDetailsById_ReturnsNotFound_WhenNull()
        {
            _mockMovieService.Setup(s => s.MovieDetailsById(1)).ReturnsAsync((MovieDto)null);

            var result = await _controller.GetMovieDetailsById(1);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // ✅ GetMovieDetailsById - Success
        [Fact]
        public async Task GetMovieDetailsById_ReturnsOk_WhenMovieFound()
        {
            var movie = new MovieDto { Title = "Test Movie" };
            _mockMovieService.Setup(s => s.MovieDetailsById(1)).ReturnsAsync(movie);

            var result = await _controller.GetMovieDetailsById(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<MovieDto>(okResult.Value);
            Assert.Equal("Test Movie", returned.Title);
        }

        // ✅ GetMovieDetailsById - BadRequest (invalid movie ID)
        [Fact]
        public async Task GetMovieDetailsById_ReturnsBadRequest_WhenInvalidId()
        {
            const  int movieId = -1;
            var result = await _controller.GetMovieDetailsById(movieId); // Assuming valid ID should be positive
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal($"Movie id cannot be {movieId}", badRequest.Value);
        }

        // ✅ GetMovieWithFilter - Valid
        [Fact]
        public async Task GetMovieWithFilter_ReturnsOk_WhenValid()
        {
            var movies = new List<MovieDto> { new() { Title = "Filtered Movie" } };
            _mockMovieService.Setup(s => s.GetMovieWithFilter(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(),
                It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync(movies);

            var result = await _controller.GetMovieWithFilter("popularity.desc", null, 1, null, null, "en-US");
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Single(returned);
        }

        // ✅ GetMovieWithFilter - Empty Result
        [Fact]
        public async Task GetMovieWithFilter_ReturnsOk_WhenNoResults()
        {
            _mockMovieService.Setup(s => s.GetMovieWithFilter(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(),
                It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>())
            ).ReturnsAsync(new List<MovieDto>());

            var result = await _controller.GetMovieWithFilter("popularity.desc", null, 1, null, null, "en-US");
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Empty(returned);
        }

   

        // ✅ GetMovieBySearch - BadRequest
        [Fact]
        public async Task GetMovieBySearch_ReturnsBadRequest_WhenQueryEmpty()                                                                                                                                                                                                                                          
        {
            var result = await _controller.GetMovieBySearch("", 1);
            var innerResult = result.Result;
            var badRequest = Assert.IsType<BadRequestObjectResult>(innerResult);
            Assert.Equal("The search query is empty", badRequest.Value);
        }

        // ✅ GetMovieBySearch - Success
        [Fact]
        public async Task GetMovieBySearch_ReturnsOk_WhenValid()
        {
            var movies = new List<MovieDto> { new() { Title = "Searched Movie" } };
            _mockMovieService.Setup(s => s.GetMovieBySearch("test", 1)).ReturnsAsync(movies);

            var result = await _controller.GetMovieBySearch("test", 1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Single(returned);
        }

        // ✅ GetMovieBySearch - Empty Results
        [Fact]
        public async Task GetMovieBySearch_ReturnsOk_WhenNoResults()
        {
            _mockMovieService.Setup(s => s.GetMovieBySearch("test", 1)).ReturnsAsync(new List<MovieDto>());

            var result = await _controller.GetMovieBySearch("test", 1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Empty(returned);
        }

        // ✅ GetMovieBySearch - BadRequest (invalid query)
        [Fact]
        public async Task GetMovieBySearch_ReturnsBadRequest_WhenInvalidQuery()
        {
            var result = await _controller.GetMovieBySearch(null, 1); // Invalid query (null)
            var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("The search query is empty", badRequest.Value);
        }
    }
}
