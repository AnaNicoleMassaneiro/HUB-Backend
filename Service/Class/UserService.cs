using System.Collections.Generic;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using HubUfpr.Service.Interface;

namespace HubUfpr.Service.Class
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetUserList()
        {
            var obj = new List<User>();
            return obj;
        }

        public User GetToken(string username, string password)
        {
            var passwordHash = Utils.HashUtil.GetSha256FromString(password);

            var ret = _userRepository.ValidateUser(username, passwordHash);

            return ret;
        }

        public int InsertUser(string name, string senha, string email, string grr, bool isVendedor)
        {
            var passwordHash = Utils.HashUtil.GetSha256FromString(senha);

            var usuario = _userRepository.InsertUser(name, passwordHash, email, grr, isVendedor);

            return usuario;
        }

        public bool IsEmailInUse(string email)
        {
            return _userRepository.IsEmailInUse(email);
        }

        public bool IsGRRInUse(string grr)
        {
            return _userRepository.IsGRRInUse(grr);
        }

        public void UpdateLastLoginTime(int id)
        {
            _userRepository.UpdateLastLoginTime(id);
        }

        public void InsertVendedor(int idUser, int isAtivo, int isOpen)
        {
            _userRepository.InsertVendedor(idUser, isAtivo, isOpen);
        }

        public void InsertCliente(int idUser)
        {
            _userRepository.InsertCliente(idUser);
        }

        public bool IsValidVendedor(int id)
        {
            return _userRepository.IsValidVendedor(id);
        }

        public int UpdateUserLocation(int userId, float latitude, float longitude)
        {
            return _userRepository.UpdateUserLocation(userId, latitude, longitude);
        }

        public int UpdatePassword(int userId, string newPassword)
        {
            return _userRepository.UpdatePassword(userId, Utils.HashUtil.GetSha256FromString(newPassword));
        }

        public int GetCustomerCode(int id)
        {
            return _userRepository.GetCustomerCode(id);
        }

        public int GetSellerCode(int id)
        {
            return _userRepository.GetSellerCode(id);
        }
    }
}