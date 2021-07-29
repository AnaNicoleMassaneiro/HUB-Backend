using Dapper;
using System.Data;
using System.Linq;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Class
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public User ValidateUser(string usario, string senha)
        {
            using var db = GetMySqlConnection();
            const string sql = @"select Id, Name, Surname, Email, Phone, LastLogon, CreatedOn, ActivationCode, GRR, TypeUser, NoteApp, Login
                from User U
                where Login = @Login and Password = @Password";

            return db.Query<User>(sql, new { Login = usario, Password = senha }, commandType: CommandType.Text).FirstOrDefault();
        }

        public void InsertUser(string usuario, string senha, string nome, string grr, string email)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into User (Login, Password, CreatedOn, LastLogon, Name, GRR, Email) values (@Login, @Password, NOW(), NOW(), @Name, @GRR, @Email)";

            db.Execute(sql, new { Login = usuario, Password = senha, Name = nome, GRR = grr, Email = email }, commandType: CommandType.Text);
        }
    }
}