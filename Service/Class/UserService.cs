﻿using System.Collections.Generic;
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
          //  var passwordHash = Utils.HashUtil.GetSha256FromString(password);

            var ret = _userRepository.ValidateUser(username, password);

            if (ret != null)
            {
                //ret.Token = Utils.JwtManager.GenerateToken(username).Value;
            }
            return ret;
        }

        public void InsertUser(string usuario, string senha, string nome, string grr, string email)
        {
            var passwordHash = Utils.HashUtil.GetSha256FromString(senha);

            _userRepository.InsertUser(usuario, passwordHash, nome, grr, email);
        }
    }
}