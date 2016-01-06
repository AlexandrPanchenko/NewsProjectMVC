using NewsDotNet.DomainModel.Entities;
using System.Data.Entity;

namespace NewsDotNet.DomainModel.Concrete
{
    class EFDBContext: DbContext
    {
        public EFDBContext()
        {
            Database.SetInitializer<EFDBContext>(new DBInitialiser());
        }

        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<MainPageEntity> MainPageEntities { get; set; }
        public DbSet<GridData> GridData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Article>()
                .HasMany(a => a.Tags)
                .WithMany(t => t.Articles)
                .Map(
                m =>
                    {
                        m.MapLeftKey("ArticleID");
                        m.MapRightKey("TagID");
                        m.ToTable("ArticlesToTags");
                    }
                );

            modelBuilder.Entity<MainPageEntity>()
                .HasRequired<Article>(e => e.Article);
        }
    }
}
