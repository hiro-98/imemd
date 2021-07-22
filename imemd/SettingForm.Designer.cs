﻿namespace imemd
{
    partial class SettingForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.labelClickWaitMS = new System.Windows.Forms.Label();
            this.numClickWaitMS = new System.Windows.Forms.NumericUpDown();
            this.labelSameWindowSec = new System.Windows.Forms.Label();
            this.numSameWindowSec = new System.Windows.Forms.NumericUpDown();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numClickWaitMS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSameWindowSec)).BeginInit();
            this.SuspendLayout();
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "IME Mode Display";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseDoubleClick);
            // 
            // labelClickWaitMS
            // 
            this.labelClickWaitMS.AutoSize = true;
            this.labelClickWaitMS.Location = new System.Drawing.Point(12, 15);
            this.labelClickWaitMS.Name = "labelClickWaitMS";
            this.labelClickWaitMS.Size = new System.Drawing.Size(139, 12);
            this.labelClickWaitMS.TabIndex = 0;
            this.labelClickWaitMS.Text = "クリック後の待機時間(ミリ秒)";
            // 
            // numClickWaitMS
            // 
            this.numClickWaitMS.Increment = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numClickWaitMS.Location = new System.Drawing.Point(14, 30);
            this.numClickWaitMS.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numClickWaitMS.Name = "numClickWaitMS";
            this.numClickWaitMS.Size = new System.Drawing.Size(63, 19);
            this.numClickWaitMS.TabIndex = 1;
            this.numClickWaitMS.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            // 
            // labelSameWindowSec
            // 
            this.labelSameWindowSec.AutoSize = true;
            this.labelSameWindowSec.Location = new System.Drawing.Point(12, 62);
            this.labelSameWindowSec.Margin = new System.Windows.Forms.Padding(3, 10, 3, 0);
            this.labelSameWindowSec.Name = "labelSameWindowSec";
            this.labelSameWindowSec.Size = new System.Drawing.Size(212, 12);
            this.labelSameWindowSec.TabIndex = 2;
            this.labelSameWindowSec.Text = "同一ウインドウで再表示するまでの時間(秒)";
            // 
            // numSameWindowSec
            // 
            this.numSameWindowSec.Location = new System.Drawing.Point(12, 77);
            this.numSameWindowSec.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numSameWindowSec.Name = "numSameWindowSec";
            this.numSameWindowSec.Size = new System.Drawing.Size(65, 19);
            this.numSameWindowSec.TabIndex = 3;
            this.numSameWindowSec.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(187, 129);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(106, 129);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 5;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // SettingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 164);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.numSameWindowSec);
            this.Controls.Add(this.labelSameWindowSec);
            this.Controls.Add(this.numClickWaitMS);
            this.Controls.Add(this.labelClickWaitMS);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SettingForm";
            this.Text = "IME Mode Display";
            ((System.ComponentModel.ISupportInitialize)(this.numClickWaitMS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSameWindowSec)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label labelClickWaitMS;
        private System.Windows.Forms.NumericUpDown numClickWaitMS;
        private System.Windows.Forms.Label labelSameWindowSec;
        private System.Windows.Forms.NumericUpDown numSameWindowSec;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}

