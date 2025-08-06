namespace CDN.Directory.Core.DTOs;

public class UpdateMemberDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<int> SkillsetIds { get; set; } = new();
    public List<int> HobbyIds { get; set; } = new();
}
