namespace CDN.Directory.Core.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public List<string> Skillsets { get; set; }
    public List<string> Hobbies { get; set; }
    public bool IsArchived { get; set; }
}
