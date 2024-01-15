using DayZ_Launcher.Properties;
using MetroFramework.Controls;
using Microsoft.VisualBasic;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

namespace DayZ_Launcher
{
	internal class Downloader
	{
		public static Mainframe mf;

		public static string downloadGame { get; set; }
		public static string gamePath;


		public static string downloadcdn { get; set; }
		public static System.Timers.Timer timer { get; set; }
		static string currentlyDownloading { get; set; }
		static string currentlyDownloadingKey { get; set; }


		public static Dictionary<string, string> downloads = new Dictionary<string, string>();
		public static int downloadCount { get; set; }

		public static Dictionary<string, string> fileNeedToDownload = new Dictionary<string, string>();



		static bool isDownloading = false;


		public static void startDownload(JArray serverjson)
		{
			////////////////////////


			mf.metroLabel1.Text = "Fetch file list from server";
			var clientfiles = Filesystems.getClientFiles();


			for (int i = 0; i < serverjson.Count; i++)
			{
				string convertedValidPath = Filesystems.clientpath + "\\" + serverjson[i][0].ToString();



				if (File.Exists(convertedValidPath))
				{
					string clientByte = "";
					clientfiles.TryGetValue(serverjson[i][0].ToString(), out clientByte);

					

					if (serverjson[i][1].ToString() != clientByte)
					{

						fileNeedToDownload.Add(convertedValidPath, serverjson[i][0].ToString());
					}
					else
					{
					}
				}
				else
				{
					fileNeedToDownload.Add(convertedValidPath, serverjson[i][0].ToString());
				}
			}


			if (fileNeedToDownload != null)
			{
				if (fileNeedToDownload.Count >= 1)
				{


					DialogResult result = MessageBox.Show("Downloading " + fileNeedToDownload.Count + " file(s). Do you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
					if (result == DialogResult.No)
					{
						Application.Exit();
						return;
					}

					downloadCount = fileNeedToDownload.Count;
					downloads = fileNeedToDownload;
					downloader();
				
				}
				else
				{
					downloader();
					
				}
			}
			else
			{

			}
		}




		public static void downloader()
		{
			isDownloading = true;	
			if (downloadCount > 0)
			{
				var file = downloads.First();

				using (WebClient wc = new WebClient())
				{
					currentlyDownloading = file.Value;
					currentlyDownloadingKey = file.Key;
					Directory.CreateDirectory(Path.GetDirectoryName(file.Key));
					mf.metroLabel2.Text = "Downloading: " + gamePath + currentlyDownloading;
					mf.metroLabel1.Text = "Status | " + Downloader.downloadCount + " file(s) left";
					wc.DownloadProgressChanged += wc_DownloadProgressChanged;
					wc.DownloadFileCompleted += wc_DownloadFileCompleted;
					wc.DownloadFileAsync(new Uri(downloadcdn + currentlyDownloading), file.Key);

				}
			}
			else
			{
				isDownloading = false;
				mf.metroLabel2.Visible = false;
				mf.metroLabel3.Visible = false;
				mf.metroLabel3.Text = "";
				mf.metroLabel2.Text = "";
				mf.metroLabel1.Text = "Finished! Loading...";
				mf.metroProgressBar1.Value = 100;
				timer = new System.Timers.Timer(3000);
				timer.Elapsed += Timer_Elapsed;
				timer.Enabled = true;
				timer.Start();

			}
		}


		//public static async void GetInternetSpeed()
		//{
		//	while (isDownloading)
		//	{
		//		const double kb = 1024;

		//		// do not use compression
		//		using var client = new HttpClient();

		//		int numberOfBytesRead = 0;

		//		var buffer = new byte[10240].AsMemory();

		//		// create request
		//		var stream = await client.GetStreamAsync("https://www.google.com");

		//		// start timer
		//		DateTime dt1 = DateTime.UtcNow;

		//		// download stuff
		//		while (true)
		//		{
		//			var i = await stream.ReadAsync(buffer);
		//			if (i < 1)
		//				break;

		//			numberOfBytesRead += i;
		//		}

		//		// end timer
		//		DateTime dt2 = DateTime.UtcNow;

		//		double kilobytes = numberOfBytesRead / kb;
		//		double time = (dt2 - dt1).TotalSeconds;

		//		// speed in Kb per Second.

		//		mf.metroLabel4.Text = Convert.ToString((int)(kilobytes / time)) + " kbps";
		//		Thread.Sleep(1000);
		//	}			
		//}

		private static void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
		{
			mf.metroLabel1.Invoke((MethodInvoker)delegate { mf.metroLabel1.Visible = false; });
			mf.Invoke((MethodInvoker)delegate { mf.BackImage = null; });
			mf.metroProgressBar1.Invoke((MethodInvoker)delegate { mf.metroProgressBar1.Visible = false; });
			mf.listView1.Invoke((MethodInvoker)delegate { mf.listView1.Visible = true; });  //serverbrowser
			mf.pictureBox4.Invoke((MethodInvoker)delegate { mf.pictureBox4.Visible = true; });

			timer.Enabled = false;
			timer.Dispose();
			timer = null;

			Serverbrowser.LoadServerbrowser();
		}




		private static void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			mf.metroProgressBar1.Value = e.ProgressPercentage;

			long totalbytes = e.TotalBytesToReceive / 1024 / 1024;
			long totalbytesKB = e.TotalBytesToReceive / 1024;
			long bytes = e.BytesReceived / 1024 / 1024;
			long gbbytes = e.BytesReceived / 1024 / 1024 / 1024;
			long totalbytesGB = e.TotalBytesToReceive / 1024 / 1024 / 1024;
			long bytesKB = e.BytesReceived / 1024;
			if (e.BytesReceived >= 999)
			{
				mf.metroLabel3.Text = bytes.ToString() + " / " + totalbytes.ToString() + " MB ";
			}
			else if (e.BytesReceived < 999)
			{
				mf.metroLabel3.Text = bytesKB.ToString() + " / " + totalbytesKB.ToString() + " KB ";
			}
			else if (e.BytesReceived >= 9999)
			{
				mf.metroLabel3.Text = gbbytes.ToString() + " / " + totalbytesGB.ToString() + " GB ";
			}
		}


		private static void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			downloads.Remove(currentlyDownloadingKey);
			downloadCount--;
			downloader();
		}
	}
}
