using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace imemd
{
    public partial class SettingForm : Form
    {
        private Hook hook;

        public SettingForm()
        {
            InitializeComponent();
            hook = new Hook();
            hook.SetMouseHook();
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            hook.clickWaitMs = (int)numClickWaitMS.Value;
            hook.sameWindowSec = (int)numSameWindowSec.Value;
        }
    }
}
