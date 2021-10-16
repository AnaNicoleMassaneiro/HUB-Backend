using Dapper;
using System.Data;
using System.Linq;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using MySql.Data.MySqlClient;

namespace HubUfpr.Data.DapperORM.Class
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public User ValidateUser(string usuario, string senha)
        {
            using var db = GetMySqlConnection();
            const string sql = @"select Id, Name, Password, Email, LastLogon, CreatedOn, ActivationCode, GRR, IsVendedor, NoteApp, Latitude, Longitude
                from User U
                where (U.GRR = @Login or U.Email = @Login) and U.Password = @Password";

            return db.Query<User>(sql, new { Login = usuario, Password = senha }, commandType: CommandType.Text).FirstOrDefault();
        }

        public int InsertUser(string name, string senha, string email, string grr, bool isVendedor)
        {
            using var db = GetMySqlConnection();
            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = @"insert into User (Name, Password, Email, GRR, CreatedOn, LastLogon, IsVendedor) values (@Name, @Password, @Email, @GRR, NOW(), NOW(), @IsVendedor)";
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@Password", senha);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@GRR", grr);
            cmd.Parameters.AddWithValue("@IsVendedor", isVendedor);
            cmd.ExecuteNonQuery();
            long id = cmd.LastInsertedId;

            return (int)id;
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

        public void UpdateLastLoginTime(int id)
        {
            using var db = GetMySqlConnection();
            const string query = @"update user u set u.LastLogon = CONVERT_TZ(NOW(), '+00:00', '-03:00') where u.Id = @ID;";
            db.Execute(query, new { ID = id }, commandType: CommandType.Text);
        }

        public void InsertVendedor(int idUser, int isAtivo, int isOpen)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into Vendedor (idUser, isAtivo, isOpen) values (@idUser, @isAtivo, @isOpen)";

            db.Execute(sql, new { idUser = idUser, isAtivo = isAtivo, isOpen = isOpen }, commandType: CommandType.Text);
        }

        public void InsertCliente(int idUser)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into Cliente (idUser) values (@idUser)";

            db.Execute(sql, new { idUser = idUser }, commandType: CommandType.Text);
        }

        public bool IsValidVendedor(int id)
        {
            using var db = GetMySqlConnection();
            const string query = @"select idVendedor from Vendedor where idVendedor = @id";
            
            return db.Query<string>(query, new { id = id }, commandType: CommandType.Text).Any();
        }

        public int UpdateUserLocation(int userId, float latitude, float longitude)
        {
            using var db = GetMySqlConnection();
            const string query = @"update User set latitude = @latitude, longitude = @longitude where id = @id";
            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("latitude", latitude);
            cmd.Parameters.AddWithValue("longitude", longitude);
            cmd.Parameters.AddWithValue("id", userId);

            return cmd.ExecuteNonQuery();
        }

        public int UpdatePassword(int userId, string newPassword)
        {
            using var db = GetMySqlConnection();
            const string query = @"update User set password = @newPassword where id = @id";
            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("newPassword", newPassword);
            cmd.Parameters.AddWithValue("id", userId);

            return cmd.ExecuteNonQuery();
        }
    }
}