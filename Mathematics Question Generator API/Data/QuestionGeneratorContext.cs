using MathematicsQuestionGeneratorAPI.DBOs;
using MathematicsQuestionGeneratorAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MathematicsQuestionGeneratorAPI.Data
{
    public class QuestionGeneratorContext : DbContext
    {
        public QuestionGeneratorContext(DbContextOptions options) : base(options) { }

        public DbSet<UserDbo> UserDbo { get; set; }
        public DbSet<WorksheetDbo> WorksheetDbo { get; set; }
        public DbSet<QuestionDbo> QuestionDbo { get; set; }
        public DbSet<QuestionTypeDbo> QuestionTypeDbo { get; set; }
        public DbSet<WorksheetQuestionDbo> WorksheetQuestionDbo { get; set; }
        public DbSet<QuadraticEquationDbo> QuadraticEquationDbo { get; set; }
        public DbSet<LinearSimultaneousEquationsDbo> LinearSimultaneousEquationsDbo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDbo>().ToTable("User");
            modelBuilder.Entity<WorksheetDbo>().ToTable("Worksheet");
            modelBuilder.Entity<QuestionDbo>().ToTable("Question");
            modelBuilder.Entity<QuestionTypeDbo>().ToTable("QuestionType");
            modelBuilder.Entity<WorksheetQuestionDbo>().ToTable("WorksheetQuestion");
            modelBuilder.Entity<QuadraticEquationDbo>().ToTable("QuadraticEquations");
            modelBuilder.Entity<LinearSimultaneousEquationsDbo>().ToTable("LinearSimultaneousEquations");
        }
    }
}
