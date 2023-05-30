using AutoMapper;
using Meetup.Core.Domain;
using Meetup.Infrastructure.SQL;
using System.Numerics;

namespace Meetup.Infrastructure;

public class EventMapProfile : Profile
{
	public EventMapProfile()
	{
		CreateMap<Event, EventInfo>()
			.ForMember(dest => dest.Name, opt => 
				opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Description, opt => 
				opt.MapFrom(src => src.Description))
			.ForMember(dest => dest.Speaker, opt =>
				opt.MapFrom(src => src.Speaker))
			.ForMember(dest => dest.Time, opt => 
				opt.MapFrom(src => src.Time))

			.ForMember(dest => dest.Organizer, opt =>
				opt.MapFrom(src => new Organizer(){Name = src.Organizer}))
			.ForMember(dest => dest.Place, opt => 
				opt.MapFrom(src => new Place(){Name = src.Place}))
			.ForMember(dest => dest.PlanSteps, opt =>
				opt.MapFrom(src => src.Plan
					.Select(step => new PlanStep()
					{
						Time = step.Key, 
						Name = step.Value
					})
					.ToList()));

		CreateMap<EventInfo, Event>()
			.ForMember(dest => dest.Id, opt =>
				opt.MapFrom(src => src.Id))
			.ForMember(dest => dest.Name, opt =>
				opt.MapFrom(src => src.Name))
			.ForMember(dest => dest.Description, opt =>
				opt.MapFrom(src => src.Description))
			.ForMember(dest => dest.Speaker, opt =>
				opt.MapFrom(src => src.Speaker))
			.ForMember(dest => dest.Time, opt =>
				opt.MapFrom(src => src.Time))

			.ForMember(dest => dest.Organizer, opt =>
				opt.MapFrom(src => src.Organizer.Name))
			.ForMember(dest => dest.Place, opt =>
				opt.MapFrom(src => src.Place.Name))
			.ForMember(dest => dest.Plan, opt =>
				opt.MapFrom(src => A(src.PlanSteps)));
	}

	private static Dictionary<DateTime, string> A(IEnumerable<PlanStep> plan)
	{
		return plan
			.OrderBy(e => e.Time)
			.ToDictionary(e => e.Time, e => e.Name);
	}
}