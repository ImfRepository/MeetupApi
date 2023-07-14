using AutoMapper;
using Meetup.Core.Domain.Models;
using Meetup.Infrastructure.Data.Entities;

namespace Meetup.Infrastructure.Mapping;

public class MeetupMapProfile : Profile
{
	public MeetupMapProfile()
	{
		CreateMap<MeetupModel, MeetupEntity>()
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
				opt.MapFrom(src => new OrganizerEntity
				{
					Id = 0,
					Name = src.Organizer
				}))
			.ForMember(dest => dest.Place, opt =>
				opt.MapFrom(src => new PlaceEntity
				{
					Id = 0,
					Name = src.Place
				}))
			.ForMember(dest => dest.PlanSteps, opt =>
				opt.MapFrom(src => src.Plan
					.Select(step => new PlanStepEntity
					{
						Id = 0,
						Time = step.Key,
						Name = step.Value
					})
					.ToList()));

		CreateMap<MeetupEntity, MeetupModel>()
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
				opt.MapFrom(src => src.Organizer!.Name))
			.ForMember(dest => dest.Place, opt =>
				opt.MapFrom(src => src.Place!.Name))
			.ForMember(dest => dest.Plan, opt =>
				opt.MapFrom(src => src.PlanSteps
					.OrderBy(e => e.Time)
					.ToDictionary(e => e.Time, e => e.Name)));
	}
}