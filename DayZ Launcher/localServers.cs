using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayZ_Launcher
{
    internal class localServers
    {
        public async Task<string> getPublicIp()
        {
            string ip = String.Empty;
            string url = "https://api.ipify.org"; // Alternativ: "https://checkip.amazonaws.com"

            try
            {
                using HttpClient client = new HttpClient();           
                ip =  await client.GetStringAsync(url);
            }
            catch (Exception ex)
            {
                ip = "Error";            
            }
            return ip;
        }
    }
}
