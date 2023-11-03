using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MscrmTools.SolutionTableIntegrityManager.AppCode
{
    internal class ActiveLayerSearch
    {
        private readonly ExecuteMultipleRequest _bulk;
        private readonly IOrganizationService _service;

        public ActiveLayerSearch(IOrganizationService service)
        {
            _service = service;

            _bulk = new ExecuteMultipleRequest
            {
                Requests = new OrganizationRequestCollection(),
                Settings = new ExecuteMultipleSettings
                {
                    ContinueOnError = true,
                    ReturnResponses = true
                }
            };
        }

        public Dictionary<Guid, string> GetActiveLayers(List<Guid> componentIds, BackgroundWorker bw, string componentName)
        {
            _bulk.Requests = new OrganizationRequestCollection();
            var changedItemsId = new Dictionary<Guid, string>();
            decimal progress = 0;
            int processed = 0;
            bool needPrecision = false;
            foreach (var id in componentIds)
            {
                if (bw.CancellationPending) return changedItemsId;

                _bulk.Requests.Add(new RetrieveMultipleRequest
                {
                    Query = new QueryExpression("msdyn_componentlayer")
                    {
                        NoLock = true,
                        ColumnSet = new ColumnSet("msdyn_changes"),
                        Criteria = new FilterExpression
                        {
                            Conditions =
                            {
                                new ConditionExpression("msdyn_solutionname",ConditionOperator.Equal, "Active"),
                                new ConditionExpression("msdyn_solutioncomponentname",ConditionOperator.Equal, componentName),
                                new ConditionExpression("msdyn_componentid",ConditionOperator.Equal, id),
                            }
                        }
                    }
                });

                processed++;
                progress = (decimal)processed / componentIds.Count * 100;

                if (_bulk.Requests.Count == 200)
                {
                    if (progress < 100 || needPrecision)
                    {
                        //bw.ReportProgress(0, $"Loading active layers for {componentName} ({Convert.ToInt32(progress)}%)...");
                        needPrecision = true;
                    }

                    var bulkResp = (ExecuteMultipleResponse)_service.Execute(_bulk);
                    foreach (var response in bulkResp.Responses)
                    {
                        var request = (RetrieveMultipleRequest)_bulk.Requests[response.RequestIndex];
                        var objectId = (Guid)((QueryExpression)request.Query).Criteria.Conditions.Last().Values.First();

                        if (((RetrieveMultipleResponse)response.Response).EntityCollection.Entities.Count == 1)
                        {
                          
                            changedItemsId.Add(objectId, ((RetrieveMultipleResponse)response.Response).EntityCollection.Entities.First().GetAttributeValue<string>("msdyn_changes"));
                        }
                    }

                    _bulk.Requests.Clear();
                }
            }

            if (_bulk.Requests.Count > 0)
            {
                if (progress < 100 || needPrecision)
                {
                    //bw.ReportProgress(0, $"Loading active layers for {componentName} ({Convert.ToInt32(progress)}%)...");
                }

                var bulkResp = (ExecuteMultipleResponse)_service.Execute(_bulk);
                foreach (var response in bulkResp.Responses)
                {
                    var request = (RetrieveMultipleRequest)_bulk.Requests[response.RequestIndex];
                    var objectId = (Guid)((QueryExpression)request.Query).Criteria.Conditions.Last().Values.First();

                    if (((RetrieveMultipleResponse)response.Response).EntityCollection.Entities.Count == 1)
                    {
                      
                        changedItemsId.Add(objectId, ((RetrieveMultipleResponse)response.Response).EntityCollection.Entities.First().GetAttributeValue<string>("msdyn_changes"));
                       
                    }
                }
            }

            return changedItemsId;
        }
    }
}