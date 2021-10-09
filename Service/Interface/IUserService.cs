using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Service.Interface
{
    public interface IUserService
    {
        List<User> GetUserList();

        User GetToken(string usuario, string senha);

        int InsertUser(string nome, string senha, string email, string grr, bool isVendedor);

        bool IsEmailInUse(string email);

        bool IsGRRInUse(string grr);

        void UpdateLastLoginTime(int id);

        void InsertVendedor(int idUser, int isAtivo, int isOpen);

        void InsertCliente(int idUser);

        bool IsValidVendedor(int id);
    }
}