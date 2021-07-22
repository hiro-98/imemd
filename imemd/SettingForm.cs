using System;
using System.Text;
using System.Windows.Forms;

namespace imemd
{
    public partial class SettingForm : Form
    {
        private Hook hook;

        public SettingForm()
        {
            InitializeComponent();

            // システムトレイ
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem
            {
                Text = "終了"
            };
            toolStripMenuItem.Click += ToolStripExit_Click;
            contextMenuStrip.Items.Add(toolStripMenuItem);
            notifyIcon.ContextMenuStrip = contextMenuStrip;

            ReadSetting();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Visible = false;
        }

        public bool SetHook()
        {
            hook = new Hook();
            if (hook.SetMouseHook() == false)
            {
                MessageBox.Show("Error: SetWindowsHookEx", "Error");
                notifyIcon.Dispose();
                this.Close();
                return false;
            }
            return true;
        }

        private void WriteSetting()
        {
            string iniFileName = AppDomain.CurrentDomain.BaseDirectory + "setting.ini";
            Win32API.WritePrivateProfileString("Main", "ClickWaitMS", this.numClickWaitMS.Value.ToString(), iniFileName);
            Win32API.WritePrivateProfileString("Main", "SameWindowSec", this.numSameWindowSec.Value.ToString(), iniFileName);
            Win32API.WritePrivateProfileString("Main", "CheckIBeam", this.checkIBeam.Checked.ToString(), iniFileName);
        }

        private void ReadSetting()
        {
            string iniFileName = AppDomain.CurrentDomain.BaseDirectory + "setting.ini";

            StringBuilder readSB = new StringBuilder(128);
            Win32API.GetPrivateProfileString(
                "Main", "ClickWaitMS", "50", readSB, Convert.ToUInt32(readSB.Capacity), iniFileName);
            this.numClickWaitMS.Value = int.Parse(readSB.ToString());

            Win32API.GetPrivateProfileString(
                "Main", "SameWindowSec", "5", readSB, Convert.ToUInt32(readSB.Capacity), iniFileName);
            this.numSameWindowSec.Value = int.Parse(readSB.ToString());

            Win32API.GetPrivateProfileString(
                "Main", "CheckIBeam", "false", readSB, Convert.ToUInt32(readSB.Capacity), iniFileName);
            this.checkIBeam.Checked = bool.Parse(readSB.ToString());
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            hook.UpdateHookSettings((int)numClickWaitMS.Value, (int)numClickWaitMS.Value, this.checkIBeam.Checked);

            this.Visible = false;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Visible = false;
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;
        }

        private void ToolStripExit_Click(object sender, EventArgs e)
        {
            WriteSetting();
            notifyIcon.Dispose();
            Application.Exit();
        }
    }
}
