using System.Collections.Generic;
using HubUfpr.Model;

namespace HubUfpr.Service.Interface
{
    public interface IUserService
    {
        User GetToken(string usuario, string senha);

        int InsertUser(string nome, string senha, string email, string grr, bool isVendedor);

        bool IsEmailInUse(string email);

        bool IsGRRInUse(string grr);

        void UpdateLastLoginTime(int id);

        void InsertVendedor(int idUser, int isAtivo, int isOpen);

        void InsertCliente(int idUser);

        bool IsValidVendedor(int id);

        int UpdateUserLocation(int userId, float Latitude, float Longitude);

        int UpdatePassword(int userId, string newPassword);

        public int GetCustomerCode(int id);

        public int GetSellerCode(int id);

        public void UpdateUser(string nome, string telefone, int id);

        User GetUserById(int id);
    }
}