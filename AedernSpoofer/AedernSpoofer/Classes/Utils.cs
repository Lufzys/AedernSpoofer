using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AedernSpoofer.Classes
{
    class Utils
    {
        private static Random random = new Random();
        public static int RandomInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public static string RandomNumber(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static bool DeleteDirectory(string sDirectoryPath)
        {
            if (Directory.Exists(sDirectoryPath))
            {
                try
                {
                    Directory.Delete(sDirectoryPath, true);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("DeleteDirectory({0}) : {1}", sDirectoryPath, ex.Message));
                    return false;
                }
            }
            else { return false; }
        }

        public static bool Strncmp(string str, byte[] data, int offset)
        {
            for (int i = 0; i < str.Length; ++i)
            {
                if (data[i + offset] != (byte)str[i]) return false;
            }
            return true;
        }

        public static void ExecuteCommand(string command)
        {
            Process process = Process.Start(new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            });
            process.WaitForExit();
            string text = process.StandardOutput.ReadToEnd();
            string text2 = process.StandardError.ReadToEnd();
            int exitCode = process.ExitCode;
            process.Close();
        }
    }
}
