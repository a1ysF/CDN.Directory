using CDN.Directory.Core.DTOs;
using CDN.Directory.Infrastructure.Data;
using FluentValidation;

namespace CDN.Directory.Core.Validators
{
    public class CreateMemberDtoValidator : AbstractValidator<CreateMemberDto>
    {
        private readonly AppDbContext _context;

        public CreateMemberDtoValidator(AppDbContext context)
        {
            _context = context;

            // Username validation
            RuleFor(x => x.Username)
            .Cascade(CascadeMode.Stop) // Stop validating further rules once one fails
            .NotEmpty().WithMessage("Username is required.")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
            .Must(username => !_context.Members.Any(m => m.Username == username))
            .WithMessage("Username is already taken.");

            // Email validation
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .Must(email => !_context.Members.Any(m => m.Email == email))
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
