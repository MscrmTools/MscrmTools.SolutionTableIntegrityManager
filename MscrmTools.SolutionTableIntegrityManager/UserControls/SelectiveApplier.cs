using MscrmTools.SolutionTableIntegrityManager.AppCode;
using Newtonsoft.Json;
using ScintillaNET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MscrmTools.SolutionTableIntegrityManager.UserControls
{
    public partial class SelectiveApplier : UserControl
    {
        private int checkedCount = 0;
        private bool isFromFix2;
        private bool isFromFix6;
        private List<TableLog> logs;
        private List<ListViewItem> lvItems;
        private int orderColumn = -1;

        public SelectiveApplier(List<TableLog> logs, bool isFromFix2, bool isFromFix6)
        {
            InitializeComponent();

            this.logs = logs;
            this.isFromFix2 = isFromFix2;
            this.isFromFix6 = isFromFix6;
            splitContainer1.SplitterDistance = 450;
            SetScintillatControl(scintilla1);

            if (isFromFix6)
            {
                lblTitle.Text = "Check items you want to remove from your solution";
                lblDesc.Text = "Check items you want to remove from in your solution and click on Apply button";
            }
        }

        public event EventHandler<OnApplyFixEventArgs> OnApply;

        public event EventHandler OnClose;

        public bool IsMaximized { get; set; }

        private static string JsonPrettify(string json)
        {
            if (string.IsNullOrEmpty(json)) return string.Empty;

            using (var stringReader = new StringReader(json))
            using (var stringWriter = new StringWriter())
            {
                var jsonReader = new JsonTextReader(stringReader);
                var jsonWriter = new JsonTextWriter(stringWriter) { Formatting = Formatting.Indented };
                jsonWriter.WriteToken(jsonReader);
                return stringWriter.ToString();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            OnApply?.Invoke(this, new OnApplyFixEventArgs { Items = lvItems.Where(i => i.Checked).Select(li => (TableLog)li.Tag).ToList(), IsFromFix2 = isFromFix2, IsFromFix6 = isFromFix6 });
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke(this, new EventArgs());
        }

        private void btnMaximize_Click(object sender, EventArgs e)
        {
            if (btnMaximize.Text == "Maximize")
            {
                btnMaximize.Text = "Reduce";
                Width = Parent.Width;
                Height = Parent.Height;
                Location = new Point(0, 0);
                IsMaximized = true;
            }
            else
            {
                btnMaximize.Text = "Maximize";
                Width = Convert.ToInt32(Parent.Width * 0.7);
                Height = Convert.ToInt32(Parent.Height * 0.7);
                Location = new Point(Parent.Width / 2 - Width / 2, Parent.Height / 2 - Height / 2);
                IsMaximized = false;
            }
        }

        private void lvLogs_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (orderColumn != e.Column) lvLogs.Sorting = SortOrder.Ascending;
            else lvLogs.Sorting = lvLogs.Sorting == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
            lvLogs.ListViewItemSorter = new ListViewItemComparer(e.Column, lvLogs.Sorting);
        }

        private void lvLogs_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (e.Item.Checked) checkedCount++;
            else checkedCount--;

            tslCount.Text = string.Format(tslCount.Tag.ToString(), checkedCount);
        }

        private void lvLogs_SelectedIndexChanged(object sender, EventArgs e)
        {
            var item = lvLogs.SelectedItems.Cast<ListViewItem>().FirstOrDefault();
            if (item == null)
            {
                splitContainer1.Panel2Collapsed = true;
                return;
            }

            if (!string.IsNullOrEmpty(((TableLog)item.Tag).ChangedProperties))
            {
                scintilla1.Text = JsonPrettify(((TableLog)item.Tag).ChangedProperties);
                splitContainer1.Panel2Collapsed = false;
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
            }
        }

        private void SelectiveApplier_Load(object sender, EventArgs e)
        {
            lvItems = logs.Select(l => new ListViewItem
            {
                Text = l.Table,
                SubItems = {
                    new ListViewItem.ListViewSubItem{Text = l.Type},
                    new ListViewItem.ListViewSubItem{Text = l.ComponentName},
                    new ListViewItem.ListViewSubItem{Text = l.GetChangedPropertiesNames()},
                    },

                Tag = l
            }).ToList();

            lvLogs.Items.AddRange(lvItems.ToArray());

            Width = Convert.ToInt32(Parent.Width * 0.7);
            Height = Convert.ToInt32(Parent.Height * 0.7);
            Location = new Point(Parent.Width / 2 - Width / 2, Parent.Height / 2 - Height / 2);

            lvLogs.ItemChecked += new ItemCheckedEventHandler(lvLogs_ItemChecked);
        }

        private void SetScintillatControl(Scintilla ctrl)
        {
            ctrl.StyleResetDefault();
            ctrl.Styles[Style.Default].Font = "Consolas";
            ctrl.Styles[Style.Default].Size = 10;
            ctrl.StyleClearAll();
            ctrl.Styles[Style.Json.Default].ForeColor = Color.Silver;
            ctrl.Styles[Style.Json.BlockComment].ForeColor = Color.Green;
            ctrl.Styles[Style.Json.PropertyName].ForeColor = Color.Red;
            ctrl.Styles[Style.Json.String].ForeColor = Color.Blue;
            ctrl.Styles[Style.Json.Number].ForeColor = Color.Blue;
            ctrl.Styles[Style.Json.Keyword].ForeColor = Color.Blue;
            ctrl.Styles[Style.Json.StringEol].BackColor = Color.Pink;
            ctrl.Styles[Style.Json.Operator].ForeColor = Color.Black;

            // Instruct the lexer to calculate folding
            ctrl.SetProperty("fold", "1");
            ctrl.SetProperty("fold.compact", "1");
            ctrl.SetProperty("fold.html", "1");

            // Configure a margin to display folding symbols
            ctrl.Margins[2].Type = MarginType.Symbol;
            ctrl.Margins[2].Mask = Marker.MaskFolders;
            ctrl.Margins[2].Sensitive = true;
            ctrl.Margins[2].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                ctrl.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                ctrl.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Configure folding markers with respective symbols
            ctrl.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            ctrl.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            ctrl.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            ctrl.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            ctrl.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            ctrl.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            ctrl.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (ListViewItem item in lvLogs.Items)
            {
                item.Checked = e.ClickedItem == tsbCheckAll;
            }
        }

        private void tstbSearch_TextChanged(object sender, EventArgs e)
        {
            lvLogs.Items.Clear();
            lvLogs.Items.AddRange(lvItems.Where(i => i.SubItems.Cast<ListViewItem.ListViewSubItem>().Any(s => s.Text.ToLower().IndexOf(tstbSearch.Text.ToLower()) >= 0)).ToArray());
        }
    }
}