using System.Data.SqlClient;
using ClaimSystem.Models;

namespace ClaimSystem.Services
{
    public class UserService
    {
        private readonly DatabaseServices _dbServices;

        public UserService(DatabaseServices dbServices)
        {
            _dbServices = dbServices;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_dbServices.ConnectionString);
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Users WHERE Username = @Username";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new User
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Username = reader.GetString(reader.GetOrdinal("Username")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                                Role = reader.GetString(reader.GetOrdinal("Role")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user != null)
            {
                return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
            }
            return false;
        }
    }
}