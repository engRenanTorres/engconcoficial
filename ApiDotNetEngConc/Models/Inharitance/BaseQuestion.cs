using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAPI.Models.Inharitance
{
    public abstract class BaseQuestion
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

        public User? CreatedBy { get; set; }

        public virtual ICollection<Choice> Choices { get; set; }
    }
}
