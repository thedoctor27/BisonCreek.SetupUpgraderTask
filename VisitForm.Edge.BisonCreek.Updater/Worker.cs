using System.Diagnostics;

namespace VisitForm.Edge.BisonCreek.Updater
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private Timer updateTimer;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            updateTimer = new Timer(CheckForUpdates, null, System.TimeSpan.Zero, System.TimeSpan.FromSeconds(30));

        }
        private void CheckForUpdates(object state)
        {
            _logger.LogInformation("Checking update at: {time}", DateTimeOffset.Now);

            // Run the installer if an update is available
            string directoryPath = @"C:\Temp\";
            string fileName = "install.exe";
            string filePath = Path.Combine(directoryPath, fileName);

            if (File.Exists(filePath))
                InstallUpdate();
        }
        private void InstallUpdate()
        {
            try
            {
                // Path to your installer
                string installerPath = @"C:\Temp\install.exe";

                // Create a new process
                ProcessStartInfo processInfo = new ProcessStartInfo
                {
                    FileName = installerPath,
                    Arguments = "/S", // Run in silent mode
                    UseShellExecute = false,
                    CreateNoWindow = true, // Hide the installer window
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process process = Process.Start(processInfo))
                {
                    // Optionally capture output and errors
                    string output = process.StandardOutput.ReadToEnd();
                    string error = process.StandardError.ReadToEnd();

                    // Wait for the process to complete
                    process.WaitForExit();

                    // Optionally log the output and errors
                    if (!string.IsNullOrEmpty(output))
                    {
                        // Log the output (e.g., write to a file or event log)
                        File.AppendAllText(@"C:\Temp\EdgeTest.txt", "Installer Output: " + output + Environment.NewLine);
                    }

                    if (!string.IsNullOrEmpty(error))
                    {
                        // Log the errors
                        File.AppendAllText(@"C:\Temp\EdgeTest.txt", "Installer Error: " + error + Environment.NewLine);
                    }

                    // Optionally check the exit code
                    int exitCode = process.ExitCode;
                    File.AppendAllText(@"C:\Temp\EdgeTest.txt", "Installer Exit Code: " + exitCode + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                // Log any exceptions
                File.AppendAllText(@"C:\Temp\EdgeTest.txt", "Exception while running installer: " + ex.Message + Environment.NewLine);
            }
        }
    }
}
