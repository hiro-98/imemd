using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace imemd
{
    public partial class SettingForm : Form
    {
        private readonly Hook hook;

        public SettingForm()
        {
            InitializeComponent();

            // コンテキストメニュー
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
            hook = new Hook();
            hook.SetMouseHook();
        }


        private void BtnOK_Click(object sender, EventArgs e)
        {
            hook.clickWaitMs = (int)numClickWaitMS.Value;
            hook.sameWindowSec = (int)numSameWindowSec.Value;

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
