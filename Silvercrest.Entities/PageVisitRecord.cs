using Silvercrest.Entities.Enums;

namespace Silvercrest.Entities
{
    public class PageVisitRecord
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Route { get; set; }
        public int Count { get; set; }
        public PageType PageType { get; set; }
        public PageVisitRecord()
        {
            Count = 1;
        }
    }
}
