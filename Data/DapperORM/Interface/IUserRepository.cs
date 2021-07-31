using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IUserRepository
    {
        User ValidateUser(string username, string password);

        void InsertUser(string usuario, string senha, string nome, string grr, string email);

        bool IsEmailInUse(string email);

        bool IsGRRInUse(string grr);
    }
}