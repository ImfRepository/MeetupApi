using System.Text.RegularExpressions;
using FluentValidation;
using Meetup.Core.Domain.Models;

namespace Meetup.WebApi.Validators;

public class MeetupModelValidator : AbstractValidator<MeetupModel>
{
	private readonly string _regex = "^[^;]+$";
	private readonly string _notMathesMsg = "Symbol ; is not allowed.";

	public MeetupModelValidator()
	{
		RuleFor(m => m.Id)
			.GreaterThanOrEqualTo(0);

		RuleFor(m => m.Name)
			.NotNull()
			.Matches(_regex)
			.WithMessage(_notMathesMsg)
			.Length(3, 50);

		RuleFor(m => m.Description)
			.NotNull()
			.Matches(_regex)
			.WithMessage(_notMathesMsg)
			.Length(1, 1000);

		RuleFor(m => m.Speaker)
			.NotNull()
			.Matches(_regex)
			.WithMessage(_notMathesMsg)
			.Length(2, 50);

		RuleFor(m => m.Organizer)
			.NotNull()
			.Matches(_regex)
			.WithMessage(_notMathesMsg)
			.Length(2, 50);

		RuleFor(m => m.Place)
			.NotNull()
			.Matches(_regex)
			.WithMessage(_notMathesMsg)
			.Length(2, 50);

		RuleForEach(m => m.Plan.Keys)
			.NotNull()
			.LessThan(new DateTime(3000, 1, 1));

		RuleForEach(m => m.Plan.Values)
			.NotNull()
			.Matches(_regex)
			.WithMessage(_notMathesMsg)
			.Length(1, 200);
	}
}