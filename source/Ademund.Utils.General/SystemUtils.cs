using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Ademund.Utils
{
    public static class SystemUtils
    {
        public static string GetSystemInfo()
        {
            var sb = new StringBuilder();
            string sysdir = Environment.GetEnvironmentVariable("systemroot");//  c:\windows

            string systeminfoFile = Path.Combine(sysdir, @"system32\systeminfo.exe");
            if (File.Exists(systeminfoFile))
            {
                var psinfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = systeminfoFile,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };
                var p = System.Diagnostics.Process.Start(psinfo);
                StreamReader sr = p.StandardOutput;
                sb.Append(sr.ReadToEnd());
                p.Close();
            }
            sb.AppendLine();
            sb.AppendLine("Running Processes");
            foreach (System.Diagnostics.Process proc in
                System.Diagnostics.Process.GetProcesses().OrderBy(p=>p.ProcessName))
            {
                try
                {
                    sb.AppendFormat("{0,6} {1,-20} {2,12:n0} {3,4} {4,25} {5,-50} {6,-50}",
                                                proc.Id,
                                                proc.ProcessName,
                                                proc.WorkingSet64,
                                                proc.Threads.Count,
                                                proc.StartTime,
                                                proc.MainWindowTitle,
                                                proc.MainModule.FileName
                                                ).AppendLine();
                }
                catch (Exception ex)
                {
                    sb.Append(proc.ProcessName).Append(" ").AppendLine(ex.Message);
                }
            }

            sb.AppendLine();
            sb.AppendLine("Env vars:");
            foreach (string envvar in
                (from string env in Environment.GetEnvironmentVariables().Keys
                 orderby env
                 select env))
            {
                sb.Append(envvar).Append("=").AppendLine(Environment.GetEnvironmentVariable(envvar));
            }
            return sb.ToString();
        }
    }
}