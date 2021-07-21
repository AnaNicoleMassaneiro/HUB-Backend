using Dapper;
using System.Data;
using System.Linq;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Class
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public User ValidateUser(string username, string password)
        {
            using var db = GetMySqlConnection();
            const string sql = @"select Id, Name, Surname, Email, Phone, LastLogon, CreatedOn, ActivationCode, GRR, TypeUser, NoteApp, Login
                from User U
                where Login = @Login and Password = @Password";

            return db.Query<User>(sql, new { Login = username, Password = password }, commandType: CommandType.Text).FirstOrDefault();
        }

        public void InsertUser(string username, string password)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into User (Login, Password, CreatedOn, LastLogon) values (@Login, @Password, NOW(), NOW())";

            db.Execute(sql, new { Login = username, Password = password }, commandType: CommandType.Text);
        }
    }
}