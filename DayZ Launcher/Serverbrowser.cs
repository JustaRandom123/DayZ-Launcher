using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Windows.Forms;
using DayZ_Launcher;
using DayZ_Launcher.Properties;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;

internal class Serverbrowser
{
    public static Mainframe mf;

    public static int playersCountGeneral = 0;

    public static bool isActive = false;

    public static long lastRefresh = 0L;

    public static int refreshLimit = 5;

    public static string selectedServer = string.Empty;

    public static bool requirePassword = false;

    public static string gameVersion { get; set; }

    public static async void LoadServerbrowser()
    {
        Mainframe.isMainScreen = false;
        mf.Invoke((MethodInvoker)delegate
        {
            mf.pictureBox4.Enabled = true;
            mf.pictureBox4.BackgroundImage = Resources.refresh;
        });
        playersCountGeneral = 0;
        mf.listView1.Activation = ItemActivation.Standard;
        mf.listView1.ItemActivate += ListView1_ItemActivate;
        mf.metroProgressSpinner1.Invoke((MethodInvoker)delegate
        {
            mf.metroProgressSpinner1.Visible = true;
            mf.metroProgressSpinner1.BringToFront();
        });

        ///loading local servers


        HttpResponseMessage responseLocalServer = await Mainframe.client.GetAsync($"getLocalServers/{localServers.localIp}/{gameVersion}");
        string receivedLocalServer = responseLocalServer.Content.ReadAsStringAsync().Result;
        if (responseLocalServer.StatusCode == HttpStatusCode.OK)
        {
            JArray localServerlist = JArray.Parse(receivedLocalServer.ToString());

            if (localServerlist.ToString() != "[]")
            {
                foreach (JToken serverInfo in localServerlist)
                {
                    ListViewItem item = new ListViewItem("Your local server");
                    item.Tag = "127.0.0.1:" + serverInfo["port"].ToString();
                    item.SubItems.Add("0 / 0");
                    item.SubItems.Add("dayz_Auto");
                    item.SubItems.Add(serverInfo["version"].ToString());
                    item.SubItems.Add("");
                    mf.listView1.Invoke((MethodInvoker)delegate
                    {
                        mf.listView1.Items.Add(item);
                    });
                }
            }
        }




        HttpResponseMessage response = await Mainframe.client.GetAsync("getServerList/" + gameVersion + "/");
        string received = response.Content.ReadAsStringAsync().Result;
        if (response.StatusCode == HttpStatusCode.OK)
        {
            JArray serverlist = JArray.Parse(received.ToString());
            if (!string.IsNullOrEmpty(serverlist[0].ToString()))
            {
                Console.WriteLine("Count: " + serverlist.Count());
                foreach (JToken serverData in serverlist)
                {
                    if (string.IsNullOrEmpty(serverData.ToString()))
                    {
                        continue;
                    }
                    ListViewItem item = new ListViewItem(serverData[0]["name"].ToString());
                    item.Tag = serverData[0]["addr"].ToString().Split(Convert.ToChar(":"))[0] + ":" + serverData[0]["gameport"].ToString();
                    item.SubItems.Add(serverData[0]["players"].ToString() + " / " + serverData[0]["max_players"].ToString());
                    item.SubItems.Add((serverData[0]["map"]?.ToString() == null) ? "None" : serverData[0]["map"].ToString());
                    item.SubItems.Add(serverData[0]["version"].ToString());
                    item.SubItems.Add("");
                    mf.listView1.Invoke((MethodInvoker)delegate
                    {
                        mf.listView1.Items.Add(item);
                    });
                    //if ((await Mainframe.client.GetAsync("serverHasPassword/" + item.Tag.ToString() + "/")).StatusCode == HttpStatusCode.Forbidden)
                    //{
                    //    Button testButton = new Button();
                    //    testButton.Text = "";
                    //    //testButton.BackgroundImage = Resources._lock;   lock image is missing
                    //    testButton.BackgroundImageLayout = ImageLayout.Stretch;
                    //    testButton.BackColor = Color.Transparent;
                    //    testButton.FlatStyle = FlatStyle.Flat;
                    //    testButton.FlatAppearance.BorderSize = 0;
                    //    mf.listView1.Invoke((MethodInvoker)delegate
                    //    {
                    //        testButton.Size = new Size(item.SubItems[4].Bounds.Size.Width, item.SubItems[4].Bounds.Size.Height);
                    //    });
                    //    mf.listView1.Invoke((MethodInvoker)delegate
                    //    {
                    //        testButton.Location = new Point(item.SubItems[4].Bounds.Location.X, item.SubItems[4].Bounds.Location.Y);
                    //    });
                    //    mf.listView1.Invoke((MethodInvoker)delegate
                    //    {
                    //        mf.listView1.Controls.Add(testButton);
                    //    });
                    //}
             

                    playersCountGeneral += Convert.ToInt32(serverData[0]["players"].ToString());
                }           
                mf.Invoke((MethodInvoker)delegate
                {
                    mf.Text = "DayZ " + gameVersion + " - " + playersCountGeneral + " players online";
                    mf.Refresh();
                });
                mf.metroProgressSpinner1.Invoke((MethodInvoker)delegate
                {
                    mf.metroProgressSpinner1.Visible = false;
                });
                isActive = true;
            }
            else
            {
                mf.Invoke((MethodInvoker)delegate
                {
                    mf.Text = "DayZ " + gameVersion + " - " + playersCountGeneral + " players online";
                    mf.Refresh();
                });
                mf.metroProgressSpinner1.Invoke((MethodInvoker)delegate
                {
                    mf.metroProgressSpinner1.Visible = false;
                });
                isActive = true;
            }
        }
        else
        {
            MessageBox.Show("Error occured! Please try again!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }




        //getLocalServers


        //HttpResponseMessage responseLocalServer = await Mainframe.client.GetAsync($"getLocalServers/{localServers.getPublicIp().Result}/{gameVersion}");
        //string receivedLocalServer = responseLocalServer.Content.ReadAsStringAsync().Result;
        //if (responseLocalServer.StatusCode == HttpStatusCode.OK)
        //{
        //    JArray localServerlist = JArray.Parse(receivedLocalServer.ToString());
        //    // MessageBox.Show(localServers.getPublicIp().Result);
        //    if (localServerlist.ToString() != "[]")
        //    {
        //        //foreach (JToken serverInfo in localServerlist)
        //        //{
        //        //    ListViewItem item = new ListViewItem("Your local server");
        //        //    item.Tag =  "localhost:" + serverInfo["port"].ToString();
        //        //    item.SubItems.Add("0 / 0");
        //        //    item.SubItems.Add("dayz_Auto");
        //        //    item.SubItems.Add(serverInfo["version"].ToString());
        //        //    item.SubItems.Add("");
        //        //    mf.listView1.Invoke((MethodInvoker)delegate
        //        //    {
        //        //        mf.listView1.Items.Add(item);
        //        //    });
        //        //}
        //        //mf.Refresh();
        //    }
        //}
    }

    public static void checkRunningGames()
    {
        Process[] processes = Process.GetProcessesByName("DayZ");
        Process[] array = processes;
        foreach (Process process in array)
        {
            process.Kill();
        }
        Process[] processes2 = Process.GetProcessesByName("DayZ_original");
        Process[] array2 = processes2;
        foreach (Process process2 in array2)
        {
            process2.Kill();
        }
    }

    private static async void ListView1_ItemActivate(object? sender, EventArgs e)
    {
        if (requirePassword)
        {
            return;
        }
        if (mf.listView1.SelectedItems.Count == 1)
        {
            selectedServer = mf.listView1.SelectedItems[0].Tag.ToString();
            if ((await Mainframe.client.GetAsync("serverHasPassword/" + mf.listView1.SelectedItems[0].Tag.ToString() + "/")).StatusCode == HttpStatusCode.Forbidden)
            {
                requirePassword = true;  

                //need to readd the code


               // mf.metroPanel2.Visible = true;
              //  mf.metroPanel2.BringToFront();
            }
            else
            {
                checkRunningGames();
                GameStarter.startGame(Downloader.downloadGame, mf.textBox2.Text, mf.listView1.SelectedItems[0].Tag.ToString());
            }
        }
        else
        {
            MessageBox.Show("Error selecting a server!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
        }
    }

    public static void refreshServerbrowser()
    {
        mf.Invoke((MethodInvoker)delegate
        {
            mf.listView1.Items.Clear();
            mf.listView1.Controls.Clear();
        });
        LoadServerbrowser();
    }

    public static void HideServerbrowser()
    {
        mf.Invoke((MethodInvoker)delegate
        {
            mf.metroProgressSpinner1.Visible = false;
            mf.Text = "DayZ Launcher";
            mf.listView1.Visible = false;
            mf.listView1.Items.Clear();
            mf.listView1.Controls.Clear();
            mf.BackImage = Resources.wallpaper1;
            mf.textBox2.Visible = true;
            mf.pictureBox1.Enabled = true;
            mf.pictureBox3.Enabled = true;
            mf.pictureBox5.Enabled = true;
            mf.textBox2.Enabled = true;
        });
    }
}