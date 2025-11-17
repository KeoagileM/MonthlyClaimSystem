using System.Data.SqlClient;
using ClaimSystem.Models;


namespace ClaimSystem.Services
{
    public class ClaimService
    {
        private readonly DatabaseServices _dbServices;

        public ClaimService(DatabaseServices dbServices)
        {
            _dbServices = dbServices;
        }

        private SqlConnection GetConnection()
        {
            return _dbServices.GetConnection();
        }

        public async Task<List<Claim>> GetAllClaimsAsync()
        {
            var claims = new List<Claim>();
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Claims ORDER BY CreatedAt DESC";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        claims.Add(new Claim
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            EmployeeNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                            LecturerName = reader.GetString(reader.GetOrdinal("LecturerName")),
                            Module = reader.GetString(reader.GetOrdinal("Module")),
                            DateSubmitted = reader.GetDateTime(reader.GetOrdinal("DateSubmitted")),
                            HourlyRate = reader.GetDecimal(reader.GetOrdinal("HourlyRate")),
                            HoursWorked = reader.GetInt32(reader.GetOrdinal("HoursWorked")),
                            TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            DocumentPath = reader.IsDBNull(reader.GetOrdinal("DocumentPath")) ? null : reader.GetString(reader.GetOrdinal("DocumentPath")),
                            RejectionReason = reader.IsDBNull(reader.GetOrdinal("RejectionReason")) ? null : reader.GetString(reader.GetOrdinal("RejectionReason")),
                            SubmittedBy = reader.GetString(reader.GetOrdinal("SubmittedBy")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                        });
                    }
                }
            }
            return claims;
        }

        public async Task<List<Claim>> GetClaimsByUserAsync(string username)
        {
            var claims = new List<Claim>();
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Claims WHERE SubmittedBy = @Username ORDER BY CreatedAt DESC";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            claims.Add(new Claim
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                EmployeeNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                                LecturerName = reader.GetString(reader.GetOrdinal("LecturerName")),
                                Module = reader.GetString(reader.GetOrdinal("Module")),
                                DateSubmitted = reader.GetDateTime(reader.GetOrdinal("DateSubmitted")),
                                HourlyRate = reader.GetDecimal(reader.GetOrdinal("HourlyRate")),
                                HoursWorked = reader.GetInt32(reader.GetOrdinal("HoursWorked")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                DocumentPath = reader.IsDBNull(reader.GetOrdinal("DocumentPath")) ? null : reader.GetString(reader.GetOrdinal("DocumentPath")),
                                RejectionReason = reader.IsDBNull(reader.GetOrdinal("RejectionReason")) ? null : reader.GetString(reader.GetOrdinal("RejectionReason")),
                                SubmittedBy = reader.GetString(reader.GetOrdinal("SubmittedBy")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            });
                        }
                    }
                }
            }
            return claims;
        }

        public async Task<List<Claim>> GetPendingClaimsAsync()
        {
            var claims = new List<Claim>();
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Claims WHERE Status = 'Pending' ORDER BY CreatedAt";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        claims.Add(new Claim
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            EmployeeNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                            LecturerName = reader.GetString(reader.GetOrdinal("LecturerName")),
                            Module = reader.GetString(reader.GetOrdinal("Module")),
                            DateSubmitted = reader.GetDateTime(reader.GetOrdinal("DateSubmitted")),
                            HourlyRate = reader.GetDecimal(reader.GetOrdinal("HourlyRate")),
                            HoursWorked = reader.GetInt32(reader.GetOrdinal("HoursWorked")),
                            TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            DocumentPath = reader.IsDBNull(reader.GetOrdinal("DocumentPath")) ? null : reader.GetString(reader.GetOrdinal("DocumentPath")),
                            RejectionReason = reader.IsDBNull(reader.GetOrdinal("RejectionReason")) ? null : reader.GetString(reader.GetOrdinal("RejectionReason")),
                            SubmittedBy = reader.GetString(reader.GetOrdinal("SubmittedBy")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                        });
                    }
                }
            }
            return claims;
        }

        public async Task<List<Claim>> GetCoordinatorApprovedClaimsAsync()
        {
            var claims = new List<Claim>();
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Claims WHERE Status = 'Coordinator Approved' ORDER BY CreatedAt";

                using (var command = new SqlCommand(sql, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        claims.Add(new Claim
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            EmployeeNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                            LecturerName = reader.GetString(reader.GetOrdinal("LecturerName")),
                            Module = reader.GetString(reader.GetOrdinal("Module")),
                            DateSubmitted = reader.GetDateTime(reader.GetOrdinal("DateSubmitted")),
                            HourlyRate = reader.GetDecimal(reader.GetOrdinal("HourlyRate")),
                            HoursWorked = reader.GetInt32(reader.GetOrdinal("HoursWorked")),
                            TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                            Status = reader.GetString(reader.GetOrdinal("Status")),
                            DocumentPath = reader.IsDBNull(reader.GetOrdinal("DocumentPath")) ? null : reader.GetString(reader.GetOrdinal("DocumentPath")),
                            RejectionReason = reader.IsDBNull(reader.GetOrdinal("RejectionReason")) ? null : reader.GetString(reader.GetOrdinal("RejectionReason")),
                            SubmittedBy = reader.GetString(reader.GetOrdinal("SubmittedBy")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                        });
                    }
                }
            }
            return claims;
        }

        public async Task AddClaimAsync(Claim claim)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = @"
                    INSERT INTO Claims (EmployeeNumber, LecturerName, Module, DateSubmitted, HourlyRate, HoursWorked, DocumentPath, SubmittedBy)
                    VALUES (@EmployeeNumber, @LecturerName, @Module, @DateSubmitted, @HourlyRate, @HoursWorked, @DocumentPath, @SubmittedBy)";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@EmployeeNumber", claim.EmployeeNumber);
                    command.Parameters.AddWithValue("@LecturerName", claim.LecturerName);
                    command.Parameters.AddWithValue("@Module", claim.Module);
                    command.Parameters.AddWithValue("@DateSubmitted", claim.DateSubmitted);
                    command.Parameters.AddWithValue("@HourlyRate", claim.HourlyRate);
                    command.Parameters.AddWithValue("@HoursWorked", claim.HoursWorked);
                    command.Parameters.AddWithValue("@DocumentPath", (object)claim.DocumentPath ?? DBNull.Value);
                    command.Parameters.AddWithValue("@SubmittedBy", claim.SubmittedBy);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<bool> UpdateClaimStatusAsync(int id, string status, string rejectionReason = null)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = @"
                    UPDATE Claims 
                    SET Status = @Status, 
                        RejectionReason = @RejectionReason,
                        UpdatedAt = GETDATE()
                    WHERE Id = @Id";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Status", status);
                    command.Parameters.AddWithValue("@RejectionReason", (object)rejectionReason ?? DBNull.Value);

                    var rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected > 0;
                }
            }
        }

        public async Task<decimal> GetTotalAcceptedAmountAsync()
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT ISNULL(SUM(TotalAmount), 0) FROM Claims WHERE Status = 'Accepted'";

                using (var command = new SqlCommand(sql, connection))
                {
                    var result = await command.ExecuteScalarAsync();
                    return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
                }
            }
        }

        public async Task<int> GetPendingClaimsCountAsync()
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT COUNT(*) FROM Claims WHERE Status = 'Pending'";

                using (var command = new SqlCommand(sql, connection))
                {
                    return (int)await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<int> GetCoordinatorApprovedCountAsync()
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT COUNT(*) FROM Claims WHERE Status = 'Coordinator Approved'";

                using (var command = new SqlCommand(sql, connection))
                {
                    return (int)await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<int> GetTotalClaimsCountAsync()
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT COUNT(*) FROM Claims";

                using (var command = new SqlCommand(sql, connection))
                {
                    return (int)await command.ExecuteScalarAsync();
                }
            }
        }

        public async Task<Claim> GetClaimByIdAsync(int id)
        {
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Claims WHERE Id = @Id";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Claim
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                EmployeeNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                                LecturerName = reader.GetString(reader.GetOrdinal("LecturerName")),
                                Module = reader.GetString(reader.GetOrdinal("Module")),
                                DateSubmitted = reader.GetDateTime(reader.GetOrdinal("DateSubmitted")),
                                HourlyRate = reader.GetDecimal(reader.GetOrdinal("HourlyRate")),
                                HoursWorked = reader.GetInt32(reader.GetOrdinal("HoursWorked")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                DocumentPath = reader.IsDBNull(reader.GetOrdinal("DocumentPath")) ? null : reader.GetString(reader.GetOrdinal("DocumentPath")),
                                RejectionReason = reader.IsDBNull(reader.GetOrdinal("RejectionReason")) ? null : reader.GetString(reader.GetOrdinal("RejectionReason")),
                                SubmittedBy = reader.GetString(reader.GetOrdinal("SubmittedBy")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // HR Methods
        public async Task<HrReports> GenerateHrReportAsync(DateTime? startDate = null, DateTime? endDate = null)
        {
            var report = new HrReports();

            // Get approved claims within date range
            var allClaims = await GetAllClaimsAsync();
            var approvedClaims = allClaims.Where(c => c.Status == "Accepted").ToList();

            if (startDate.HasValue)
                approvedClaims = approvedClaims.Where(c => c.DateSubmitted >= startDate.Value).ToList();

            if (endDate.HasValue)
                approvedClaims = approvedClaims.Where(c => c.DateSubmitted <= endDate.Value).ToList();

            report.ApprovedClaims = approvedClaims.OrderByDescending(c => c.DateSubmitted).ToList();
            report.TotalAmount = approvedClaims.Sum(c => c.TotalAmount);
            report.TotalClaims = approvedClaims.Count;

            // Get total lecturers
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT COUNT(*) FROM Users WHERE Role = 'Lecturer'";
                using (var command = new SqlCommand(sql, connection))
                {
                    report.TotalLecturers = (int)await command.ExecuteScalarAsync();
                }
            }

            return report;
        }

        public async Task<List<Claim>> GetClaimsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var claims = new List<Claim>();
            using (var connection = GetConnection())
            {
                await connection.OpenAsync();
                string sql = "SELECT * FROM Claims WHERE DateSubmitted BETWEEN @StartDate AND @EndDate ORDER BY DateSubmitted DESC";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            claims.Add(new Claim
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                EmployeeNumber = reader.GetString(reader.GetOrdinal("EmployeeNumber")),
                                LecturerName = reader.GetString(reader.GetOrdinal("LecturerName")),
                                Module = reader.GetString(reader.GetOrdinal("Module")),
                                DateSubmitted = reader.GetDateTime(reader.GetOrdinal("DateSubmitted")),
                                HourlyRate = reader.GetDecimal(reader.GetOrdinal("HourlyRate")),
                                HoursWorked = reader.GetInt32(reader.GetOrdinal("HoursWorked")),
                                TotalAmount = reader.GetDecimal(reader.GetOrdinal("TotalAmount")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                DocumentPath = reader.IsDBNull(reader.GetOrdinal("DocumentPath")) ? null : reader.GetString(reader.GetOrdinal("DocumentPath")),
                                RejectionReason = reader.IsDBNull(reader.GetOrdinal("RejectionReason")) ? null : reader.GetString(reader.GetOrdinal("RejectionReason")),
                                SubmittedBy = reader.GetString(reader.GetOrdinal("SubmittedBy")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                            });
                        }
                    }
                }
            }
            return claims;
        }
    }
}