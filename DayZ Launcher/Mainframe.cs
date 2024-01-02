using DayZ_Launcher.Properties;
using Newtonsoft.Json.Linq;
using System.Net;

namespace DayZ_Launcher
{
	public partial class Mainframe : MetroFramework.Forms.MetroForm
	{
		public HttpClient client = new HttpClient();
		System.Timers.Timer pingTimer = new System.Timers.Timer(5000);


		public Mainframe()
		{
			InitializeComponent();
			this.pictureBox1.MouseEnter += PictureBox1_MouseEnter;
			this.pictureBox1.MouseLeave += PictureBox1_MouseLeave;

			this.pictureBox2.MouseEnter += PictureBox2_MouseEnter;
			this.pictureBox2.MouseLeave += PictureBox2_MouseLeave;



			this.pictureBox3.MouseEnter += PictureBox3_MouseEnter;
			this.pictureBox3.MouseLeave += PictureBox3_MouseLeave;


			Downloader.mf = this;
			client.Timeout = TimeSpan.FromSeconds(10);
			client.BaseAddress = new Uri("http://87.237.52.165:8000/");
			pingTimer.Elapsed += PingTimer_Elapsed;
			pingTimer.AutoReset = true;

			Discord.Initialize();
		}

		private void PictureBox1_MouseLeave(object? sender, EventArgs e)
		{
			pictureBox1.Size = new Size(pictureBox1.Width - 8, pictureBox1.Height - 8);
			pictureBox1.Refresh();
		}

		private void PictureBox2_MouseLeave(object? sender, EventArgs e)
		{
			pictureBox2.Size = new Size(pictureBox2.Width - 8, pictureBox2.Height - 8);
			pictureBox2.Refresh();
		}

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

		private void PictureBox2_MouseEnter(object? sender, EventArgs e)
		{
			pictureBox2.Size = new Size(pictureBox2.Width + 8, pictureBox2.Height + 8);
			pictureBox2.Refresh();
		}

		private void PictureBox1_MouseEnter(object? sender, EventArgs e)
		{
			pictureBox1.Size = new Size(pictureBox1.Width + 8, pictureBox1.Height + 8);
			pictureBox1.Refresh();
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
					pingTimer.Start();
				}
				Console.WriteLine("Handshake: [" + response.StatusCode + "]");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Server connection timeout! Please try again later");
				return;
			}


			//Handshake end
		}




		//ping timer
		async void PingTimer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
		{
			HttpResponseMessage response = await client.GetAsync("ping/");
			if (response.StatusCode == HttpStatusCode.OK)
			{
				Console.WriteLine("Ping success!");
				return;
			}
			Console.WriteLine("Not authorized!");
			pingTimer.Stop();
			pingTimer.Dispose();

		}

		//ping timer end





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
				}
			}

			Downloader.downloadGame = "DayZ_028";
			Downloader.gamePath = Settings.Default.DayZ028;
			Filesystems.clientpath = Settings.Default.DayZ028;	


			this.Controls.Remove(pictureBox1);
			this.Controls.Remove(pictureBox2);
			this.Controls.Remove(pictureBox3);
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

			}
			else
			{
				MessageBox.Show("Error occured! Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
					
		}

		private void pictureBox2_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Currently not available! Coming soon!","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;

			if (String.IsNullOrEmpty(Settings.Default.DayZ059))
			{
				using (var fbd = new FolderBrowserDialog())
				{
					fbd.Description = "Select the path where you want to install DayZ 0.59 or the path of already existing files!";
					DialogResult result = fbd.ShowDialog();


					if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
					{
						Settings.Default.DayZ059 = fbd.SelectedPath;
						Settings.Default.Save();
					}
				}
			}
			Downloader.downloadGame = "DayZ_059";
			Downloader.gamePath = Settings.Default.DayZ059;
			this.Controls.Remove(pictureBox1);
			this.Controls.Remove(pictureBox2);
			this.Controls.Remove(pictureBox3);
			this.BackImage = null;
		}

		private void pictureBox3_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Currently not available! Coming soon!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			return;

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
				}
			}
			Downloader.downloadGame = "DayZ_62";
			Downloader.gamePath = Settings.Default.DayZ062;
			this.Controls.Remove(pictureBox1);
			this.Controls.Remove(pictureBox2);
			this.Controls.Remove(pictureBox3);
			this.BackImage = null;
		}
	}
}