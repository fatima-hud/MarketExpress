using MarketExpress.Core.Entity.Identity;
using MarketExpress.Core.Enums;

namespace MarketExpress.Core.Entity
{
    public class Document
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public User? User { get; set; }
        public int? StoreId { get; set; }
        public Store? Store { get; set; }

        public string FilePath { get; set; }
        public enFileDocumentType FileType { get; set; }
        public enDocumentOwnerType OwnerType { get; set; }

      
    }
}
