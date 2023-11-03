using System;

namespace MscrmTools.SolutionTableIntegrityManager
{
    partial class PluginControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginControl));
            this.scSolutions = new System.Windows.Forms.SplitContainer();
            this.solutionPicker1 = new MscrmTools.SolutionTableIntegrityManager.UserControls.SolutionPicker();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbLoadSolutions = new System.Windows.Forms.ToolStripButton();
            this.tablePicker1 = new MscrmTools.SolutionTableIntegrityManager.UserControls.TablePicker();
            this.goodPracticeControl2 = new MscrmTools.SolutionTableIntegrityManager.UserControls.GoodPracticeControl();
            this.badPracticeControl1 = new MscrmTools.SolutionTableIntegrityManager.UserControls.BadPracticeControl();
            this.fixControl1 = new MscrmTools.SolutionTableIntegrityManager.UserControls.FixControl();
            this.fixControl2 = new MscrmTools.SolutionTableIntegrityManager.UserControls.FixControl();
            this.fixControl3 = new MscrmTools.SolutionTableIntegrityManager.UserControls.FixControl();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.progressControl1 = new MscrmTools.SolutionTableIntegrityManager.UserControls.ProgressControl();
            this.pnlResult = new System.Windows.Forms.Panel();
            this.fixControl4 = new MscrmTools.SolutionTableIntegrityManager.UserControls.FixControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pnlSeparator1 = new System.Windows.Forms.Panel();
            this.pnlSeparator2 = new System.Windows.Forms.Panel();
            this.pnlMode = new System.Windows.Forms.Panel();
            this.rdbSimulate = new System.Windows.Forms.RadioButton();
            this.rdbRealAction = new System.Windows.Forms.RadioButton();
            this.lblSimulation = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.scSolutions)).BeginInit();
            this.scSolutions.Panel1.SuspendLayout();
            this.scSolutions.Panel2.SuspendLayout();
            this.scSolutions.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.pnlResult.SuspendLayout();
            this.pnlMode.SuspendLayout();
            this.SuspendLayout();
            // 
            // scSolutions
            // 
            this.scSolutions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSolutions.Location = new System.Drawing.Point(0, 0);
            this.scSolutions.Name = "scSolutions";
            // 
            // scSolutions.Panel1
            // 
            this.scSolutions.Panel1.Controls.Add(this.solutionPicker1);
            this.scSolutions.Panel1.Controls.Add(this.toolStrip1);
            // 
            // scSolutions.Panel2
            // 
            this.scSolutions.Panel2.Controls.Add(this.tablePicker1);
            this.scSolutions.Size = new System.Drawing.Size(1145, 1328);
            this.scSolutions.SplitterDistance = 299;
            this.scSolutions.TabIndex = 5;
            // 
            // solutionPicker1
            // 
            this.solutionPicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.solutionPicker1.Location = new System.Drawing.Point(0, 34);
            this.solutionPicker1.Name = "solutionPicker1";
            this.solutionPicker1.Size = new System.Drawing.Size(299, 1294);
            this.solutionPicker1.TabIndex = 2;
            this.solutionPicker1.SolutionSelected += new System.EventHandler<MscrmTools.SolutionTableIntegrityManager.UserControls.SolutionSelectedEventArgs>(this.solutionPicker1_SolutionSelected);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLoadSolutions});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(299, 34);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbLoadSolutions
            // 
            this.tsbLoadSolutions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbLoadSolutions.Name = "tsbLoadSolutions";
            this.tsbLoadSolutions.Size = new System.Drawing.Size(132, 29);
            this.tsbLoadSolutions.Text = "Load solutions";
            this.tsbLoadSolutions.Click += new System.EventHandler(this.tsbSample_Click);
            // 
            // tablePicker1
            // 
            this.tablePicker1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tablePicker1.Location = new System.Drawing.Point(0, 0);
            this.tablePicker1.Name = "tablePicker1";
            this.tablePicker1.Size = new System.Drawing.Size(842, 1328);
            this.tablePicker1.TabIndex = 0;
            // 
            // goodPracticeControl2
            // 
            this.goodPracticeControl2.BackColor = System.Drawing.SystemColors.Info;
            this.goodPracticeControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.goodPracticeControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.goodPracticeControl2.Location = new System.Drawing.Point(10, 10);
            this.goodPracticeControl2.Name = "goodPracticeControl2";
            this.goodPracticeControl2.Size = new System.Drawing.Size(818, 354);
            this.goodPracticeControl2.TabIndex = 7;
            // 
            // badPracticeControl1
            // 
            this.badPracticeControl1.BackColor = System.Drawing.SystemColors.Info;
            this.badPracticeControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.badPracticeControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.badPracticeControl1.Location = new System.Drawing.Point(10, 364);
            this.badPracticeControl1.Name = "badPracticeControl1";
            this.badPracticeControl1.Size = new System.Drawing.Size(818, 466);
            this.badPracticeControl1.TabIndex = 7;
            // 
            // fixControl1
            // 
            this.fixControl1.BackColor = System.Drawing.SystemColors.Info;
            this.fixControl1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fixControl1.Description = resources.GetString("fixControl1.Description");
            this.fixControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.fixControl1.Location = new System.Drawing.Point(10, 886);
            this.fixControl1.Name = "fixControl1";
            this.fixControl1.Padding = new System.Windows.Forms.Padding(10);
            this.fixControl1.Size = new System.Drawing.Size(818, 295);
            this.fixControl1.TabIndex = 3;
            this.fixControl1.Title = "Fix 1 : Add unmanaged assets only";
            this.fixControl1.Apply += new System.EventHandler(this.fixControl_Apply);
            // 
            // fixControl2
            // 
            this.fixControl2.BackColor = System.Drawing.SystemColors.Info;
            this.fixControl2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fixControl2.Description = resources.GetString("fixControl2.Description");
            this.fixControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.fixControl2.Location = new System.Drawing.Point(10, 1191);
            this.fixControl2.Name = "fixControl2";
            this.fixControl2.Padding = new System.Windows.Forms.Padding(10);
            this.fixControl2.Size = new System.Drawing.Size(818, 287);
            this.fixControl2.TabIndex = 4;
            this.fixControl2.Title = "Fix 2 : Add all updated assets";
            this.fixControl2.Apply += new System.EventHandler(this.fixControl_Apply);
            // 
            // fixControl3
            // 
            this.fixControl3.BackColor = System.Drawing.SystemColors.Info;
            this.fixControl3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fixControl3.Description = "This fix will remove the table from the solution, add it back to the solution wit" +
    "hout any asset if the table is managed and all assets if the table is unmanaged." +
    " It\'s up to you to add assets you need";
            this.fixControl3.Dock = System.Windows.Forms.DockStyle.Top;
            this.fixControl3.Location = new System.Drawing.Point(10, 1488);
            this.fixControl3.Name = "fixControl3";
            this.fixControl3.Padding = new System.Windows.Forms.Padding(10);
            this.fixControl3.Size = new System.Drawing.Size(818, 228);
            this.fixControl3.TabIndex = 5;
            this.fixControl3.Title = "Fix 3 : Choose yourself the components to add in the solution";
            this.fixControl3.Apply += new System.EventHandler(this.fixControl_Apply);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 0);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.scSolutions);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.progressControl1);
            this.scMain.Panel2Collapsed = true;
            this.scMain.Size = new System.Drawing.Size(1145, 1328);
            this.scMain.SplitterDistance = 817;
            this.scMain.TabIndex = 6;
            // 
            // progressControl1
            // 
            this.progressControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressControl1.Location = new System.Drawing.Point(0, 0);
            this.progressControl1.Name = "progressControl1";
            this.progressControl1.Padding = new System.Windows.Forms.Padding(10);
            this.progressControl1.Size = new System.Drawing.Size(150, 46);
            this.progressControl1.TabIndex = 0;
            this.progressControl1.OnApply += new System.EventHandler<MscrmTools.SolutionTableIntegrityManager.AppCode.OnApplyFixEventArgs>(this.progressControl1_OnApply);
            // 
            // pnlResult
            // 
            this.pnlResult.AutoScroll = true;
            this.pnlResult.Controls.Add(this.fixControl4);
            this.pnlResult.Controls.Add(this.panel1);
            this.pnlResult.Controls.Add(this.fixControl3);
            this.pnlResult.Controls.Add(this.pnlSeparator1);
            this.pnlResult.Controls.Add(this.fixControl2);
            this.pnlResult.Controls.Add(this.pnlSeparator2);
            this.pnlResult.Controls.Add(this.fixControl1);
            this.pnlResult.Controls.Add(this.pnlMode);
            this.pnlResult.Controls.Add(this.badPracticeControl1);
            this.pnlResult.Controls.Add(this.goodPracticeControl2);
            this.pnlResult.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnlResult.Location = new System.Drawing.Point(1145, 0);
            this.pnlResult.Name = "pnlResult";
            this.pnlResult.Padding = new System.Windows.Forms.Padding(10);
            this.pnlResult.Size = new System.Drawing.Size(864, 1328);
            this.pnlResult.TabIndex = 8;
            // 
            // fixControl4
            // 
            this.fixControl4.BackColor = System.Drawing.SystemColors.Info;
            this.fixControl4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.fixControl4.Description = "This will find managed assets that have been modified and you would like to embed" +
    " in the solution";
            this.fixControl4.Dock = System.Windows.Forms.DockStyle.Top;
            this.fixControl4.Location = new System.Drawing.Point(10, 1726);
            this.fixControl4.Name = "fixControl4";
            this.fixControl4.Padding = new System.Windows.Forms.Padding(10);
            this.fixControl4.Size = new System.Drawing.Size(818, 228);
            this.fixControl4.TabIndex = 12;
            this.fixControl4.Title = "Add managed assets that were updated";
            this.fixControl4.Apply += new System.EventHandler(this.fixControl_Apply);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(10, 1716);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(818, 10);
            this.panel1.TabIndex = 11;
            // 
            // pnlSeparator1
            // 
            this.pnlSeparator1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSeparator1.Location = new System.Drawing.Point(10, 1478);
            this.pnlSeparator1.Name = "pnlSeparator1";
            this.pnlSeparator1.Size = new System.Drawing.Size(818, 10);
            this.pnlSeparator1.TabIndex = 9;
            // 
            // pnlSeparator2
            // 
            this.pnlSeparator2.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSeparator2.Location = new System.Drawing.Point(10, 1181);
            this.pnlSeparator2.Name = "pnlSeparator2";
            this.pnlSeparator2.Size = new System.Drawing.Size(818, 10);
            this.pnlSeparator2.TabIndex = 9;
            // 
            // pnlMode
            // 
            this.pnlMode.Controls.Add(this.rdbSimulate);
            this.pnlMode.Controls.Add(this.rdbRealAction);
            this.pnlMode.Controls.Add(this.lblSimulation);
            this.pnlMode.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMode.Location = new System.Drawing.Point(10, 830);
            this.pnlMode.Name = "pnlMode";
            this.pnlMode.Size = new System.Drawing.Size(818, 56);
            this.pnlMode.TabIndex = 8;
            this.pnlMode.Visible = false;
            // 
            // rdbSimulate
            // 
            this.rdbSimulate.Checked = true;
            this.rdbSimulate.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdbSimulate.Location = new System.Drawing.Point(171, 0);
            this.rdbSimulate.Name = "rdbSimulate";
            this.rdbSimulate.Size = new System.Drawing.Size(307, 56);
            this.rdbSimulate.TabIndex = 2;
            this.rdbSimulate.TabStop = true;
            this.rdbSimulate.Text = "Simulation (nothing will be updated)";
            this.rdbSimulate.UseVisualStyleBackColor = true;
            // 
            // rdbRealAction
            // 
            this.rdbRealAction.Dock = System.Windows.Forms.DockStyle.Left;
            this.rdbRealAction.Location = new System.Drawing.Point(68, 0);
            this.rdbRealAction.Name = "rdbRealAction";
            this.rdbRealAction.Size = new System.Drawing.Size(103, 56);
            this.rdbRealAction.TabIndex = 1;
            this.rdbRealAction.Text = "Update";
            this.rdbRealAction.UseVisualStyleBackColor = true;
            // 
            // lblSimulation
            // 
            this.lblSimulation.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblSimulation.Location = new System.Drawing.Point(0, 0);
            this.lblSimulation.Name = "lblSimulation";
            this.lblSimulation.Size = new System.Drawing.Size(68, 56);
            this.lblSimulation.TabIndex = 0;
            this.lblSimulation.Text = "Mode";
            this.lblSimulation.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.pnlResult);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PluginControl";
            this.Size = new System.Drawing.Size(2009, 1328);
            this.Resize += new System.EventHandler(this.PluginControl_Resize);
            this.scSolutions.Panel1.ResumeLayout(false);
            this.scSolutions.Panel1.PerformLayout();
            this.scSolutions.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scSolutions)).EndInit();
            this.scSolutions.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.pnlResult.ResumeLayout(false);
            this.pnlMode.ResumeLayout(false);
            this.ResumeLayout(false);

        }

     

        #endregion
        private System.Windows.Forms.SplitContainer scSolutions;
        private UserControls.TablePicker tablePicker1;
        private UserControls.FixControl fixControl1;
        private UserControls.FixControl fixControl2;
        private UserControls.FixControl fixControl3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private UserControls.SolutionPicker solutionPicker1;
        private UserControls.GoodPracticeControl goodPracticeControl2;
        private UserControls.BadPracticeControl badPracticeControl1;
        private System.Windows.Forms.ToolStripButton tsbLoadSolutions;
        private System.Windows.Forms.SplitContainer scMain;
        private UserControls.ProgressControl progressControl1;
        private System.Windows.Forms.Panel pnlResult;
        private System.Windows.Forms.Panel pnlMode;
        private System.Windows.Forms.RadioButton rdbSimulate;
        private System.Windows.Forms.RadioButton rdbRealAction;
        private System.Windows.Forms.Label lblSimulation;
        private System.Windows.Forms.Panel pnlSeparator1;
        private System.Windows.Forms.Panel pnlSeparator2;
        private UserControls.FixControl fixControl4;
        private System.Windows.Forms.Panel panel1;
    }
}
