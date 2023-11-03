using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using MscrmTools.SolutionTableIntegrityManager.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MscrmTools.SolutionTableIntegrityManager.UserControls
{
    public partial class SolutionPicker : UserControl
    {
        private int orderColumn = -1;
        private List<Entity> solutions = new List<Entity>();

        public SolutionPicker()
        {
            InitializeComponent();
        }

        public event EventHandler<SolutionSelectedEventArgs> SolutionSelected;

        public Entity SelectedSolution => lvSolutions.SelectedItems.Cast<ListViewItem>().FirstOrDefault()?.Tag as Entity;

        public void DisplaySolutions()
        {
            lvSolutions.Items.Clear();
            lvSolutions.Items.AddRange(solutions.Select(s => new ListViewItem(s.GetAttributeValue<string>("friendlyname"))
            {
                SubItems =
                {
                    new ListViewItem.ListViewSubItem{Text = s.GetAttributeValue<string>("uniquename")},
                    new ListViewItem.ListViewSubItem{Text = s.GetAttributeValue<string>("version")}
                },
                Tag = s
            }).ToArray());
        }

        public void LoadSolutions(IOrganizationService service)
        {
            solutions = service.RetrieveMultiple(new QueryExpression("solution")
            {
                NoLock = true,
                ColumnSet = new ColumnSet("friendlyname", "uniquename", "version"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("uniquename", ConditionOperator.NotEqual, "Basic"),
                        new ConditionExpression("uniquename", ConditionOperator.NotEqual, "Active"),
                        new ConditionExpression("uniquename", ConditionOperator.NotEqual, "Default"),
                        new ConditionExpression("ismanaged", ConditionOperator.Equal, false),
                    }
                }
            }).Entities.ToList();
        }

        private void lvSolutions_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (orderColumn != e.Column) lvSolutions.Sorting = SortOrder.Ascending;
            else lvSolutions.Sorting = lvSolutions.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            lvSolutions.ListViewItemSorter = new ListViewItemComparer(e.Column, lvSolutions.Sorting);
        }

        private void lvSolutions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvSolutions.SelectedItems.Count == 0) return;

            SolutionSelected?.Invoke(this, new SolutionSelectedEventArgs((Entity)lvSolutions.SelectedItems[0].Tag));
        }
    }

    public class SolutionSelectedEventArgs : EventArgs
    {
        public SolutionSelectedEventArgs(Entity solution)
        {
            Solution = solution;
        }

        public Entity Solution { get; set; }
    }
}