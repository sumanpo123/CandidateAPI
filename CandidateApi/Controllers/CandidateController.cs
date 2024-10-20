using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using CandidateApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace CandidateApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidateController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMemoryCache _cache;

        public CandidateController(AppDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateCandidate([FromBody] Candidate candidate)
        {
            if (candidate == null)
            {
                return BadRequest("Candidate data cannot be null.");
            }

            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(candidate);
            if (!Validator.TryValidateObject(candidate, validationContext, validationResults, true))
            {
                return BadRequest("Invalid candidate data: " + string.Join(", ", validationResults.Select(r => r.ErrorMessage)));
            }

            // Check for existing candidate by email
            var existingCandidate = await _context.Candidates.FirstOrDefaultAsync(c => c.Email == candidate.Email);

            if (existingCandidate != null)
            {
                // Update existing candidate's information
                existingCandidate.FirstName = candidate.FirstName;
                existingCandidate.LastName = candidate.LastName;
                existingCandidate.PhoneNumber = candidate.PhoneNumber;
                existingCandidate.CallInterval = candidate.CallInterval;
                existingCandidate.LinkedInProfileUrl = candidate.LinkedInProfileUrl;
                existingCandidate.GitHubProfileUrl = candidate.GitHubProfileUrl;
                existingCandidate.Comment = candidate.Comment;

                _context.Candidates.Update(existingCandidate);
            }
            else
            {
                // Add new candidate
                await _context.Candidates.AddAsync(candidate);
            }

            await _context.SaveChangesAsync();

            // Cache the candidate for future requests
            _cache.Set(candidate.Email, candidate, TimeSpan.FromMinutes(30));

            return Ok("Candidate created or updated successfully.");
        }

        [HttpGet("{email}")]
        public IActionResult GetCandidate(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Email cannot be null or empty.");
            }

            // Check if the candidate is already cached
            if (_cache.TryGetValue(email, out Candidate cachedCandidate))
            {
                return Ok(cachedCandidate);
            }

            // Retrieve candidate from the database
            var candidate = _context.Candidates.FirstOrDefault(c => c.Email == email);
            if (candidate == null)
            {
                return NotFound("Candidate not found.");
            }

            // Cache the retrieved candidate
            _cache.Set(email, candidate, TimeSpan.FromMinutes(30));

            return Ok(candidate);
        }
    }
}
