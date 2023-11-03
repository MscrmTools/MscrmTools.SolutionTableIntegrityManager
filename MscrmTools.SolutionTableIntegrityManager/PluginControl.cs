using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using MscrmTools.SolutionTableIntegrityManager.AppCode;
using MscrmTools.SolutionTableIntegrityManager.UserControls;
using System;
using System.Linq;
using System.Windows.Forms;
using XrmToolBox.Extensibility;

namespace MscrmTools.SolutionTableIntegrityManager
{
    public partial class PluginControl : PluginControlBase
    {
        public PluginControl()
        {
            InitializeComponent();

            pnlResult.Visible = false;
            pnlResult.Width = 350;
            goodPracticeControl2.Height = 200;
            badPracticeControl1.Height = 250;
            fixControl1.Height = 150;
            fixControl2.Height = 150;
            fixControl3.Height = 150;
            fixControl4.Height = 150;
            progressControl1.CloseRequested += (sender, e) => { scMain.Panel2Collapsed = true; };
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);

            ExecuteMethod(GetSolutions);
        }

        private void fixControl_Apply(object sender, EventArgs e)
        {
            var solution = solutionPicker1.SelectedSolution?.GetAttributeValue<string>("uniquename");

            if (solution == null)
            {
                MessageBox.Show(this, "Please select a solution", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var tables = tablePicker1.SelectedTables.Where(t => !t.IsBestPractice).Select(t => t.Metadata).ToList();
            var correctTables = tablePicker1.SelectedTables.Where(t => t.IsBestPractice).Select(t => t.Metadata).ToList();
            var isSimulation = rdbSimulate.Checked;

            if (tables.Count == 0 && sender != fixControl4 || correctTables.Count == 0 && sender == fixControl4)
            {
                MessageBox.Show(this, "Please check tables that need to be fixed", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (rdbRealAction.Checked)
            {
                if (DialogResult.No == MessageBox.Show(this, "Are you sure you want to apply the selected fix?\n\nTHIS IS NOT A SIMULATION\n\nThe solution will be fixed to remove checked tables and add them back following best practices according to the fix you chose.\n\nOr you can choose to simulate the fix process to understand what actions will be taken.", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
            }

            progressControl1.SetSelectiveApplierButtonVisibility(false);
            progressControl1.Clear();
            scMain.Panel2Collapsed = false;
            SetWorkingState(true);

            WorkAsync(new WorkAsyncInfo
            {
                Message = isSimulation ? "Performing simulation..." : "Processing...",
                Work = (worker, args) =>
                {
                    var tc = new TableCleaner(Service, isSimulation);
                    tc.Worker = worker;
                    if (sender == fixControl1)
                    {
                        tc.Clean(solution, tables, true, false);
                    }
                    else if (sender == fixControl2)
                    {
                        tc.Clean(solution, tables, true, true);
                    }
                    else if (sender == fixControl3)
                    {
                        tc.Clean(solution, tables, false, false);
                    }
                    else if (sender == fixControl4)
                    {
                        tc.Clean(solution, correctTables, false, true, true, true);
                    }
                },
                ProgressChanged = evt =>
                {
                    progressControl1.AddLog((TableLog)evt.UserState, evt.ProgressPercentage);
                },
                PostWorkCallBack = (args) =>
                {
                    SetWorkingState(false);

                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (!isSimulation) { 
                        solutionPicker1_SolutionSelected(solutionPicker1, new UserControls.SolutionSelectedEventArgs(solutionPicker1.SelectedSolution));
                    }

                    if (isSimulation && sender == fixControl2 || sender == fixControl4)
                    {
                        progressControl1.SetSelectiveApplierButtonVisibility(true);
                        progressControl1.FixSender = fixControl2;
                        MessageBox.Show(this, "You can now review the logs and apply the update for all assets detected or you can select only the one you want to add in your solution.", "Simulation finished!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        progressControl1.SetSelectiveApplierButtonVisibility(false);
                    }

                    rdbSimulate.Checked = true;
                }
            });
        }

        private void GetSolutions()
        {
            scMain.Panel2Collapsed = true;
            SetWorkingState(true);
            tablePicker1.Clear();
            pnlResult.Visible = false;

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading solutions...",
                Work = (worker, args) =>
                {
                    solutionPicker1.LoadSolutions(Service);
                },
                PostWorkCallBack = (args) =>
                {
                    SetWorkingState(false);

                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    solutionPicker1.DisplaySolutions();
                }
            });
        }

        private void PluginControl_Resize(object sender, EventArgs e)
        {
            var control = Controls.Find("selectiveApplier1", true);
            if (control.Length == 1)
            {
                if (((SelectiveApplier)control[0]).IsMaximized)
                {
                    control[0].Width = Width;
                    control[0].Height = Height;
                    control[0].Location = new System.Drawing.Point(0, 0);
                }
                else
                {
                    control[0].Width = Convert.ToInt32(Width * 0.7);
                    control[0].Height = Convert.ToInt32(Height * 0.7);
                    control[0].Location = new System.Drawing.Point(Width / 2 - control[0].Width / 2, Height / 2 - control[0].Height / 2);
                }
            }
        }

        private void progressControl1_OnApply(object sender, OnApplyFixEventArgs e)
        {
            var solution = solutionPicker1.SelectedSolution?.GetAttributeValue<string>("uniquename");

            if (solution == null)
            {
                MessageBox.Show(this, "Please select a solution", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DialogResult.No == MessageBox.Show(this, "Are you sure you want to add the selected assets to the solution?\n\nTHIS IS NOT A SIMULATION", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }

            scMain.Panel2Collapsed = false;
            progressControl1.SetSelectiveApplierButtonVisibility(false);
            progressControl1.Clear();
            SetWorkingState(true);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Processing...",
                Work = (worker, args) =>
                {
                    var tc = new TableCleaner(Service, false);
                    tc.Worker = worker;
                    tc.AddSelectedComponents(e.Items, solution, e.IsFromFix2);
                },
                ProgressChanged = evt =>
                {
                    progressControl1.AddLog((TableLog)evt.UserState, evt.ProgressPercentage);
                },
                PostWorkCallBack = (args) =>
                {
                    SetWorkingState(false);

                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    solutionPicker1_SolutionSelected(solutionPicker1, new UserControls.SolutionSelectedEventArgs(solutionPicker1.SelectedSolution));
                }
            });
        }

        private void SetWorkingState(bool isWorking)
        {
            toolStrip1.Enabled = !isWorking;
            solutionPicker1.Enabled = !isWorking;
            tablePicker1.Enabled = !isWorking;
            fixControl1.Enabled = !isWorking;
            fixControl2.Enabled = !isWorking;
            fixControl3.Enabled = !isWorking;
            fixControl4.Enabled = !isWorking;
        }

        private void solutionPicker1_SolutionSelected(object sender, UserControls.SolutionSelectedEventArgs e)
        {
            SetWorkingState(true);
            WorkAsync(new WorkAsyncInfo
            {
                Message = "Loading tables from solution...",
                Work = (worker, args) =>
                {
                    tablePicker1.LoadTables(Service, e.Solution.Id);
                },
                PostWorkCallBack = (args) =>
                {
                    SetWorkingState(false);

                    if (args.Error != null)
                    {
                        MessageBox.Show(args.Error.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    tablePicker1.DisplayTables();

                    if (tablePicker1.HasBadPracticeTables)
                    {
                        badPracticeControl1.Visible = true;
                        goodPracticeControl2.Visible = false;
                        fixControl1.Visible = true;
                        fixControl2.Visible = true;
                        fixControl3.Visible = true;
                        fixControl4.Visible = false;
                        pnlMode.Visible = true;
                    }
                    else
                    {
                        badPracticeControl1.Visible = false;
                        goodPracticeControl2.Visible = true;
                        fixControl1.Visible = false;
                        fixControl2.Visible = false;
                        fixControl3.Visible = false;
                        fixControl4.Visible = true;
                        pnlMode.Visible = true;
                    }

                    pnlResult.Visible = true;
                }
            });
        }

        private void tsbSample_Click(object sender, EventArgs e)
        {
            // The ExecuteMethod method handles connecting to an
            // organization if XrmToolBox is not yet connected
            ExecuteMethod(GetSolutions);
        }
    }
}