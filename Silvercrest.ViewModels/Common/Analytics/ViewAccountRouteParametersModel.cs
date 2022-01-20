namespace Silvercrest.ViewModels.Common.Analytics
{
    public class ViewAccountRouteParametersModel
    {
        public string EntityIdString { get; set; } = "entityId=";
        public int? EntityId { get; set; }
        public string ContactIdString { get; set; } = "contactId=";
        public int? ContactId { get; set; }
        public string IsGroupString { get; set; } = "isGroup=";
        public bool? IsGroup { get; set; }
        public string IsClientGroupString { get; set; } = "isClientGroup=";
        public bool? IsClientGroup { get; set; }
    }
}
