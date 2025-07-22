using AutoMapper;
using CDN.Directory.Core.DTOs;
using CDN.Directory.Core.Entities;
using CDN.Directory.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CDN.Directory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MemberController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MemberController(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetMembers()
    {
        var members = await _context.Members
            .Include(m => m.MemberSkillsets).ThenInclude(ms => ms.Skillset)
            .Include(m => m.MemberHobbies).ThenInclude(mh => mh.Hobby)
            .Where(m => !m.IsArchived)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<MemberDto>>(members));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> GetMember(int id)
    {
        var member = await _context.Members
            .Include(m => m.MemberSkillsets).ThenInclude(ms => ms.Skillset)
            .Include(m => m.MemberHobbies).ThenInclude(mh => mh.Hobby)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (member == null) return NotFound();
        return Ok(_mapper.Map<MemberDto>(member));
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> CreateMember(CreateMemberDto createDto)
    {
        var member = _mapper.Map<Member>(createDto);
        member.MemberSkillsets = createDto.SkillsetIds.Select(id => new MemberSkillset { SkillsetId = id }).ToList();
        member.MemberHobbies = createDto.HobbyIds.Select(id => new MemberHobby { HobbyId = id }).ToList();

        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var memberDto = _mapper.Map<MemberDto>(member);
        return CreatedAtAction(nameof(GetMember), new { id = member.Id }, memberDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MemberDto>> UpdateMember(int id, UpdateMemberDto updateDto)
    {
        var member = await _context.Members
            .Include(m => m.MemberSkillsets)
            .Include(m => m.MemberHobbies)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (member == null) return NotFound();

        member.Username = updateDto.Username;
        member.Email = updateDto.Email;
        member.PhoneNumber = updateDto.PhoneNumber;

        member.MemberSkillsets = updateDto.SkillsetIds.Select(skillsetId => new MemberSkillset
        {
            MemberId = id,
            SkillsetId = skillsetId
        }).ToList();

        member.MemberHobbies = updateDto.HobbyIds.Select(hobbyId => new MemberHobby
        {
            MemberId = id,
            HobbyId = hobbyId
        }).ToList();

        await _context.SaveChangesAsync();
        var memberDto = _mapper.Map<MemberDto>(member);
        return Ok(memberDto);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMember(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member == null) return NotFound();

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/archive")]
    public async Task<IActionResult> ArchiveMember(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member == null) return NotFound();

        member.IsArchived = true;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPatch("{id}/unarchive")]
    public async Task<IActionResult> UnarchiveMember(int id)
    {
        var member = await _context.Members.FindAsync(id);
        if (member == null) return NotFound();

        member.IsArchived = false;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("search-username/{username}")]
    public async Task<IActionResult> SearchByUsername(string username)
    {
        var members = await _context.Members
            .Include(m => m.MemberSkillsets).ThenInclude(ms => ms.Skillset)
            .Include(m => m.MemberHobbies).ThenInclude(mh => mh.Hobby)
            .Where(m => m.Username.Contains(username))
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<MemberDto>>(members));
    }

    [HttpGet("search-email/{email}")]
    public async Task<IActionResult> SearchByEmail(string email)
    {
        var members = await _context.Members
            .Include(m => m.MemberSkillsets).ThenInclude(ms => ms.Skillset)
            .Include(m => m.MemberHobbies).ThenInclude(mh => mh.Hobby)
            .Where(m => m.Email.Contains(email))
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<MemberDto>>(members));
    }
}
