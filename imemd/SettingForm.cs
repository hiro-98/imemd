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
    }
}
