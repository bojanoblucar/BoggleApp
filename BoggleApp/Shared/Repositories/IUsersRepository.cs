using BoggleApp.Game.Setup;

namespace BoggleApp.Shared.Repositories
{
    public interface IUsersRepository
    {
        User CreateUser(string username);
        User GetByConnectionId(string connectionId);
        User GetById(string id);
        void RemoveInactiveUsers();
        void RemoveUser(string connectionId);
    }
}