namespace Silvercrest.Entities
{
    public class AccountAccess
    {
        public int? AccessType { get; set; }
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public string ManagerCode { get; set; }
        public string ShortName { get; set; }
    }
}