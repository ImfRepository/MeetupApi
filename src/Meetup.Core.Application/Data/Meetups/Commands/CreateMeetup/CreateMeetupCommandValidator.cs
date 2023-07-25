using static Meetup.Core.Application.Common.Validation.ValidationConstants;

namespace Meetup.Core.Application.Data.Meetups.Commands.CreateMeetup;

public class CreateMeetupCommandValidator : AbstractValidator<CreateMeetupCommand>
{
    public CreateMeetupCommandValidator()
    {
        RuleFor(m => m.Name)
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(3, 50);

        RuleFor(m => m.Description)
            .Matches(NoSemicolonRegex)
            .WithMessage(NoSemicolonMsg)
            .Length(1, 1000);

        RuleFor(m => m.Speaker)
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