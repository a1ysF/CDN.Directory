using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CDN.Directory.Core.Entities
{
    public class Member
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public bool IsArchived { get; set; } = false;

        
        public ICollection<MemberSkillset> MemberSkillsets { get; set; } = new List<MemberSkillset>();
        public ICollection<MemberHobby> MemberHobbies { get; set; } = new List<MemberHobby>();
    }

    public class SkillsetMaster
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<MemberSkillset> MemberSkillsets { get; set; } = new List<MemberSkillset>();
    }

    public class HobbyMaster
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<MemberHobby> MemberHobbies { get; set; } = new List<MemberHobby>();
    }

    public class MemberSkillset
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int SkillsetId { get; set; }

        public Member Member { get; set; } = null!;
        public SkillsetMaster Skillset { get; set; } = null!;
    }

    public class MemberHobby
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public int HobbyId { get; set; }

        public Member Member { get; set; } = null!;
        public HobbyMaster Hobby { get; set; } = null!;
    }
}