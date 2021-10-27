using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Interface
{
    public interface IUserRepository
    {
        User ValidateUser(string username, string password);

        int InsertUser(string name, string senha, string email, string grr, bool isVendedor);

        bool IsEmailInUse(string email);

        bool IsGRRInUse(string grr);

        void UpdateLastLoginTime(int id);

        void InsertVendedor(int idUser, int isAtivo, int isOpen);

        void InsertCliente(int idUser);

        bool IsValidVendedor(int id);

        int UpdateUserLocation(int userId, float latitude, float longitude);

        int UpdatePassword(int userId, string newPassword);

        int GetCustomerCode(int id);

        int GetSellerCode(int id);

        User GetUserFromCustomerCode(int id);
    }
}