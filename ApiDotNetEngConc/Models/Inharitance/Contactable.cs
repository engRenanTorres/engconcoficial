namespace DotnetAPI.Models.Inharitance;
public abstract class Contactable
{
    public virtual string Email { get; set; } = "";
    public string? Website { get; set; }
    public string? SocialMedia { get; set; }
}