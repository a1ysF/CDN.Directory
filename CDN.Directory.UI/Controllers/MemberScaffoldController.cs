using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CDN.Directory.Infrastructure.Data;
using CDN.Directory.Core.Entities;

namespace CDN.Directory.API.Controllers
{
    public class MemberScaffoldController : Controller
    {
        private readonly AppDbContext _context;

        
        public MemberScaffoldController(AppDbContext context)
        {
            _context = context;
        }

        // GET: MemberScaffold
        public async Task<IActionResult> Index(string searchString)
        {
            var membersQuery = _context.Members
                .Include(m => m.MemberSkillsets)
                .ThenInclude(ms => ms.Skillset)
                .Include(m => m.MemberHobbies)
                .ThenInclude(mh => mh.Hobby)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchString))
            {
                membersQuery = membersQuery.Where(m =>
                    m.Username.Contains(searchString) || m.Email.Contains(searchString));
            }

            var members = await membersQuery.ToListAsync();
            return View(members);
        }

        // GET: MemberScaffold/Create
        public IActionResult Create()
        {
            ViewData["Skillsets"] = _context.Skillsets.OrderBy(s => s.Name).ToList();
            ViewData["Hobbies"] = _context.Hobbies.OrderBy(h => h.Name).ToList();
            return View();
        }

        // POST: MemberScaffold/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Member member, string[] skillsetIds, string[] hobbyIds)
        {
            if (ModelState.IsValid)
            {
                // Handle Skillsets
                var skillsetEntities = new List<MemberSkillset>();

                foreach (var skill in skillsetIds)
                {
                    if (int.TryParse(skill, out int existingSkillId))
                    {
                        skillsetEntities.Add(new MemberSkillset { SkillsetId = existingSkillId });
                    }
                    else
                    {
                        var trimmedName = skill.Trim();
                        if (!string.IsNullOrEmpty(trimmedName))
                        {
                            var existingSkill = await _context.Skillsets.FirstOrDefaultAsync(s => s.Name == trimmedName);
                            if (existingSkill == null)
                            {
                                existingSkill = new SkillsetMaster { Name = trimmedName };
                                _context.Skillsets.Add(existingSkill);
                                await _context.SaveChangesAsync(); // Save to generate Id
                            }
                            skillsetEntities.Add(new MemberSkillset { SkillsetId = existingSkill.Id });
                        }
                    }
                }

                // Handle Hobbies
                var hobbyEntities = new List<MemberHobby>();

                foreach (var hobby in hobbyIds)
                {
                    if (int.TryParse(hobby, out int existingHobbyId))
                    {
                        hobbyEntities.Add(new MemberHobby { HobbyId = existingHobbyId });
                    }
                    else
                    {
                        var trimmedName = hobby.Trim();
                        if (!string.IsNullOrEmpty(trimmedName))
                        {
                            var existingHobby = await _context.Hobbies.FirstOrDefaultAsync(h => h.Name == trimmedName);
                            if (existingHobby == null)
                            {
                                existingHobby = new HobbyMaster { Name = trimmedName };
                                _context.Hobbies.Add(existingHobby);
                                await _context.SaveChangesAsync();
                            }
                            hobbyEntities.Add(new MemberHobby { HobbyId = existingHobby.Id });
                        }
                    }
                }

                // Save Member with linked skillsets and hobbies
                member.MemberSkillsets = skillsetEntities;
                member.MemberHobbies = hobbyEntities;

                _context.Add(member);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Skillsets"] = _context.Skillsets.OrderBy(s => s.Name).ToList();
            ViewData["Hobbies"] = _context.Hobbies.OrderBy(h => h.Name).ToList();
            return View(member);
        }


        // GET: MemberScaffold/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var member = await _context.Members
                .Include(m => m.MemberSkillsets)
                .Include(m => m.MemberHobbies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
                return NotFound();

            ViewData["Skillsets"] = _context.Skillsets.OrderBy(s => s.Name).ToList();
            ViewData["Hobbies"] = _context.Hobbies.OrderBy(h => h.Name).ToList();
            ViewData["SelectedSkillsets"] = member.MemberSkillsets.Select(ms => ms.SkillsetId).ToList();
            ViewData["SelectedHobbies"] = member.MemberHobbies.Select(mh => mh.HobbyId).ToList();

            return View(member);
        }

        // POST: MemberScaffold/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Member member, int[] skillsetIds, int[] hobbyIds)
        {
            if (id != member.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existingMember = await _context.Members
                    .Include(m => m.MemberSkillsets)
                    .Include(m => m.MemberHobbies)
                    .FirstOrDefaultAsync(m => m.Id == id);

                if (existingMember == null)
                    return NotFound();

                existingMember.Username = member.Username;
                existingMember.Email = member.Email;
                existingMember.PhoneNumber = member.PhoneNumber;
                existingMember.IsArchived = member.IsArchived;

                existingMember.MemberSkillsets = skillsetIds.Select(sid => new MemberSkillset { MemberId = id, SkillsetId = sid }).ToList();
                existingMember.MemberHobbies = hobbyIds.Select(hid => new MemberHobby { MemberId = id, HobbyId = hid }).ToList();

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Skillsets"] = _context.Skillsets.OrderBy(s => s.Name).ToList();
            ViewData["Hobbies"] = _context.Hobbies.OrderBy(h => h.Name).ToList();
            return View(member);
        }

        // GET: MemberScaffold/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var member = await _context.Members
                .Include(m => m.MemberSkillsets)
                .ThenInclude(ms => ms.Skillset)
                .Include(m => m.MemberHobbies)
                .ThenInclude(mh => mh.Hobby)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (member == null)
                return NotFound();

            return View(member);
        }

        // POST: MemberScaffold/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Archived Members Page
        public async Task<IActionResult> Archived(string searchString)
        {
            var members = from m in _context.Members
                          where m.IsArchived
                          select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                members = members.Where(s => s.Username.Contains(searchString) || s.Email.Contains(searchString));
            }

            return View(await members.ToListAsync());
        }

        // Archive Action
        [HttpPost]
        public async Task<IActionResult> Archive(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                member.IsArchived = true;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // Unarchive Action
        [HttpPost]
        public async Task<IActionResult> Unarchive(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                member.IsArchived = false;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Archived));
        }

    }
}