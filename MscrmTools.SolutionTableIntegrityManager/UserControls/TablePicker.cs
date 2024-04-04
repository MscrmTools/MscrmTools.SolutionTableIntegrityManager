using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using MscrmTools.SolutionTableIntegrityManager.AppCode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MscrmTools.SolutionTableIntegrityManager.UserControls
{
    public partial class TablePicker : UserControl
    {
        private int orderColumn = -1;
        private List<Table> tables = new List<Table>();

        public TablePicker()
        {
            InitializeComponent();
        }

        public bool HasBadPracticeTables => tables.Any(t => !t.IsBestPractice);
        public bool HasGoodPracticeTables => tables.Any(t => t.IsBestPractice);
        public List<Table> SelectedTables => lvSolutions.CheckedItems.Cast<ListViewItem>().Select(li => (Table)li.Tag).ToList();

        public void DisplayTables(bool onlyFaulty = false)
        {
            lvSolutions.Items.Clear();
            lvSolutions.Items.AddRange(tables.Where(t => onlyFaulty && !t.IsBestPractice || !onlyFaulty).Select(s => new ListViewItem(s.Metadata.DisplayName?.UserLocalizedLabel?.Label)
            {
                SubItems =
                {
                    new ListViewItem.ListViewSubItem{Text = s.Metadata.SchemaName},
                    new ListViewItem.ListViewSubItem{Text = s.Metadata.IsManaged.Value.ToString()},
                    new ListViewItem.ListViewSubItem{Text = GetReason(s.ComponentBehavior, s.Metadata)}
                },
                Tag = s,
                ForeColor = s.IsBestPractice ? Color.Green : Color.Red
            }).ToArray());
        }

        public void LoadTables(IOrganizationService service, Guid solutionId)
        {
            tables.Clear();
            var tableComponents = service.RetrieveMultiple(new QueryExpression("solutioncomponent")
            {
                NoLock = true,
                ColumnSet = new ColumnSet("objectid", "rootcomponentbehavior"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("solutionid", ConditionOperator.Equal, solutionId),
                        new ConditionExpression("componenttype", ConditionOperator.Equal, 1)
                    }
                }
            }).Entities.ToList();

            var query = new EntityQueryExpression
            {
                Properties = new MetadataPropertiesExpression("DisplayName", "SchemaName", "MetadataId", "IsManaged", "IsIntersect"),
                Criteria = new MetadataFilterExpression
                {
                    Conditions =
                    {
                        new MetadataConditionExpression("MetadataId", MetadataConditionOperator.In, tableComponents.Select(tc => tc.GetAttributeValue<Guid>("objectid")).ToArray())
                    }
                }
            };

            var response = (RetrieveMetadataChangesResponse)service.Execute(new RetrieveMetadataChangesRequest { Query = query });

            foreach (var emd in response.EntityMetadata)
            {
                var componentBehavior = tableComponents.First(t => t.GetAttributeValue<Guid>("objectid") == emd.MetadataId).GetAttributeValue<OptionSetValue>("rootcomponentbehavior").Value;

                tables.Add(new Table
                {
                    SolutionComponent = tableComponents.First(t => t.GetAttributeValue<Guid>("objectid") == emd.MetadataId),
                    Metadata = emd,
                    IsBestPractice = componentBehavior == 0 && !emd.IsManaged.Value // All assets and unmanaged
                                || (componentBehavior == 1 || componentBehavior == 2) && emd.IsManaged.Value // Not all asset and managed
                                || componentBehavior == 2 && !emd.IsManaged.Value && emd.IsIntersect.Value // Shell only, unmanaged and intersect table (NN)
                });
            }
        }

        internal void Clear()
        {
            lvSolutions.Items.Clear();
        }

        private string GetReason(int componentBehavior, Microsoft.Xrm.Sdk.Metadata.EntityMetadata metadata)
        {
            if (componentBehavior == 0 && metadata.IsManaged.Value)
            {
                return "Managed table should not be added with all their assets to unmanaged solution";
            }
            else if ((componentBehavior == 1 || componentBehavior == 2) && !metadata.IsManaged.Value && !metadata.IsIntersect.Value)
            {
                return "Unmanaged table should not be added partially, but with all their assets to unmanaged solution";
            }
            else
            {
                return "";
            }
        }

        private void lvSolutions_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (orderColumn != e.Column) lvSolutions.Sorting = SortOrder.Ascending;
            else lvSolutions.Sorting = lvSolutions.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            lvSolutions.ListViewItemSorter = new ListViewItemComparer(e.Column, lvSolutions.Sorting);
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == tsbUncheckAll)
            {
                foreach (ListViewItem item in lvSolutions.Items)
                {
                    item.Checked = false;
                }
            }
            else if (e.ClickedItem == tsbCheckFaulty)
            {
                foreach (ListViewItem item in lvSolutions.Items)
                {
                    item.Checked = !((Table)item.Tag).IsBestPractice;
                }
            }
            else if (e.ClickedItem == tsbShowAll)
            {
                DisplayTables();
            }
            else if (e.ClickedItem == tsbShowOnlyFaulty)
            {
                DisplayTables(true);
            }
            else if (e.ClickedItem == tsbCheckManagedTables)
            {
                foreach (ListViewItem item in lvSolutions.Items)
                {
                    item.Checked = ((Table)item.Tag).Metadata.IsManaged.Value;
                }
            }
        }
    }
}