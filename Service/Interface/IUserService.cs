using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Service.Interface
{
    public interface IUserService
    {
        List<User> GetUserList();

        User GetToken(string usuario, string senha);

        void InsertUser(string nome, string senha, string email, string grr, bool isVendedor);

        bool IsEmailInUse(string email);

        bool IsGRRInUse(string grr);
    }
}