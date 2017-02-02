using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MYVote.Models
{
    public partial class EF_DB_Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlite(@"Datasource=C:\Users\Kulu-M\Documents\MyQuizBackend\MyQuizBackend\src\MYVote\Database\MYVote_DB_V2.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<AnswerOption> AnswerOption { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<DeviceGroup> DeviceGroup { get; set; }
        public virtual DbSet<GivenAnswer> GivenAnswer { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupSingleTopic> GroupSingleTopic { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionAnswerOption> QuestionAnswerOption { get; set; }
        public virtual DbSet<QuestionBlock> QuestionBlock { get; set; }
        public virtual DbSet<QuestionQuestionBlock> QuestionQuestionBlock { get; set; }
        public virtual DbSet<SingleTopic> SingleTopic { get; set; }
    }
}