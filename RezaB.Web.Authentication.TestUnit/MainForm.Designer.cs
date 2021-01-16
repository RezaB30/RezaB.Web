
namespace RezaB.Web.Authentication.TestUnit
{
    partial class MainForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.UsernameTextbox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PasswordTextbox = new System.Windows.Forms.TextBox();
            this.SignInButton = new System.Windows.Forms.Button();
            this.SignInWithPermissionsButton = new System.Windows.Forms.Button();
            this.ResultsListbox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Username:";
            // 
            // UsernameTextbox
            // 
            this.UsernameTextbox.Location = new System.Drawing.Point(77, 13);
            this.UsernameTextbox.Name = "UsernameTextbox";
            this.UsernameTextbox.Size = new System.Drawing.Size(158, 20);
            this.UsernameTextbox.TabIndex = 1;
            this.UsernameTextbox.Text = "r.barzegaran@gmail.com";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Password:";
            // 
            // PasswordTextbox
            // 
            this.PasswordTextbox.Location = new System.Drawing.Point(77, 39);
            this.PasswordTextbox.Name = "PasswordTextbox";
            this.PasswordTextbox.Size = new System.Drawing.Size(158, 20);
            this.PasswordTextbox.TabIndex = 1;
            // 
            // SignInButton
            // 
            this.SignInButton.Location = new System.Drawing.Point(77, 65);
            this.SignInButton.Name = "SignInButton";
            this.SignInButton.Size = new System.Drawing.Size(158, 23);
            this.SignInButton.TabIndex = 2;
            this.SignInButton.Text = "Sign In";
            this.SignInButton.UseVisualStyleBackColor = true;
            this.SignInButton.Click += new System.EventHandler(this.SignInButton_Click);
            // 
            // SignInWithPermissionsButton
            // 
            this.SignInWithPermissionsButton.Location = new System.Drawing.Point(77, 94);
            this.SignInWithPermissionsButton.Name = "SignInWithPermissionsButton";
            this.SignInWithPermissionsButton.Size = new System.Drawing.Size(158, 23);
            this.SignInWithPermissionsButton.TabIndex = 3;
            this.SignInWithPermissionsButton.Text = "Sign In With Permissions";
            this.SignInWithPermissionsButton.UseVisualStyleBackColor = true;
            this.SignInWithPermissionsButton.Click += new System.EventHandler(this.SignInWithPermissionsButton_Click);
            // 
            // ResultsListbox
            // 
            this.ResultsListbox.FormattingEnabled = true;
            this.ResultsListbox.Location = new System.Drawing.Point(261, 13);
            this.ResultsListbox.Name = "ResultsListbox";
            this.ResultsListbox.Size = new System.Drawing.Size(527, 420);
            this.ResultsListbox.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ResultsListbox);
            this.Controls.Add(this.SignInWithPermissionsButton);
            this.Controls.Add(this.SignInButton);
            this.Controls.Add(this.PasswordTextbox);
            this.Controls.Add(this.UsernameTextbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Authenticator Test Unit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox UsernameTextbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PasswordTextbox;
        private System.Windows.Forms.Button SignInButton;
        private System.Windows.Forms.Button SignInWithPermissionsButton;
        private System.Windows.Forms.ListBox ResultsListbox;
    }
}

