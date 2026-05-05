using System;
using System.Data;
using System.Data.SqlClient;
using TaskManagementSystem.Models;
using TaskManagementSystem.Helpers;
using System.Security.Cryptography;
using System.Text;

namespace TaskManagementSystem.Data
{
    public class UserRepository
    {
        // Hash password for security
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool RegisterUser(RegisterModel model)
        {
            string query = @"INSERT INTO Users (Username, Email, PasswordHash, FullName, Role, CreatedAt) 
                            VALUES (@Username, @Email, @PasswordHash, @FullName, 'User', @CreatedAt)";

            SqlParameter[] parameters = {
                new SqlParameter("@Username", model.Username),
                new SqlParameter("@Email", model.Email),
                new SqlParameter("@PasswordHash", HashPassword(model.Password)),
                new SqlParameter("@FullName", model.FullName),
                new SqlParameter("@CreatedAt", DateTime.Now)
            };

            int result = DatabaseHelper.ExecuteNonQuery(query, parameters);
            return result > 0;
        }

        public User AuthenticateUser(string username, string password)
        {
            string query = "SELECT Id, Username, Email, FullName, Role, CreatedAt, LastLogin FROM Users WHERE Username = @Username AND PasswordHash = @PasswordHash";

            string hashedPassword = HashPassword(password);

            SqlParameter[] parameters = {
                new SqlParameter("@Username", username),
                new SqlParameter("@PasswordHash", hashedPassword)
            };

            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                User user = new User
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Username = row["Username"].ToString(),
                    Email = row["Email"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Role = row["Role"].ToString(),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                    LastLogin = row["LastLogin"] != DBNull.Value ? Convert.ToDateTime(row["LastLogin"]) : (DateTime?)null
                };

                // Update last login
                UpdateLastLogin(user.Id);

                return user;
            }

            return null;
        }

        private void UpdateLastLogin(int userId)
        {
            string query = "UPDATE Users SET LastLogin = @LastLogin WHERE Id = @UserId";
            SqlParameter[] parameters = {
                new SqlParameter("@LastLogin", DateTime.Now),
                new SqlParameter("@UserId", userId)
            };
            DatabaseHelper.ExecuteNonQuery(query, parameters);
        }

        public User GetUserById(int userId)
        {
            string query = "SELECT Id, Username, Email, FullName, Role, CreatedAt, LastLogin FROM Users WHERE Id = @UserId";

            SqlParameter[] parameters = { new SqlParameter("@UserId", userId) };
            DataTable dt = DatabaseHelper.ExecuteQuery(query, parameters);

            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                return new User
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Username = row["Username"].ToString(),
                    Email = row["Email"].ToString(),
                    FullName = row["FullName"].ToString(),
                    Role = row["Role"].ToString(),
                    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                    LastLogin = row["LastLogin"] != DBNull.Value ? Convert.ToDateTime(row["LastLogin"]) : (DateTime?)null
                };
            }
            return null;
        }

        public bool UserExists(string username)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
            SqlParameter[] parameters = { new SqlParameter("@Username", username) };
            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }

        public bool EmailExists(string email)
        {
            string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
            SqlParameter[] parameters = { new SqlParameter("@Email", email) };
            int count = Convert.ToInt32(DatabaseHelper.ExecuteScalar(query, parameters));
            return count > 0;
        }
    }
}