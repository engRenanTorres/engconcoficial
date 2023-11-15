using DotnetAPI.Models;
using DotnetAPI.Models.Inharitance;
using Microsoft.EntityFrameworkCore;

namespace DotnetAPI.Data
{
    public class DataContextEF : DbContext
    {

        public DataContextEF(DbContextOptions<DataContextEF> options) : base(options)
        {
            // _conectionString = config.GetConnectionString("DefaultConnection");
        }
        public DbSet<User>? Users { get; set; }
        public DbSet<BaseQuestion> Questions { get; set; }
        public DbSet<BooleanQuestion> BooleanQuestions { get; set; }
        public DbSet<MultipleChoicesQuestion> MultipleChoiceQuestions { get; set; }
        public DbSet<Choice> Choices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
              .HasMany(q => q.Questions)
              .WithOne(u => u.CreatedBy)
              .IsRequired();
            // To use Table Per Type TPT:
            // modelBuilder.Entity<MultipleChoicesQuestion>().ToTable("Muliple_Choice_Questions");
            // modelBuilder.Entity<BooleanQuestion>().ToTable("Boolean_Questions");
        }

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          modelBuilder
            .Entity<User>()//.ToTable("User").HasKey(q => q.Id);
            .HasMany(u => u.Questions)
            .WithOne(q => q.CreatedBy)
            .HasForeignKey(q => q.CreatedById)
            .HasPrincipalKey(u => u.Id);
          modelBuilder
            .Entity<Question>()
            .ToTable("Questions")//.HasKey(q => q.Id);
            .HasOne(q => q.CreatedBy)
            .WithMany(u => u.Questions)
            .HasForeignKey(u => u.Id)
            .HasPrincipalKey(q => q.Id);
        }*/
    }
}