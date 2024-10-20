using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using Xunit;
using CandidateApi.Models;
using System.Threading.Tasks;
using CandidateApi.Controllers;

namespace CandidateApi.Tests
{
    public class UnitTest1
    {
        private readonly CandidateController _controller;
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public UnitTest1()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new AppDbContext(options);
            _cache = new MemoryCache(new MemoryCacheOptions());
            _controller = new CandidateController(_context, _cache);
        }

        [Fact]
        public async Task Test_CreateOrUpdateCandidate()
        {
            // Arrange
            var candidate = new Candidate
            {
                FirstName = "Suman",
                LastName = "Pokharel",
                PhoneNumber = "9849325009",
                Email = "sum3pok@gmail.com",
                CallInterval = "7 AM - 10 AM",
                LinkedInProfileUrl = "https://www.linkedin.com/in/suman-pokharel-127140131/",
                GitHubProfileUrl = "https://github.com/suman",
                Comment = "Looking for Software Developer position."
            };

            // Act
            var result = await _controller.CreateOrUpdateCandidate(candidate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Candidate created or updated successfully.", okResult.Value);

            // Verify that the candidate was added to the database
            var addedCandidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Email == candidate.Email);
            Assert.NotNull(addedCandidate); // Ensure candidate was added
            Assert.Equal("Suman", addedCandidate.FirstName);
            Assert.Equal("Pokharel", addedCandidate.LastName);
            Assert.Equal("9849325009", addedCandidate.PhoneNumber);
            Assert.Equal("7 AM - 10 AM", addedCandidate.CallInterval);
            Assert.Equal("Looking for Software Developer position.", addedCandidate.Comment);
        }
    }
}
