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
            return _dbServices.GetConnection();
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
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                EmployeeNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber")),
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
                // Compare plain text passwords
                return password == user.PasswordHash;
            }
            return false;
        }

        public async Task<bool> RegisterUserAsync(string username, string password, string role, string firstName, string lastName, string employeeNumber)
        {
            // Check if username already exists
            var existingUser = await GetUserByUsernameAsync(username);
            if (existingUser != null)
            {
                return false; // Username already exists
            }

            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = @"
            INSERT INTO Users (Username, PasswordHash, Role, FirstName, LastName, EmployeeNumber)
            VALUES (@Username, @PasswordHash, @Role, @FirstName, @LastName, @EmployeeNumber)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", password);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<bool> UsernameExistsAsync(string username)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT COUNT(*) FROM Users WHERE Username = @Username";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    var count = (int)await command.ExecuteScalarAsync();
                    return count > 0;
                }
            }
        }


    }
}