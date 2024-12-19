using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZ_Launcher
{
    internal class SteamHandler
    {
        public static void InitializeSteam()
        {
            try
            {
                SteamClient.Init(221100u);
                if (!SteamClient.IsValid)
                {
                    MessageBox.Show("Steam isn`t running or inactive steam session!", "Status", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    SteamClient.Shutdown();
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Application.Exit();
            }
        }
    }
}
