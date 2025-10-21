using ClaimSystem.Models;
using System.Data.SqlClient;
using System.Web.Helpers;

namespace ClaimSystem.Services
{
    public class DatabaseService
    {
        private readonly string _connectionString;

        public DatabaseService()
        {
            _connectionString = @"Server=(localdb)\ClaimSystem;Trusted_Connection=true;TrustServerCertificate=true;";
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    // Create database if it doesn't exist
                    string createDbSql = @"
                        IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'ClaimSystem')
                        BEGIN
                            CREATE DATABASE ClaimSystem;
                        END";

                    using (var command = new SqlCommand(createDbSql, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Switch to ClaimSystem database
                    connection.ChangeDatabase("ClaimSystem");

                    // Create Users table
                    string createUsersTable = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Users' and xtype='U')
                        BEGIN
                            CREATE TABLE Users (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                Username NVARCHAR(50) NOT NULL UNIQUE,
                                PasswordHash NVARCHAR(255) NOT NULL,
                                Role NVARCHAR(20) NOT NULL,
                                CreatedAt DATETIME2 DEFAULT GETDATE()
                            )
                        END";

                    // Create Claims table
                    string createClaimsTable = @"
                        IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Claims' and xtype='U')
                        BEGIN
                            CREATE TABLE Claims (
                                Id INT IDENTITY(1,1) PRIMARY KEY,
                                EmployeeNumber NVARCHAR(20) NOT NULL,
                                LecturerName NVARCHAR(100) NOT NULL,
                                Module NVARCHAR(100) NOT NULL,
                                DateSubmitted DATETIME2 NOT NULL,
                                HourlyRate DECIMAL(10,2) NOT NULL,
                                HoursWorked INT NOT NULL,
                                TotalAmount AS (HourlyRate * HoursWorked),
                                Status NVARCHAR(50) NOT NULL DEFAULT 'Pending',
                                DocumentPath NVARCHAR(500) NULL,
                                RejectionReason NVARCHAR(MAX) NULL,
                                SubmittedBy NVARCHAR(50) NOT NULL,
                                CreatedAt DATETIME2 DEFAULT GETDATE(),
                                UpdatedAt DATETIME2 DEFAULT GETDATE()
                            )
                        END";

                    using (var command = new SqlCommand(createUsersTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    using (var command = new SqlCommand(createClaimsTable, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Seed initial users if they don't exist
                    string checkUsersSql = "SELECT COUNT(*) FROM Users";
                    using (var command = new SqlCommand(checkUsersSql, connection))
                    {
                        var userCount = (int)command.ExecuteScalar();
                        if (userCount == 0)
                        {
                            string insertUsersSql = @"
                                INSERT INTO Users (Username, PasswordHash, Role) VALUES
                                ('lecturer', @lecturerHash, 'Lecturer'),
                                ('coordinator', @coordinatorHash, 'Coordinator'),
                                ('manager', @managerHash, 'Manager')";

                            using (var insertCommand = new SqlCommand(insertUsersSql, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@lecturerHash", BCrypt.Net.BCrypt.HashPassword("1234"));
                                insertCommand.Parameters.AddWithValue("@coordinatorHash", BCrypt.Net.BCrypt.HashPassword("5678"));
                                insertCommand.Parameters.AddWithValue("@managerHash", BCrypt.Net.BCrypt.HashPassword("9999"));
                                insertCommand.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                throw;
            }
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString + "Database=ClaimSystem;");
        }
    }
}