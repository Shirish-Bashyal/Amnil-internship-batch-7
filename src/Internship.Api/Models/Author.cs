namespace Internship.Api.Models;

public class Author: BaseEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
}
