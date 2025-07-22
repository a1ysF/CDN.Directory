using CDN.Directory.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CDN.Directory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HobbiesController : ControllerBase
{
    private readonly AppDbContext _context;

    
    public HobbiesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetHobbies()
        {
            var hobbies = await _context.Hobbies.OrderBy(h => h.Name).ToListAsync();
            return Ok(hobbies.Select(h => new { h.Id, h.Name }));
        }
}