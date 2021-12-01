using Dapper;
using System.Data;
using System.Linq;
using HubUfpr.Data.DapperORM.Interface;
using HubUfpr.Model;
using MySql.Data.MySqlClient;
using System;

namespace HubUfpr.Data.DapperORM.Class
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        public User ValidateUser(string usuario, string senha)
        {
            using var db = GetMySqlConnection();
            const string sql = @"select Id, Name, Password, Email, LastLogon, CreatedOn, ActivationCode, GRR, Telefone, IsVendedor, NoteApp, Latitude, Longitude
                from User U
                where (U.GRR = @Login or U.Email = @Login) and U.Password = @Password";

            var ret = db.Query<User>(sql, new { Login = usuario, Password = senha }, commandType: CommandType.Text).FirstOrDefault();

            db.Close();

            return ret;
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

            db.Close();

            return (int)id;
        }

        public bool IsEmailInUse(string email)
        {
            using var db = GetMySqlConnection();
            const string query = @"select Email from User where Email = @Email";

            var ret =  db.Query<string>(query, new { Email = email }, commandType: CommandType.Text).Any();

            db.Close();

            return ret;
        }

        public bool IsGRRInUse(string grr)
        {
            using var db = GetMySqlConnection();
            const string query = @"select GRR from User where GRR = @grr";
            
            var ret = db.Query<string>(query, new { GRR = grr }, commandType: CommandType.Text).Any();

            db.Close();

            return ret;
        }

        public void UpdateLastLoginTime(int id)
        {
            using var db = GetMySqlConnection();
            const string query = @"update user u set u.LastLogon = CONVERT_TZ(NOW(), '+00:00', '-03:00') where u.Id = @ID;";
            db.Execute(query, new { ID = id }, commandType: CommandType.Text);
            db.Close();
        }

        public void InsertVendedor(int idUser, int isAtivo, int isOpen)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into Vendedor (idUser, isAtivo, isOpen) values (@idUser, @isAtivo, @isOpen)";

            db.Execute(sql, new { idUser = idUser, isAtivo = isAtivo, isOpen = isOpen }, commandType: CommandType.Text);
            db.Close();
        }

        public void InsertCliente(int idUser)
        {
            using var db = GetMySqlConnection();
            const string sql = @"insert into Cliente (idUser) values (@idUser)";

            db.Execute(sql, new { idUser = idUser }, commandType: CommandType.Text);
            db.Close();
        }

        public bool IsValidVendedor(int id)
        {
            using var db = GetMySqlConnection();
            const string query = @"select idVendedor from Vendedor where idVendedor = @id";

            var ret = db.Query<string>(query, new { id }, commandType: CommandType.Text).Any();

            db.Close();

            return ret;
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

            var ret = cmd.ExecuteNonQuery();

            db.Close();

            return ret;
        }

        public int UpdatePassword(int userId, string newPassword)
        {
            using var db = GetMySqlConnection();
            const string query = @"update User set password = @newPassword where id = @id";
            MySqlCommand cmd = db.CreateCommand();

            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("newPassword", newPassword);
            cmd.Parameters.AddWithValue("id", userId);

            var ret = cmd.ExecuteNonQuery();

            db.Close();

            return ret;
        }

        public int GetCustomerCode(int id)
        {
            using var db = GetMySqlConnection();
            string sql = @"select idCliente from Cliente where idUser = @id";
            
            var ret = db.Query<int>(sql, new { id }, commandType: CommandType.Text).FirstOrDefault();

            db.Close();

            return ret;
        }

        public int GetSellerCode(int id)
        {
            using var db = GetMySqlConnection();
            string sql = @"select idVendedor from Vendedor where idUser = @id";
            
            var ret = db.Query<int>(sql, new { id }, commandType: CommandType.Text).FirstOrDefault();

            db.Close();

            return ret;
        }

        public User GetUserFromCustomerCode(int id)
        {
            using var db = GetMySqlConnection();
            const string sql = @"SELECT id, name, password, latitude, longitude, noteApp, email, grr, telefone, lastLogon, createdOn, isVendedor " +
                "FROM User u " +
                "JOIN Cliente c ON c.idUser = u.Id " +
                "WHERE c.idCliente = @id";
            User ret = new User();
            MySqlCommand cmd = db.CreateCommand();

            cmd.Parameters.AddWithValue("id", id);
            cmd.CommandText = sql;

            MySqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                dr.Read();

                ret.Id = (int)dr["id"];
                ret.Name = (string)dr["name"];
                ret.Password = (string)dr["password"];
                if (dr["latitude"] != DBNull.Value) { ret.Latitude = (float)dr["latitude"]; }
                if (dr["longitude"] != DBNull.Value) ret.Longitude = (float)dr["longitude"];
                if (dr["noteApp"] != DBNull.Value) ret.NoteApp = (float)dr["noteApp"];
                ret.Email = (string)dr["email"];
                ret.GRR = (string)dr["grr"];
                ret.Telefone = (string)dr["telefone"];
                ret.LastLogon = (DateTime)dr["lastLogon"];
                ret.CreatedOn = (DateTime)dr["createdOn"];
                ret.IsVendedor = (bool)dr["isVendedor"];

                dr.Close();
                db.Close();

                return ret;
            }
            else
            {
                dr.Close();
                db.Close();

                return null;
            }
        }

        public void UpdateUser(string name, string telefone, int id)
        {
            using var db = GetMySqlConnection();
            MySqlCommand cmd = db.CreateCommand();
            string query = @"UPDATE USER SET ";

            if (name != null)
            {
                query += "Name = @name";
                query += telefone == null ? " " : ", ";
                cmd.Parameters.AddWithValue("name", name);
            }

            if (telefone != null)
            {
                query += "Telefone = @telefone ";
                cmd.Parameters.AddWithValue("telefone", telefone);
            }

            query += "WHERE Id = @id";
            cmd.Parameters.AddWithValue("id", id);

            cmd.CommandText = query;

            cmd.ExecuteNonQuery();

            db.Close();
        }

        public User getUserById(int id)
        {
            using var db = GetMySqlConnection();

            const string sql = @"select id, name, latitude, longitude, noteApp, email, grr, telefone from User where Id = @id";

            User user = db.Query<User>(sql, new { id }, commandType: CommandType.Text).FirstOrDefault();

            var ret = user;

            db.Close();

            return ret;
        }
    }
}