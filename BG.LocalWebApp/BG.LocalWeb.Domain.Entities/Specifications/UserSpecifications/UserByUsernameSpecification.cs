using Ardalis.Specification;
using BG.LocalWeb.Domain.Entities;

namespace BG.LocalWeb.Domain.Specifications.UserSpecifications
{
    /// <summary>
    /// Specification for querying <see cref="User"/> entities by their username.
    /// </summary>
    public class UserByUsernameSpecification : Specification<User>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserByUsernameSpecification"/> class.
        /// </summary>
        /// <param name="username">The username used to filter <see cref="User"/> entities.</param>
        public UserByUsernameSpecification(string username)
        {
            // Configure the query to filter users based on their username.
            Query.AsNoTracking()
                .Where(user => user.UserName == username);
        }
    }
}
