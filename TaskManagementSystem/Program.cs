using System;
using System.Windows.Forms;
using TaskManagementSystem.Forms;
using TaskManagementSystem.Helpers;

namespace TaskManagementSystem
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Test database connection
            if (!DatabaseHelper.TestConnection())
            {
                MessageBox.Show("Cannot connect to database. Please ensure LocalDB is installed and running.\n\n" +
                    "To fix:\n" +
                    "1. Open SQL Server Object Explorer in Visual Studio\n" +
                    "2. Add SQL Server: (localdb)\\MSSQLLocalDB\n" +
                    "3. Make sure TaskManagementDB database exists\n" +
                    "4. Run the SQL script to create tables if not already done",
                    "Database Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            Application.Run(new LoginForm());
        }
    }
}