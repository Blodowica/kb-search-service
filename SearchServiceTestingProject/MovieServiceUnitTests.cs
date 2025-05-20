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
    public class MovieServiceTest
    {
        private readonly Mock<IMovieService> _mockMovieService;
        private readonly MoviesController _controller;

        // Constructor with non-nullable initialization
        public MovieServiceTest()
        {
            _mockMovieService = new Mock<IMovieService>();
            _controller = new MoviesController(_mockMovieService.Object);
        }

        // GetTrendingMovies - Returns OK with movies
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

        // GetTrendingMovies - Returns OK with empty list
        [Fact]
        public async Task GetTrendingMovies_ReturnsOk_WithEmptyList()
        {
            _mockMovieService.Setup(s => s.GetTrendingMovies(1)).ReturnsAsync(new List<MovieDto>());

            var result = await _controller.GetTrendingMovies(1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Empty(returned);
        }

        // GetMovieDetailsById - NotFound when null
        [Fact]
        public async Task GetMovieDetailsById_ReturnsNotFound_WhenNull()
        {
            _mockMovieService.Setup(s => s.MovieDetailsById(1)).ReturnsAsync((MovieDto)null);

            var result = await _controller.GetMovieDetailsById(1);
            Assert.IsType<NotFoundResult>(result.Result);
        }

        // GetMovieDetailsById - Returns OK with movie found
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

        // GetMovieWithFilter - Returns OK when valid filter
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

        // GetMovieWithFilter - Returns OK when no results
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

        // GetMovieBySearch - Returns BadRequest for empty query
        [Fact]
        public async Task GetMovieBySearch_ReturnsBadRequest_WhenQueryEmpty()
        {
            var result = await _controller.GetMovieBySearch("", 1);
            var innerResult = result.Result;
            var badRequest = Assert.IsType<BadRequestObjectResult>(innerResult);
            Assert.Equal("The search query is empty", badRequest.Value);
        }

        // GetMovieBySearch - Returns OK when valid search
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

        // GetMovieBySearch - Returns OK when no results
        [Fact]
        public async Task GetMovieBySearch_ReturnsOk_WhenNoResults()
        {
            _mockMovieService.Setup(s => s.GetMovieBySearch("test", 1)).ReturnsAsync(new List<MovieDto>());

            var result = await _controller.GetMovieBySearch("test", 1);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returned = Assert.IsType<List<MovieDto>>(okResult.Value);
            Assert.Empty(returned);
        }

        // MovieService - Error handling in GetMovieWithFilter
        [Fact]
        public async Task GetMovieWithFilter_ReturnsBadRequest_OnError()
        {
            _mockMovieService.Setup(s => s.GetMovieWithFilter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int?>(),
                                                               It.IsAny<int?>(), It.IsAny<string>(), It.IsAny<string>()))
                             .ThrowsAsync(new Exception("Error in service"));

            var result = await _controller.GetMovieWithFilter("popularity.desc", null, 1, null, null, "en-US");
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal("Error in service", badRequestResult.Value);
        }
    }
}
