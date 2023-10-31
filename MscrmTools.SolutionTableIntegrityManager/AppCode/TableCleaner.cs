using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MscrmTools.SolutionTableIntegrityManager.AppCode
{
    public class TableCleanerProgressEventArgs : EventArgs
    {
        public int ImageIndex { get; set; }
        public TableLog Log { get; set; }
    }

    internal class TableCleaner
    {
        private readonly bool isSimulation;
        private readonly ActiveLayerSearch searchService;
        private readonly IOrganizationService service;
        private int commandTypeCode;

        public TableCleaner(IOrganizationService service, bool isSimulation)
        {
            this.service = service;
            this.isSimulation = isSimulation;
            searchService = new ActiveLayerSearch(service);
        }

        public event EventHandler<TableCleanerProgressEventArgs> OnLog;

        public BackgroundWorker Worker { get; set; }

        public void Clean(string solutionUniqueName, List<EntityMetadata> metadata, bool addUnmanaged, bool addChangedManaged)
        {
            LoadCommandTypeCode();

            foreach (var emd in metadata)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Table", emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", $"Removing table {emd.SchemaName} from solution {solutionUniqueName}", 1);

                if (!isSimulation)
                {
                    service.Execute(new RemoveSolutionComponentRequest
                    {
                        SolutionUniqueName = solutionUniqueName,
                        ComponentType = 1,
                        ComponentId = emd.MetadataId.Value
                    });
                }

                if (emd.IsManaged.Value)
                {
                    if (!emd.IsIntersect.Value)
                    {
                        Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", $"Searching for changes for table {emd.DisplayName?.UserLocalizedLabel?.Label ?? emd.SchemaName}", 0);
                        var result = searchService.GetActiveLayers(new List<Guid> { emd.MetadataId.Value }, Worker, "Entity");
                        if (result.Count > 0)
                        {
                            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Table", emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", $"Adding managed table {emd.SchemaName} to solution {solutionUniqueName} without any assets but with metadata", 1);
                            AddComponent(solutionUniqueName, 1, emd.MetadataId.Value, false, true, true);
                        }
                        else
                        {
                            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Table", emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", $"Adding managed table {emd.SchemaName} to solution {solutionUniqueName} without any assets or metadata", 1);
                            AddComponent(solutionUniqueName, 1, emd.MetadataId.Value, false, true, false);
                        }
                    }
                }
                else
                {
                    Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Table", emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", $"Adding unmanaged table {emd.SchemaName} to solution {solutionUniqueName} without all assets", 1);
                    AddComponent(solutionUniqueName, 1, emd.MetadataId.Value, false, false, true);
                }

                if (addUnmanaged || addChangedManaged)
                {
                    var emdFull = GetTableMetadata(emd.MetadataId.Value);

                    if (addUnmanaged)
                    {
                        AddUnmanagedComponents(emdFull, solutionUniqueName);
                    }

                    if (addChangedManaged)
                    {
                        AddChangedManagedComponents(emdFull, solutionUniqueName);
                    }
                }

                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Process completed!", 0);
            }
        }

        private void AddChangedManagedComponents(EntityMetadata emd, string solutionUniqueName)
        {
            var attributes = emd.Attributes.Where(a => a.IsManaged.Value == true);
            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed attributes", 0);
            var ids = searchService.GetActiveLayers(attributes.Select(a => a.MetadataId.Value).ToList(), Worker, "Attribute");

            foreach (var id in ids)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Attribute", attributes.First(a => a.MetadataId.Value == id).DisplayName?.UserLocalizedLabel?.Label ?? "N/A", $"Adding attribute {attributes.First(a => a.MetadataId.Value == id).SchemaName} to solution {solutionUniqueName}", 2);
                AddComponent(solutionUniqueName, 2, id, false);
            }

            // One-To-Many are handled by the other table
            //var rels1 = emd.OneToManyRelationships.Where(a => a.IsManaged.Value == true);
            //Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed One-To-Many relationships", 0);
            //var rel1Ids = searchService.GetActiveLayers(rels1.Select(a => a.MetadataId.Value).ToList(), Worker, "Relationship");

            //foreach (var id in rel1Ids)
            //{
            //    Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "One-To-Many relationship", rels1.First(a => a.MetadataId.Value == id).SchemaName, $"Adding One-To-Many relationship {rels1.First(r => r.MetadataId.Value == id).SchemaName} to solution {solutionUniqueName}", 3);
            //    AddComponent(solutionUniqueName, 3, id, false);
            //}

            var rels2 = emd.ManyToManyRelationships.Where(a => a.IsManaged.Value == true);
            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed Many-To-One relationships", 0);
            var rel2Ids = searchService.GetActiveLayers(rels2.Select(a => a.MetadataId.Value).ToList(), Worker, "Relationship");

            foreach (var id in rel2Ids)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Many-To-One relationship", rels2.First(a => a.MetadataId.Value == id).SchemaName, $"Adding Many-To-One relationship {rels2.First(r => r.MetadataId.Value == id).SchemaName} to solution {solutionUniqueName}", 3);

                AddComponent(solutionUniqueName, 3, id, false);
            }

            var rels3 = emd.ManyToManyRelationships.Where(a => a.IsManaged.Value == true);
            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed Many-To-Many relationships", 0);
            var rel3Ids = searchService.GetActiveLayers(rels2.Select(a => a.MetadataId.Value).ToList(), Worker, "Relationship");

            foreach (var id in rel3Ids)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Many-To-Many relationship", rels3.First(a => a.MetadataId.Value == id).SchemaName, $"Adding Many-To-Many relationship {rels3.First(r => r.MetadataId.Value == id).SchemaName} to solution {solutionUniqueName}", 3);

                AddComponent(solutionUniqueName, 3, id, false);
            }

            var keys = emd.Keys.Where(a => a.IsManaged.Value == true);
            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed keys", 0);
            var keysIds = searchService.GetActiveLayers(keys.Select(a => a.MetadataId.Value).ToList(), Worker, "EntityKey");

            foreach (var id in keysIds)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Key", keys.First(a => a.MetadataId.Value == id).DisplayName?.UserLocalizedLabel?.Label ?? "N/A", $"Adding Key {keys.First(r => r.MetadataId.Value == id).SchemaName} to solution {solutionUniqueName}", 4);
                AddComponent(solutionUniqueName, 14, id, false);
            }

            var forms = GetForms(emd.LogicalName, true, false);
            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed forms", 0);
            var formsIds = searchService.GetActiveLayers(forms.Select(a => a.Id).ToList(), Worker, "SystemForm");

            foreach (var id in formsIds)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Form", forms.First(f => f.Id == id).GetAttributeValue<string>("name"), $"Adding form {forms.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 5);
                AddComponent(solutionUniqueName, 60, id, false);
            }

            forms = GetForms(emd.LogicalName, true, true);
            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed dashboards", 0);
            var dashboardsIds = searchService.GetActiveLayers(forms.Select(a => a.Id).ToList(), Worker, "SystemForm");

            foreach (var id in dashboardsIds)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Dashboard", forms.First(f => f.Id == id).GetAttributeValue<string>("name"), $"Adding dashboard {forms.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 6);
                AddComponent(solutionUniqueName, 60, id, false);
            }

            var views = GetViews(emd.LogicalName, true);
            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed views", 0);
            var viewsIds = searchService.GetActiveLayers(views.Select(a => a.Id).ToList(), Worker, "SavedQuery");

            foreach (var id in viewsIds)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "View", views.First(f => f.Id == id).GetAttributeValue<string>("name"), $"Adding view {views.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 7);
                AddComponent(solutionUniqueName, 26, id, false);
            }

            var charts = GetCharts(emd.LogicalName, true);
            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed charts", 0);
            var chartsIds = searchService.GetActiveLayers(charts.Select(a => a.Id).ToList(), Worker, "SavedQueryVisualization");

            foreach (var id in chartsIds)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Chart", charts.First(f => f.Id == id).GetAttributeValue<string>("name"), $"Adding chart {charts.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 8);
                AddComponent(solutionUniqueName, 59, id, false);
            }

            var businessRules = GetBusinessRules(emd.LogicalName, true);
            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed business rules", 0);
            var businessRulesIds = searchService.GetActiveLayers(businessRules.Select(a => a.Id).ToList(), Worker, "Workflow");

            foreach (var id in businessRulesIds)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Business Rule", businessRules.First(f => f.Id == id).GetAttributeValue<string>("name"), $"Adding business rule {businessRules.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 9);
                AddComponent(solutionUniqueName, 29, id, false);
            }

            var commands = GetCommands(emd.MetadataId.Value, true);
            Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Information", "", "Searching for changes for managed commands", 0);
            var commandsIds = searchService.GetActiveLayers(commands.Select(a => a.Id).ToList(), Worker, "appaction");

            foreach (var id in commandsIds)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Command", commands.First(f => f.Id == id).GetAttributeValue<string>("name"), $"Adding command {commands.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 10);
                AddComponent(solutionUniqueName, commandTypeCode, id, false);
            }
        }

        private void AddComponent(string solutionUniqueName, int type, Guid id, bool? addRequiredComponents = null, bool? doNotIncludeSubcomponents = null, bool? includeMetadata = null)
        {
            var request = new AddSolutionComponentRequest
            {
                SolutionUniqueName = solutionUniqueName,
                ComponentType = type,
                ComponentId = id
            };

            if (addRequiredComponents.HasValue)
            {
                request.AddRequiredComponents = addRequiredComponents.Value;
            }

            if (doNotIncludeSubcomponents.HasValue)
            {
                request.DoNotIncludeSubcomponents = doNotIncludeSubcomponents.Value;
            }

            if (includeMetadata.HasValue)
            {
                request.IncludedComponentSettingsValues = includeMetadata.Value ? null : new string[] { };
            }

            if (!isSimulation) service.Execute(request);
        }

        private void AddUnmanagedComponents(EntityMetadata emd, string solutionUniqueName)
        {
            foreach (var amd in emd.Attributes.Where(a => a.IsManaged.Value == false))
            {
                if (amd.AttributeOf != null && emd.Attributes.First(a => a.LogicalName == amd.AttributeOf).IsManaged.Value) continue;

                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Attribute", amd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", $"Adding attribute {amd.SchemaName} to solution {solutionUniqueName}", 2);
                AddComponent(solutionUniqueName, 2, amd.MetadataId.Value, false);
            }

            // One-To-Many are handled by the other table
            //foreach (var rmd1 in emd.OneToManyRelationships.Where(a => a.IsManaged.Value == false))
            //{
            //    Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "One-To-Many relationship", rmd1.SchemaName, $"Adding One-To-Many relationship {rmd1.SchemaName} to solution {solutionUniqueName}", 3);
            //    AddComponent(solutionUniqueName, 3, rmd1.MetadataId.Value, false);
            //}

            foreach (var rmd2 in emd.ManyToManyRelationships.Where(a => a.IsManaged.Value == false))
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Many-To-One relationship", rmd2.SchemaName, $"Adding Many-To-One relationship {rmd2.SchemaName} to solution {solutionUniqueName}", 3);
                AddComponent(solutionUniqueName, 3, rmd2.MetadataId.Value, false);
            }

            foreach (var mmrmd in emd.ManyToManyRelationships.Where(a => a.IsManaged.Value == false))
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Many-To-Many relationship", mmrmd.SchemaName, $"Adding Many-To-Many relationship {mmrmd.SchemaName} to solution {solutionUniqueName}", 3);
                AddComponent(solutionUniqueName, 3, mmrmd.MetadataId.Value, false);
            }

            foreach (var kmd in emd.Keys.Where(a => a.IsManaged.Value == false))
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Key", kmd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", $"Adding Key {kmd.SchemaName} to solution {solutionUniqueName}", 4);
                AddComponent(solutionUniqueName, 14, kmd.MetadataId.Value, false);
            }

            var forms = GetForms(emd.LogicalName, false, false);
            foreach (var form in forms)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Form", form.GetAttributeValue<string>("name"), $"Adding form {form.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 5);
                AddComponent(solutionUniqueName, 60, form.Id, false);
            }

            forms = GetForms(emd.LogicalName, false, true);
            foreach (var form in forms)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Dashboard", form.GetAttributeValue<string>("name"), $"Adding dashboard {form.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 6);
                AddComponent(solutionUniqueName, 60, form.Id, false);
            }

            var views = GetViews(emd.LogicalName, false);
            foreach (var view in views)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "View", view.GetAttributeValue<string>("name"), $"Adding view {view.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 7);
                AddComponent(solutionUniqueName, 26, view.Id, false);
            }

            var charts = GetCharts(emd.LogicalName, false);
            foreach (var chart in charts)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Chart", chart.GetAttributeValue<string>("name"), $"Adding chart {chart.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 8);
                AddComponent(solutionUniqueName, 59, chart.Id, false);
            }

            var businessRules = GetBusinessRules(emd.LogicalName, false);
            foreach (var businessRule in businessRules)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Business Rule", businessRule.GetAttributeValue<string>("name"), $"Adding business rule {businessRule.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 9);
                AddComponent(solutionUniqueName, 29, businessRule.Id, false);
            }

            var commands = GetCommands(emd.MetadataId.Value, false);
            foreach (var command in commands)
            {
                Log(emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", "Command", command.GetAttributeValue<string>("name"), $"Adding command {command.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 10);
                AddComponent(solutionUniqueName, commandTypeCode, command.Id, false);
            }
        }

        private List<Entity> GetBusinessRules(string logicalName, bool retrieveManaged)
        {
            return service.RetrieveMultiple(new QueryExpression("workflow")
            {
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("category", ConditionOperator.Equal, 2),
                        new ConditionExpression("primaryentityname", ConditionOperator.Equal, logicalName),
                        new ConditionExpression("ismanaged", ConditionOperator.Equal, retrieveManaged),
                    }
                }
            }).Entities.ToList();
        }

        private List<Entity> GetCharts(string logicalName, bool retrieveManaged)
        {
            return service.RetrieveMultiple(new QueryExpression("savedqueryvisualization")
            {
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("primaryentitytypecode", ConditionOperator.Equal, logicalName),
                        new ConditionExpression("ismanaged", ConditionOperator.Equal, retrieveManaged),
                    }
                }
            }).Entities.ToList();
        }

        private List<Entity> GetCommands(Guid tableId, bool retrieveManaged)
        {
            return service.RetrieveMultiple(new QueryExpression("appaction")
            {
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("contextentity", ConditionOperator.Equal, tableId),
                        new ConditionExpression("ismanaged", ConditionOperator.Equal, retrieveManaged),
                    }
                }
            }).Entities.ToList();
        }

        private List<Entity> GetForms(string logicalName, bool retrieveManaged, bool isDashboard)
        {
            return service.RetrieveMultiple(new QueryExpression("systemform")
            {
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("objecttypecode", ConditionOperator.Equal, logicalName),
                        new ConditionExpression("ismanaged", ConditionOperator.Equal, retrieveManaged),
                        new ConditionExpression("type", isDashboard ? ConditionOperator.Equal : ConditionOperator.NotEqual, 0)
                    }
                }
            }).Entities.ToList();
        }

        private EntityMetadata GetTableMetadata(Guid tableMetadataId)
        {
            var query = new EntityQueryExpression
            {
                Properties = new MetadataPropertiesExpression("DisplayName", "Attributes", "Keys", "OneToManyRelationships", "ManyToOneRelationships", "ManyToManyRelationships"),
                AttributeQuery = new AttributeQueryExpression
                {
                    Properties = new MetadataPropertiesExpression("DisplayName", "SchemaName", "MetadataId", "IsManaged", "AttributeOf"),
                },
                RelationshipQuery = new RelationshipQueryExpression
                {
                    Properties = new MetadataPropertiesExpression("MetadataId", "SchemaName", "IsManaged"),
                },
                KeyQuery = new EntityKeyQueryExpression
                {
                    Properties = new MetadataPropertiesExpression("MetadataId", "SchemaName", "IsManaged", "DisplayName"),
                },
                Criteria = new MetadataFilterExpression
                {
                    Conditions =
                    {
                        new MetadataConditionExpression("MetadataId", MetadataConditionOperator.Equals, tableMetadataId)
                    }
                }
            };

            var response = (RetrieveMetadataChangesResponse)service.Execute(new RetrieveMetadataChangesRequest { Query = query });

            return response.EntityMetadata.First();
        }

        private List<Entity> GetViews(string logicalName, bool retrieveManaged)
        {
            return service.RetrieveMultiple(new QueryExpression("savedquery")
            {
                ColumnSet = new ColumnSet("name"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("returnedtypecode", ConditionOperator.Equal, logicalName),
                        new ConditionExpression("ismanaged", ConditionOperator.Equal, retrieveManaged),
                    }
                }
            }).Entities.ToList();
        }

        private void LoadCommandTypeCode()
        {
            var def = service.RetrieveMultiple(new QueryExpression("solutioncomponentdefinition")
            {
                ColumnSet = new ColumnSet("name", "solutioncomponenttype"),
                Criteria = new FilterExpression
                {
                    Conditions =
                    {
                        new ConditionExpression("name", ConditionOperator.Equal, "appaction"),
                    }
                }
            }).Entities.FirstOrDefault();

            if (def == null)
            {
                throw new Exception("Unable to retrieve Solution Component definition for type appaction");
            }

            commandTypeCode = def.GetAttributeValue<int>("solutioncomponenttype");
        }

        private void Log(string table, string type, string name, string message, int imageIndex)
        {
            var log = new TableLog
            {
                Table = table,
                Type = type,
                Message = message,
                ComponentName = name
            };

            if (Worker?.WorkerReportsProgress ?? false)
            {
                Worker.ReportProgress(imageIndex, log);
            }

            OnLog?.Invoke(this, new TableCleanerProgressEventArgs { Log = log, ImageIndex = imageIndex });
        }
    }
}