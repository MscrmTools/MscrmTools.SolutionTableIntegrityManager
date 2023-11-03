using System;
using System.Collections.Generic;

namespace MscrmTools.SolutionTableIntegrityManager.AppCode
{
    public class OnApplyFixEventArgs : EventArgs
    {
        public bool IsFromFix2 { get; set; }
        public List<TableLog> Items { get; set; }
    }
}