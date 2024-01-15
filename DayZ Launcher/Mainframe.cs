using DayZ_Launcher.Properties;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Net;

namespace DayZ_Launcher
{
	public partial class Mainframe : MetroFramework.Forms.MetroForm
	{
		public static HttpClient client = new HttpClient();
		System.Timers.Timer pingTimer = new System.Timers.Timer(15000);
		public static bool isMainScreen = true;
		public static bool settingsOpen = false;

		public Mainframe()
		{
			InitializeComponent();
			this.pictureBox1.MouseEnter += PictureBox1_MouseEnter;
			this.pictureBox1.MouseLeave += PictureBox1_MouseLeave;

			//this.pictureBox2.MouseEnter += PictureBox2_MouseEnter;  //059
			//this.pictureBox2.MouseLeave += PictureBox2_MouseLeave;



			this.pictureBox3.MouseEnter += PictureBox3_MouseEnter;
			this.pictureBox3.MouseLeave += PictureBox3_MouseLeave;


			this.pictureBox5.MouseEnter += PictureBox5_MouseEnter;
			this.pictureBox5.MouseLeave += PictureBox5_MouseLeave;



			this.FormClosing += Mainframe_FormClosing;

			Downloader.mf = this;
			GameStarter.mf = this;
			Serverbrowser.mf = this;
			VisualEffects.mf = this;
			client.Timeout = TimeSpan.FromSeconds(14);
			client.BaseAddress = new Uri("http://193.23.161.212:8000/");
			pingTimer.Elapsed += PingTimer_Elapsed;
			pingTimer.AutoReset = true;

			Discord.Initialize();
		}

		private void Mainframe_FormClosing(object? sender, FormClosingEventArgs e)
		{
			
			if (Serverbrowser.isActive) 
			{
				e.Cancel = true;
				Serverbrowser.HideServerbrowser();
				Serverbrowser.isActive= false;
				pictureBox4.BackgroundImage = DayZ_Launcher.Properties.Resources.settings;
				isMainScreen = true;
			}		
		}

		private void PictureBox1_MouseLeave(object? sender, EventArgs e)
		{
			pictureBox1.Size = new Size(pictureBox1.Width - 8, pictureBox1.Height - 8);
			pictureBox1.Refresh();
		}

		//private void PictureBox2_MouseLeave(object? sender, EventArgs e)
		//{
		//	pictureBox2.Size = new Size(pictureBox2.Width - 8, pictureBox2.Height - 8);
		//	pictureBox2.Refresh();
		//}

		private void PictureBox3_MouseLeave(object? sender, EventArgs e)
		{
			pictureBox3.Size = new Size(pictureBox3.Width - 8, pictureBox3.Height - 8);
			pictureBox3.Refresh();
		}

		private void PictureBox3_MouseEnter(object? sender, EventArgs e)
		{	
	
			pictureBox3.Size = new Size(pictureBox3.Width + 8, pictureBox3.Height + 8);
			pictureBox3.Refresh();
		}

		private void PictureBox5_MouseLeave(object? sender, EventArgs e)
		{
			pictureBox5.Size = new Size(pictureBox5.Width - 8, pictureBox5.Height - 8);
			pictureBox5.Refresh();
		}

		//private void PictureBox2_MouseEnter(object? sender, EventArgs e)
		//{
		//	pictureBox2.Size = new Size(pictureBox2.Width + 8, pictureBox2.Height + 8);
		//	pictureBox2.Refresh();
		//}

		private void PictureBox1_MouseEnter(object? sender, EventArgs e)
		{
			pictureBox1.Size = new Size(pictureBox1.Width + 8, pictureBox1.Height + 8);
			pictureBox1.Refresh();
		}

		private void PictureBox5_MouseEnter(object? sender, EventArgs e)
		{
			pictureBox5.Size = new Size(pictureBox5.Width + 8, pictureBox5.Height + 8);
			pictureBox5.Refresh();
		}





		private void Form1_Load(object sender, EventArgs e)
		{
		
		}

		private void pictureBox1_Click(object sender, EventArgs e)
		{

		}

		private async void Mainframe_Load(object sender, EventArgs e)
		{
			 //Handshake
			
			try
			{
				HttpResponseMessage response = await client.GetAsync("handshake/");		
				string received = response.Content.ReadAsStringAsync().Result;			
				if (response.StatusCode == HttpStatusCode.OK)
				{			
					HttpResponseMessage versionCheck = await client.GetAsync("update/dayzlauncher/" + Settings.Default.version + "/");					
					if (versionCheck.StatusCode == HttpStatusCode.UpgradeRequired)  //update needed
					{
						MessageBox.Show("New update required!", "Update",MessageBoxButtons.OK,MessageBoxIcon.Warning);
						ExecuteAsAdmin(Application.StartupPath + "\\UpdaterSystem.exe");
						Application.Exit();
						return;
					}

				    if(!String.IsNullOrEmpty(Settings.Default.username))
					{
						textBox2.Text = Settings.Default.username;
					}
					
					pingTimer.Start();
				}
				else if (response.StatusCode == HttpStatusCode.Unauthorized)
				{					
					MessageBox.Show("To many requests! Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					Application.Exit();
					return;
				}
				//Console.WriteLine("Handshake: [" + response.StatusCode + "]");
			}
			catch (Exception ex)
			{
				MessageBox.Show("Server connection timeout! Please try again later","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				Application.Exit();
				return;
			}


			//Handshake end
		}




		//ping timer
		async void PingTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
		{
			try
			{
				HttpResponseMessage response = await client.GetAsync("ping/");
				if (response.StatusCode == HttpStatusCode.OK)
				{
					Console.WriteLine("Ping success!");
					return;
				}					
				pingTimer.Stop();
				pingTimer.Dispose();
				MessageBox.Show("Not authorized! Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();
				return;
			}
			catch (Exception ex)
			{
				pingTimer.Stop();
				pingTimer.Dispose();
				MessageBox.Show("Master server is currently unavailable!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
				Application.Exit();	
				return;
			}

		}

		//ping timer end



		public static void ExecuteAsAdmin(string fileName, string args = "")
		{
			Process proc = new Process();
			proc.StartInfo.FileName = fileName;
			proc.StartInfo.UseShellExecute = true;
			proc.StartInfo.Verb = "runas";
			proc.StartInfo.Arguments = args;
			proc.Start();
		}

		private async void pictureBox1_Click_1(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(Settings.Default.DayZ028)) 
			{
				using (var fbd = new FolderBrowserDialog())
				{
					fbd.Description = "Select the path where you want to install DayZ 0.28 or the path of already existing files!";
					DialogResult result = fbd.ShowDialog();


					if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
					{
						Settings.Default.DayZ028 = fbd.SelectedPath;
						Settings.Default.Save();										
					}
					else
					{
						return;
					}
				}
			}

			if (String.IsNullOrEmpty(textBox2.Text))
			{
				MessageBox.Show("Please enter your username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Settings.Default.username = textBox2.Text;
			Settings.Default.Save();
			Downloader.downloadGame = "DayZ_028";
			Downloader.downloadcdn = "http://193.23.161.212:9660/DayZ028/";
			Downloader.gamePath = Settings.Default.DayZ028;
			Filesystems.clientpath = Settings.Default.DayZ028;
			Serverbrowser.gameVersion = "0.28.113734";


		//	this.Controls.Remove(pictureBox1);
		//	this.Controls.Remove(pictureBox2);
		//this.Controls.Remove(pictureBox3);
		//this.BackImage = null;



			HttpResponseMessage response = await client.GetAsync("filelist/" + Downloader.downloadGame + "/");
			string received = response.Content.ReadAsStringAsync().Result;

			if (response.StatusCode == HttpStatusCode.OK)
			{
				Console.WriteLine("Filelist ok!");
			
				Downloader.startDownload(JArray.Parse(received));
				metroProgressBar1.Visible = true;
				metroLabel1.Visible = true;
				metroLabel2.Visible = true;
				metroLabel3.Visible = true;

				pictureBox1.Enabled= false;
				//pictureBox2.Enabled = false;
				pictureBox3.Enabled = false;
				pictureBox5.Enabled = false;
				textBox2.Enabled = false;
				pictureBox4.Enabled = false;

			}
			else
			{
				MessageBox.Show("Error occured! Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
					
		}

		//private async void pictureBox2_Click(object sender, EventArgs e)
		//{
		////	MessageBox.Show("Currently not available! Coming soon!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
		////	return;

		//	if (String.IsNullOrEmpty(Settings.Default.DayZ059))
		//	{
		//		using (var fbd = new FolderBrowserDialog())
		//		{
		//			fbd.Description = "Select the path where you want to install DayZ 0.59 or the path of already existing files!";
		//			DialogResult result = fbd.ShowDialog();


		//			if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
		//			{
		//				Settings.Default.DayZ059 = fbd.SelectedPath;
		//				Settings.Default.Save();
		//			}
		//			else
		//			{
		//				return;
		//			}
		//		}
		//	}


		//	if (String.IsNullOrEmpty(textBox2.Text))
		//	{
		//		MessageBox.Show("Please enter your username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//		return;
		//	}

		//	Settings.Default.username = textBox2.Text;
		//	Settings.Default.Save();
		//	Downloader.downloadGame = "DayZ_059";
		//	Downloader.downloadcdn = "http://193.23.161.212:9660/DayZ059/";
		//	Downloader.gamePath = Settings.Default.DayZ059;
		//	Filesystems.clientpath = Settings.Default.DayZ059;
		//	Serverbrowser.gameVersion = "0.53.126002";
		//	//this.Controls.Remove(pictureBox1);
		//	//	this.Controls.Remove(pictureBox2);
		//	//	this.Controls.Remove(pictureBox3);
		//	//this.BackImage = null;

		//	HttpResponseMessage response = await client.GetAsync("filelist/" + Downloader.downloadGame + "/");
		//	string received = response.Content.ReadAsStringAsync().Result;

		//	if (response.StatusCode == HttpStatusCode.OK)
		//	{
		//		Console.WriteLine("Filelist ok!");

		//		Downloader.startDownload(JArray.Parse(received));
		//		metroProgressBar1.Visible = true;
		//		metroLabel1.Visible = true;
		//		metroLabel2.Visible = true;
		//		metroLabel3.Visible = true;

		//		pictureBox1.Enabled = false;
		//		//pictureBox2.Enabled = false;
		//		pictureBox3.Enabled = false;
		//		pictureBox5.Enabled = false;

		//		textBox2.Enabled = false;

		//	}
		//	else
		//	{
		//		MessageBox.Show("Error occured! Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//		return;
		//	}
		//}

		private async void pictureBox3_Click(object sender, EventArgs e)
		{
			//MessageBox.Show("Currently not available! Coming soon!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		//	return;

			if (String.IsNullOrEmpty(Settings.Default.DayZ062))
			{
				using (var fbd = new FolderBrowserDialog())
				{
					fbd.Description = "Select the path where you want to install DayZ 0.62 or the path of already existing files!";
					DialogResult result = fbd.ShowDialog();


					if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
					{
						Settings.Default.DayZ062 = fbd.SelectedPath;
						Settings.Default.Save();
					}
					else
					{
						return;
					}
				}
			}


			if (String.IsNullOrEmpty(textBox2.Text))
			{
				MessageBox.Show("Please enter your username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Settings.Default.username = textBox2.Text;
			Settings.Default.Save();
			Downloader.downloadGame = "DayZ_062";
			Downloader.downloadcdn = "http://193.23.161.212:9660/DayZ062/";
			Downloader.gamePath = Settings.Default.DayZ062;
			Filesystems.clientpath = Settings.Default.DayZ062;
			Serverbrowser.gameVersion = "0.62.140099";
			//this.Controls.Remove(pictureBox1);
			//this.Controls.Remove(pictureBox2);
		//	this.Controls.Remove(pictureBox3);
			//this.BackImage = null;



			HttpResponseMessage response = await client.GetAsync("filelist/" + Downloader.downloadGame + "/");
			string received = response.Content.ReadAsStringAsync().Result;

			if (response.StatusCode == HttpStatusCode.OK)
			{
				Console.WriteLine("Filelist ok!");

				Downloader.startDownload(JArray.Parse(received));
				metroProgressBar1.Visible = true;
				metroLabel1.Visible = true;
				metroLabel2.Visible = true;
				metroLabel3.Visible = true;

				pictureBox1.Enabled = false;
				//pictureBox2.Enabled = false;
				pictureBox3.Enabled = false;
				pictureBox5.Enabled = false;
				textBox2.Enabled = false;
				pictureBox4.Enabled = false;

			}
			else
			{
				MessageBox.Show("Error occured! Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}


		private async void pictureBox5_Click(object sender, EventArgs e)
		{
			if (String.IsNullOrEmpty(Settings.Default.DayZ046))
			{
				using (var fbd = new FolderBrowserDialog())
				{
					fbd.Description = "Select the path where you want to install DayZ 0.46 or the path of already existing files!";
					DialogResult result = fbd.ShowDialog();


					if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
					{
						Settings.Default.DayZ046 = fbd.SelectedPath;
						Settings.Default.Save();
					}
					else
					{
						return;
					}
				}
			}


			if (String.IsNullOrEmpty(textBox2.Text))
			{
				MessageBox.Show("Please enter your username!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			Settings.Default.username = textBox2.Text;
			Settings.Default.Save();
			Downloader.downloadGame = "DayZ_046";
			Downloader.downloadcdn = "http://193.23.161.212:9660/DayZ046/";
			Downloader.gamePath = Settings.Default.DayZ046;
			Filesystems.clientpath = Settings.Default.DayZ046;
			Serverbrowser.gameVersion = "0.46.126002";
			//this.Controls.Remove(pictureBox1);
			//this.Controls.Remove(pictureBox2);
			//	this.Controls.Remove(pictureBox3);
			//this.BackImage = null;



			HttpResponseMessage response = await client.GetAsync("filelist/" + Downloader.downloadGame + "/");
			string received = response.Content.ReadAsStringAsync().Result;

			if (response.StatusCode == HttpStatusCode.OK)
			{
				Console.WriteLine("Filelist ok!");

				Downloader.startDownload(JArray.Parse(received));
				metroProgressBar1.Visible = true;
				metroLabel1.Visible = true;
				metroLabel2.Visible = true;
				metroLabel3.Visible = true;

				pictureBox1.Enabled = false;
				//pictureBox2.Enabled = false;
				pictureBox3.Enabled = false;
				pictureBox5.Enabled = false;
				textBox2.Enabled = false;
				pictureBox4.Enabled = false;

			}
			else
			{
				MessageBox.Show("Error occured! Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void pictureBox4_Click(object sender, EventArgs e)
		{
			if (isMainScreen == false)
			{
				long refresh = DateTimeOffset.Now.ToUnixTimeSeconds() - Serverbrowser.lastRefresh;
				if (refresh <= Serverbrowser.refreshLimit)
				{
					long needtowait = Serverbrowser.refreshLimit - refresh;
					MessageBox.Show("Wait " + TimeSpan.FromSeconds(needtowait).TotalSeconds + " seconds before refreshing again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
				
				Serverbrowser.lastRefresh = DateTimeOffset.Now.ToUnixTimeSeconds();
				Serverbrowser.refreshServerbrowser();
			}
			else if (isMainScreen == true)
			{
				settingsOpen= true;
				loadSettings();
				VisualEffects.RenderSettingsTab();
			}
		}

		public void loadSettings()
		{
			metroLabel4.Text = "Version: " + Settings.Default.version;
			textBox1.Text = (string.IsNullOrEmpty(Settings.Default.DayZ028) == true ? "None" : Settings.Default.DayZ028);
			textBox3.Text = (string.IsNullOrEmpty(Settings.Default.DayZ046) == true ? "None" : Settings.Default.DayZ046);
			textBox4.Text = (string.IsNullOrEmpty(Settings.Default.DayZ062) == true ? "None" : Settings.Default.DayZ062);
		}


		private void pictureBox2_Click(object sender, EventArgs e)
		{

		}

		private void pictureBox2_Click_1(object sender, EventArgs e)  //close settings
		{
			settingsOpen = false;
			VisualEffects.disableEnableControls();
			VisualEffects.MoveAnimation(metroPanel1, 810, 1250, 0);
		}

		private void pictureBox6_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				fbd.Description = "Select the path where you want to install DayZ 0.28 or the path of already existing files!";
				DialogResult result = fbd.ShowDialog();


				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					Settings.Default.DayZ028 = fbd.SelectedPath;
					Settings.Default.Save();
					textBox1.Text = fbd.SelectedPath;
				}
				else
				{
					return;
				}
			}
		}

		private void pictureBox7_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				fbd.Description = "Select the path where you want to install DayZ 0.46 or the path of already existing files!";
				DialogResult result = fbd.ShowDialog();


				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					Settings.Default.DayZ046 = fbd.SelectedPath;
					Settings.Default.Save();
					textBox3.Text = fbd.SelectedPath;
				}
				else
				{
					return;
				}
			}
		}

		private void pictureBox8_Click(object sender, EventArgs e)
		{
			using (var fbd = new FolderBrowserDialog())
			{
				fbd.Description = "Select the path where you want to install DayZ 0.62 or the path of already existing files!";
				DialogResult result = fbd.ShowDialog();


				if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
				{
					Settings.Default.DayZ062 = fbd.SelectedPath;
					Settings.Default.Save();
					textBox4.Text = fbd.SelectedPath;
				}
				else
				{
					return;
				}
			}
		}
	}
}