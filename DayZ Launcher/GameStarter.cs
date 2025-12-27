using DayZ_Launcher.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DayZ_Launcher
{
    internal class GameStarter
    {
        public static Mainframe mf;

        public static void StartGame(string game, string username, string ip, string password = "")
        {
            if (!TryParseIp(ip, out string serverIp, out string port))
            {
                MessageBox.Show("Ungültiges IP:PORT Format", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string dayzExe = Path.Combine(Downloader.gamePath, "DayZ.exe");

            if (!File.Exists(dayzExe))
            {
                MessageBox.Show($"Missing DayZ.exe in {Downloader.gamePath}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string startParams = BuildStartParams(serverIp, port, username, password);

            if (game == "DayZ_052")
            {
                startParams += " -mod=@cars";
            }

            var startInfo = new ProcessStartInfo
            {
                FileName = dayzExe,
                Arguments = startParams,
                UseShellExecute = true,
                WorkingDirectory = Downloader.gamePath
            };

            Process.Start(startInfo);

            MinimizeLauncher();
            UpdateDiscordRPC(game);
        }

        private static string BuildStartParams(string ip, string port, string username, string password)
        {
            return Settings.Default.startParam
                .Replace("#IP#", ip)
                .Replace("#PORT#", port)
                .Replace("#USERNAME#", username)
                .Replace("#PASSWORD#", password);
        }

        private static bool TryParseIp(string ip, out string serverIp, out string port)
        {
            serverIp = port = string.Empty;

            var parts = ip.Split(':');
            if (parts.Length != 2)
                return false;

            serverIp = parts[0];
            port = parts[1];
            return true;
        }

        private static void MinimizeLauncher()
        {
            mf?.Invoke((MethodInvoker)(() =>
            {
                mf.WindowState = FormWindowState.Minimized;
            }));
        }

        private static void UpdateDiscordRPC(string game)
        {
            string version = game switch
            {
                "DayZ_052" => "DayZ 0.52",
                "DayZ_062" => "DayZ 0.62",
                "DayZ_046" => "DayZ 0.46",
                _ => "DayZ"
            };

            Discord.changeDiscordRPC(version, "Playing on", "OSD Launcher", "logo");
        }
    }
}




//using DayZ_Launcher.Properties;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Forms;

//namespace DayZ_Launcher
//{
//	internal class GameStarter
//	{
//		public static Mainframe mf;

//        public static void startGame(string game, string username, string ip, string password = "")
//        {
//            string[] info = ip.Split(Convert.ToChar(":"));
//            string param = Settings.Default.startParam;
//            param = param.Replace("#IP#", info[0].ToString());
//            param = param.Replace("#PORT#", info[1].ToString());
//            param = param.Replace("#USERNAME#", username);
//            param = param.Replace("#PASSWORD#", password);
//            if (!File.Exists(Downloader.gamePath + "\\DayZ.exe"))
//            {
//                MessageBox.Show("Missing DayZ.exe in " + Downloader.gamePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
//                return;
//            }
//            switch (game)
//            {
//                case "DayZ_052":
//                    Process.Start(Downloader.gamePath + "\\DayZ.exe", param);
//                    Discord.changeDiscordRPC("DayZ 0.52", "Playing on", "OSD Launcher", "logo");
//                    mf.Invoke((MethodInvoker)delegate
//                    {
//                        mf.WindowState = FormWindowState.Minimized;
//                    });
//                    break;
//                case "DayZ_062":
//                    Process.Start(Downloader.gamePath + "\\DayZ.exe", param);
//                    mf.Invoke((MethodInvoker)delegate
//                    {
//                        mf.WindowState = FormWindowState.Minimized;
//                    });
//                    Discord.changeDiscordRPC("DayZ 0.62", "Playing on", "OSD Launcher", "logo");
//                    break;
//                case "DayZ_046":
//                    Process.Start(Downloader.gamePath + "\\DayZ.exe", param);
//                    mf.Invoke((MethodInvoker)delegate
//                    {
//                        mf.WindowState = FormWindowState.Minimized;
//                    });
//                    Discord.changeDiscordRPC("DayZ 0.46", "Playing on", "OSD Launcher", "logo");
//                    break;
//            }
//        }
//    }
//}
