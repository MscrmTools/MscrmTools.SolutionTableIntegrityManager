using Microsoft.Xrm.Sdk.Metadata;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace MscrmTools.SolutionTableIntegrityManager.AppCode
{
    public class TableLog
    {
        public string ChangedProperties { get; set; }
        public Guid ComponentId { get; set; }
        public string ComponentName { get; set; }
        public EntityMetadata EntityMetadata { get; set; }
        public string Message { get; set; }
        public string Table { get; set; }
        public string Type { get; set; }
        public int TypeCode { get; set; }

        public string GetChangedPropertiesNames()
        {
            if (string.IsNullOrEmpty(ChangedProperties)) return "";
            var jsonObject = JObject.Parse(ChangedProperties);
            return string.Join(", ", ((JArray)jsonObject["Attributes"]).SelectMany(a => ((JObject)a).Properties().Where(p => p.Name == "Key").Select(p => p.Value)));
        }

        public int GetImageIndex()
        {
            switch (TypeCode)
            {
                case 1: return 1;
                case 2: return 2;
                case 3:
                case 10:
                    return 3;

                case 14: return 4;
                case 60: return 5;
                case 26: return 7;
                case 59: return 8;
                case 29: return 9;
                default: return 10;
            }
        }
    }
}