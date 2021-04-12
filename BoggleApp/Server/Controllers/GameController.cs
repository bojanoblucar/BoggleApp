using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BoggleApp.Game.Setup;
using BoggleApp.Server.Helpers;
using BoggleApp.Shared.Repositories;
using BoggleApp.Shared.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BoggleApp.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly IRoomRepository roomRepository;
        private readonly IUsersRepository usersRepository;
        private readonly IMapper mapper;

        public GameController(
            IRoomRepository roomRepository,
            IUsersRepository usersRepository,
            IMapper mapper)
        {
            this.roomRepository = roomRepository;
            this.usersRepository = usersRepository;
            this.mapper = mapper;
        }


        [HttpPost("create")]
        public ActionResult<Room> CreateRoom([FromBody] string roomName, [FromServices] IGameTicker gameTicker)
        {
            var room = roomRepository.CreateRoom(roomName, gameTicker);
            return Ok(mapper.Map<RoomViewModel>(room));
        }

        [HttpGet("global")]
        public ActionResult<RoomViewModel> GetGlobalRoom()
        {
            var room = roomRepository.GetGlobalRoom();
            return Ok(mapper.Map<RoomViewModel>(room));
        }

        [HttpGet]
        public ActionResult<RoomViewModel> GetRoomById([FromQuery] string user, [FromQuery] string roomId)
        {
            var room = roomRepository.GetRoomById(roomId);
            if (user != null && !room.HasUser(user))
                return new BadRequestObjectResult("User is not allowed to join room");

            return Ok(mapper.Map<RoomViewModel>(room));
        }

        [HttpGet("rooms")]
        public ActionResult<IEnumerable<RoomViewModel>> GetRooms()
        {
            var rooms = roomRepository.GetRooms();
            return Ok(mapper.Map<IEnumerable<RoomViewModel>>(rooms));
        }

        [HttpPost("user")]
        public ActionResult<LoginResponse> CreateUser([FromBody] string username)
        {           
            var user = usersRepository.CreateUser(username);
            var token = TokenGenerator.GenerateToken(user);

            return Ok(new LoginResponse()
            {
                User = mapper.Map<UserViewModel>(user),
                Token = token
            });
        }

    }
}
