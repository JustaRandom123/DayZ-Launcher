using DayZ_Launcher.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DayZ_Launcher
{
	internal class GameStarter
	{
		public static Mainframe mf;

        public static void startGame(string game, string username, string ip, string password = "")
        {
            string[] info = ip.Split(Convert.ToChar(":"));
            string param = Settings.Default.startParam;
            param = param.Replace("#IP#", info[0].ToString());
            param = param.Replace("#PORT#", info[1].ToString());
            param = param.Replace("#USERNAME#", username);
            param = param.Replace("#PASSWORD#", password);
            if (!File.Exists(Downloader.gamePath + "\\DayZ.exe"))
            {
                MessageBox.Show("Missing DayZ.exe in " + Downloader.gamePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                return;
            }
            switch (game)
            {
                case "DayZ_052":
                    Process.Start(Downloader.gamePath + "\\DayZ.exe", param);
                    Discord.changeDiscordRPC("DayZ 0.52", "Playing on", "OSD Launcher", "logo");
                    mf.Invoke((MethodInvoker)delegate
                    {
                        mf.WindowState = FormWindowState.Minimized;
                    });
                    break;
                case "DayZ_062":
                    Process.Start(Downloader.gamePath + "\\DayZ.exe", param);
                    mf.Invoke((MethodInvoker)delegate
                    {
                        mf.WindowState = FormWindowState.Minimized;
                    });
                    Discord.changeDiscordRPC("DayZ 0.62", "Playing on", "OSD Launcher", "logo");
                    break;
                case "DayZ_046":
                    Process.Start(Downloader.gamePath + "\\DayZ.exe", param);
                    mf.Invoke((MethodInvoker)delegate
                    {
                        mf.WindowState = FormWindowState.Minimized;
                    });
                    Discord.changeDiscordRPC("DayZ 0.46", "Playing on", "OSD Launcher", "logo");
                    break;
            }
        }
    }
}
