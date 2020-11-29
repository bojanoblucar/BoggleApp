using System;
using System.Collections.Generic;
using System.Linq;
using BoggleApp.Shared.Enums;

namespace BoggleApp.Shared.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private Dictionary<string, User> users;

        public UsersRepository()
        {
            users = new Dictionary<string, User>();
        }

        public User CreateUser(string username)
        {
            User user = new User(username);
            users.Add(user.Id, user);

            return user;
        }


        public void RemoveUser(string connectionId)
        {
            var user = GetByConnectionId(connectionId);
            if (user != null)
                users.Remove(user.Id);
        }

        public User GetByConnectionId(string connectionId)
        {
            return users.Values.Where(u => u.ConnectionId == connectionId).SingleOrDefault();
        }

        public User GetById(string id)
        {
            return users[id];
        }

        public void RemoveInactiveUsers()
        {         
            var disconnectedUsers = users.Values.Where(u => u.ConnectionStatus == ConnectionStatus.Disconnected
                                                            && u.LastSeen.Value.AddMinutes(5) < DateTime.Now);
            foreach (var user in disconnectedUsers)
            {
                foreach (var room in user.JoinedRooms)
                {
                    room.RemoveUser(user);
                }

                users.Remove(user.Id);
            }

            Console.WriteLine($"counting {users.Count()} users");
            disconnectedUsers = null;
        }
    }
}
