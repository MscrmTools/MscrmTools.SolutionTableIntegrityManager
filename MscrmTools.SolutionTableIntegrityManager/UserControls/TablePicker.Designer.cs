namespace MscrmTools.SolutionTableIntegrityManager.UserControls
{
    partial class TablePicker
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TablePicker));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lvSolutions = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chUniqueName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chManaged = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chReason = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbCheckFaulty = new System.Windows.Forms.ToolStripButton();
            this.tsbUncheckAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbShowOnlyFaulty = new System.Windows.Forms.ToolStripButton();
            this.tsbShowAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCheckManagedTables = new System.Windows.Forms.ToolStripButton();
            this.pnlMain.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(738, 77);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.Visible = false;
            // 
            // pnlFooter
            // 
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 1053);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(738, 102);
            this.pnlFooter.TabIndex = 1;
            this.pnlFooter.Visible = false;
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lvSolutions);
            this.pnlMain.Controls.Add(this.toolStrip1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 77);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(738, 976);
            this.pnlMain.TabIndex = 2;
            // 
            // lvSolutions
            // 
            this.lvSolutions.CheckBoxes = true;
            this.lvSolutions.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chUniqueName,
            this.chManaged,
            this.chReason});
            this.lvSolutions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvSolutions.FullRowSelect = true;
            this.lvSolutions.HideSelection = false;
            this.lvSolutions.Location = new System.Drawing.Point(0, 86);
            this.lvSolutions.MultiSelect = false;
            this.lvSolutions.Name = "lvSolutions";
            this.lvSolutions.Size = new System.Drawing.Size(1107, 1379);
            this.lvSolutions.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvSolutions.TabIndex = 2;
            this.lvSolutions.UseCompatibleStateImageBehavior = false;
            this.lvSolutions.View = System.Windows.Forms.View.Details;
            this.lvSolutions.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvSolutions_ColumnClick);
            // 
            // chName
            // 
            this.chName.Text = "Name";
            this.chName.Width = 200;
            // 
            // chUniqueName
            // 
            this.chUniqueName.Text = "Schema Name";
            this.chUniqueName.Width = 200;
            // 
            // chManaged
            // 
            this.chManaged.Text = "Is managed";
            this.chManaged.Width = 150;
            // 
            // chReason
            // 
            this.chReason.Text = "Reason";
            this.chReason.Width = 300;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCheckFaulty,
            this.tsbUncheckAll,
            this.toolStripSeparator1,
            this.tsbShowOnlyFaulty,
            this.tsbShowAll,
            this.toolStripSeparator2,
            this.tsbCheckManagedTables});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1107, 57);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // tsbCheckFaulty
            // 
            this.tsbCheckFaulty.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCheckFaulty.Image = ((System.Drawing.Image)(resources.GetObject("tsbCheckFaulty.Image")));
            this.tsbCheckFaulty.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCheckFaulty.Name = "tsbCheckFaulty";
            this.tsbCheckFaulty.Size = new System.Drawing.Size(186, 52);
            this.tsbCheckFaulty.Text = "Check all faulty tables";
            // 
            // tsbUncheckAll
            // 
            this.tsbUncheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbUncheckAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbUncheckAll.Image")));
            this.tsbUncheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUncheckAll.Name = "tsbUncheckAll";
            this.tsbUncheckAll.Size = new System.Drawing.Size(102, 52);
            this.tsbUncheckAll.Text = "UncheckAll";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 57);
            // 
            // tsbShowOnlyFaulty
            // 
            this.tsbShowOnlyFaulty.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbShowOnlyFaulty.Image = ((System.Drawing.Image)(resources.GetObject("tsbShowOnlyFaulty.Image")));
            this.tsbShowOnlyFaulty.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowOnlyFaulty.Name = "tsbShowOnlyFaulty";
            this.tsbShowOnlyFaulty.Size = new System.Drawing.Size(200, 52);
            this.tsbShowOnlyFaulty.Text = "Show only faulty tables";
            // 
            // tsbShowAll
            // 
            this.tsbShowAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbShowAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbShowAll.Image")));
            this.tsbShowAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbShowAll.Name = "tsbShowAll";
            this.tsbShowAll.Size = new System.Drawing.Size(82, 52);
            this.tsbShowAll.Text = "Show all";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 57);
            // 
            // tsbCheckManagedTables
            // 
            this.tsbCheckManagedTables.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCheckManagedTables.Image = ((System.Drawing.Image)(resources.GetObject("tsbCheckManagedTables.Image")));
            this.tsbCheckManagedTables.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCheckManagedTables.Name = "tsbCheckManagedTables";
            this.tsbCheckManagedTables.Size = new System.Drawing.Size(195, 52);
            this.tsbCheckManagedTables.Text = "Check managed tables";
            // 
            // TablePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlHeader);
            this.Name = "TablePicker";
            this.Size = new System.Drawing.Size(738, 1155);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.ListView lvSolutions;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chUniqueName;
        private System.Windows.Forms.ColumnHeader chManaged;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbCheckFaulty;
        private System.Windows.Forms.ToolStripButton tsbUncheckAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbShowOnlyFaulty;
        private System.Windows.Forms.ToolStripButton tsbShowAll;
        private System.Windows.Forms.ColumnHeader chReason;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbCheckManagedTables;
    }
}
