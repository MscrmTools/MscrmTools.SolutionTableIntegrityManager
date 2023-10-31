using System;
using System.Windows.Forms;

namespace MscrmTools.SolutionTableIntegrityManager.UserControls
{
    public partial class BadPracticeControl : UserControl
    {
        public BadPracticeControl()
        {
            InitializeComponent();
        }

        private void BadPracticeControl_Resize(object sender, EventArgs e)
        {
            while (pictureBox1.Height + lblTitle.Height + label1.Height > Height)
            {
                Height += 10;
            }
        }
    }
}