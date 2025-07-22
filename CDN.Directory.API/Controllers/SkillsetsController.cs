using CDN.Directory.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace CDN.Directory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SkillsetsController : ControllerBase
{
    private readonly AppDbContext _context;

    
    public SkillsetsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkillsets()
        {
            var skillsets = await _context.Skillsets.OrderBy(s => s.Name).ToListAsync();
            return Ok(skillsets.Select(s => new { s.Id, s.Name }));
        }
}