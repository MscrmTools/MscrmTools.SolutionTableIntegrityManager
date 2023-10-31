using System;
using XrmToolBox.Extensibility.Interfaces;

namespace MscrmTools.SolutionTableIntegrityManager
{
    public partial class PluginControl : IPayPalPlugin, IGitHubPlugin
    {
        public string DonationDescription => "Donation for Solution Table Integrity Manager";

        public string EmailAccount => "tanguy92@hotmail.com";

        public string RepositoryName => "MscrmTools.SolutionTableIntegrityManager";

        public string UserName => "MscrmTools";
    }
}