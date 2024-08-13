using Ardalis.Specification;
using BG.LocalWebApp.Common.Entities;

namespace BG.LocalWebApp.Application.Common.Specifications.Base
{
    /// <summary>
    /// A specification to retrieve an entity by its unique identifier.
    /// </summary>
    /// <typeparam name="T">The type of the entity, which must inherit from <see cref="BaseEntity"/>.</typeparam>
    public class GetByIdSpecification<T> : Specification<T> where T : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetByIdSpecification{T}"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the entity to retrieve.</param>
        public GetByIdSpecification(Guid id)
        {
            Query.AsNoTracking().Where(x => x.Id == id);
        }
    }

}
