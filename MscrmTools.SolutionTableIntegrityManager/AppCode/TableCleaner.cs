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

        public void Clean(string solutionUniqueName, List<EntityMetadata> metadata, bool addUnmanaged, bool addChangedManaged, bool skipTable = false, bool addOnlyManagedNotPresent = false)
        {
            LoadCommandTypeCode();

            foreach (var emd in metadata)
            {
                if (!skipTable)
                {
                    RemoveTableFromSolution(emd, solutionUniqueName);

                    AddTableToSolution(emd, solutionUniqueName);
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
                        AddChangedManagedComponents(emdFull, solutionUniqueName, addOnlyManagedNotPresent);
                    }
                }
            }

            Log(null, "Information", Guid.Empty, "", -1, "Process completed!", 0);
        }

        internal void AddSelectedComponents(List<TableLog> items, string solutionUniqueName, bool isFromFix2 = false)
        {
            List<string> processedTablesForFix2 = new List<string>();

            foreach (var component in items)
            {
                if (isFromFix2 && !processedTablesForFix2.Contains(component.Table))
                {
                    RemoveTableFromSolution(component.EntityMetadata, solutionUniqueName);

                    AddTableToSolution(component.EntityMetadata, solutionUniqueName);

                    processedTablesForFix2.Add(component.Table);
                }

                Log(component.EntityMetadata, component.Type, component.ComponentId, component.ComponentName, component.TypeCode, $"{(isSimulation ? "Should add" : "Adding")} {component.Type} {component.ComponentName} to solution {solutionUniqueName}", component.GetImageIndex());
                AddComponent(solutionUniqueName, component.TypeCode, component.ComponentId, false);
            }

            Log(null, "Information", Guid.Empty, "", -1, "Process completed!", 0);
        }

        private void AddChangedManagedComponents(EntityMetadata emd, string solutionUniqueName, bool addOnlyNotPresent = false)
        {
            List<Entity> components = new List<Entity>();
            if (addOnlyNotPresent)
            {
                components = service.RetrieveMultiple(
                    new QueryExpression("solutioncomponent")
                    {
                        ColumnSet = new ColumnSet("objectid", "componenttype"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("componenttype", ConditionOperator.In, new int[]{2,3,10,14,60,26,59,29,commandTypeCode })
                            }
                        },
                        LinkEntities =
                        {
                            new LinkEntity
                            {
                                LinkFromEntityName = "solutioncomponent",
                                LinkFromAttributeName = "solutionid",
                                LinkToAttributeName = "solutionid",
                                LinkToEntityName = "solution",
                                LinkCriteria = new FilterExpression
                                {
                                    Conditions =
                                    {
                                        new ConditionExpression("uniquename", ConditionOperator.Equal,solutionUniqueName)
                                    }
                                }
                            }
                        }
                    }
                    ).Entities.ToList();
            }

            var attributes = emd.Attributes.Where(a => a.IsManaged.Value);
            Log(emd, "Information", Guid.Empty, "", -1, "Searching for changes for managed attributes", 0);
            var ids = searchService.GetActiveLayers(attributes.Select(a => a.MetadataId.Value).ToList(), Worker, "Attribute");

            foreach (var id in ids.Keys)
            {
                if (components.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == id) != null) continue;

                Log(emd, "Attribute", id, attributes.First(a => a.MetadataId.Value == id).DisplayName?.UserLocalizedLabel?.Label ?? "N/A", 2, $"{(isSimulation ? "Should add" : "Adding")} attribute {attributes.First(a => a.MetadataId.Value == id).SchemaName} to solution {solutionUniqueName}", 2, ids[id]);
                AddComponent(solutionUniqueName, 2, id, false);
            }

            var rels2 = emd.ManyToOneRelationships.Where(a => a.IsManaged.Value);
            Log(emd, "Information", Guid.Empty, "", -1, "Searching for changes for managed Many-To-One relationships", 0);
            var rel2Ids = searchService.GetActiveLayers(rels2.Select(a => a.MetadataId.Value).ToList(), Worker, "Relationship");

            foreach (var id in rel2Ids.Keys)
            {
                if (components.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == id) != null) continue;

                Log(emd, "Many-To-One relationship", id, rels2.First(a => a.MetadataId.Value == id).SchemaName, 10, $"{(isSimulation ? "Should add" : "Adding")} Many-To-One relationship {rels2.First(r => r.MetadataId.Value == id).SchemaName} to solution {solutionUniqueName}", 3, rel2Ids[id]);
                AddComponent(solutionUniqueName, 10, id, false);
            }

            var rels3 = emd.ManyToManyRelationships.Where(a => a.IsManaged.Value);
            Log(emd, "Information", Guid.Empty, "", -1, "Searching for changes for managed Many-To-Many relationships", 0);
            var rel3Ids = searchService.GetActiveLayers(rels2.Select(a => a.MetadataId.Value).ToList(), Worker, "Relationship");

            foreach (var id in rel3Ids.Keys)
            {
                if (components.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == id) != null) continue;

                Log(emd, "Many-To-Many relationship", id, rels3.First(a => a.MetadataId.Value == id).SchemaName, 3, $"{(isSimulation ? "Should add" : "Adding")} Many-To-Many relationship {rels3.First(r => r.MetadataId.Value == id).SchemaName} to solution {solutionUniqueName}", 10, rel3Ids[id]);

                AddComponent(solutionUniqueName, 10, id, false);
            }

            var keys = emd.Keys.Where(a => a.IsManaged.Value);
            Log(emd, "Information", Guid.Empty, "", -1, "Searching for changes for managed keys", 0);
            var keysIds = searchService.GetActiveLayers(keys.Select(a => a.MetadataId.Value).ToList(), Worker, "EntityKey");

            foreach (var id in keysIds.Keys)
            {
                if (components.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == id) != null) continue;

                Log(emd, "Key", id, keys.First(a => a.MetadataId.Value == id).DisplayName?.UserLocalizedLabel?.Label ?? "N/A", 14, $"{(isSimulation ? "Should add" : "Adding")} Key {keys.First(r => r.MetadataId.Value == id).SchemaName} to solution {solutionUniqueName}", 4, keysIds[id]);
                AddComponent(solutionUniqueName, 14, id, false);
            }

            var forms = GetForms(emd.LogicalName, true, false);
            Log(emd, "Information", Guid.Empty, "", -1, "Searching for changes for managed forms", 0);
            var formsIds = searchService.GetActiveLayers(forms.Select(a => a.Id).ToList(), Worker, "SystemForm");

            foreach (var id in formsIds.Keys)
            {
                if (components.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == id) != null) continue;

                Log(emd, "Form", id, forms.First(f => f.Id == id).GetAttributeValue<string>("name"), 60, $"{(isSimulation ? "Should add" : "Adding")} form {forms.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 5, formsIds[id]);
                AddComponent(solutionUniqueName, 60, id, false);
            }

            forms = GetForms(emd.LogicalName, true, true);
            Log(emd, "Information", Guid.Empty, "", -1, "Searching for changes for managed dashboards", 0);
            var dashboardsIds = searchService.GetActiveLayers(forms.Select(a => a.Id).ToList(), Worker, "SystemForm");

            foreach (var id in dashboardsIds.Keys)
            {
                if (components.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == id) != null) continue;

                Log(emd, "Dashboard", id, forms.First(f => f.Id == id).GetAttributeValue<string>("name"), 60, $"{(isSimulation ? "Should add" : "Adding")} dashboard {forms.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 6, dashboardsIds[id]);
                AddComponent(solutionUniqueName, 60, id, false);
            }

            var views = GetViews(emd.LogicalName, true);
            Log(emd, "Information", Guid.Empty, "", -1, "Searching for changes for managed views", 0);
            var viewsIds = searchService.GetActiveLayers(views.Select(a => a.Id).ToList(), Worker, "SavedQuery");

            foreach (var id in viewsIds.Keys)
            {
                if (components.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == id) != null) continue;

                Log(emd, "View", id, views.First(f => f.Id == id).GetAttributeValue<string>("name"), 26, $"{(isSimulation ? "Should add" : "Adding")} view {views.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 7, viewsIds[id]);
                AddComponent(solutionUniqueName, 26, id, false);
            }

            var charts = GetCharts(emd.LogicalName, true);
            Log(emd, "Information", Guid.Empty, "", -1, "Searching for changes for managed charts", 0);
            var chartsIds = searchService.GetActiveLayers(charts.Select(a => a.Id).ToList(), Worker, "SavedQueryVisualization");

            foreach (var id in chartsIds.Keys)
            {
                if (components.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == id) != null) continue;

                Log(emd, "Chart", id, charts.First(f => f.Id == id).GetAttributeValue<string>("name"), 59, $"{(isSimulation ? "Should add" : "Adding")} chart {charts.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 8, chartsIds[id]);
                AddComponent(solutionUniqueName, 59, id, false);
            }

            var businessRules = GetBusinessRules(emd.LogicalName, true);
            Log(emd, "Information", Guid.Empty, "", -1, "Searching for changes for managed business rules", 0);
            var businessRulesIds = searchService.GetActiveLayers(businessRules.Select(a => a.Id).ToList(), Worker, "Workflow");

            foreach (var id in businessRulesIds.Keys)
            {
                if (components.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == id) != null) continue;

                Log(emd, "Business Rule", id, businessRules.First(f => f.Id == id).GetAttributeValue<string>("name"), 29, $"{(isSimulation ? "Should add" : "Adding")} business rule {businessRules.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 9, businessRulesIds[id]);
                AddComponent(solutionUniqueName, 29, id, false);
            }

            var commands = GetCommands(emd.MetadataId.Value, true);
            Log(emd, "Information", Guid.Empty, "", -1, "Searching for changes for managed commands", 0);
            var commandsIds = searchService.GetActiveLayers(commands.Select(a => a.Id).ToList(), Worker, "appaction");

            foreach (var id in commandsIds.Keys)
            {
                if (components.FirstOrDefault(c => c.GetAttributeValue<Guid>("objectid") == id) != null) continue;

                Log(emd, "Command", id, commands.First(f => f.Id == id).GetAttributeValue<string>("name"), commandTypeCode, $"{(isSimulation ? "Should add" : "Adding")} command {commands.First(f => f.Id == id).GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 10, commandsIds[id]);
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

        private void AddTableToSolution(EntityMetadata emd, string solutionUniqueName)
        {
            if (emd.IsManaged.Value)
            {
                if (!emd.IsIntersect.Value)
                {
                    Log(emd, "Information", Guid.Empty, "", -1, $"Searching for changes for table {emd.DisplayName?.UserLocalizedLabel?.Label ?? emd.SchemaName}", 0);
                    var result = searchService.GetActiveLayers(new List<Guid> { emd.MetadataId.Value }, Worker, "Entity");
                    if (result.Count > 0)
                    {
                        Log(emd, "Table", emd.MetadataId.Value, emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", 1, $"{(isSimulation ? "Should add" : "Adding")} managed table {emd.SchemaName} to solution {solutionUniqueName} without any assets but with metadata", 1);
                        AddComponent(solutionUniqueName, 1, emd.MetadataId.Value, false, true, true);
                    }
                    else
                    {
                        Log(emd, "Table", emd.MetadataId.Value, emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", 1, $"{(isSimulation ? "Should add" : "Adding")} managed table {emd.SchemaName} to solution {solutionUniqueName} without any assets or metadata", 1);
                        AddComponent(solutionUniqueName, 1, emd.MetadataId.Value, false, true, false);
                    }
                }
            }
            else
            {
                Log(emd, "Table", emd.MetadataId.Value, emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", 1, $"{(isSimulation ? "Should add" : "Adding")} unmanaged table {emd.SchemaName} to solution {solutionUniqueName} without all assets", 1);
                AddComponent(solutionUniqueName, 1, emd.MetadataId.Value, false, false, true);
            }
        }

        private void AddUnmanagedComponents(EntityMetadata emd, string solutionUniqueName)
        {
            foreach (var amd in emd.Attributes.Where(a => a.IsManaged.Value == false))
            {
                if (amd.AttributeOf != null && emd.Attributes.First(a => a.LogicalName == amd.AttributeOf).IsManaged.Value) continue;

                Log(emd, "Attribute", amd.MetadataId.Value, amd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", 2, $"{(isSimulation ? "Should add" : "Adding")} attribute {amd.SchemaName} to solution {solutionUniqueName}", 2);
                AddComponent(solutionUniqueName, 2, amd.MetadataId.Value, false);
            }

            foreach (var rmd2 in emd.ManyToOneRelationships.Where(a => a.IsManaged.Value == false))
            {
                Log(emd, "Many-To-One relationship", rmd2.MetadataId.Value, rmd2.SchemaName, 10, $"{(isSimulation ? "Should add" : "Adding")} Many-To-One relationship {rmd2.SchemaName} to solution {solutionUniqueName}", 3);
                AddComponent(solutionUniqueName, 10, rmd2.MetadataId.Value, false);
            }

            foreach (var mmrmd in emd.ManyToManyRelationships.Where(a => a.IsManaged.Value == false))
            {
                Log(emd, "Many-To-Many relationship", mmrmd.MetadataId.Value, mmrmd.SchemaName, 10, $"{(isSimulation ? "Should add" : "Adding")} Many-To-Many relationship {mmrmd.SchemaName} to solution {solutionUniqueName}", 3);
                AddComponent(solutionUniqueName, 10, mmrmd.MetadataId.Value, false);
            }

            foreach (var kmd in emd.Keys.Where(a => a.IsManaged.Value == false))
            {
                Log(emd, "Key", kmd.MetadataId.Value, kmd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", 14, $"{(isSimulation ? "Should add" : "Adding")} Key {kmd.SchemaName} to solution {solutionUniqueName}", 4);
                AddComponent(solutionUniqueName, 14, kmd.MetadataId.Value, false);
            }

            var forms = GetForms(emd.LogicalName, false, false);
            foreach (var form in forms)
            {
                Log(emd, "Form", form.Id, form.GetAttributeValue<string>("name"), 60, $"{(isSimulation ? "Should add" : "Adding")} form {form.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 5);
                AddComponent(solutionUniqueName, 60, form.Id, false);
            }

            forms = GetForms(emd.LogicalName, false, true);
            foreach (var form in forms)
            {
                Log(emd, "Dashboard", form.Id, form.GetAttributeValue<string>("name"), 60, $"{(isSimulation ? "Should add" : "Adding")} dashboard {form.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 6);
                AddComponent(solutionUniqueName, 60, form.Id, false);
            }

            var views = GetViews(emd.LogicalName, false);
            foreach (var view in views)
            {
                Log(emd, "View", view.Id, view.GetAttributeValue<string>("name"), 26, $"{(isSimulation ? "Should add" : "Adding")} view {view.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 7);
                AddComponent(solutionUniqueName, 26, view.Id, false);
            }

            var charts = GetCharts(emd.LogicalName, false);
            foreach (var chart in charts)
            {
                Log(emd, "Chart", chart.Id, chart.GetAttributeValue<string>("name"), 59, $"{(isSimulation ? "Should add" : "Adding")} chart {chart.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 8);
                AddComponent(solutionUniqueName, 59, chart.Id, false);
            }

            var businessRules = GetBusinessRules(emd.LogicalName, false);
            foreach (var businessRule in businessRules)
            {
                Log(emd, "Business Rule", businessRule.Id, businessRule.GetAttributeValue<string>("name"), 29, $"{(isSimulation ? "Should add" : "Adding")} business rule {businessRule.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 9);
                AddComponent(solutionUniqueName, 29, businessRule.Id, false);
            }

            var commands = GetCommands(emd.MetadataId.Value, false);
            foreach (var command in commands)
            {
                Log(emd, "Command", command.Id, command.GetAttributeValue<string>("name"), commandTypeCode, $"{(isSimulation ? "Should add" : "Adding")} command {command.GetAttributeValue<string>("name")} to solution {solutionUniqueName}", 10);
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
                Properties = new MetadataPropertiesExpression("MetadataId", "DisplayName", "SchemaName", "IsManaged", "IsIntersect", "Attributes", "Keys", "OneToManyRelationships", "ManyToOneRelationships", "ManyToManyRelationships"),
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

        private void Log(EntityMetadata emd, string type, Guid id, string name, int typeCode, string message, int imageIndex, string changedProperties = null)
        {
            var log = new TableLog
            {
                Table = emd?.DisplayName?.UserLocalizedLabel?.Label ?? "N/A",
                Type = type,
                TypeCode = typeCode,
                Message = message,
                ComponentId = id,
                ComponentName = name,
                ChangedProperties = changedProperties,
                EntityMetadata = emd
            };

            if (Worker?.WorkerReportsProgress ?? false)
            {
                Worker.ReportProgress(imageIndex, log);
            }

            OnLog?.Invoke(this, new TableCleanerProgressEventArgs { Log = log, ImageIndex = imageIndex });
        }

        private void RemoveTableFromSolution(EntityMetadata emd, string solutionUniqueName)
        {
            Log(emd, "Table", emd.MetadataId.Value, emd.DisplayName?.UserLocalizedLabel?.Label ?? "N/A", 1, $"Removing table {emd.SchemaName} from solution {solutionUniqueName}", 1);

            if (!isSimulation)
            {
                service.Execute(new RemoveSolutionComponentRequest
                {
                    SolutionUniqueName = solutionUniqueName,
                    ComponentType = 1,
                    ComponentId = emd.MetadataId.Value
                });
            }
        }
    }
}