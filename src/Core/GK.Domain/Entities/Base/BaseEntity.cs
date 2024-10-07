namespace GK.Domain.Entities.Base
{
    public class BaseEntity
    {
        public Guid Id { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public Guid CreatedUserId { get; set; }

        public Guid UpdatedUserId { get; set; }

        public DateTime CreatedDateTime { get; set; }

        public DateTime UpdatedDateTime { get; set; }
    }
}
