using System;
using System.Windows.Forms;

namespace imemd
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            SettingForm settingForm = new SettingForm();
            if (settingForm.SetHook() == false)
            {
                return;
            }
            Application.Run();
        }
    }
}
