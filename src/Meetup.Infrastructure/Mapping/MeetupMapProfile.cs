using AutoMapper;
using Meetup.Core.Domain.Entities;
using Meetup.Infrastructure.Data.Models;

namespace Meetup.Infrastructure.Mapping;

public class MeetupMapProfile : Profile
{
    public MeetupMapProfile()
    {
        CreateMap<MeetupEntity, MeetupDto>()
            .ForMember(dest => dest.Name, opt =>
                opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt =>
                opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.Speaker, opt =>
                opt.MapFrom(src => src.Speaker))
            .ForMember(dest => dest.Time, opt =>
                opt.MapFrom(src => src.Time))

            .ForMember(dest => dest.OrganizerDto, opt =>
                opt.MapFrom(src => new OrganizerDto() { Name = src.Organizer }))
            .ForMember(dest => dest.PlaceDto, opt =>
                opt.MapFrom(src => new PlaceDto() { Name = src.Place }))
            .ForMember(dest => dest.PlanSteps, opt =>
                opt.MapFrom(src => src.Plan
                    .Select(step => new PlanStepDto()
                    {
                        Time = step.Key,
                        Name = step.Value
                    })
                    .ToList()));

        CreateMap<MeetupDto, MeetupEntity>()
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
                opt.MapFrom(src => src.OrganizerDto.Name))
            .ForMember(dest => dest.Place, opt =>
                opt.MapFrom(src => src.PlaceDto.Name))
            .ForMember(dest => dest.Plan, opt =>
                opt.MapFrom(src => src.PlanSteps
                    .OrderBy(e => e.Time)
                    .ToDictionary(e => e.Time, e => e.Name)));
    }
}