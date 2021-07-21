using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Service.Interface
{
    public interface IUserService
    {
        List<User> GetUserList();

        User GetToken(string usuario, string senha);

        void InsertUser(string usuario, string senha);
    }
}