using static Meetup.Core.Application.Common.Validation.ValidationConstants;

namespace Meetup.Core.Application.Data.Meetups.Commands.UpdateMeetup;

internal class UpdateMeetupCommandValidator : AbstractValidator<UpdateMeetupCommand>
{
    public UpdateMeetupCommandValidator()
    {
        RuleFor(m => m.Name)
            .NotNull()
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(3, 50);

        RuleFor(m => m.Description)
            .NotNull()
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(1, 1000);

        RuleFor(m => m.Speaker)
            .NotNull()
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(2, 50);

        RuleFor(m => m.OrganizerId)
            .GreaterThan(0);

        RuleFor(m => m.PlaceId)
            .GreaterThan(0);

        RuleFor(m => m.Time)
            .GreaterThan(MinDateTime)
            .LessThan(MaxDateTime);
    }
}