namespace BG.LocalWebApp.Common.Entities
{
    /// <summary>
    /// Represents the base class for all entities, providing a unique identifier.
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the unique identifier for the entity.
        /// </summary>
        public Guid Id { get; set; }
    }

}
