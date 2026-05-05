using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;
using System.Diagnostics;

namespace TaskManagementSystem.Helpers
{
    public class DatabaseHelper
    {
        private static string connectionString;
        private static readonly object lockObj = new object();

        // Logger class for error logging
        private static class Logger
        {
            private static string logFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", "app_log.txt");

            static Logger()
            {
                string logDir = Path.GetDirectoryName(logFile);
                if (!Directory.Exists(logDir))
                    Directory.CreateDirectory(logDir);
            }

            public static void LogError(string message, Exception ex = null)
            {
                try
                {
                    string logEntry = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
                    if (ex != null)
                        logEntry += $"\nException: {ex.Message}\nStack Trace: {ex.StackTrace}";
                    logEntry += "\n----------------------------------------\n";
                    File.AppendAllText(logFile, logEntry);
                }
                catch { /* Silently fail if logging fails */ }
            }
        }

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    lock (lockObj)
                    {
                        if (string.IsNullOrEmpty(connectionString))
                        {
                            // Check if we're running with Docker (environment variable)
                            string useDockerDb = Environment.GetEnvironmentVariable("USE_DOCKER_DB");

                            if (useDockerDb == "true")
                            {
                                // Connection for Docker container database
                                connectionString = @"Server=localhost,1433;Database=TaskManagementDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;";
                            }
                            else
                            {
                                // Original LocalDB connection
                                connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TaskManagementDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;";
                            }
                        }
                    }
                }
                return connectionString;
            }
        }

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        // Execute non-query (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"ExecuteNonQuery failed: {query}", ex);
                throw;
            }
        }

        // Execute scalar (single value)
        public static object ExecuteScalar(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteScalar();
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"ExecuteScalar failed: {query}", ex);
                throw;
            }
        }

        // Execute reader (SELECT queries)
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);
                        using (var adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"ExecuteQuery failed: {query}", ex);
                throw;
            }
        }

        // Test database connection
        public static bool TestConnection()
        {
            try
            {
                using (var conn = GetConnection())
                {
                    conn.Open();
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}