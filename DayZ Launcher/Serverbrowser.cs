using DayZ_Launcher.Properties;
using MetroFramework.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DayZ_Launcher
{
	internal class Serverbrowser
	{
		public static Mainframe mf;
		public static string gameVersion { get; set; }
		public static int playersCountGeneral = 0;
		public static bool isActive = false;
		public static long lastRefresh = 0;
		public static int refreshLimit = 5; //refresh limit in seconds
		public async static void LoadServerbrowser()
		{
			Mainframe.isMainScreen = false;
			mf.Invoke((MethodInvoker)delegate { mf.pictureBox4.Enabled = true; mf.pictureBox4.BackgroundImage = DayZ_Launcher.Properties.Resources.refresh; });

			playersCountGeneral = 0;
			mf.listView1.Activation = System.Windows.Forms.ItemActivation.Standard;
			mf.listView1.ItemActivate += ListView1_ItemActivate;

			mf.metroProgressSpinner1.Invoke((MethodInvoker)delegate { mf.metroProgressSpinner1.Visible = true; mf.metroProgressSpinner1.BringToFront(); });  //progess spinner
			//mf.listView1.Invoke((MethodInvoker)delegate { mf.listView1.Enabled = false; });  

			HttpResponseMessage response = await Mainframe.client.GetAsync("getServerList/" + gameVersion + "/");
			string received = response.Content.ReadAsStringAsync().Result;

			if (response.StatusCode == HttpStatusCode.OK)
			{			
				JObject serverlist = JObject.Parse(received);
				if (serverlist["response"].ToString() != "{}")
				{
					foreach (var table in serverlist["response"]["servers"])
					{


						ListViewItem item = new ListViewItem(table["name"].ToString());
						item.Tag = table["addr"].ToString().Split(Convert.ToChar(":"))[0] + ":" + table["gameport"].ToString();
						item.SubItems.Add(table["players"].ToString() + " / " + table["max_players"].ToString());
						item.SubItems.Add(table["map"].ToString());
						item.SubItems.Add(table["version"].ToString());
						
							
						mf.listView1.Invoke((MethodInvoker)delegate { mf.listView1.Items.Add(item); });  //serverbrowser
						playersCountGeneral = playersCountGeneral + Convert.ToInt32(table["players"].ToString());
					}
				}
				mf.Invoke((MethodInvoker)delegate { mf.Text = "DayZ " + gameVersion + " - " + playersCountGeneral.ToString() + " players online"; mf.Refresh(); });
				mf.metroProgressSpinner1.Invoke((MethodInvoker)delegate { mf.metroProgressSpinner1.Visible = false; });  //progess spinner
			//	mf.listView1.Invoke((MethodInvoker)delegate { mf.listView1.Enabled = true; });
				isActive = true;
			}		
			else
			{
				MessageBox.Show("Error occured! Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}

		private static void checkRunningGames()
		{	
			Process[] processes = Process.GetProcessesByName("DayZ");
			foreach (Process process in processes) 
			{ 
				process.Kill();	
			}
			Process[] processes2 = Process.GetProcessesByName("DayZ_original");
			foreach (Process process in processes2)
			{
				process.Kill();
			}
		}

		private static void ListView1_ItemActivate(object? sender, EventArgs e)
		{
		
			if (mf.listView1.SelectedItems.Count == 1)
			{
				checkRunningGames();
				GameStarter.startGame(Downloader.downloadGame, mf.textBox2.Text, mf.listView1.SelectedItems[0].Tag.ToString());
				return;
			}
			else
			{
				MessageBox.Show("Error selecting a server!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error); 
				return;	
			}			
		}

	   public static void refreshServerbrowser()
		{

			mf.Invoke((MethodInvoker)delegate {
			
				mf.listView1.Items.Clear();				
			});
			LoadServerbrowser();
		}

		public static void HideServerbrowser()
		{			
			mf.Invoke((MethodInvoker)delegate {
				mf.metroProgressSpinner1.Visible = false;
				mf.Text = "DayZ Launcher";
				mf.listView1.Visible = false;
				mf.listView1.Items.Clear();
				mf.BackImage = Resources.wallpaper1; 
				mf.textBox2.Visible = true; 

				mf.pictureBox1.Enabled= true;
			//	mf.pictureBox2.Enabled = true;
				mf.pictureBox3.Enabled = true;
				mf.pictureBox5.Enabled = true;
			    mf.textBox2.Enabled = true;
				//mf.pictureBox4.Visible = false;  //refresh button
			});	
		}
	}
}
