using AlunoApiFullStack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AlunoApiFullStack.Context
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Aluno> Alunos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração para IdentityUserLogin que pode estar causando problemas
            modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });

            // Configuração de dados iniciais
            modelBuilder.Entity<Aluno>().HasData(new Aluno
            {
                Id = 1,
                Nome = "Marta",
                Email = "pupupi@gmail.com",
                Idade = 23
            },
            new Aluno
            {
                Id = 2,
                Nome = "Paula",
                Email = "paulinha@gmail.com",
                Idade = 80
            });
        }
    }
}
