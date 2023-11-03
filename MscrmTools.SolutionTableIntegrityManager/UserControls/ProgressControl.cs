using MscrmTools.SolutionTableIntegrityManager.AppCode;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MscrmTools.SolutionTableIntegrityManager.UserControls
{
    public partial class ProgressControl : UserControl
    {
        public ProgressControl()
        {
            InitializeComponent();
        }

        public event EventHandler CloseRequested;

        public event EventHandler<OnApplyFixEventArgs> OnApply;

        public FixControl FixSender { get; internal set; }

        public void AddLog(TableLog log, int imageIndex)
        {
            var item = new ListViewItem
            {
                Text = log.Table,
                SubItems = {
                    new ListViewItem.ListViewSubItem{Text = log.Type},
                    new ListViewItem.ListViewSubItem{Text = log.ComponentName},
                    new ListViewItem.ListViewSubItem{Text = log.Message},
                    new ListViewItem.ListViewSubItem{Text = log.GetChangedPropertiesNames()},
                    },
                ImageIndex = imageIndex,
                Tag = log
            };

            lvLogs.Items.Add(item);
            item.EnsureVisible();
        }

        public void SetSelectiveApplierButtonVisibility(bool isVisible)
        {
            btnOpenSelectiveApplier.Visible = isVisible;
        }

        internal void Clear()
        {
            lvLogs.Items.Clear();
        }

        private void btnClearLogs_Click(object sender, EventArgs e)
        {
            lvLogs.Items.Clear();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            CloseRequested?.Invoke(this, new EventArgs());
        }

        private void btnExportLogs_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog
            {
                Title = "Choose a location for the log file",
                Filter = "CSV file (*.csv)|*.csv"
            })
            {
                if (DialogResult.OK == sfd.ShowDialog(this))
                {
                    using (var writer = new StreamWriter(sfd.FileName, false, Encoding.Default))
                    {
                        writer.WriteLine("Table,Type,Component,Message");
                        foreach (ListViewItem item in lvLogs.Items)
                        {
                            writer.WriteLine($"{item.Text},{item.SubItems[1].Text},{item.SubItems[2].Text},{item.SubItems[3].Text}");
                        }
                    }

                    var open = MessageBox.Show(this, $"File saved to {sfd.FileName}\n\nDo you want to open it now?", "Export completed", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (open == DialogResult.Yes)
                    {
                        Process.Start(sfd.FileName);
                    }
                }
            }
        }

        private void btnOpenSelectiveApplier_Click(object sender, EventArgs e)
        {
            var ctrl = new SelectiveApplier(lvLogs.Items.Cast<ListViewItem>().Where(i => ((TableLog)i.Tag).Type != "Information" && ((TableLog)i.Tag).ChangedProperties?.Length > 0).Select(i => (TableLog)i.Tag).ToList(), FixSender.Name == "fixControl2");
            ctrl.Name = "selectiveApplier1";
            ctrl.OnClose += (s, evt) => { Controls.Remove(ctrl); ctrl.Dispose(); };
            ctrl.OnApply += (s, evt) =>
            {
                Controls.Remove(ctrl); ctrl.Dispose();
                OnApply?.Invoke(this, evt);
            };

            Control parent = Parent;
            while (!(parent is PluginControl)) parent = parent.Parent;

            parent.Controls.Add(ctrl);
            ctrl.BringToFront();
        }
    }
}