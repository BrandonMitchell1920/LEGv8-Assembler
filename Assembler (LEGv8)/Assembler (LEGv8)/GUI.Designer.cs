
namespace Assembler__LEGv8_
{
    partial class GUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openSourceFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveOutputFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assemblerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.translateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyRightToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.outputLabel = new System.Windows.Forms.Label();
            this.sourceFileTextBox = new System.Windows.Forms.TextBox();
            this.sourceFileLabel = new System.Windows.Forms.Label();
            this.sourceFileBrowseButton = new System.Windows.Forms.Button();
            this.translateButton = new System.Windows.Forms.Button();
            this.translateLabel = new System.Windows.Forms.Label();
            this.menuBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // outputTextBox
            // 
            this.outputTextBox.DetectUrls = false;
            resources.ApplyResources(this.outputTextBox, "outputTextBox");
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.assemblerToolStripMenuItem,
            this.helpToolStripMenuItem});
            resources.ApplyResources(this.menuBar, "menuBar");
            this.menuBar.Name = "menuBar";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openSourceFileToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveOutputFileToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            resources.ApplyResources(this.fileToolStripMenuItem, "fileToolStripMenuItem");
            // 
            // openSourceFileToolStripMenuItem
            // 
            this.openSourceFileToolStripMenuItem.Name = "openSourceFileToolStripMenuItem";
            resources.ApplyResources(this.openSourceFileToolStripMenuItem, "openSourceFileToolStripMenuItem");
            this.openSourceFileToolStripMenuItem.Click += new System.EventHandler((sender, e) => openSourceFile());
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // saveOutputFileToolStripMenuItem
            // 
            this.saveOutputFileToolStripMenuItem.Name = "saveOutputFileToolStripMenuItem";
            resources.ApplyResources(this.saveOutputFileToolStripMenuItem, "saveOutputFileToolStripMenuItem");
            this.saveOutputFileToolStripMenuItem.Click += new System.EventHandler((sender, e) => saveOutputText());
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            resources.ApplyResources(this.exitToolStripMenuItem, "exitToolStripMenuItem");
            this.exitToolStripMenuItem.Click += new System.EventHandler((sender, e) => System.Windows.Forms.Application.Exit());
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearOutputToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            resources.ApplyResources(this.editToolStripMenuItem, "editToolStripMenuItem");
            // 
            // clearOutputToolStripMenuItem
            // 
            this.clearOutputToolStripMenuItem.Name = "clearOutputToolStripMenuItem";
            resources.ApplyResources(this.clearOutputToolStripMenuItem, "clearOutputToolStripMenuItem");
            this.clearOutputToolStripMenuItem.Click += new System.EventHandler((sender, e) => outputTextBox.Clear());
            // 
            // assemblerToolStripMenuItem
            // 
            this.assemblerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.translateToolStripMenuItem});
            this.assemblerToolStripMenuItem.Name = "assemblerToolStripMenuItem";
            resources.ApplyResources(this.assemblerToolStripMenuItem, "assemblerToolStripMenuItem");
            // 
            // translateToolStripMenuItem
            // 
            this.translateToolStripMenuItem.Name = "translateToolStripMenuItem";
            resources.ApplyResources(this.translateToolStripMenuItem, "translateToolStripMenuItem");
            this.translateToolStripMenuItem.Click += new System.EventHandler((sender, e) => translateManager());
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.copyRightToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            resources.ApplyResources(this.helpToolStripMenuItem, "helpToolStripMenuItem");
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            resources.ApplyResources(this.aboutToolStripMenuItem, "aboutToolStripMenuItem");
            this.aboutToolStripMenuItem.Click += new System.EventHandler((sender, e) => about());
            // 
            // helpToolStripMenuItem1
            // 
            this.copyRightToolStripMenuItem.Name = "copyRightToolStripMenuItem";
            resources.ApplyResources(this.copyRightToolStripMenuItem, "copyRightToolStripMenuItem");
            this.copyRightToolStripMenuItem.Click += new System.EventHandler((sender, e) => copyright());
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // outputLabel
            // 
            resources.ApplyResources(this.outputLabel, "outputLabel");
            this.outputLabel.Name = "outputLabel";
            // 
            // sourceFileTextBox
            // 
            resources.ApplyResources(this.sourceFileTextBox, "sourceFileTextBox");
            this.sourceFileTextBox.Name = "sourceFileTextBox";
            this.sourceFileTextBox.ReadOnly = true;
            // 
            // sourceFileLabel
            // 
            resources.ApplyResources(this.sourceFileLabel, "sourceFileLabel");
            this.sourceFileLabel.Name = "sourceFileLabel";
            // 
            // sourceFileBrowseButton
            // 
            resources.ApplyResources(this.sourceFileBrowseButton, "sourceFileBrowseButton");
            this.sourceFileBrowseButton.Name = "sourceFileBrowseButton";
            this.sourceFileBrowseButton.UseVisualStyleBackColor = true;
            this.sourceFileBrowseButton.Click += new System.EventHandler((sender, e) => openSourceFile());
            // 
            // translateButton
            // 
            resources.ApplyResources(this.translateButton, "translateButton");
            this.translateButton.Name = "translateButton";
            this.translateButton.UseVisualStyleBackColor = true;
            this.translateButton.Click += new System.EventHandler((sender, e) => translateManager());
            // 
            // translateLabel
            // 
            resources.ApplyResources(this.translateLabel, "translateLabel");
            this.translateLabel.Name = "translateLabel";
            // 
            // GUI
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.translateLabel);
            this.Controls.Add(this.translateButton);
            this.Controls.Add(this.sourceFileBrowseButton);
            this.Controls.Add(this.sourceFileLabel);
            this.Controls.Add(this.sourceFileTextBox);
            this.Controls.Add(this.outputLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputTextBox);
            this.Controls.Add(this.menuBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.menuBar;
            this.MaximizeBox = false;
            this.Name = "GUI";
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox outputTextBox;
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openSourceFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem saveOutputFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearOutputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assemblerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem translateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyRightToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.TextBox sourceFileTextBox;
        private System.Windows.Forms.Label sourceFileLabel;
        private System.Windows.Forms.Button sourceFileBrowseButton;
        private System.Windows.Forms.Button translateButton;
        private System.Windows.Forms.Label translateLabel;
    }
}

