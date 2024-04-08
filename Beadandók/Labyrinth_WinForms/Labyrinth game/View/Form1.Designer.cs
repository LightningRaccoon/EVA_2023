namespace Labyrinth_game.View
{
    partial class FormWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormWindow));
            formPanel = new Panel();
            TimerLabel = new Label();
            pause_button = new Button();
            menuStrip = new MenuStrip();
            gameMenuItem = new ToolStripMenuItem();
            newMenuItem = new ToolStripMenuItem();
            easyMenuItem = new ToolStripMenuItem();
            mediumMenuItem = new ToolStripMenuItem();
            hardMenuItem = new ToolStripMenuItem();
            saveMenuItem = new ToolStripMenuItem();
            loadMenuItem = new ToolStripMenuItem();
            exitMenuItem = new ToolStripMenuItem();
            formPanel.SuspendLayout();
            menuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // formPanel
            // 
            formPanel.BackColor = SystemColors.Info;
            formPanel.BackgroundImageLayout = ImageLayout.Stretch;
            formPanel.BorderStyle = BorderStyle.FixedSingle;
            formPanel.Controls.Add(TimerLabel);
            formPanel.Controls.Add(pause_button);
            formPanel.Dock = DockStyle.Bottom;
            formPanel.Location = new Point(0, 262);
            formPanel.Name = "formPanel";
            formPanel.Size = new Size(395, 31);
            formPanel.TabIndex = 0;
            // 
            // TimerLabel
            // 
            TimerLabel.Dock = DockStyle.Left;
            TimerLabel.Location = new Point(0, 0);
            TimerLabel.Margin = new Padding(3);
            TimerLabel.Name = "TimerLabel";
            TimerLabel.Size = new Size(170, 29);
            TimerLabel.TabIndex = 3;
            TimerLabel.Text = "Elapsed time: -";
            TimerLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // pause_button
            // 
            pause_button.Dock = DockStyle.Right;
            pause_button.Location = new Point(299, 0);
            pause_button.Margin = new Padding(5);
            pause_button.Name = "pause_button";
            pause_button.Size = new Size(94, 29);
            pause_button.TabIndex = 2;
            pause_button.Text = "Pause Game";
            pause_button.UseVisualStyleBackColor = true;
            pause_button.Click += Pause_button_Click;
            // 
            // menuStrip
            // 
            menuStrip.ImageScalingSize = new Size(20, 20);
            menuStrip.Items.AddRange(new ToolStripItem[] { gameMenuItem });
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "menuStrip";
            menuStrip.Size = new Size(395, 28);
            menuStrip.TabIndex = 1;
            menuStrip.Text = "menuStrip";
            // 
            // gameMenuItem
            // 
            gameMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newMenuItem, saveMenuItem, loadMenuItem, exitMenuItem });
            gameMenuItem.Name = "gameMenuItem";
            gameMenuItem.Size = new Size(118, 24);
            gameMenuItem.Text = "Game Options";
            // 
            // newMenuItem
            // 
            newMenuItem.DropDownItems.AddRange(new ToolStripItem[] { easyMenuItem, mediumMenuItem, hardMenuItem });
            newMenuItem.Name = "newMenuItem";
            newMenuItem.Size = new Size(224, 26);
            newMenuItem.Text = "New Game";
            // 
            // easyMenuItem
            // 
            easyMenuItem.Name = "easyMenuItem";
            easyMenuItem.Size = new Size(147, 26);
            easyMenuItem.Text = "Easy";
            easyMenuItem.Click += New_easy_game_Click;
            // 
            // mediumMenuItem
            // 
            mediumMenuItem.Name = "mediumMenuItem";
            mediumMenuItem.Size = new Size(147, 26);
            mediumMenuItem.Text = "Medium";
            mediumMenuItem.Click += New_medium_game_Click;
            // 
            // hardMenuItem
            // 
            hardMenuItem.Name = "hardMenuItem";
            hardMenuItem.Size = new Size(147, 26);
            hardMenuItem.Text = "Hard";
            hardMenuItem.Click += New_hard_game_Click;
            // 
            // saveMenuItem
            // 
            saveMenuItem.Name = "saveMenuItem";
            saveMenuItem.Size = new Size(224, 26);
            saveMenuItem.Text = "Save Game";
            saveMenuItem.Click += Save_button_Click;
            // 
            // loadMenuItem
            // 
            loadMenuItem.Name = "loadMenuItem";
            loadMenuItem.Size = new Size(224, 26);
            loadMenuItem.Text = "Load Game";
            loadMenuItem.Click += Load_button_Click;
            // 
            // exitMenuItem
            // 
            exitMenuItem.Name = "exitMenuItem";
            exitMenuItem.Size = new Size(224, 26);
            exitMenuItem.Text = "Exit Game";
            exitMenuItem.Click += Exit_button_Click;
            // 
            // FormWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSize = true;
            BackColor = SystemColors.ScrollBar;
            BackgroundImage = Properties.Resources._45908;
            BackgroundImageLayout = ImageLayout.Zoom;
            ClientSize = new Size(395, 293);
            Controls.Add(formPanel);
            Controls.Add(menuStrip);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip;
            Name = "FormWindow";
            Text = "LabyrinthGame";
            formPanel.ResumeLayout(false);
            menuStrip.ResumeLayout(false);
            menuStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel formPanel;
        private Button pause_button;
        private Label TimerLabel;
        private MenuStrip menuStrip;
        private ToolStripMenuItem gameMenuItem;
        private ToolStripMenuItem newMenuItem;
        private ToolStripMenuItem saveMenuItem;
        private ToolStripMenuItem loadMenuItem;
        private ToolStripMenuItem exitMenuItem;
        private ToolStripMenuItem easyMenuItem;
        private ToolStripMenuItem mediumMenuItem;
        private ToolStripMenuItem hardMenuItem;
    }
}