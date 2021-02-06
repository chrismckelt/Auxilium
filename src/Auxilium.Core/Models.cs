using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Auxilium.Core
{

    #region http response

    public class Headers
    {

        [JsonProperty("Pragma")]
        public string Pragma { get; set; }

        [JsonProperty("Transfer-Encoding")]
        public string TransferEncoding { get; set; }

        [JsonProperty("Retry-After")]
        public string RetryAfter { get; set; }

        [JsonProperty("Vary")]
        public string Vary { get; set; }

        [JsonProperty("x-ms-request-id")]
        public string RequestId { get; set; }

        [JsonProperty("Strict-Transport-Security")]
        public string StrictTransportSecurity { get; set; }

        [JsonProperty("X-Content-Type-Options")]
        public string XContentTypeOptions { get; set; }

        [JsonProperty("X-Frame-Options")]
        public string XFrameOptions { get; set; }

        [JsonProperty("Timing-Allow-Origin")]
        public string TimingAllowOrigin { get; set; }

        [JsonProperty("x-ms-apihub-cached-response")]
        public string xmsapihubcachedresponse { get; set; }

        [JsonProperty("Cache-Control")]
        public string CacheControl { get; set; }

        [JsonProperty("Date")]
        public string Date { get; set; }

        [JsonProperty("Location")]
        public string Location { get; set; }

        [JsonProperty("Content-Type")]
        public string ContentType { get; set; }

        [JsonProperty("Expires")]
        public string Expires { get; set; }

        [JsonProperty("Content-Length")]
        public string ContentLength { get; set; }
    }

    public class Properties
    {

        [JsonProperty("$AzureWebJobsParentId")]
        public string AzureWebJobsParentId { get; set; }

        [JsonProperty("Diagnostic-Id")]
        public string DiagnosticId { get; set; }

        [JsonProperty("DeliveryCount")]
        public string DeliveryCount { get; set; }

        [JsonProperty("EnqueuedSequenceNumber")]
        public string EnqueuedSequenceNumber { get; set; }

        [JsonProperty("EnqueuedTimeUtc")]
        public DateTime EnqueuedTimeUtc { get; set; }

        [JsonProperty("ExpiresAtUtc")]
        public DateTime ExpiresAtUtc { get; set; }

        [JsonProperty("LockedUntilUtc")]
        public DateTime LockedUntilUtc { get; set; }

        [JsonProperty("LockToken")]
        public string LockToken { get; set; }

        [JsonProperty("MessageId")]
        public string MessageId { get; set; }

        [JsonProperty("ScheduledEnqueueTimeUtc")]
        public DateTime ScheduledEnqueueTimeUtc { get; set; }

        [JsonProperty("SequenceNumber")]
        public string SequenceNumber { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("State")]
        public string State { get; set; }

        [JsonProperty("TimeToLive")]
        public string TimeToLive { get; set; }
    }

    public class Body
    {
        private string _contentData;

        [JsonProperty("Content")]
        public string Content { get; set; }

        [JsonProperty("ContentData")]
        public string ContentData
        {
            get
            {
                if (string.IsNullOrEmpty(_contentData)) return Content;// hack for Service Bus XML payload - not HTTP trigger

                return _contentData;
            }

            set
            {
                if (string.IsNullOrEmpty(Content)) Content = _contentData;
                _contentData = value;
            }
        }

        [JsonProperty("ContentType")]
        public string ContentType { get; set; }

        [JsonProperty("ContentTransferEncoding")]
        public string ContentTransferEncoding { get; set; }

        [JsonProperty("Properties")]
        public Properties Properties { get; set; }

        [JsonProperty("MessageId")]
        public string MessageId { get; set; }

        [JsonProperty("To")]
        public object To { get; set; }

        [JsonProperty("ReplyTo")]
        public object ReplyTo { get; set; }

        [JsonProperty("ReplyToSessionId")]
        public object ReplyToSessionId { get; set; }

        [JsonProperty("Label")]
        public object Label { get; set; }

        [JsonProperty("ScheduledEnqueueTimeUtc")]
        public DateTime ScheduledEnqueueTimeUtc { get; set; }

        [JsonProperty("SessionId")]
        public object SessionId { get; set; }

        [JsonProperty("CorrelationId")]
        public object CorrelationId { get; set; }

        [JsonProperty("SequenceNumber")]
        public int SequenceNumber { get; set; }

        [JsonProperty("LockToken")]
        public string LockToken { get; set; }

        [JsonProperty("TimeToLive")]
        public string TimeToLive { get; set; }
    }

    public class HttpResponse
    {

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("headers")]
        public Headers Headers { get; set; }

        [JsonProperty("body")]
        public Body Body { get; set; }
    }

    public class HttpResponse2
    {

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }

        [JsonProperty("headers")]
        public Headers Headers { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        //[JsonProperty("body")]
        //public Body Body { get; set; }
    }


    #endregion

    #region  models

    public class AzureResultList<T>
    {
        [JsonProperty("value")]
        public IList<T> Value { get; set; }
        [JsonProperty("nextLink")]
        public string NextLink { get; set; }
    }

    public class SubscriptionPolicies
    {

        [JsonProperty("locationPlacementId")]
        public string LocationPlacementId { get; set; }

        [JsonProperty("quotaId")]
        public string QuotaId { get; set; }

        [JsonProperty("spendingLimit")]
        public string SpendingLimit { get; set; }
    }

    public class SubscriptionValue
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("subscriptionId")]
        public string SubscriptionId { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("subscriptionPolicies")]
        public SubscriptionPolicies SubscriptionPolicies { get; set; }

        [JsonProperty("authorizationSource")]
        public string AuthorizationSource { get; set; }
    }

    public class AzureSubscriptions
    {

        [JsonProperty("value")]
        public IList<SubscriptionValue> Value { get; set; }
    }



    public class AzureLoginRequest
    {

        public string Username { get; set; }

        public string Password { get; set; }

        public string TenantId { get; set; }
    }



    public class AzureLogicAppWorkflowTriggerRecurrence
    {

        [JsonProperty("frequency")]
        public string Frequency { get; set; }

        [JsonProperty("interval")]
        public int Interval { get; set; }
    }

    public class AzureLogicAppWorkflowTriggerWorkflow
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class AzureLogicAppWorkflowTriggerProperties
    {

        [JsonProperty("provisioningState")]
        public string ProvisioningState { get; set; }

        [JsonProperty("createdTime")]
        public DateTime CreatedTime { get; set; }

        [JsonProperty("changedTime")]
        public DateTime ChangedTime { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("lastExecutionTime")]
        public DateTime LastExecutionTime { get; set; }

        [JsonProperty("nextExecutionTime")]
        public DateTime NextExecutionTime { get; set; }

        [JsonProperty("recurrence")]
        public AzureLogicAppWorkflowTriggerRecurrence Recurrence { get; set; }

        [JsonProperty("workflow")]
        public AzureLogicAppWorkflowTriggerWorkflow Workflow { get; set; }
    }

    public class AzureLogicAppWorkflowTriggerValue
    {

        [JsonProperty("properties")]
        public AzureLogicAppWorkflowTriggerProperties Properties { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }





    public class IntegrationAccount
    {
        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class DefaultValue
    {
    }

    public class Connections
    {
        [JsonProperty("defaultValue")] public DefaultValue DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ContainerName
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class EventFullname
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ValidationSchemaName
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class SplitXpathExpression
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class UseSoapResponses
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class TransformHeaderMapName
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class Constants
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class EventName
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class EventNameLink
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ContractCode
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class Customer
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class Sftp
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class EventNameBatch
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class XmlServicesHost
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class RunConcurrency
    {
        [JsonProperty("defaultValue")] public int DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class DashboardServicesHost
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ResourceGroupName
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class PollingInterval
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class Maxcolumnsize
    {
        [JsonProperty("defaultValue")] public int DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class TriggerInterval
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class OmitLogicApps
    {
        [JsonProperty("defaultValue")] public IList<string> DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class GroupingNode
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class GroupingPath
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class GroupingBatchSize
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class AxServicesHost
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class CdmServicesHost
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class LogicAppServicesHost
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ContactIdSuffixes
    {
        [JsonProperty("defaultValue")] public IList<string> DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class SharePointSiteUrl
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class SsrsFunctionAuthKey
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class TaskName
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ExpiredRecurrenceEventName
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class SsrsPropertyConditionReportPath
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class SsrsRoutineInspectionReportPath
    {
        [JsonProperty("defaultValue")] public string DefaultValue { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ExcelServicesHost
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class SharePointUploadSiteUrl
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class SharePointUploadFolder
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class TopicName
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class Parameters
    {
        [JsonProperty("$connections")] public Connections Connections { get; set; }

        [JsonProperty("containerName")] public ContainerName ContainerName { get; set; }

        [JsonProperty("eventFullname")] public EventFullname EventFullname { get; set; }

        [JsonProperty("validationSchemaName")] public ValidationSchemaName ValidationSchemaName { get; set; }

        [JsonProperty("splitXpathExpression")] public SplitXpathExpression SplitXpathExpression { get; set; }

        [JsonProperty("useSoapResponses")] public UseSoapResponses UseSoapResponses { get; set; }

        [JsonProperty("transformHeaderMapName")]
        public TransformHeaderMapName TransformHeaderMapName { get; set; }

        [JsonProperty("constants")] public Constants Constants { get; set; }

        [JsonProperty("eventName")] public EventName EventName { get; set; }

        [JsonProperty("eventNameLink")] public EventNameLink EventNameLink { get; set; }

        [JsonProperty("contractCode")] public ContractCode ContractCode { get; set; }

        [JsonProperty("customer")] public Customer Customer { get; set; }

        [JsonProperty("sftp")] public Sftp Sftp { get; set; }

        [JsonProperty("eventNameBatch")] public EventNameBatch EventNameBatch { get; set; }

        [JsonProperty("xmlServicesHost")] public XmlServicesHost XmlServicesHost { get; set; }

        [JsonProperty("runConcurrency")] public RunConcurrency RunConcurrency { get; set; }

        [JsonProperty("dashboardServicesHost")]
        public DashboardServicesHost DashboardServicesHost { get; set; }

        [JsonProperty("resourceGroupName")] public ResourceGroupName ResourceGroupName { get; set; }

        [JsonProperty("pollingInterval")] public PollingInterval PollingInterval { get; set; }

        [JsonProperty("maxcolumnsize")] public Maxcolumnsize Maxcolumnsize { get; set; }

        [JsonProperty("triggerInterval")] public TriggerInterval TriggerInterval { get; set; }

        [JsonProperty("omitLogicApps")] public OmitLogicApps OmitLogicApps { get; set; }

        [JsonProperty("groupingNode")] public GroupingNode GroupingNode { get; set; }

        [JsonProperty("groupingPath")] public GroupingPath GroupingPath { get; set; }

        [JsonProperty("groupingBatchSize")] public GroupingBatchSize GroupingBatchSize { get; set; }

        [JsonProperty("axServicesHost")] public AxServicesHost AxServicesHost { get; set; }

        [JsonProperty("cdmServicesHost")] public CdmServicesHost CdmServicesHost { get; set; }

        [JsonProperty("logicAppServicesHost")] public LogicAppServicesHost LogicAppServicesHost { get; set; }

        [JsonProperty("contactIdSuffixes")] public ContactIdSuffixes ContactIdSuffixes { get; set; }

        [JsonProperty("sharePointSiteUrl")] public SharePointSiteUrl SharePointSiteUrl { get; set; }

        [JsonProperty("ssrsFunctionAuthKey")] public SsrsFunctionAuthKey SsrsFunctionAuthKey { get; set; }

        [JsonProperty("taskName")] public TaskName TaskName { get; set; }

        [JsonProperty("expiredRecurrenceEventName")]
        public ExpiredRecurrenceEventName ExpiredRecurrenceEventName { get; set; }

        [JsonProperty("ssrsPropertyConditionReportPath")]
        public SsrsPropertyConditionReportPath SsrsPropertyConditionReportPath { get; set; }

        [JsonProperty("ssrsRoutineInspectionReportPath")]
        public SsrsRoutineInspectionReportPath SsrsRoutineInspectionReportPath { get; set; }

        [JsonProperty("excelServicesHost")] public ExcelServicesHost ExcelServicesHost { get; set; }

        [JsonProperty("sharePointUploadSiteUrl")]
        public SharePointUploadSiteUrl SharePointUploadSiteUrl { get; set; }

        [JsonProperty("sharePointUploadFolder")]
        public SharePointUploadFolder SharePointUploadFolder { get; set; }

        [JsonProperty("topicName")] public TopicName TopicName { get; set; }
    }

    public class Recurrence
    {
        [JsonProperty("frequency")] public string Frequency { get; set; }

        [JsonProperty("interval")] public int Interval { get; set; }
    }

    public class Api
    {
        [JsonProperty("runtimeUrl")] public string RuntimeUrl { get; set; }
    }

    public class Connection
    {
        [JsonProperty("name")] public string Name { get; set; }
    }

    public class Host
    {
        [JsonProperty("api")] public Api Api { get; set; }

        [JsonProperty("connection")] public Connection Connection { get; set; }
    }

    public class Inputs
    {
        [JsonProperty("host")] public Host Host { get; set; }

        [JsonProperty("method")] public string Method { get; set; }

        [JsonProperty("path")] public string Path { get; set; }
    }

    public class WhenAMessageIsReceivedInTopicSubscription
    {
        [JsonProperty("recurrence")] public Recurrence Recurrence { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class FileCreatedInBlobStorage
    {
        [JsonProperty("recurrence")] public Recurrence Recurrence { get; set; }

        [JsonProperty("splitOn")] public string SplitOn { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("apiDefinitionUrl")] public string ApiDefinitionUrl { get; set; }

        [JsonProperty("swaggerSource")] public string SwaggerSource { get; set; }
    }

    public class FileTriggerOnFileAvailable
    {
        [JsonProperty("recurrence")] public Recurrence Recurrence { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class Manual
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("kind")] public string Kind { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class WhenAFileIsAddedOrModified
    {
        [JsonProperty("recurrence")] public Recurrence Recurrence { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class Concurrency
    {
        [JsonProperty("runs")] public int Runs { get; set; }
    }

    public class RuntimeConfiguration
    {
        [JsonProperty("concurrency")] public Concurrency Concurrency { get; set; }
    }

    public class Topic
    {
        [JsonProperty("recurrence")] public Recurrence Recurrence { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }

        [JsonProperty("runtimeConfiguration")] public RuntimeConfiguration RuntimeConfiguration { get; set; }

        [JsonProperty("operationOptions")] public string OperationOptions { get; set; }
    }

    public class ManualTrigger
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("kind")] public string Kind { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class PeriodicTrigger
    {
        [JsonProperty("recurrence")] public Recurrence Recurrence { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class WhenAMessageIsReceivedInQueue
    {
        [JsonProperty("recurrence")] public Recurrence Recurrence { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class WhenAFileIsCreated
    {
        [JsonProperty("recurrence")] public Recurrence Recurrence { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class Triggers
    {
        [JsonProperty("When_a_message_is_received_in_topic_subscription")]
        public WhenAMessageIsReceivedInTopicSubscription WhenAMessageIsReceivedInTopicSubscription { get; set; }

        [JsonProperty("FileCreatedInBlobStorage")]
        public FileCreatedInBlobStorage FileCreatedInBlobStorage { get; set; }

        [JsonProperty("File_TriggerOnFileAvailable")]
        public FileTriggerOnFileAvailable FileTriggerOnFileAvailable { get; set; }

        [JsonProperty("manual")] public Manual Manual { get; set; }

        [JsonProperty("When_a_file_is_added_or_modified")]
        public WhenAFileIsAddedOrModified WhenAFileIsAddedOrModified { get; set; }

        [JsonProperty("topic")] public Topic Topic { get; set; }

        [JsonProperty("Manual_trigger")] public ManualTrigger ManualTrigger { get; set; }

        [JsonProperty("Periodic_trigger")] public PeriodicTrigger PeriodicTrigger { get; set; }

        [JsonProperty("When_a_message_is_received_in_queue")]
        public WhenAMessageIsReceivedInQueue WhenAMessageIsReceivedInQueue { get; set; }

        [JsonProperty("When_a_file_is_created")]
        public WhenAFileIsCreated WhenAFileIsCreated { get; set; }
    }

    public class GetFileMetadataUsingPath
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class RunAfter
    {
        [JsonProperty("Get_File_Metadata_using_path")]
        public IList<string> GetFileMetadataUsingPath { get; set; }
    }

    public class GetFileContentUsingPath
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class FlatFileToXml
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class SetXml
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public string Inputs { get; set; }
    }

    public class Transform
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class XmlFix
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class SendMessage
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class LogicAppActions
    {
        [JsonProperty("Transform")] public Transform Transform { get; set; }

        [JsonProperty("Xml_Fix")] public XmlFix XmlFix { get; set; }

        [JsonProperty("Send_message")] public SendMessage SendMessage { get; set; }
    }

    public class ForEachItem
    {
        [JsonProperty("foreach")] public string Foreach { get; set; }

        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class CreateFile
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class DeleteFile
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class GetFile
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class EntityBuilderBuildFileEntity
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class EntityBuilderBuildEventHeaderEntity
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class Validate
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class Else
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }
    }

    public class IsValid
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("else")] public Else Else { get; set; }

        [JsonProperty("expression")] public string Expression { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class AuditMessageReceived
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class SetFileName
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public string Inputs { get; set; }
    }

    public class SetFolderPath
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public string Inputs { get; set; }
    }

    public class SetFilePath
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public string Inputs { get; set; }
    }

    public class CheckFile
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class FileNotExists
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("expression")] public string Expression { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ProcessErrors
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class IsNotValid
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class Decode
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public object Inputs { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }
    }

    public class CoalesceTransform
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public string Inputs { get; set; }
    }

    public class EachMessage
    {
        [JsonProperty("foreach")] public string Foreach { get; set; }

        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class GetLogicAppFailures
    {
        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class ForEachRunFailure
    {
        [JsonProperty("foreach")] public string Foreach { get; set; }

        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class Response
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class GetLogicApps
    {
        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class FilterLogicApps
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class ForEachLogicApp
    {
        [JsonProperty("foreach")] public string Foreach { get; set; }

        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class InsertRow
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class BatchAndGroup
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class EachBatch
    {
        [JsonProperty("foreach")] public string Foreach { get; set; }

        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class Json
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class HeaderToJson
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public string Inputs { get; set; }
    }

    public class MapToAXCompany
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class JsonCustomer
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public string Inputs { get; set; }
    }

    public class GetAccountNumber
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public string Inputs { get; set; }
    }

    public class IsContactUpdate
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("else")] public Else Else { get; set; }

        [JsonProperty("expression")] public string Expression { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class GetAgreementDimension
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class AgreementNotExists
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("else")] public Else Else { get; set; }

        [JsonProperty("expression")] public string Expression { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ConstructReponse
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class SucceedResponse
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class FailedResponse
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class CallReceiveWorkOrder
    {
        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }
    }

    public class CheckResponse
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("else")] public Else Else { get; set; }

        [JsonProperty("expression")] public string Expression { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ComposeReportRequest
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class GetSSRSReport
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class UploadReportToSharePoint
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class RestResponseAccepted
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class ProcessPrimaryTenant
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class HasPrimaryTenant
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("expression")] public string Expression { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class IfRevenueGeneratingLease
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("expression")] public string Expression { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class TransformPropertyToFixedAsset
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class JsonFixedAsset
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public string Inputs { get; set; }
    }

    public class ExistingFixedAsset
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class IfFixedAssetExists
    {
        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("else")] public Else Else { get; set; }

        [JsonProperty("expression")] public string Expression { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class PublishInspectionReport
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class DecodeExcel
    {
        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class EachRow
    {
        [JsonProperty("foreach")] public string Foreach { get; set; }

        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("type")] public string Type { get; set; }
    }

    public class SetCustomerContractDetail
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class ConvertFaultDesc
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class CreateEntityMap
    {
        [JsonProperty("runAfter")] public RunAfter RunAfter { get; set; }

        [JsonProperty("metadata")] public Metadata Metadata { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("inputs")] public Inputs Inputs { get; set; }
    }

    public class Actions
    {
        [JsonProperty("Get_File_Metadata_using_path")]
        public GetFileMetadataUsingPath GetFileMetadataUsingPath { get; set; }

        [JsonProperty("Get_file_content_using_path")]
        public GetFileContentUsingPath GetFileContentUsingPath { get; set; }

        [JsonProperty("FlatFileToXml")] public FlatFileToXml FlatFileToXml { get; set; }

        [JsonProperty("Set_Xml")] public SetXml SetXml { get; set; }

        [JsonProperty("ForEach_Item")] public ForEachItem ForEachItem { get; set; }

        [JsonProperty("Create_file")] public CreateFile CreateFile { get; set; }

        [JsonProperty("Delete_file")] public DeleteFile DeleteFile { get; set; }

        [JsonProperty("Get_File")] public GetFile GetFile { get; set; }

        [JsonProperty("EntityBuilder_BuildFileEntity")]
        public EntityBuilderBuildFileEntity EntityBuilderBuildFileEntity { get; set; }

        [JsonProperty("EntityBuilder_BuildEventHeaderEntity")]
        public EntityBuilderBuildEventHeaderEntity EntityBuilderBuildEventHeaderEntity { get; set; }

        [JsonProperty("Send_message")] public SendMessage SendMessage { get; set; }

        [JsonProperty("Validate")] public Validate Validate { get; set; }

        [JsonProperty("Is_Valid")] public IsValid IsValid { get; set; }

        [JsonProperty("Audit_MessageReceived")]
        public AuditMessageReceived AuditMessageReceived { get; set; }

        [JsonProperty("Set_FileName")] public SetFileName SetFileName { get; set; }

        [JsonProperty("Set_FolderPath")] public SetFolderPath SetFolderPath { get; set; }

        [JsonProperty("Set_FilePath")] public SetFilePath SetFilePath { get; set; }

        [JsonProperty("Check_File")] public CheckFile CheckFile { get; set; }

        [JsonProperty("File_NotExists")] public FileNotExists FileNotExists { get; set; }

        [JsonProperty("Process_Errors")] public ProcessErrors ProcessErrors { get; set; }

        [JsonProperty("Is_NotValid")] public IsNotValid IsNotValid { get; set; }

        [JsonProperty("Decode")] public Decode Decode { get; set; }

        [JsonProperty("Transform")] public Transform Transform { get; set; }

        [JsonProperty("Coalesce_Transform")] public CoalesceTransform CoalesceTransform { get; set; }

        [JsonProperty("Xml_Fix")] public XmlFix XmlFix { get; set; }

        [JsonProperty("Each_Message")] public EachMessage EachMessage { get; set; }

        [JsonProperty("Get_LogicAppFailures")] public GetLogicAppFailures GetLogicAppFailures { get; set; }

        [JsonProperty("ForEach_RunFailure")] public ForEachRunFailure ForEachRunFailure { get; set; }

        [JsonProperty("Response")] public Response Response { get; set; }

        [JsonProperty("Get_LogicApps")] public GetLogicApps GetLogicApps { get; set; }

        [JsonProperty("Filter_LogicApps")] public FilterLogicApps FilterLogicApps { get; set; }

        [JsonProperty("ForEach_LogicApp")] public ForEachLogicApp ForEachLogicApp { get; set; }

        [JsonProperty("Insert_row")] public InsertRow InsertRow { get; set; }

        [JsonProperty("BatchAndGroup")] public BatchAndGroup BatchAndGroup { get; set; }

        [JsonProperty("Each_Batch")] public EachBatch EachBatch { get; set; }

        [JsonProperty("Json")] public Json Json { get; set; }

        [JsonProperty("HeaderToJson")] public HeaderToJson HeaderToJson { get; set; }

        [JsonProperty("MapToAXCompany")] public MapToAXCompany MapToAXCompany { get; set; }

        [JsonProperty("Json_Customer")] public JsonCustomer JsonCustomer { get; set; }

        [JsonProperty("Get_AccountNumber")] public GetAccountNumber GetAccountNumber { get; set; }

        [JsonProperty("Is_ContactUpdate")] public IsContactUpdate IsContactUpdate { get; set; }

        [JsonProperty("Get_AgreementDimension")]
        public GetAgreementDimension GetAgreementDimension { get; set; }

        [JsonProperty("AgreementNotExists")] public AgreementNotExists AgreementNotExists { get; set; }

        [JsonProperty("ConstructReponse")] public ConstructReponse ConstructReponse { get; set; }

        [JsonProperty("SucceedResponse")] public SucceedResponse SucceedResponse { get; set; }

        [JsonProperty("FailedResponse")] public FailedResponse FailedResponse { get; set; }

        [JsonProperty("Call_ReceiveWorkOrder")]
        public CallReceiveWorkOrder CallReceiveWorkOrder { get; set; }

        [JsonProperty("Check_Response")] public CheckResponse CheckResponse { get; set; }

        [JsonProperty("Compose_ReportRequest")]
        public ComposeReportRequest ComposeReportRequest { get; set; }

        [JsonProperty("Get_SSRS_Report")] public GetSSRSReport GetSSRSReport { get; set; }

        [JsonProperty("Upload_Report_to_SharePoint")]
        public UploadReportToSharePoint UploadReportToSharePoint { get; set; }

        [JsonProperty("RestResponseAccepted")] public RestResponseAccepted RestResponseAccepted { get; set; }

        [JsonProperty("Process_PrimaryTenant")]
        public ProcessPrimaryTenant ProcessPrimaryTenant { get; set; }

        [JsonProperty("Has_PrimaryTenant")] public HasPrimaryTenant HasPrimaryTenant { get; set; }

        [JsonProperty("IfRevenueGeneratingLease")]
        public IfRevenueGeneratingLease IfRevenueGeneratingLease { get; set; }

        [JsonProperty("Transform_PropertyToFixedAsset")]
        public TransformPropertyToFixedAsset TransformPropertyToFixedAsset { get; set; }

        [JsonProperty("Json_FixedAsset")] public JsonFixedAsset JsonFixedAsset { get; set; }

        [JsonProperty("ExistingFixedAsset")] public ExistingFixedAsset ExistingFixedAsset { get; set; }

        [JsonProperty("IfFixedAssetExists")] public IfFixedAssetExists IfFixedAssetExists { get; set; }

        [JsonProperty("Publish_InspectionReport")]
        public PublishInspectionReport PublishInspectionReport { get; set; }

        [JsonProperty("Decode_Excel")] public DecodeExcel DecodeExcel { get; set; }

        [JsonProperty("Each_Row")] public EachRow EachRow { get; set; }

        [JsonProperty("Set_CustomerContractDetail")]
        public SetCustomerContractDetail SetCustomerContractDetail { get; set; }

        [JsonProperty("Convert_FaultDesc")] public ConvertFaultDesc ConvertFaultDesc { get; set; }

        [JsonProperty("Create_EntityMap")] public CreateEntityMap CreateEntityMap { get; set; }
    }

    public class LogicAppValue
    {
        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("company")] public string Company { get; set; }

        [JsonProperty("entity")] public string Entity { get; set; }
    }

    public class Identifier
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("value")] public AzureLogicAppValue AzureLogicAppValue { get; set; }
    }

    public class Results
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("value")] public LogicAppValue Value { get; set; }
    }

    public class Outputs
    {
        [JsonProperty("identifier")] public Identifier Identifier { get; set; }

        [JsonProperty("results")] public Results Results { get; set; }
    }

    public class Definition
    {
        [JsonProperty("$schema")] public string Schema { get; set; }

        [JsonProperty("contentVersion")] public string ContentVersion { get; set; }

        [JsonProperty("parameters")] public Parameters Parameters { get; set; }

        [JsonProperty("triggers")] public Triggers Triggers { get; set; }

        [JsonProperty("actions")] public Actions Actions { get; set; }

        [JsonProperty("outputs")] public Outputs Outputs { get; set; }
    }


    public class AccessEndpointIpAddress
    {
        [JsonProperty("address")] public string Address { get; set; }
    }

    public class Workflow
    {
        [JsonProperty("outgoingIpAddresses")] public IList<OutgoingIpAddress> OutgoingIpAddresses { get; set; }

        [JsonProperty("accessEndpointIpAddresses")]
        public IList<AccessEndpointIpAddress> AccessEndpointIpAddresses { get; set; }
    }

    public class OutgoingIpAddress
    {
        [JsonProperty("address")] public string Address { get; set; }
    }

    public class Connector
    {
        [JsonProperty("outgoingIpAddresses")] public IList<OutgoingIpAddress> OutgoingIpAddresses { get; set; }
    }

    public class EndpointsConfiguration
    {
        [JsonProperty("workflow")] public Workflow Workflow { get; set; }

        [JsonProperty("connector")] public Connector Connector { get; set; }
    }


    public class AzureResourceValue
    {
        [JsonProperty("properties")] public Properties Properties { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("location")] public string Location { get; set; }

        [JsonProperty("tags")] public Tags Tags { get; set; }
        public string ResourceGroupName { get; set; }
    }

    public class AzureLogicAppValue : AzureResourceValue
    {
    
    }



    public class AzureResourceGroupProperties
    {

        [JsonProperty("provisioningState")]
        public string ProvisioningState { get; set; }
    }

    public class AzureTenantValue
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("domains")]
        public IList<string> Domains { get; set; }
    }

    public class AzureResourceGroupValue
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("properties")]
        public AzureResourceGroupProperties AzureResourceGroupProperties { get; set; }

        [JsonProperty("managedBy")]
        public string ManagedBy { get; set; }

        [JsonProperty("tags")]
        public Tags Tags { get; set; }
    }


    public class LogAnalyticsQuery
    {
        // TODO see Log Analytics OneNote for more queries
        public LogAnalyticsQuery()
        {
            Query = @"AzureDiagnostics | where status_s  !in ('Running', 'Skipped') | limit 50 | order by TimeGenerated desc";
            Start = DateTime.Now.AddDays(-90);
        }

        public string Query { get; set; }
        public int Top { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }

    public class AzureLogicAppWorkflowRunsError
    {

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class AzureLogicAppWorkflowRunsWorkflow
    {

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }


    public class AzureLogicAppWorkflowRunsTrigger
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("inputsLink")]
        public Link InputsLink { get; set; }

        [JsonProperty("outputsLink")]
        public Link OutputsLink { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("scheduledTime")]
        public DateTime ScheduledTime { get; set; }

        [JsonProperty("originHistoryName")]
        public string OriginHistoryName { get; set; }

        [JsonProperty("correlation")]
        public Correlation Correlation { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class AzureLogicAppWorkflowRunsOutputs
    {
    }

    public class AzureLogicAppWorkflowRunsProperties
    {

        [JsonProperty("waitEndTime")]
        public DateTime WaitEndTime { get; set; }

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("error")]
        public AzureLogicAppWorkflowRunsError Error { get; set; }

        [JsonProperty("correlation")]
        public Correlation Correlation { get; set; }

        [JsonProperty("workflow")]
        public AzureLogicAppWorkflowRunsWorkflow Workflow { get; set; }

        [JsonProperty("trigger")]
        public AzureLogicAppWorkflowRunsTrigger Trigger { get; set; }

        [JsonProperty("outputs")]
        public AzureLogicAppWorkflowRunsOutputs Outputs { get; set; }
    }

    public class LogicAppWorkflowRunValue
    {

        [JsonProperty("properties")]
        public AzureLogicAppWorkflowRunsProperties Properties { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class Link
    {

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("contentVersion")]
        public string ContentVersion { get; set; }

        [JsonProperty("contentSize")]
        public int ContentSize { get; set; }

        [JsonProperty("contentHash")]
        public ContentHash ContentHash { get; set; }
    }

    public class Tags
    {
        [JsonProperty("displayName")] public string DisplayName { get; set; }

        [JsonProperty("ManagedApi")] public string ManagedApi { get; set; }

        [JsonProperty("functionalArea")] public string FunctionalArea { get; set; }

        [JsonProperty("env")] public string Env { get; set; }
    }

    public class HttpPayload
    {
        [JsonProperty("statusCode")] public int StatusCode { get; set; }

        [JsonProperty("headers")] public Headers Headers { get; set; }

        [JsonProperty("body")] public Body Body { get; set; }
    }

    public class AzureLogicAppWorkflowTriggers
    {

        [JsonProperty("value")]
        public IList<AzureLogicAppWorkflowTriggerValue> Value { get; set; }
    }

    public class AzureLogicApps
    {

        [JsonProperty("value")]
        public IList<AzureLogicAppValue> Value { get; set; }

        [JsonProperty("nextLink")]
        public string NextLink { get; set; }
    }

    public class AzureLogicAppWorkflowRuns
    {

        [JsonProperty("value")]
        public IList<LogicAppWorkflowRunValue> Value { get; set; }

        [JsonProperty("nextLink")]
        public string NextLink { get; set; }
    }


    public class LogicAppWorkflowRun
    {
        public virtual DateTime CreationTime { get; set; }
        public virtual long? CreatorUserId { get; set; }
        public virtual DateTime? LastModificationTime { get; set; }

        public virtual long LastModifierUserId { get; set; }
        public virtual string SubscriptionId { get; set; }
        public virtual string ResourceGroupName { get; set; }

        public virtual string LogicAppName { get; set; }


        public virtual string RunId { get; set; }
        public virtual string Name { get; set; }
        public virtual string Status { get; set; }
        public virtual DateTime StartTimeUtc { get; set; }
        public virtual DateTime EndTimeUtc { get; set; }
        public virtual string Correlation { get; set; }
        public virtual string Trigger { get; set; }
        public virtual string Outputs { get; set; }

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsResolved { get; set; }
        public string Resolution { get; set; }

        /// <summary>
        /// http content body from outputLink=$_.properties.trigger.outputsLink.uri web request
        /// </summary>
        public virtual string TriggerInput { get; set; }

        public virtual string TriggerOutput { get; set; }

        public string InputContentData { get; set; }
        public string OutputContentData { get; set; }

        public string Code { get; set; }

        public string ExtensionData { get; set; }


    }

    public class LogicAppRunAction
    {
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }

        public string LogicAppName { get; set; }
        
        public string RunId { get; set; }
        public string Id { get; set; }

        public string ActionName { get; set; }
        public string Status { get; set; }
        public string StatusCode { get; set; }

        public DateTime? StartTimeUtc { get; set; }
        public DateTime? EndTimeUtc { get; set; }
        public string Correlation { get; set; }

        public string InputLink { get; set; }
        public string InputContentData { get; set; }
        public string OutputLink { get; set; }
        public string OutputContentData { get; set; }

    }


    public class Correlation
    {

        [JsonProperty("clientTrackingId")]
        public string ClientTrackingId { get; set; }

        [JsonProperty("actionTrackingId")]
        public string ActionTrackingId { get; set; }

    }

    public class ContentHash
    {

        [JsonProperty("algorithm")]
        public string Algorithm { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class InputsLink
    {

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("contentVersion")]
        public string ContentVersion { get; set; }

        [JsonProperty("contentSize")]
        public int ContentSize { get; set; }

        [JsonProperty("contentHash")]
        public ContentHash ContentHash { get; set; }
    }

    public class OutputsLink
    {

        [JsonProperty("uri")]
        public string Uri { get; set; }

        [JsonProperty("contentVersion")]
        public string ContentVersion { get; set; }

        [JsonProperty("contentSize")]
        public int ContentSize { get; set; }

        [JsonProperty("contentHash")]
        public ContentHash ContentHash { get; set; }
    }

    public class HistoryProperties
    {

        [JsonProperty("startTime")]
        public DateTime StartTime { get; set; }

        [JsonProperty("endTime")]
        public DateTime EndTime { get; set; }

        [JsonProperty("scheduledTime")]
        public DateTime ScheduledTime { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("correlation")]
        public Correlation Correlation { get; set; }

        [JsonProperty("inputsLink")]
        public InputsLink InputsLink { get; set; }

        [JsonProperty("outputsLink")]
        public OutputsLink OutputsLink { get; set; }

        [JsonProperty("fired")]
        public bool Fired { get; set; }
    }

    public class ActionValue
    {

        [JsonProperty("properties")]
        public HistoryProperties Properties { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class AzureLogicAppWorkflowTriggerHistoryRun
    {

        [JsonProperty("value")]
        public IList<ActionValue> Value { get; set; }

        [JsonProperty("nextLink")]
        public string NextLink { get; set; }
    }

    #endregion

    public class LogicAppResubmittedRun
    {
        public DateTime CreationTime { get; set; }

        public string SubscriptionId { get; set; }

        public string ResourceGroupName { get; set; }

        public string LogicAppName { get; set; }

        public string NewRunId { get; set; }

        public string OldRunId { get; set; }

        public HttpStatusCode Status { get; set; }
        public bool IsSuccessStatusCode { get; set; }

        public string Correlation { get; set; }
        public string Trigger { get; set; }
        public string Content { get; set; }

        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public string Code { get; set; }

    }

}
