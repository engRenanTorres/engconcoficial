namespace DotnetAPI.Models;
using DotnetAPI.Enums;
using DotnetAPI.Models.Inharitance;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

[Index(nameof(User.Email), IsUnique = true)]
public class User : Contactable
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Column(name: "Created_at")]
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; } = "";
    public required byte[] Password { get; set; }
    [JsonIgnore]
    public ICollection<BaseQuestion>? Questions { get; set; }
    public Roles Role { get; set; } = Roles.User;
}

public class EmailQueryParam
{
    [BindRequired]
    public required string Email { get; set; }
}

