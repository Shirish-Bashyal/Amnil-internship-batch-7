using System.Text.Json.Serialization;

namespace LibraryManagement.Model;

/// <summary>
/// Author entity representing book authors
/// </summary>
public class Author
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    // Navigation property, virtual for Lazy Loading
    [JsonIgnore]
    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
