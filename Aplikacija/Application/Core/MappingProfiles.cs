using Application.Activities;
using Application.Courts;
using Application.Messages;
using Application.PrivateActivities;
using Application.Teams;
using Application.TimeSlot;
using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Core
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, Activity>()
                .ForMember(dest => dest.Court, opt => opt.Ignore())
                .ForMember(dest => dest.CourtId, opt => opt.Ignore())
                .ForMember(dest => dest.TimeSlot, opt => opt.Ignore())
                .ForMember(dest => dest.TimeSlotId, opt => opt.Ignore())
                .ForMember(dest => dest.NumOfParticipants, opt => opt.Ignore());
            CreateMap<Activity, ActivityDto>()
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Participants.FirstOrDefault(x => x.IsHost).AppUser.UserName))
                .ForMember(d => d.TimeSlot, o => o.MapFrom(s => new TimeSlotDto
                {
                    StartTime = s.TimeSlot.StartTime,
                    EndTime = s.TimeSlot.EndTime,
                    Day = s.TimeSlot.Day
                }))
                .ForMember(d => d.Participants, o => o.MapFrom(s => s.Participants.Select(p => p.AppUser)));

            CreateMap<PrivateActivity, PrivateActivityDto>()
                .ForMember(d => d.HostUsername, o => o.MapFrom(s => s.Participants.FirstOrDefault(x => x.IsHost).AppUser.UserName))
                .ForMember(d => d.TimeSlot, o => o.MapFrom(s => new TimeSlotDto
                {
                    StartTime = s.TimeSlot.StartTime,
                    EndTime = s.TimeSlot.EndTime,
                    Day = s.TimeSlot.Day
                }))
                .ForMember(d => d.Participants, o => o.MapFrom(s => s.Participants.Select(p => p.AppUser)))
                .ForMember(d => d.TeamName, o => o.MapFrom(s => s.Team.Name));


            CreateMap<PrivateActivity, PrivateActivity>()
                .ForMember(dest => dest.Court, opt => opt.Ignore())
                .ForMember(dest => dest.CourtId, opt => opt.Ignore())
                .ForMember(dest => dest.TimeSlot, opt => opt.Ignore())
                .ForMember(dest => dest.TimeSlotId, opt => opt.Ignore())
                .ForMember(dest => dest.NumOfParticipants, opt => opt.Ignore())
                .ForMember(dest => dest.Team, opt => opt.Ignore())
                .ForMember(dest => dest.TeamId, opt => opt.Ignore());



            CreateMap<Team, TeamDto>()
                .ForMember(d => d.CaptainUsername, o => o.MapFrom(s => s.Participants.FirstOrDefault(x => x.IsCaptain).AppUser.UserName))
                .ForMember(d => d.Participants, o => o.MapFrom(s => s.Participants.Select(p => p.AppUser)));

            CreateMap<Team, Team>()
                .ForMember(dest => dest.Participants, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<ActivityParticipant, Profiles.Profile>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.AppUser.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.AppUser.DisplayName))
                .ForMember(d => d.Address, o => o.MapFrom(s => s.AppUser.Address))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.AppUser.Bio));

            CreateMap<Court, Court>()
                .ForMember(dest => dest.Manager, opt => opt.Ignore())
                .ForMember(dest => dest.ManagerId, opt => opt.Ignore());

            CreateMap<Domain.TimeSlot, TimeSlotDto>();


            CreateMap<Court, CourtDto>()
                .ForMember(d => d.Activities, o => o.MapFrom(s => s.Activities))
                .ForMember(d => d.ManagerUsername, o => o.MapFrom(s => s.Manager.UserName));
            CreateMap<Activity, Profiles.Event>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
                .ForMember(d => d.Date, o => o.MapFrom(s => s.Date))
                .ForMember(d => d.Sport, o => o.MapFrom(s => s.Sport))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.Description))
                .ForMember(d => d.NumOfParticipants, o => o.MapFrom(s => s.NumOfParticipants))
                .ForMember(d => d.MaxParticipants, o => o.MapFrom(s => s.MaxParticipants))
                .ForMember(d => d.TimeSlot, o => o.MapFrom(s => s.TimeSlot))
                .ForMember(d => d.HostName, o => o.MapFrom(s => s.Participants.FirstOrDefault(p => p.IsHost).AppUser.UserName))
                .ForMember(d => d.Participants, o => o.MapFrom(s => s.Participants.Select(p => p.AppUser)));

            CreateMap<AppUser, Profiles.Profile>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(d => d.ReceivedRatings, o => o.MapFrom(s => s.ReceivedRatings));

            CreateMap<ActivityParticipant, ParticipantDto>();

            CreateMap<AppUser, ParticipantDto>();

            CreateMap<Rating, Profiles.RatingDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.RatedByUser.UserName));

            CreateMap<Profiles.Profile, Profiles.ProfileDto>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Image))
                .ForMember(d => d.ReceivedRatings, o => o.MapFrom(s => s.ReceivedRatings));

            CreateMap<AppUser, Profiles.ProfileDto>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Image.Url))
                .ForMember(d => d.ReceivedRatings, o => o.MapFrom(s => s.ReceivedRatings))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id));


            CreateMap<Message, MessageDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Author.Photos.FirstOrDefault(x => x.IsMain).Url));

           







        }

    }
}