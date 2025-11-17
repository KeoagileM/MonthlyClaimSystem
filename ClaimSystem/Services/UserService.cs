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
                                Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                                PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                                Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? null : reader.GetString(reader.GetOrdinal("Department")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                                UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
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

        public async Task<bool> RegisterUserAsync(
            string username,
            string password,
            string role,
            string firstName,
            string lastName,
            string employeeNumber,
            string email = null,
            string phoneNumber = null,
            string department = null)
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
                    INSERT INTO Users (Username, PasswordHash, Role, FirstName, LastName, EmployeeNumber, Email, PhoneNumber, Department)
                    VALUES (@Username, @PasswordHash, @Role, @FirstName, @LastName, @EmployeeNumber, @Email, @PhoneNumber, @Department)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", password);
                    command.Parameters.AddWithValue("@Role", role);
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.Parameters.AddWithValue("@EmployeeNumber", employeeNumber);
                    command.Parameters.AddWithValue("@Email", (object)email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", (object)phoneNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Department", (object)department ?? DBNull.Value);

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

        // HR Methods
        public async Task<List<User>> GetAllLecturersAsync()
        {
            var lecturers = new List<User>();
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Users WHERE Role = 'Lecturer' ORDER BY FirstName, LastName";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        lecturers.Add(new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Username = reader.GetString(reader.GetOrdinal("Username")),
                            PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                            Role = reader.GetString(reader.GetOrdinal("Role")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            EmployeeNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                            Email = reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
                            PhoneNumber = reader.IsDBNull(reader.GetOrdinal("PhoneNumber")) ? null : reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            Department = reader.IsDBNull(reader.GetOrdinal("Department")) ? null : reader.GetString(reader.GetOrdinal("Department")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                            UpdatedAt = reader.GetDateTime(reader.GetOrdinal("UpdatedAt"))
                        });
                    }
                }
            }
            return lecturers;
        }

        public async Task<bool> UpdateLecturerAsync(LecturerUpdateModel lecturer)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = @"
                    UPDATE Users 
                    SET FirstName = @FirstName, 
                        LastName = @LastName,
                        EmployeeNumber = @EmployeeNumber,
                        Email = @Email,
                        PhoneNumber = @PhoneNumber,
                        Department = @Department,
                        UpdatedAt = GETDATE()
                    WHERE Username = @Username AND Role = 'Lecturer'";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", lecturer.Username);
                    command.Parameters.AddWithValue("@FirstName", lecturer.FirstName);
                    command.Parameters.AddWithValue("@LastName", lecturer.LastName);
                    command.Parameters.AddWithValue("@EmployeeNumber", lecturer.EmployeeNumber);
                    command.Parameters.AddWithValue("@Email", (object)lecturer.Email ?? DBNull.Value);
                    command.Parameters.AddWithValue("@PhoneNumber", (object)lecturer.PhoneNumber ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Department", (object)lecturer.Department ?? DBNull.Value);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }
    }
}