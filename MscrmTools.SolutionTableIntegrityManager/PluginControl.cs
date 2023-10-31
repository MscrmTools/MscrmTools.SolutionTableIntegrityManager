using McTools.Xrm.Connection;
using Microsoft.Xrm.Sdk;
using MscrmTools.SolutionTableIntegrityManager.AppCode;
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
            progressControl1.CloseRequested += (sender, e) => { scMain.Panel2Collapsed = true; };
        }

        /// <summary>
        /// This event occurs when the connection has been updated in XrmToolBox
        /// </summary>
        public override void UpdateConnection(IOrganizationService newService, ConnectionDetail detail, string actionName, object parameter)
        {
            base.UpdateConnection(newService, detail, actionName, parameter);
        }

        private void fixControl_Apply(object sender, EventArgs e)
        {
            var solution = solutionPicker1.SelectedSolution.GetAttributeValue<string>("uniquename");
            var tables = tablePicker1.SelectedTables.Where(t => !t.IsBestPractice).Select(t => t.Metadata).ToList();
            var isSimulation = rdbSimulate.Checked;

            if (tables.Count == 0)
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

            scMain.Panel2Collapsed = false;
            SetWorkingState(true);

            WorkAsync(new WorkAsyncInfo
            {
                Message = "Processing...",
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

        private void MyPluginControl_Load(object sender, EventArgs e)
        {
        }

        private void SetWorkingState(bool isWorking)
        {
            toolStrip1.Enabled = !isWorking;
            solutionPicker1.Enabled = !isWorking;
            tablePicker1.Enabled = !isWorking;
            fixControl1.Enabled = !isWorking;
            fixControl2.Enabled = !isWorking;
            fixControl3.Enabled = !isWorking;
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
                        pnlMode.Visible = true;
                    }
                    else
                    {
                        badPracticeControl1.Visible = false;
                        goodPracticeControl2.Visible = true;
                        fixControl1.Visible = false;
                        fixControl2.Visible = false;
                        fixControl3.Visible = false;
                        pnlMode.Visible = false;
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