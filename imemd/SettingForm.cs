using System;
using System.Text;
using System.Windows.Forms;

namespace imemd
{
    public partial class SettingForm : Form
    {
        private const string DEFAULT_CLICK_WAIT_MS = "50";
        private const string DEFAULT_SAME_WINDOW_CHECK = "3";
        private const string DEFAULT_CHECK_IBEAM = "true";

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

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Visible = false;
        }

        public bool SetHook()
        {
            hook = new Hook();
            ReadSetting();
            hook.UpdateHookSettings((int)numClickWaitMS.Value, (int)numSameWindowSec.Value, this.checkIBeam.Checked);

            if (hook.SetMouseHook() == false)
            {
                MessageBox.Show("Error: SetWindowsHookEx", "Error");
                notifyIcon.Dispose();
                hook.FreeAllocHandle();
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
                "Main", "ClickWaitMS", DEFAULT_CLICK_WAIT_MS,
                readSB, Convert.ToUInt32(readSB.Capacity), iniFileName);
            this.numClickWaitMS.Value = int.Parse(readSB.ToString());

            Win32API.GetPrivateProfileString(
                "Main", "SameWindowSec", DEFAULT_SAME_WINDOW_CHECK,
                readSB, Convert.ToUInt32(readSB.Capacity), iniFileName);
            this.numSameWindowSec.Value = int.Parse(readSB.ToString());

            Win32API.GetPrivateProfileString(
                "Main", "CheckIBeam", DEFAULT_CHECK_IBEAM,
                readSB, Convert.ToUInt32(readSB.Capacity), iniFileName);
            this.checkIBeam.Checked = bool.Parse(readSB.ToString());
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            hook.UpdateHookSettings((int)numClickWaitMS.Value, (int)numSameWindowSec.Value, this.checkIBeam.Checked);
            WriteSetting();

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
