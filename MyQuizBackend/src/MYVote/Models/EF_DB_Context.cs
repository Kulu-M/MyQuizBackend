using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MYVote.Models
{
    public partial class EF_DB_Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Path.Combine(Startup._iHostingEnv.ContentRootPath, @"Database\MYVoteDB_Final.db");

            optionsBuilder.UseSqlite(@"Datasource=" + path);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public virtual DbSet<AnswerOption> AnswerOption { get; set; }
        public virtual DbSet<AnswerOptionGivenAnswer> AnswerOptionGivenAnswer { get; set; }
        public virtual DbSet<Device> Device { get; set; }
        public virtual DbSet<DeviceGroup> DeviceGroup { get; set; }
        public virtual DbSet<FinalQuestion> FinalQuestion { get; set; }
        public virtual DbSet<FinalQuestionGivenAnswer> FinalQuestionGivenAnswer { get; set; }
        public virtual DbSet<GivenAnswer> GivenAnswer { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<QuestionAnswerOption> QuestionAnswerOption { get; set; }
        public virtual DbSet<QuestionBlock> QuestionBlock { get; set; }
        public virtual DbSet<QuestionQuestionBlock> QuestionQuestionBlock { get; set; }
        public virtual DbSet<SingleTopic> SingleTopic { get; set; }
        public virtual DbSet<Topic> Topic { get; set; }
        public virtual DbSet<TopicSingleTopic> TopicSingleTopic { get; set; }
    }
}