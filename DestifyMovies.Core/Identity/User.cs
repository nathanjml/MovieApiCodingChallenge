using DestifyMovies.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DestifyMovies.Core.Identity
{
    public class User : Entity
    {
        public string UserApiToken { get; set; } = "";
        public string EmailAddress { get; set; } = "";
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }

    public class EditUserDto
    {
        public long Id { get; set; }
    }

    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasIndex(x => x.UserApiToken);
        }
    }
}
