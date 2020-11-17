using System;
using System.Threading;
using System.Threading.Tasks;
using BoggleApp.Shared.Repositories;
using Microsoft.Extensions.Hosting;

namespace BoggleApp.Server.HostedServices
{
    public class BoggleHostedService : IHostedService, IDisposable
    {
        private Timer _roomsTimer;
        private Timer _usersTimer;

        private readonly IRoomRepository roomRepository;
        private readonly IUsersRepository usersRepository;

        private readonly int _cleanUsersRepeatingInMin = 30;
        private readonly int _cleanRoomsRepeatingInMin = 60;

        public BoggleHostedService(IRoomRepository roomRepository, IUsersRepository usersRepository)
        {
            this.roomRepository = roomRepository;
            this.usersRepository = usersRepository;
        }

        public void Dispose()
        {
            _roomsTimer.Dispose();
            _usersTimer.Dispose();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _usersTimer = new Timer(CleanUsers, null, 0, 1000 * 60 * _cleanUsersRepeatingInMin);
            _roomsTimer = new Timer(CleanRooms, null, 0, 1000 * 60 * _cleanRoomsRepeatingInMin);
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _usersTimer.Change(Timeout.Infinite, 0);
            _roomsTimer.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }


        public void CleanUsers(object state)
        {
            usersRepository.RemoveInactiveUsers();
        }

        public void CleanRooms(object state)
        {
            roomRepository.RemoveEmptyRooms();
        }
    }
}
