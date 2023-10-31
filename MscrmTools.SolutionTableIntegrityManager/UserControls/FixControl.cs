using System;
using System.Windows.Forms;

namespace MscrmTools.SolutionTableIntegrityManager.UserControls
{
    public partial class FixControl : UserControl
    {
        public FixControl()
        {
            InitializeComponent();
        }

        public event EventHandler Apply;

        public string Description
        {
            get { return lblDescription.Text; }
            set { lblDescription.Text = value; }
        }

        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            Apply?.Invoke(this, new EventArgs());
        }
    }
}