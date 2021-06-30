namespace Mawse2D
{
    partial class Console
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Console));
            this.console_input = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // console_input
            // 
            this.console_input.BackColor = System.Drawing.SystemColors.MenuText;
            this.console_input.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.console_input.ForeColor = System.Drawing.SystemColors.Window;
            this.console_input.Location = new System.Drawing.Point(-1, -3);
            this.console_input.Name = "console_input";
            this.console_input.Size = new System.Drawing.Size(801, 456);
            this.console_input.TabIndex = 0;
            this.console_input.Text = "";
            // 
            // Console
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(508, 108);
            this.Controls.Add(this.console_input);
            this.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Console";
            this.Text = "Console";
            this.Load += new System.EventHandler(this.Console_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox console_input;
    }
}