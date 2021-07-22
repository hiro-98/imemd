using System;
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

            // フォーム表示
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
            notifyIcon.Dispose();
            Application.Exit();
        }
    }
}
