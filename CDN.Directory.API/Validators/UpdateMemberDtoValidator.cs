using CDN.Directory.Core.DTOs;
using CDN.Directory.Infrastructure.Data;
using FluentValidation;

namespace CDN.Directory.Core.Validators
{
    public class UpdateMemberDtoValidator : AbstractValidator<UpdateMemberDto>
    {
        private readonly AppDbContext _context;

        public UpdateMemberDtoValidator(AppDbContext context)
        {
            _context = context;

            // Username validation
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required.")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
                .Must((dto, username) => !_context.Members.Any(m => m.Username == username && m.Id != dto.Id))
                .WithMessage("Username is already taken.");

            // Email validation
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must((dto, email) => !_context.Members.Any(m => m.Email == email && m.Id != dto.Id))
                .WithMessage("Email is already taken.");

            // PhoneNumber validation
            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.");

            // SkillsetIds validation
            RuleFor(x => x.SkillsetIds)
                .NotEmpty().WithMessage("At least one skillset is required.");

            // HobbyIds validation
            RuleFor(x => x.HobbyIds)
                .NotEmpty().WithMessage("At least one hobby is required.");
        }
    }
}
