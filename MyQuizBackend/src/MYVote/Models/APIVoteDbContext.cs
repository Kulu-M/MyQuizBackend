using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MYVote.Models
{
    public partial class APIVoteDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            #warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
            optionsBuilder.UseSqlite(@"Datasource=C:\Users\marhei\Documents\Visual_Studio_2015\Projects\MYVote\MYVote\src\MYVote\Database\MYVoteDB_Final.db");
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