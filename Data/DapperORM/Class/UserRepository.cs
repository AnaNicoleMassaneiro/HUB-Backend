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

        public bool IsEmailInUse(string email)
        {
            using var db = GetMySqlConnection();
            const string query = @"select Email from User where Email = @Email";
            return db.Query<string>(query, new { Email = email }, commandType: CommandType.Text).Any();
        }

        public bool IsGRRInUse(string grr)
        {
            using var db = GetMySqlConnection();
            const string query = @"select GRR from User where GRR = @grr";
            return db.Query<string>(query, new { GRR = grr }, commandType: CommandType.Text).Any();
        }
    }
}