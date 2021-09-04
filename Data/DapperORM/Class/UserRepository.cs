using Dapper;
using System.Data;
using System.Linq;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;

namespace HubUfpr.Data.DapperORM.Class
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public User ValidateUser(string usuario, string senha)
        {
            using var db = GetMySqlConnection();
            const string sql = @"select Id, Name, Email, LastLogon, CreatedOn, ActivationCode, GRR, IsVendedor, NoteApp, Latitude, Longitude
                from User U
                where (U.GRR = @Login or U.Email = @Login) and U.Password = @Password";

            return db.Query<User>(sql, new { Login = usuario, Password = senha }, commandType: CommandType.Text).FirstOrDefault();
        }

        public void InsertUser(string name, string senha, string email, string grr, bool isVendedor)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into User (Name, Password, Email, GRR, CreatedOn, LastLogon, IsVendedor) values (@Name, @Password, @Email, @GRR, NOW(), NOW(), @IsVendedor)";

            db.Execute(sql, new { Name = name, Password = senha, Email = email, GRR = grr, IsVendedor = isVendedor}, commandType: CommandType.Text);
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