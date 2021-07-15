
namespace AmusementPark
{
    partial class GameForm
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.openParkToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.buildMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.roadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restaurantMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fuseBoxMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.waterGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bushMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grassMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restaurantMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.gameMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.settingMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startCampaignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startStopTimeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.timeStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.moneyStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openParkToolStripMenuItem1,
            this.buildMenuItem,
            this.settingMenuItem,
            this.startStopTimeMenuItem,
            this.exitMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1333, 33);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "Menu";
            this.menuStrip.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.MenuStrip_ItemClicked);
            // 
            // openParkToolStripMenuItem1
            // 
            this.openParkToolStripMenuItem1.Name = "openParkToolStripMenuItem1";
            this.openParkToolStripMenuItem1.Size = new System.Drawing.Size(110, 29);
            this.openParkToolStripMenuItem1.Text = "Open Park";
            this.openParkToolStripMenuItem1.Click += new System.EventHandler(this.OpenParkToolStripMenuItem1_Click);
            // 
            // buildMenuItem
            // 
            this.buildMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.roadMenuItem,
            this.restaurantMenuItem,
            this.gameMenuItem,
            this.treeMenuItem,
            this.fuseBoxMenuItem,
            this.waterToolStripMenuItem,
            this.waterGameToolStripMenuItem,
            this.bushMenuItem,
            this.grassMenuItem,
            this.restaurantMenuItem2,
            this.gameMenuItem2});
            this.buildMenuItem.Name = "buildMenuItem";
            this.buildMenuItem.Size = new System.Drawing.Size(67, 29);
            this.buildMenuItem.Text = "Build";
            // 
            // roadMenuItem
            // 
            this.roadMenuItem.Name = "roadMenuItem";
            this.roadMenuItem.Size = new System.Drawing.Size(272, 34);
            this.roadMenuItem.Text = "Road              ctrl+r";
            // 
            // restaurantMenuItem
            // 
            this.restaurantMenuItem.Name = "restaurantMenuItem";
            this.restaurantMenuItem.Size = new System.Drawing.Size(272, 34);
            this.restaurantMenuItem.Text = "Restaurant      ctrl+e";
            // 
            // gameMenuItem
            // 
            this.gameMenuItem.Name = "gameMenuItem";
            this.gameMenuItem.Size = new System.Drawing.Size(272, 34);
            this.gameMenuItem.Text = "Giant Wheel   ctrl+g";
            // 
            // treeMenuItem
            // 
            this.treeMenuItem.Name = "treeMenuItem";
            this.treeMenuItem.Size = new System.Drawing.Size(272, 34);
            this.treeMenuItem.Text = "Tree                ctrl+a";
            // 
            // fuseBoxMenuItem
            // 
            this.fuseBoxMenuItem.Name = "fuseBoxMenuItem";
            this.fuseBoxMenuItem.Size = new System.Drawing.Size(272, 34);
            this.fuseBoxMenuItem.Text = "Fuse Box        ctrl+f";
            // 
            // waterToolStripMenuItem
            // 
            this.waterToolStripMenuItem.Name = "waterToolStripMenuItem";
            this.waterToolStripMenuItem.Size = new System.Drawing.Size(272, 34);
            this.waterToolStripMenuItem.Text = "Water            ctrl+w";
            // 
            // waterGameToolStripMenuItem
            // 
            this.waterGameToolStripMenuItem.Name = "waterGameToolStripMenuItem";
            this.waterGameToolStripMenuItem.Size = new System.Drawing.Size(272, 34);
            this.waterGameToolStripMenuItem.Text = "Water Game  ctrl+q";
            // 
            // bushMenuItem
            // 
            this.bushMenuItem.Name = "bushMenuItem";
            this.bushMenuItem.Size = new System.Drawing.Size(272, 34);
            this.bushMenuItem.Text = "Bush              ctrl+b";
            // 
            // grassMenuItem
            // 
            this.grassMenuItem.Name = "grassMenuItem";
            this.grassMenuItem.Size = new System.Drawing.Size(272, 34);
            this.grassMenuItem.Text = "Grass             ctrl+x";
            // 
            // restaurantMenuItem2
            // 
            this.restaurantMenuItem2.Name = "restaurantMenuItem2";
            this.restaurantMenuItem2.Size = new System.Drawing.Size(272, 34);
            this.restaurantMenuItem2.Text = "Buffet            ctrl+d";
            // 
            // gameMenuItem2
            // 
            this.gameMenuItem2.Name = "gameMenuItem2";
            this.gameMenuItem2.Size = new System.Drawing.Size(272, 34);
            this.gameMenuItem2.Text = "Ringlispil       ctrl+y";
            // 
            // settingMenuItem
            // 
            this.settingMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startCampaignToolStripMenuItem,
            this.saveGameToolStripMenuItem,
            this.loadGameToolStripMenuItem});
            this.settingMenuItem.Name = "settingMenuItem";
            this.settingMenuItem.Size = new System.Drawing.Size(92, 29);
            this.settingMenuItem.Text = "Settings";
            // 
            // startCampaignToolStripMenuItem
            // 
            this.startCampaignToolStripMenuItem.Name = "startCampaignToolStripMenuItem";
            this.startCampaignToolStripMenuItem.Size = new System.Drawing.Size(251, 34);
            this.startCampaignToolStripMenuItem.Text = "Start Campaign";
            // 
            // saveGameToolStripMenuItem
            // 
            this.saveGameToolStripMenuItem.Name = "saveGameToolStripMenuItem";
            this.saveGameToolStripMenuItem.Size = new System.Drawing.Size(251, 34);
            this.saveGameToolStripMenuItem.Text = "Save Game ctrl+s";
            // 
            // loadGameToolStripMenuItem
            // 
            this.loadGameToolStripMenuItem.Name = "loadGameToolStripMenuItem";
            this.loadGameToolStripMenuItem.Size = new System.Drawing.Size(251, 34);
            this.loadGameToolStripMenuItem.Text = "Load Game ctrl+l";
            // 
            // startStopTimeMenuItem
            // 
            this.startStopTimeMenuItem.Name = "startStopTimeMenuItem";
            this.startStopTimeMenuItem.Size = new System.Drawing.Size(108, 29);
            this.startStopTimeMenuItem.Text = "Stop Time";
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(55, 29);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.ExitMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.timeStatusLabel,
            this.moneyStatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 833);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(2, 0, 23, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1333, 32);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip";
            // 
            // timeStatusLabel
            // 
            this.timeStatusLabel.Name = "timeStatusLabel";
            this.timeStatusLabel.Size = new System.Drawing.Size(54, 25);
            this.timeStatusLabel.Text = "Time:";
            // 
            // moneyStatusLabel
            // 
            this.moneyStatusLabel.Name = "moneyStatusLabel";
            this.moneyStatusLabel.Size = new System.Drawing.Size(71, 25);
            this.moneyStatusLabel.Text = "Money:";
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1333, 865);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "GameForm";
            this.Text = "AmusementPark";
            this.Load += new System.EventHandler(this.GameForm_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripMenuItem settingMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startStopTimeMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel timeStatusLabel;
        private System.Windows.Forms.ToolStripStatusLabel moneyStatusLabel;
        private System.Windows.Forms.ToolStripMenuItem roadMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restaurantMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem treeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fuseBoxMenuItem;
        private System.Windows.Forms.ToolStripMenuItem waterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem startCampaignToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openParkToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem waterGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem bushMenuItem;
        private System.Windows.Forms.ToolStripMenuItem grassMenuItem;
        private System.Windows.Forms.ToolStripMenuItem restaurantMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem gameMenuItem2;
    }
}

