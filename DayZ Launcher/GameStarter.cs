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
		public static void startGame(string game,string username, string ip)
		{
			string[] info = ip.Split(Convert.ToChar(":"));
			string param = Settings.Default.startParam;
			param = param.Replace("#IP#", info[0].ToString());
			param = param.Replace("#PORT#", info[1].ToString());
			param = param.Replace("#USERNAME#", username);




			if (game == "DayZ_028")
			{
				if (File.Exists(Downloader.gamePath + "\\DayZ.exe"))
				{
					Process.Start(Downloader.gamePath + "\\DayZ.exe", param);
					//Mainframe.ExecuteAsAdmin(Downloader.gamePath + @"\DayZ.exe", param);
					Discord.changeDiscordRPC("DayZ 0.28", "Playing on", "OSD Launcher", "logo");
					mf.Invoke((MethodInvoker)delegate { mf.WindowState = FormWindowState.Minimized; });
					return;
				}
				else
				{
					MessageBox.Show("Missing DayZ.exe");
					return;
				}
			}
			else if(game == "DayZ_062")
			{
				Process.Start(Downloader.gamePath + "\\DayZ.exe", param);
				//Mainframe.ExecuteAsAdmin(Downloader.gamePath + @"\DayZ.exe", param);
				mf.Invoke((MethodInvoker)delegate { mf.WindowState = FormWindowState.Minimized; });	
				Discord.changeDiscordRPC("DayZ 0.62", "Playing on", "OSD Launcher", "logo");
				return;
			}
			//else if (game == "DayZ_059")
			//{
			//	param = param.Replace("#MOD#", @"@dayzdevru;Mods\Maps\Core;Mods\WeaponsArma2;Mods\Building;");
			//	Process.Start(Downloader.gamePath + "\\DayZ_original.exe", param);
			//	//	Mainframe.ExecuteAsAdmin(Downloader.gamePath + @"\DayZ_original.exe", param);
			//	mf.Invoke((MethodInvoker)delegate { mf.WindowState = FormWindowState.Minimized; });
			//	Discord.changeDiscordRPC("DayZ 0.59", "Playing on", "OSD Launcher", "logo");
			//	return;
			//}
			else if (game == "DayZ_046")
			{
			//	param = param.Replace("#MOD#", @"");
				Process.Start(Downloader.gamePath + "\\DayZ.exe", param);
				//	Mainframe.ExecuteAsAdmin(Downloader.gamePath + @"\DayZ_original.exe", param);
				mf.Invoke((MethodInvoker)delegate { mf.WindowState = FormWindowState.Minimized; });
				Discord.changeDiscordRPC("DayZ 0.46", "Playing on", "OSD Launcher", "logo");
				return;
			}
		}
	}
}
