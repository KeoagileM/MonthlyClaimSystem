using System.Data.SqlClient;
using ClaimSystem.Models;
using System.Diagnostics;
using System.ComponentModel;

namespace ClaimSystem.Services
{
    public class DatabaseServices
    {
        public string ConnectionString { get; }
        private readonly string instanceName = "ClaimSystem";

        public DatabaseServices()
        {
            ConnectionString = $@"Server=(localdb)\{instanceName};Database=ClaimSystem;Trusted_Connection=true;TrustServerCertificate=true;";

            // Create LocalDB instance first, then initialize database
            CreateClaimSystemInstance();
            InitializeDatabase();
        }

        // -----------------------------
        // LocalDB Instance Handling
        // -----------------------------
        private void CreateClaimSystemInstance()
        {
            if (CheckInstanceExists())
            {
                Console.WriteLine($"LocalDB instance '{instanceName}' already exists.");
                return;
            }

            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c sqllocaldb create \"{instanceName}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    Console.WriteLine($"Creating LocalDB instance '{instanceName}'...");
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                        Console.WriteLine($"LocalDB instance '{instanceName}' created successfully!");
                    else
                    {
                        Console.WriteLine($"Error creating instance: {error}");
                        throw new Exception($"Failed to create LocalDB instance: {error}");
                    }
                }

                // Start the instance after creation
                StartInstance();
            }
            catch (Win32Exception ex) when (ex.NativeErrorCode == 2)
            {
                throw new Exception("SQL Server LocalDB is not installed. Please install SQL Server Express LocalDB from Microsoft.");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create LocalDB instance: {ex.Message}");
            }
        }

        private void StartInstance()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c sqllocaldb start \"{instanceName}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    Console.WriteLine($"Starting LocalDB instance '{instanceName}'...");
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    if (process.ExitCode == 0)
                        Console.WriteLine($"LocalDB instance '{instanceName}' started successfully!");
                    else if (!error.Contains("is already running", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine($"Warning: Could not start instance: {error}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not start instance: {ex.Message}");
            }
        }

        private bool CheckInstanceExists()
        {
            try
            {
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = $"/c sqllocaldb info \"{instanceName}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (var process = new Process { StartInfo = processStartInfo })
                {
                    process.Start();
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();
                    process.WaitForExit();

                    // If there's an error containing "doesn't exist", instance doesn't exist
                    if (!string.IsNullOrWhiteSpace(error) &&
                        error.Contains($"LocalDB instance \"{instanceName}\" doesn't exist", StringComparison.OrdinalIgnoreCase))
                    {
                        return false;
                    }

                    // If we get output and no "doesn't exist" error, instance exists
                    return !string.IsNullOrWhiteSpace(output)
                        && !output.Contains("doesn't exist", StringComparison.OrdinalIgnoreCase);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking instance existence: {ex.Message}");
                return false;
            }
        }

        private void InitializeDatabase()
        {
            try
            {
                // First, create database if it doesn't exist
                CreateDatabase();

                // Then create tables and seed data
                CreateTablesAndSeedData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                throw;
            }
        }

        private void CreateDatabase()
        {
            // Use master database connection string for initial creation
            var masterConnectionString = $@"Server=(localdb)\{instanceName};Database=master;Trusted_Connection=true;TrustServerCertificate=true;";

            using (var connection = new SqlConnection(masterConnectionString))
            {
                try
                {
                    connection.Open();

                    string createDbSql = @"
                        IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'ClaimSystem')
                        BEGIN
                            CREATE DATABASE ClaimSystem;
                        END";

                    using (var command = new SqlCommand(createDbSql, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Database 'ClaimSystem' verified/created successfully!");
                    }
                }
                catch (SqlException ex) when (ex.Number == 4060) // Database doesn't exist
                {
                    // This shouldn't happen due to our check, but handle it anyway
                    Console.WriteLine("Database connection failed. Retrying...");
                    Thread.Sleep(2000); // Wait 2 seconds
                    CreateDatabase(); // Recursive retry
                }
            }
        }

        private void CreateTablesAndSeedData()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

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
                    Console.WriteLine("Users table verified/created successfully!");
                }

                using (var command = new SqlCommand(createClaimsTable, connection))
                {
                    command.ExecuteNonQuery();
                    Console.WriteLine("Claims table verified/created successfully!");
                }

                // Seed initial users if they don't exist
                SeedInitialUsers(connection);
            }
        }

        private void SeedInitialUsers(SqlConnection connection)
        {
            string checkUsersSql = "SELECT COUNT(*) FROM Users";
            using (var command = new SqlCommand(checkUsersSql, connection))
            {
                var userCount = (int)command.ExecuteScalar();
                if (userCount == 0)
                {
                    string insertUsersSql = @"
                INSERT INTO Users (Username, PasswordHash, Role) VALUES
                ('lecturer', @lecturerPassword, 'Lecturer'),
                ('coordinator', @coordinatorPassword, 'Coordinator'),
                ('manager', @managerPassword, 'Manager')";

                    using (var insertCommand = new SqlCommand(insertUsersSql, connection))
                    {
                        insertCommand.Parameters.AddWithValue("@lecturerPassword", "1234");
                        insertCommand.Parameters.AddWithValue("@coordinatorPassword", "5678");
                        insertCommand.Parameters.AddWithValue("@managerPassword", "9999");
                        insertCommand.ExecuteNonQuery();
                        Console.WriteLine("Default users seeded successfully!");
                    }
                }
            }
        }
    }
}