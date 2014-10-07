using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VsEmacs
{
    public class CopyFileConfirmationDialog : Form
    {
        private Button btnCancel;
        private Button btnOk;
        private IContainer components;
        private Label label1;

        public CopyFileConfirmationDialog()
        {
            InitializeComponent();
            btnOk.FlatStyle = FlatStyle.System;
            NativeMethods.SendMessage((int) btnOk.Handle, 5644, 0, 1);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            var componentResourceManager = new ComponentResourceManager(typeof (CopyFileConfirmationDialog));
            label1 = new Label();
            btnOk = new Button();
            btnCancel = new Button();
            SuspendLayout();
            label1.Location = new Point(8, 13);
            label1.Name = "label1";
            label1.Size = new Size(449, 68);
            label1.TabIndex = 0;
            label1.Text = componentResourceManager.GetString("label1.Text");
            btnOk.DialogResult = DialogResult.OK;
            btnOk.Location = new Point(291, 84);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 1;
            btnOk.Text = "&OK";
            btnOk.UseVisualStyleBackColor = true;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.Location = new Point(372, 84);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 2;
            btnCancel.Text = "&Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            AcceptButton = btnOk;
            AutoScaleDimensions = new SizeF(6f, 13f);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = btnCancel;
            ClientSize = new Size(463, 116);
            Controls.Add(btnCancel);
            Controls.Add(btnOk);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "CopyFileConfirmationDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            Text = "VsEmacs";
            ResumeLayout(false);
        }
    }
}