using MetroFramework;
using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DayZ_Launcher
{
	internal class VisualEffects
	{
		public static Mainframe mf;
		
		
		public static void RenderSettingsTab()
		{
			disableEnableControls();
			mf.metroPanel1.BringToFront();		
			MoveAnimation(mf.metroPanel1, 810,1250,1);		
		}
		
	

		private static MetroPanel getSettingsPanel()
		{
			MetroPanel settingsPanel = null;
			foreach(Control ctrl in mf.Controls)
			{
				if (ctrl.Name == "settings")
				{
					settingsPanel = ctrl as MetroPanel;
				}
			}
			return settingsPanel;
		}

		public static void MoveAnimation(MetroPanel ctrl,int ziel, int start,int state)
		{
		
			if (state == 1)  //slide in (open)
			{
				while (true)
				{
					if (ctrl.Location.X > ziel)
					{
						ctrl.Location = new Point(ctrl.Location.X - 1, ctrl.Location.Y);
					}
					else
					{					
						return;
					}

				}
			}
			else if (state == 0) //slide out (close)
			{
				ctrl.Location = new Point(1252, 0);
				//while (true)
				//{
				//	if (ctrl.Location.X < start)
				//	{
				//		ctrl.Location = new Point(ctrl.Location.X + 1, ctrl.Location.Y);									
				//	}
				//	else
				//	{										
				//		return;
				//	}

				//}
			}
		}


		public static void disableEnableControls()
		{
				mf.Invoke((MethodInvoker)delegate {

					if (Mainframe.settingsOpen)
					{
						mf.pictureBox1.Enabled = false;
						mf.pictureBox3.Enabled = false;
						mf.pictureBox5.Enabled = false;
						mf.textBox2.Enabled = false;
					}
					else
					{
						mf.pictureBox1.Enabled = true;
						mf.pictureBox3.Enabled = true;
						mf.pictureBox5.Enabled = true;
						mf.textBox2.Enabled = true;
						Mainframe.settingsOpen = false;
					}				
				});			
		}
	}
}
