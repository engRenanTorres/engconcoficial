namespace DotnetAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Question
{
    [Key]
    public int Id { get; set; }
    [Column(name: "Created_at")]
    public DateTime CreatedAt { get; set; }
    [Column(name: "Last_updated_at")]
    public DateTime LastUpdatedAt { get; set; }

    public string Body { get; set; } = "";

    public char Answer { get; set; }

    public string? Tip { get; set; } = "";
    public int CreatedById { get; set; }
    //[JsonIgnore]
    //[ForeignKey("Users")]
    public User? CreatedBy { get; set; }
}
