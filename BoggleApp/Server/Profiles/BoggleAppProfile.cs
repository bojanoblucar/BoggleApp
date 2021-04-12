using System;
using AutoMapper;
using BoggleApp.Game.Setup;
using BoggleApp.Shared;
using BoggleApp.Shared.Shared;
using BoggleApp.Shared.ViewModels;

namespace BoggleApp.Server.Profiles
{
    public class BoggleAppProfile : Profile
    {
        public BoggleAppProfile()
        {
            CreateMap<Room, RoomViewModel>();
            CreateMap<User, UserViewModel>();
            CreateMap<Player, UserViewModel>();
        }
    }
}
