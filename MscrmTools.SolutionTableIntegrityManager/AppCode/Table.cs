using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using System;

namespace MscrmTools.SolutionTableIntegrityManager.AppCode
{
    public class Table
    {
        public bool IsBestPractice { get; set; }
        public EntityMetadata Metadata { get; set; }
        public Entity SolutionComponent { get; set; }
        public int ComponentBehavior => SolutionComponent.GetAttributeValue<OptionSetValue>("rootcomponentbehavior").Value;
        public override string ToString()
        {
            return Metadata.DisplayName?.UserLocalizedLabel?.Label;
        }
    }
}