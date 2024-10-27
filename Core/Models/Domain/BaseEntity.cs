using System.ComponentModel.DataAnnotations;

namespace Core.Models.Domain
{
    public abstract class BaseEntity
    {
        [Key]   
        public required int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public required string UserCreated { get; set; }
        public required string UserModified { get; set; }
        public required bool IsRetired { get; set; } = false;
    }
}
