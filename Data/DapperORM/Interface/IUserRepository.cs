using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IUserRepository
    {
        User ValidateUser(string username, string password);

        void InsertUser(string username, string password);
    }
}