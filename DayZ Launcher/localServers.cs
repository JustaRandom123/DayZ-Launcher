using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZ_Launcher
{
    internal class localServers
    {
        public static string localIp {  get; set; } 

        public static async Task getPublicIp()
        {
            string ip = String.Empty;
            string url = "https://api.ipify.org"; // Alternativ: "https://checkip.amazonaws.com"

            try
            {
                using HttpClient client = new HttpClient();
                localIp =  await client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                localIp = "Error";            
            }
           // return ip;
        }
    }
}
