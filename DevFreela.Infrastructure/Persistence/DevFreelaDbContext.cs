using DevFreela.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevFreela.Infrastructure.Persistence {
    public class DevFreelaDbContext : DbContext{
        public DevFreelaDbContext(DbContextOptions<DevFreelaDbContext> options) : base(options) {
            
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
        public DbSet<ProjectComment> ProjectComments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            //Implementação da modelagem

            //Builder das entidades
            builder.Entity<Skill>(e =>
            {
                e.HasKey(s => s.Id);
            });

            builder.Entity<UserSkill>(e => {
                e.HasKey(us => us.Id);
                e.HasOne(u => u.Skill)
                    .WithMany(u => u.UserSkills)
                    .HasForeignKey(s => s.IdSkill)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ProjectComment>(e => {
                e.HasKey(p => p.Id);

                e.HasOne(p => p.Project) //O comentário está dentro de 1 projeto (has one)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(p => p.IdProject)
                    .OnDelete(DeleteBehavior.Restrict);

                //Erro de exceção ocorrido por não configurar o ProjectComment junto ao IdUsuario
                e.HasOne(p => p.User)
                    .WithMany(u => u.Comments)
                    .HasForeignKey(p => p.IdUser)
                    .OnDelete(DeleteBehavior.Restrict);

            });

            builder.Entity<User>(e => {
                e.HasKey(p => p.Id);

                e.HasMany(u => u.Skills)
                    .WithOne(us => us.User)
                    .HasForeignKey(u => u.IdUser)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Project>(e => {
                e.HasKey(p => p.Id);

                //Relacionamento do freelancer com o projeto
                e.HasOne(p => p.Freelancer)
                    .WithMany(f => f.FreelanceProjects)
                    .HasForeignKey(p => p.IdFreelancer)
                    .OnDelete(DeleteBehavior.Restrict);

                //Relacionamento do cliente com o projeto - e portanto, atributos referenciais ao cliente
                e.HasOne(p => p.Client)
                    .WithMany(c => c.OwnedProjects)
                    .HasForeignKey(p => p.IdClient)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(builder);
        }

    }
}
