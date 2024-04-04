
namespace MscrmTools.SolutionTableIntegrityManager.UserControls
{
    partial class SelectiveApplier
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectiveApplier));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblDesc = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnMaximize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lvLogs = new System.Windows.Forms.ListView();
            this.chTable = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chType = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chComponent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chChangedProperties = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tslSearch = new System.Windows.Forms.ToolStripLabel();
            this.tstbSearch = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCheckAll = new System.Windows.Forms.ToolStripButton();
            this.tsbUncheckAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tslCount = new System.Windows.Forms.ToolStripLabel();
            this.scintilla1 = new ScintillaNET.Scintilla();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pnlTop.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.White;
            this.pnlTop.Controls.Add(this.lblDesc);
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Padding = new System.Windows.Forms.Padding(10);
            this.pnlTop.Size = new System.Drawing.Size(1647, 105);
            this.pnlTop.TabIndex = 0;
            // 
            // lblDesc
            // 
            this.lblDesc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDesc.Font = new System.Drawing.Font("Segoe UI Variable Text", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDesc.Location = new System.Drawing.Point(10, 55);
            this.lblDesc.Name = "lblDesc";
            this.lblDesc.Size = new System.Drawing.Size(1627, 40);
            this.lblDesc.TabIndex = 1;
            this.lblDesc.Text = "Select items to see details of the changed properties, check those you want to in" +
    "clude in your solution and click on Apply button";
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Variable Text", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(1627, 45);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Check items you want to include in your solution";
            // 
            // pnlBottom
            // 
            this.pnlBottom.Controls.Add(this.btnApply);
            this.pnlBottom.Controls.Add(this.btnMaximize);
            this.pnlBottom.Controls.Add(this.btnClose);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Location = new System.Drawing.Point(0, 875);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Padding = new System.Windows.Forms.Padding(10);
            this.pnlBottom.Size = new System.Drawing.Size(1647, 70);
            this.pnlBottom.TabIndex = 1;
            // 
            // btnApply
            // 
            this.btnApply.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnApply.Location = new System.Drawing.Point(1043, 10);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(261, 50);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply fix for checked items";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnMaximize
            // 
            this.btnMaximize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMaximize.Location = new System.Drawing.Point(1304, 10);
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.Size = new System.Drawing.Size(172, 50);
            this.btnMaximize.TabIndex = 2;
            this.btnMaximize.Text = "Maximize";
            this.btnMaximize.UseVisualStyleBackColor = true;
            this.btnMaximize.Click += new System.EventHandler(this.btnMaximize_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(1476, 10);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(161, 50);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.splitContainer1);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 105);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMain.Size = new System.Drawing.Size(1647, 770);
            this.pnlMain.TabIndex = 2;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(10, 10);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lvLogs);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.scintilla1);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(1627, 750);
            this.splitContainer1.SplitterDistance = 1050;
            this.splitContainer1.TabIndex = 0;
            // 
            // lvLogs
            // 
            this.lvLogs.CheckBoxes = true;
            this.lvLogs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTable,
            this.chType,
            this.chComponent,
            this.chChangedProperties});
            this.lvLogs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvLogs.FullRowSelect = true;
            this.lvLogs.HideSelection = false;
            this.lvLogs.Location = new System.Drawing.Point(0, 34);
            this.lvLogs.Name = "lvLogs";
            this.lvLogs.Size = new System.Drawing.Size(1627, 716);
            this.lvLogs.TabIndex = 6;
            this.lvLogs.UseCompatibleStateImageBehavior = false;
            this.lvLogs.View = System.Windows.Forms.View.Details;
            this.lvLogs.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvLogs_ColumnClick);
            this.lvLogs.SelectedIndexChanged += new System.EventHandler(this.lvLogs_SelectedIndexChanged);
            // 
            // chTable
            // 
            this.chTable.Text = "Table";
            this.chTable.Width = 200;
            // 
            // chType
            // 
            this.chType.Text = "Type";
            this.chType.Width = 150;
            // 
            // chComponent
            // 
            this.chComponent.Text = "Name";
            this.chComponent.Width = 150;
            // 
            // chChangedProperties
            // 
            this.chChangedProperties.Text = "Changed properties";
            this.chChangedProperties.Width = 150;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslSearch,
            this.tstbSearch,
            this.toolStripSeparator1,
            this.tsbCheckAll,
            this.tsbUncheckAll,
            this.toolStripSeparator2,
            this.tslCount});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1627, 34);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.toolStrip1_ItemClicked);
            // 
            // tslSearch
            // 
            this.tslSearch.Name = "tslSearch";
            this.tslSearch.Size = new System.Drawing.Size(64, 29);
            this.tslSearch.Text = "Search";
            // 
            // tstbSearch
            // 
            this.tstbSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstbSearch.Name = "tstbSearch";
            this.tstbSearch.Size = new System.Drawing.Size(200, 34);
            this.tstbSearch.TextChanged += new System.EventHandler(this.tstbSearch_TextChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 34);
            // 
            // tsbCheckAll
            // 
            this.tsbCheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCheckAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbCheckAll.Image")));
            this.tsbCheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCheckAll.Name = "tsbCheckAll";
            this.tsbCheckAll.Size = new System.Drawing.Size(85, 29);
            this.tsbCheckAll.Text = "Check all";
            // 
            // tsbUncheckAll
            // 
            this.tsbUncheckAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbUncheckAll.Image = ((System.Drawing.Image)(resources.GetObject("tsbUncheckAll.Image")));
            this.tsbUncheckAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUncheckAll.Name = "tsbUncheckAll";
            this.tsbUncheckAll.Size = new System.Drawing.Size(104, 29);
            this.tsbUncheckAll.Text = "Uncheck all";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 34);
            // 
            // tslCount
            // 
            this.tslCount.Name = "tslCount";
            this.tslCount.Size = new System.Drawing.Size(156, 29);
            this.tslCount.Tag = "Checked assets : {0}";
            this.tslCount.Text = "Checked assets : 0";
            // 
            // scintilla1
            // 
            this.scintilla1.AutomaticFold = ((ScintillaNET.AutomaticFold)(((ScintillaNET.AutomaticFold.Show | ScintillaNET.AutomaticFold.Click) 
            | ScintillaNET.AutomaticFold.Change)));
            this.scintilla1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintilla1.IndentationGuides = ScintillaNET.IndentView.Real;
            this.scintilla1.Lexer = ScintillaNET.Lexer.Json;
            this.scintilla1.Location = new System.Drawing.Point(0, 0);
            this.scintilla1.Name = "scintilla1";
            this.scintilla1.Size = new System.Drawing.Size(96, 100);
            this.scintilla1.TabIndex = 0;
            this.scintilla1.Text = "scintilla1";
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "info.png");
            this.imageList1.Images.SetKeyName(1, "cells.png");
            this.imageList1.Images.SetKeyName(2, "letter.png");
            this.imageList1.Images.SetKeyName(3, "share.png");
            this.imageList1.Images.SetKeyName(4, "key.png");
            this.imageList1.Images.SetKeyName(5, "forms.png");
            this.imageList1.Images.SetKeyName(6, "dashboard.png");
            this.imageList1.Images.SetKeyName(7, "folder.png");
            this.imageList1.Images.SetKeyName(8, "line-chart.png");
            this.imageList1.Images.SetKeyName(9, "loop.png");
            this.imageList1.Images.SetKeyName(10, "button.png");
            // 
            // SelectiveApplier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlBottom);
            this.Controls.Add(this.pnlTop);
            this.Name = "SelectiveApplier";
            this.Size = new System.Drawing.Size(1647, 945);
            this.Load += new System.EventHandler(this.SelectiveApplier_Load);
            this.pnlTop.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlMain.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDesc;
        private System.Windows.Forms.ListView lvLogs;
        private System.Windows.Forms.ColumnHeader chTable;
        private System.Windows.Forms.ColumnHeader chType;
        private System.Windows.Forms.ColumnHeader chComponent;
        private System.Windows.Forms.ColumnHeader chChangedProperties;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel tslSearch;
        private System.Windows.Forms.ToolStripTextBox tstbSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbCheckAll;
        private System.Windows.Forms.ToolStripButton tsbUncheckAll;
        private ScintillaNET.Scintilla scintilla1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel tslCount;
    }
}
